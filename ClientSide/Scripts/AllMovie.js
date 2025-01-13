const { createApp, ref } = Vue;

createApp({
    setup() {
        const movies = ref(moviesData);
        return {
            movies
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