var isMobile = false;
function MakeMobileFriendly() {
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

window.onload = function () {
    var path = window.location.pathname;
    if (path.includes("/Submit") || path.includes("/Edit")) {
        document.getElementsByTagName("BODY")[0].onresize = function () { MakeMobileFriendly() };
        MakeMobileFriendly();
    }
}

