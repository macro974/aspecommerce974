$('.slider').slick({
    slidesToShow: 2,
    slidesToScroll: 2,
    autoplay: true,
    autoplaySpeed: 2500,
    adaptiveHeight:true
});
$('.paginator').slick({
    slidesToShow: 15,
    slidesToScroll: 1,
    autoplay: false,
});
$(".full-product").click(function () {
    $(this).effect("highlight", 100);
})


$(".full-product").mouseover(function () {
    $(this).css({ "border-left": "5px orange solid" });
})
$(".full-product").mouseout(function () {
    $(this).css({ "border-left": "5px whitesmoke solid" });
})