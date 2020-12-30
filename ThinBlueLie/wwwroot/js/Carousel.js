var galleryThumbs;
var galleryTop;
var slideCount;
var slides;
var loadedEvent;
var players = [];
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
            on: {
                slideChange: function () {
                    $("videowrapper:not(.paused) btn.toggle-play").mouseup(); //pauses reddit videos
                    players.forEach(function (element) {
                        element.pauseVideo();
                    });
                },
            }
        });
        loadedEvent = window.location.href;
    }    
}
function onYouTubePlayerAPIReady() {
    $("iframe").toArray().forEach(function (element) { //pauses youtube videos
        var player = new YT.Player(element.id, {});
        players.push(player);
    });
}
