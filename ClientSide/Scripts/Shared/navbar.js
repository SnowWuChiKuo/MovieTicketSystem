
document.addEventListener('DOMContentLoaded', function () {
    // 只保留滾動處理功能
    let lastScrollTop = 0;
    const navbar = document.querySelector('.navbar');

    window.addEventListener('scroll', function () {
        let scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        if (scrollTop > lastScrollTop) {
            // 向下滾動
            navbar.style.top = '-80px';
        } else {
            // 向上滾動
            navbar.style.top = '0';
        }

        lastScrollTop = scrollTop;
    });
});