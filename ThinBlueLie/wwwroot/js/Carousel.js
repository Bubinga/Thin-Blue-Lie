var galleryThumbs;
var galleryTop;
var slideCount;
var slides;
var loadedEvent;
//TODO Turn on looping
function InitializeSwiper() {
    slideCount = jQuery(".gallery-thumbs .swiper-wrapper .swiper-slide").length;
    slides = (slideCount > 3 ? 'auto' : `${slideCount}`);
    if (loadedEvent != window.location.href) {
        galleryThumbs = new Swiper(".gallery-thumbs", {
            spaceBetween: 2,
            slidesPerView: 'auto',
            loop: true,
            freeMode: true,
            loopedSlides: slideCount, //looped slides should be the same
            watchSlidesVisibility: true,
            watchSlidesProgress: true,
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
        galleryTop = new Swiper(".gallery-top", {
            spaceBetween: 2,
            slidesPerView: 'auto',
            loop: true,
            loopedSlides: slideCount,
            navigation: {
                nextEl: ".swiper-button-next",
                prevEl: ".swiper-button-prev",
            },
            thumbs: {
                swiper: galleryThumbs,
            },
        });
        loadedEvent = window.location.href;
    }    
}
