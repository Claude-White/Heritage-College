let carouselElement = document.getElementById("carousel");

function scrollCarousel(targetImageNumber) {
    let carouselWidth = carouselElement.clientWidth;
    let targetImage = targetImageNumber - 1;
    let targetXPixel = (carouselWidth * targetImage) + 1
    carouselElement.scrollTo(targetXPixel, 0);
}

/**
 * For the Release Notes page: will put styling on the list items (list-disc)
 */
let releaseNoteContent = document.querySelectorAll(".releaseNotesContent");

if (releaseNoteContent != null) {
    let uls = document.querySelectorAll(".releaseNotesContent ul");
    uls.forEach(ul => {
        ul.style.listStyleType = 'disc';
        ul.style.marginLeft = '5%';
    })
}

let originalArticles = $("#Articles .articleElement");
$("#FilterForm").on('change', '#FilterSelect', function () {
    let filterBy = $("#FilterSelect").find(":selected").val();
    if (filterBy != "default") {
        originalArticles.filter(function () {
            $(this).toggle($(this).data('category') == filterBy)
        });
    } else {
        originalArticles.show()
    }
});

$(document).ready(function () {
    var desiredHeight = $("body").outerHeight() - $("header").outerHeight() - $("footer").outerHeight();
    $("#ReleaseNotesSection").css("min-height", desiredHeight);
    if ($("#ContactUsForm").length == 0)
        $("#FAQ").css("min-height", desiredHeight);
});