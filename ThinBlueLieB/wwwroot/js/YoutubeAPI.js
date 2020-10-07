// 2. This code loads the IFrame Player API code asynchronously.
////var id;
//    var tag = document.createElement('script');

//    tag.src = "https://www.youtube.com/iframe_api";
//    var firstScriptTag = document.getElementsByTagName('script')[0];
//    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

// 3. This function creates an <iframe> (and YouTube player)
//    after the API code downloads.

//var player;
//function onYouTubeIframeAPIReady() {
//    player = new YT.Player('player', {
//        height: '390',
//        width: '640',
//        videoId: id,
//        playerVars: { 'rel': 0}
//    });
//}

function loadYouTubeFromId(inputId) {
    player = new YT.Player(inputId, {      
        videoId: inputId,
        playerVars: { rel: 0, showinfo: 0, ecver: 2 }
    });
}

function LoadVideo() {
    loadYTScript();
    onYouTubeIframeAPIReady();
}

function stopVideo() {
    player.stopVideo();
}

function stopVideo() {
    player.playVideo();
}