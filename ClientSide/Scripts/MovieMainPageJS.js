
    const {createApp, ref} = Vue;
    const config = {
        setup(){
        const movies = ref([
    {id: 1, name: "電影1", description: "是得羊罪成郭惶，皇促后许丈什。", imgUrl: "/Images/mv1.jpg" },
    {id: 2, name: "電影2", description: "了土商啊以作安不并学憾有，觉。", imgUrl: "/Images/mv2.jpg" },
    {id: 3, name: "電影3", description: "程人不商非高以，路未备，由币。", imgUrl: "/Images/mv3.jpg" },
    {id: 4, name: "電影4", description: "放了秦法如谭何快不厅洪，韩历。", imgUrl: "/Images/mv4.jpg" },
    {id: 5, name: "電影5", description: "若才那秦之哥拿皇日之磊，中生。", imgUrl: "/Images/mv5.jpg" },
    {id: 6, name: "電影6", description: "今气关派严着张交家，的见不不。", imgUrl: "/Images/mv6.jpg" },
    {id: 7, name: "電影7", description: "在小德未极派人但生他联判，是。", imgUrl: "/Images/mv7.jpg" },
    {id: 8, name: "電影8", description: "上忧到司由后斯，得又开，死认。", imgUrl: "/Images/mv8.jpg" },
    ]);

    const mostViewedCardRow = ref(null);
    const mostCommentedCardRow = ref(null);
    const allMoviesCardRow = ref(null);

    function scrollLeft(section) {
        let target;
    if(section === 'mostViewed') target = mostViewedCardRow.value;
    else if(section === 'mostCommented') target = mostCommentedCardRow.value;
    else if(section === 'allMovies') target = allMoviesCardRow.value;

    if(target){
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
        alert(`查看 ${movie.name} 的詳細資訊`);
                }

    return {
        movies,
        mostViewedCardRow,
        mostCommentedCardRow,
        allMoviesCardRow,
        scrollLeft,
        scrollRight,
        viewMovie,
                }
            }
        }
    createApp(config).mount("#app");