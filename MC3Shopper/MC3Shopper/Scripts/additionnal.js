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

