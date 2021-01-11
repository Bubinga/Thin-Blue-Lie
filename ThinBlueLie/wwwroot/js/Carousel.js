var galleryThumbs;
var galleryTop;
var slideCount;
var slides;
var loadedEvent;
var players = [];
function InitializeSwiper() {
    slideCount = jQuery(".gallery-thumbs .swiper-wrapper .swiper-slide").length;
    slides = (slideCount > 3 ? 'auto' : `${slideCount}`);

    //TODO only loop if item count > slides per view
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
        $("videowrapper:not(.paused) btn.toggle-play").mouseup();
        loadedEvent = window.location.href;        
    }       
}
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
// 2. This code loads the IFrame Player API code asynchronously.
function getYoutubeAPIscript() {
    var tag = document.createElement('script');
    tag.src = "http://www.youtube.com/player_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}

async function onYouTubePlayerAPIReady() {
    while (galleryTop == null) {
          await sleep(2000);
    }
    $("iframe").toArray().forEach(function (element) { //pauses youtube videos
        element.id = uuidv4();
        var player = new YT.Player(element.id, {});
        players.push(player);
    });
}
