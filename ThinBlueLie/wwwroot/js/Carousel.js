var galleryThumbs;
var galleryTop;
//TODO Turn on looping
function InitializeSwiper() {
    galleryTop = new Swiper(".gallery-top", {
        spaceBetween: 2,
        slidesPerView: 'auto',
        loop: true,
        loopedSlides: $(".gallery-thumbs .swiper-wrapper").childElementCount,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        thumbs: {
            swiper: galleryThumbs,
        },
    });
    galleryThumbs = new Swiper(".gallery-thumbs", {
        spaceBetween: 2,
        slidesPerView: 'auto',
        // loop: true,
        freeMode: true,
        //loopedSlides: $(".gallery-thumbs .swiper-wrapper").childElementCount, //looped slides should be the same
        watchSlidesVisibility: true,
        watchSlidesProgress: true,
        //loop: true,
        //loopedSlides: $(".gallery-thumbs .swiper-wrapper").childElementCount,
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
}