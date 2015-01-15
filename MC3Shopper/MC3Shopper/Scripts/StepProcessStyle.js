function StepOne() {
    $("#part1").addClass("active");
    $("#part2").addClass("disabled");
    $("#part3").addClass("disabled");
    $("#part1").css({
        "-moz-box-shadow": "0px 12px 10px -10px #656565",
        "-webkit-box-shadow": "0px 12px 10px -10px #656565",
        "-o-box-shadow": "0px 12px 10px -10px #656565",
        "box-shadow": "0px 12px 10px -10px #656565",
        "filter": "progid:DXImageTransform.Microsoft.Shadow(color=#656565, Direction=180, Strength=10)"
    });
    $("#part2").css({
        "-moz-box-shadow": "",
        "-webkit-box-shadow": "",
        "-o-box-shadow": "",
        "box-shadow": ""
    });
    $("#part3").css({
        "-moz-box-shadow": "",
        "-webkit-box-shadow": "",
        "-o-box-shadow": "",
        "box-shadow": ""
    });
}
function StepTwo() {
    $("#part1").removeClass("active");
    $("#part1").addClass("complete");
    $("#part2").removeClass("disabled");
    $("#part2").addClass("active");
    $("#part1").css({
        "-moz-box-shadow": "",
        "-webkit-box-shadow": "",
        "-o-box-shadow": "",
        "box-shadow": ""
    });
    $("#part2").css({
        "-moz-box-shadow": "0px 12px 10px -10px #656565",
        "-webkit-box-shadow": "0px 12px 10px -10px #656565",
        "-o-box-shadow": "0px 12px 10px -10px #656565",
        "box-shadow": "0px 12px 10px -10px #656565",
        "filter": "progid:DXImageTransform.Microsoft.Shadow(color=#656565, Direction=180, Strength=10)"
    });
    $("#part3").css({
        "-moz-box-shadow": "",
        "-webkit-box-shadow": "",
        "-o-box-shadow": "",
        "box-shadow": ""
    });
}
function StepThree() {
    $("#part2").removeClass("active");
    $("#part2").addClass("complete");
    $("#part3").removeClass("disabled");
    $("#part3").addClass("active");
    $("#part1").css({
        "-moz-box-shadow": "",
        "-webkit-box-shadow": "",
        "-o-box-shadow": "",
        "box-shadow": ""
    });
    $("#part2").css({
        "-moz-box-shadow": "",
        "-webkit-box-shadow": "",
        "-o-box-shadow": "",
        "box-shadow": ""
    });
    $("#part3").css({
        "-moz-box-shadow": "0px 12px 10px -10px #656565",
        "-webkit-box-shadow": "0px 12px 10px -10px #656565",
        "-o-box-shadow": "0px 12px 10px -10px #656565",
        "box-shadow": "0px 12px 10px -10px #656565",
        "filter": "progid:DXImageTransform.Microsoft.Shadow(color=#656565, Direction=180, Strength=10)"
    });
}