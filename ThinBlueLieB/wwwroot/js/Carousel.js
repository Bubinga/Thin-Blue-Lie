var galleryThumbs;
var galleryTop;
function InitializeSwiper() {
    galleryThumbs = new Swiper(".gallery-thumbs", {
        spaceBetween: 2,
        slidesPerView: 'auto',
        loop: true,
        loopedSlides: $(".swiper-wrapper").length,
        freeMode: true,
        watchSlidesVisibility: true,
        watchSlidesProgress: true
        //breakpoints: {
        //    640: {
        //        slidesPerView: 2,
        //        spaceBetween: 20,
        //    },
        //    768: {
        //        slidesPerView: 4,
        //        spaceBetween: 40,
        //    },
        //    1024: {
        //        slidesPerView: 5,
        //        spaceBetween: 50,
        //    }
        //}
    });
    galleryTop = new Swiper(".gallery-top", {
        spaceBetween: 10,
        slidesPerView: 'auto',
        loop: true,
        loopedSlides: $(".swiper-wrapper").length,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        thumbs: {
            swiper: galleryThumbs,
        },
    });
}