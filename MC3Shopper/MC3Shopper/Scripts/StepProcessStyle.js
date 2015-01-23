function StepOne() {
    $(".progress-bar").animate({
        width: "0%"
    });

    $("#part1").addClass("active");

    $("#part2").addClass("disabled");
    $("#part3").addClass("disabled");

    $("#part2").removeClass("active");
    $("#part3").removeClass("active");

    $("#part2").removeClass("complete");
    $("#part3").removeClass("complete");

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

    $(".step-1").css({ "font-size": "1.5em", "color": "black" });
    $(".step-2").css({ "font-size": "1em", "color": "slategrey" });
    $(".step-3").css({ "font-size": "1em", "color": "slategrey" });

    $("#slide-step2").hide();
    $("#slide-step3").hide();
    $("#slide-step1").fadeIn();
    
}
function StepTwo() {

    $(".progress-bar").animate({
        width: "50%"
    });
    $("#part1").removeClass("active");
    $("#part1").addClass("complete");

    $("#part2").removeClass("disabled");
    $("#part3").removeClass("active");

    $("#part2").addClass("active");
    $("#part3").addClass("disabled");

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

    $(".step-1").css({ "font-size": "1em", "color": "slategrey" });
    $(".step-2").css({ "font-size": "1.5em", "color": "black" });
    $(".step-3").css({ "font-size": "1em", "color": "slategrey" });

    $("#slide-step1").hide();    
    $("#slide-step3").hide();
    $("#slide-step2").fadeIn();
    
}
function StepThree() {
    $(".progress-bar").animate({
        width: "100%"
    });
    $("#part3").addClass("active");

    $("#part2").addClass("complete");
    $("#part1").addClass("complete");
    
    $("#part3").removeClass("disabled");
    $("#part2").removeClass("disabled");
    $("#part2").removeClass("active");
    $("#part1").removeClass("active");

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

    $(".step-1").css({ "font-size": "1em", "color": "slategrey" });
    $(".step-2").css({ "font-size": "1em", "color": "slategrey" });
    $(".step-3").css({ "font-size": "1.5em", "color": "black" });

    $("#slide-step1").hide();
    $("#slide-step2").hide();
    $("#slide-step3").fadeIn();

}
$("#datepicker").datepicker({
    altField: "#datepicker",
    closeText: 'Fermer',
    prevText: 'Précédent',
    nextText: 'Suivant',
    currentText: 'Aujourd\'hui',
    monthNames: ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'],
    monthNamesShort: ['Janv.', 'Févr.', 'Mars', 'Avril', 'Mai', 'Juin', 'Juil.', 'Août', 'Sept.', 'Oct.', 'Nov.', 'Déc.'],
    dayNames: ['Dimanche', 'Lundi', 'Mardi', 'Mercredi', 'Jeudi', 'Vendredi', 'Samedi'],
    dayNamesShort: ['Dim.', 'Lun.', 'Mar.', 'Mer.', 'Jeu.', 'Ven.', 'Sam.'],
    dayNamesMin: ['D', 'L', 'M', 'M', 'J', 'V', 'S'],
    weekHeader: 'Sem.',
    dateFormat: 'yy-mm-dd'
});
