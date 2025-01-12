const { createApp, ref } = Vue;
const config = {
    setup() {
        const movies = ref(window.moviesData || []);
        const mostViewedMovies = ref(window.mostReviewedMoviesData || []);
        const mostCommentedMovies = ref(window.upcomingMoviesData || []);

        const mostViewedCardRow = ref(null);
        const mostCommentedCardRow = ref(null);
        const allMoviesCardRow = ref(null);

        const loading = ref(false);

        function scrollLeft(section) {
            let target;
            if (section === 'mostViewed') target = mostViewedCardRow.value;
            else if (section === 'mostCommented') target = mostCommentedCardRow.value;
            else if (section === 'allMovies') target = allMoviesCardRow.value;

            if (target) {
                target.scrollBy({ left: -600, behavior: 'smooth' });
            }
        }

        function scrollRight(section) {
            let target;
            if (section === 'mostViewed') target = mostViewedCardRow.value;
            else if (section === 'mostCommented') target = mostCommentedCardRow.value;
            else if (section === 'allMovies') target = allMoviesCardRow.value;

            if (target) {
                // 檢查是否已經到最右邊
                if (target.scrollLeft + target.clientWidth >= target.scrollWidth) {
                    target.scrollLeft = 0;  // 回到最左邊
                } else {
                    target.scrollBy({ left: 600, behavior: 'smooth' });
                }
            }
        }

        function viewMovie(movie) {
            window.location.href = `/Movies/Movie?movieId=${movie.Id}`;
        }

        return {
            movies,
            mostViewedMovies,
            mostCommentedMovies,
            mostViewedCardRow,
            mostCommentedCardRow,
            allMoviesCardRow,
            scrollLeft,
            scrollRight,
            viewMovie,
            loading,
        }
    }
}
createApp(config).mount("#app");