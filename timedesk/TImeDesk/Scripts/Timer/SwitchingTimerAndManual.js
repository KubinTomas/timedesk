
/* Switching between manual mode and timer */

/* Properties set up */
var activeColor = "#4bc700";
var deactiveColor = "#747474";

//Switch to manual timer
$(document).on('click',
    '#Timer__switchToInputs',
    function (e) {
   
        if (!AutomaticTimer)
            return;

        changeTimetStatus();

        //Set text of description
        $("#TimeForm__timeEntryDescriptionId").attr('placeholder', 'What have you done?');

        //Change color status
        $(this).css("color", activeColor);
        $("#Timer__switchToTimer").css("color", deactiveColor);

    
        //Load partial with manual inputs
        $("#ManualAutomaticTimer").empty();
        $("#ManualAutomaticTimer").load(manualPartialViewUrl);

        location.reload();


    });

//Switch to automatic timer
$(document).on('click',
    '#Timer__switchToTimer',
    function (e) {

        if (AutomaticTimer)
            return;


        changeTimetStatus();

        //Set text of description
        $("#TimeForm__timeEntryDescriptionId").attr('placeholder', 'What are you working on?');
      
        //Change color status
        $(this).css("color", activeColor);
        $("#Timer__switchToInputs").css("color", deactiveColor);

        //Load partial with automatic inputs
        $("#ManualAutomaticTimer").empty(); 
        $("#ManualAutomaticTimer").load(automaticPartialViewUrl);
       // $('#Timer__OnOff').attr("src", playImageUrl);
    });

function changeTimetStatus() {
    $.ajax({
        url: changeTimerStatusUrl,
        type: "PUT",
        success: function () {
            AutomaticTimer = !AutomaticTimer;
        }
    });
}



$(document).ready(function() {
    setManAutTimerColors();
});

function setManAutTimerColors() {
    if (AutomaticTimer) {
        $("#Timer__switchToTimer").css("color", activeColor);
        $("#TimeForm__timeEntryDescriptionId").attr('placeholder', 'What are you working on?');
    } else {
        $("#Timer__switchToInputs").css("color", activeColor);
        $("#TimeForm__timeEntryDescriptionId").attr('placeholder', 'What have you done?');
    }
}
function IsTimerAutomatic() {
    return AutomaticTimer;
}