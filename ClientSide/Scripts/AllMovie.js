const { createApp, ref } = Vue;

createApp({
    setup() {
        const movies = ref([]);
        const loading = ref(true);

        // 延長加載時間，確保能看到骨架效果
        setTimeout(() => {
            movies.value = moviesData?.map(movie => ({
                ...movie,
                ReleaseDate: movie.ReleaseDate.replace(/\/Date\((-?\d+)\)\//, function (match, timestamp) {
                    return new Date(parseInt(timestamp))
                        .toLocaleDateString('zh-TW', {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit'
                        });
                })
            })) || [];
            loading.value = false;
        }, 500); // 延長至2秒

        return {
            movies,
            loading
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
        goToMovie(id) {
            window.location.href = `/Movies/Movie/?movieId=${id}`;
        }
    }
}).mount('#movie-list-app');