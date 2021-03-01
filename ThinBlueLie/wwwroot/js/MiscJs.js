var isMobile = false;
var isInfoPage = false;
let test;
function MakeMobileFriendly() {
    if (isInfoPage == false) {
        return;
    }
    var addMedia = document.getElementById('addMedia');
    var similarEvents = document.getElementById('similarEvents');
    var element = document.getElementById('context');
    if ($(window).width() < 976) {  
        element.parentNode.insertBefore(addMedia, element);
        if (similarEvents != null) {
            document.getElementById('basicInfo').appendChild(similarEvents);
        }
        isMobile = true;
    }
    else if (isMobile == true) {
        if (similarEvents != null) {
            document.getElementById('otherColumn').appendChild(similarEvents);
        }
        document.getElementById('otherColumn').appendChild(addMedia);
        isMobile = false;
    }
}

$(document).ready(function () {
    var path = window.location.pathname;
    if (path.includes("/Submit") || path.includes("/Edit")) {
        document.getElementsByTagName("BODY")[0].onresize = function () { MakeMobileFriendly() };
        test = document.getElementById('addMedia');
        isInfoPage = true;
    }
    else {
        isInfoPage = false;
    }
});