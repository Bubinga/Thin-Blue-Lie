var isMobile = false;
var isInfoPage = false;
function MakeMobileFriendly() {
    if (isInfoPage == false) {
        return;
    }
    if ($(window).width() < 976) {  
        var element = document.getElementById('context');
        var newElement = document.getElementById('addMedia');
        element.parentNode.insertBefore(newElement, element);
        document.getElementById('basicInfo').appendChild(
            document.getElementById('similarEvents')
        );
        isMobile = true;
    }
    else if (isMobile == true) {
        document.getElementById('otherColumn').appendChild(
            document.getElementById('similarEvents')
        );
        document.getElementById('otherColumn').appendChild(
            document.getElementById('addMedia')
        );
        isMobile = false;
    }
}

$(document).ready(function () {
    var path = window.location.pathname;
    if (path.includes("/Submit") || path.includes("/Edit")) {
        document.getElementsByTagName("BODY")[0].onresize = function () { MakeMobileFriendly() };
        isInfoPage = true;
    }
    else {
        isInfoPage = false;
    }
});