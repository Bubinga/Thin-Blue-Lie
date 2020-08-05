$(document).ready(function () { //enables popover
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();
});

//If Date changes, run the getsimilar action in the Timeline Controller and load the partial view.
$(".SimFinder").change(function () {
    $('#VideoList').load('@Url.Action("GetSimilar","Timeline")', { TempDate: $("#DateInput").val() });
});


var attempt = 0;
$('#submit').on('click', function (e) {
    // submitForms = function () {  
    e.preventDefault();
    $.get('Submit/CheckSignedIn', function (data) {
        if (data == false && attempt == 0) {
            console.log("if ran");
            $('#submit').popover('show');
            attempt = 1;
        }
        else if (attempt == 1 || data == true) {
            console.log("else if ran");
            $('form').submit();
            // SubmitMedia();
            $('#submit').popover('hide');
        }
    });
    //  }
})
var SubjectCount = 1;
$("#MoreSubjects").click(function () {
    //$("form").validate().options.ignore = "*";
    $.ajax({
        url: '/Submit/MoreSubjects',
        method: 'get',
        data: { data: SubjectCount },
        success: function (result) {
            $("#SubjectList").append(result);
            SubjectCount++;
            $.validator.unobtrusive.parse($("#SubjectList").last());
            //$("form").validate().settings.ignore = ":hidden";

            //rerun event listeners               
            $(".close").on('click', function (e) {
                closeMedia(e, $(this).parents(".AddMedia, .AddSubject, .AddOfficer"));
            });
        }
    })
});

var OfficerCount = 1;
$("#MoreOfficers").click(function () {
    //$("form").validate().options.ignore = "*";
    $.ajax({
        url: '/Submit/MoreOfficers',
        method: 'get',
        data: { data: OfficerCount },
        success: function (result) {
            $("#OfficerList").append(result);
            SubjectCount++;
            $.validator.unobtrusive.parse($("#OfficerList").last());
            //$("form").validate().settings.ignore = ":hidden";

            //rerun event listeners               
            $(".close").on('click', function (e) {
                closeMedia(e, $(this).parents(".AddMedia, .AddSubject, .AddOfficer"));
            });
        }
    })
});


var MediaCount = 1;
$("#MoreMedia").click(function () {
    //$("form").validate().options.ignore = "*";
    $.ajax({
        url: '/Submit/MoreMedia',
        method: 'get',
        data: { data: MediaCount },
        success: function (result) {
            $("#MediaList").append(result);
            MediaCount++;
            $.validator.unobtrusive.parse($("#MediaList").last());
            //$("form").validate().settings.ignore = ":hidden";
            $("#MediaList").last().find('.MediaFile').css('display', 'none'); //Set the last item in the list's mediaFile input to hidden
            //rerun event listeners
            $('.AddMedia').on('change', 'select', function (e) {
                changeFile(e, $(this).parents(".AddMedia"));
            });
            $(".close").on('click', function (e) {
                closeMedia(e, $(this).parents(".AddMedia"));
            });
        }
    })
});

$(".close").on('click', function (e) {
    closeMedia(e, $(this).parents(".AddMedia, .AddSubject, .AddOfficer"));
});

function closeMedia(e, parent) {
    parent.remove();
}

$('.MediaFile').css('display', 'none');

$('.AddMedia').on('change', 'select', function (e) {
    changeFile(e, $(this).parents(".AddMedia"));
});

function changeFile(e, parent) {
    let el = e.target;
    var strUser = el.options[el.selectedIndex].text;
    //console.log(el, parent.find('.MediaFile'));
    if (strUser == "Phone or Computer") {
        parent.find('.MediaFile').css('display', 'inline-block');
        parent.find('.MediaLink').css('display', 'none');
    }
    else {
        parent.find('.MediaLink').css('display', 'inline-block');
        parent.find('.MediaFile').css('display', 'none');
    }
}



//magic function disables inspect element
//eval(function (p, a, c, k, e, d) { e = function (c) { return c.toString(36) }; if (!''.replace(/^/, String)) { while (c--) { d[c.toString(a)] = k[c] || c.toString(a) } k = [function (e) { return d[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) { if (k[c]) { p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]) } } return p }('(3(){(3 a(){8{(3 b(2){7((\'\'+(2/2)).6!==1||2%5===0){(3(){}).9(\'4\')()}c{4}b(++2)})(0)}d(e){g(a,f)}})()})();', 17, 17, '||i|function|debugger|20|length|if|try|constructor|||else|catch||5000|setTimeout'.split('|'), 0, {}))

document.getElementById("weaponDiv").style.display = "none";
var WeaponCheckState = 0;

//gets first box in misconduct and on click runs showWeapon
document.getElementById("1").onclick = function () { showWeapon(); };

function showWeapon() {
    if (WeaponCheckState === 0) {
        document.getElementById("weaponDiv").style.display = "block";
        WeaponCheckState = 1;
    }
    else {
        $('input[name=SelectedWeapons]')
            .val('')
            .prop('checked', false)
            .prop('selected', false);
        document.getElementById("weaponDiv").style.display = "none";
        WeaponCheckState = 0;
    }
}