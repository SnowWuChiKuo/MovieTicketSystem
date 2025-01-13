// 初始化 Vue 應用
const { createApp, ref, onMounted } = Vue;

createApp({
    setup() {
        const movie = ref({
            id: movieData.Id,
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
            canReview: movieData.CanReview,
            reviews: movieData.Reviews || []
        });
        
        onMounted(() => {
            // 更新頁面標題
            document.title = `${movieData.Title} - 電影訂票網`;
        });

        return {
            activeNames: ['1'], 
            movie,
            isCommentsOpen: true,
            newReview: {
                rating: 0,
                comment: ''
            },
            isLoggedIn: movieData.IsLoggedIn || false,
            currentUserName: movieData.CurrentUserName || '',
            isSubmitting: false
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
            return new Date(date).toLocaleDateString();
        },
        bookTicket() {
                window.location.href = `/Tickets/Index?movieId=${this.movie.id}`;
        },
        async submitReview() {
            if (!this.canSubmitReview) return;

            this.isSubmitting = true;
            try {
                const response = await fetch(`/Movies/SubmitReview?movieId=${this.movie.id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        rating: this.newReview.rating,
                        comment: this.newReview.comment
                    })
                });

                const result = await response.json();

                if (result.success) {
                    // 將新評論加入列表
                    this.movie.reviews.unshift(result.data);

                    // 更新評論數量和平均分
                    this.movie.reviewCount++;

                    // 清空表單
                    this.newReview.rating = 0;
                    this.newReview.comment = '';

                    // 顯示成功
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
