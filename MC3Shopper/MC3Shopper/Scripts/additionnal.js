$('.slider').slick({
    slidesToShow: 2,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 5500,
    adaptiveHeight: true,
    accessibility: true,
    arrows: true,
    dots: true,

    slide: 'div',
    cssEase: 'linear'
});
$('.paginator').slick({
    slidesToShow: 15,
    slidesToScroll: 1,
    autoplay: false,
});

function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('/');
    if(sParam>0 && sURLVariables.length>0)
    {
        return sURLVariables[sParam];
    }
};

$(".full-product").click(function() {
    $(this).effect("highlight", 100);
});
$(".full-product").mouseover(function() {
    $(this).css({ "border-left": "5px orange solid" });
});
$(".full-product").mouseout(function() {
    $(this).css({ "border-left": "5px whitesmoke solid" });
});

$(".search-form").mouseover(function () {
    $("#search-icone").css({ "color": "#ff7d00" });
});
$(".search-form").mouseout(function () {
    $("#search-icone").css({ "color": "#fff" });
});
$(function () {
    // Initialize numeric spinner input boxes
    //$(".numeric-spinner").spinedit();
    // Initialize modal dialog
    // attach modal-container bootstrap attributes to links with .modal-link class.
    // when a link is clicked with these attributes, bootstrap will display the href content in a modal dialog.
    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });
    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });
    //clear modal cache, so that new content can be loaded
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
    $('#CancelModal').on('click', function () {
        return false;
    });
});