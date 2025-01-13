// 初始化 Vue 應用
const { createApp, ref, onMounted } = Vue;

createApp({
    setup() {
        const movie = ref({
            title: movieData.Title,
            poster: movieData.PosterURL, 
            genre: movieData.GenreName,
            rating: movieData.RatingName,
            duration: `${movieData.RunTime}分鐘`,
            releaseDate: new Date(movieData.ReleaseDate).toLocaleDateString(),
            director: movieData.Director,
            cast: movieData.Cast,
            rating_score: movieData.AverageRating || 0,
            rating_count: movieData.ReviewCount,
            description: movieData.Description,
            canReview: movieData.CanReview || false,
            reviews: movieData.Reviews?.map(r => ({
                id: r.Id,
                memberName: r.MemberName,
                content: r.Comment,
                createdAt: new Date(r.CreatedAt).toLocaleDateString('zh-TW', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit'
                }),
                rating: r.Rating
            })) || []
        });
        
        const newReview = ref({
            rating: 0,
            comment: ''
        });

        const isSubmitting = ref(false);

        onMounted(() => {
            // 更新頁面標題
            document.title = `${movieData.Title} - 電影訂票網`;
        });

        return {
            activeNames: ['1'], 
            movie,
            isCommentsOpen: ref(true),
            newReview,
            isSubmitting,
            isLoggedIn: movieData.IsLoggedIn || false,
            currentUserName: movieData.CurrentUserName || '',
        }
    },
    computed: {
        shouldShowExpandButton() {
            // 檢查文字是否超過一行
            const castElement = this.$el.querySelector('.cast-text');
            if (castElement) {
                return castElement.scrollHeight > castElement.clientHeight;
            }
            return false;
        },
        canSubmitReview() {
            return !this.isSubmitting &&
                this.newReview.rating > 0 &&
                this.newReview.comment.trim().length > 0;
        }
    },
    methods: {
        formatDate(date) {
            return new Date(date).toLocaleDateString('zh-TW', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit'
            });
        },
        bookTicket() {
            window.location.href = '/Tickets/Index';
        },
        async submitReview() {
            if (!this.canSubmitReview) return;

            this.isSubmitting = true;
            try {
                const response = await fetch(`/Movies/SubmitReview`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        movieId: movieData.Id,
                        rating: this.newReview.rating,
                        comment: this.newReview.comment
                    })
                });

                if (!response.ok) {
                    throw new Error('網路請求失敗');
                }

                const result = await response.json();

                if (result.success) {
                    // 將新評論加入列表
                    this.movie.reviews.unshift({
                        id: result.data.id,
                        memberName: this.currentUserName,
                        content: result.data.comment,
                        createdAt: new Date().toLocaleDateString('zh-TW', {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit'
                        }),
                        rating: result.data.rating
                    });

                    // 更新評論數量
                    this.movie.rating_count++;

                    // 清空表單
                    this.newReview.rating = 0;
                    this.newReview.comment = '';

                    // 顯示成功訊息
                    ElementPlus.ElMessage({
                        message: '評論發表成功！',
                        type: 'success'
                    });
                } else {
                    throw new Error(result.message);
                }
            } catch (error) {
                ElementPlus.ElMessage({
                    message: error.message || '評論發表失敗',
                    type: 'error'
                });
            } finally {
                this.isSubmitting = false;
            }
        },
        toggleComments() {
            this.isCommentsOpen = !this.isCommentsOpen;
        }
    }
}).use(ElementPlus).mount('#movie-detail-app');
