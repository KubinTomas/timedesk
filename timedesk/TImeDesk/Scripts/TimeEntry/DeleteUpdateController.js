$(document).ready(function () {

    // Delete time entry on click
    $('#TimeEntriesPartial').on("click", ".js-delete", function () {
        var button = $(this);
       
        var entry = button.parent().parent();
        var headerContainer = entry.parent().children(":first");

        //console.log(entry.children().querySelector("TimeEntry__TimeHolder"));
        //entry.css({
        //        "border-color": "#C1E0FF",
        //        "border-weight": "1px",
        //        "border-style": "solid"
        //    });

        // ToDo získat efektivně children
        button.parent().prev().css({
            "border-color": "#C1E0FF",
            "border-weight": "1px",
            "border-style": "solid"
        });

    //    console.log();

        //Time entry time
        var entryTime = button.parent().prev().html();
        var y = entryTime.split(':');

        var entrySeconds = (+y[0]) * 60 * 60 + (y[1]) * 60 + (y[2]);

        entrySeconds = entryTime.split(':').reverse().reduce((prev, curr, i) => prev + curr * Math.pow(60, i), 0);
        

        var numberRef = headerContainer.children().eq(1).children();
        var timeRef = headerContainer.children().eq(2);
      

        //Total time
        var hms = timeRef.html();
        var x = hms.split(':');
        var totalSeconds = (+x[0]) * 60 * 60 + (x[1]) * 60 + (x[2]);

        totalSeconds = hms.split(':').reverse().reduce((prev, curr, i) => prev + curr * Math.pow(60, i), 0);

        var restMilisecond = totalSeconds - entrySeconds;

        //Convert time
        var convertTime = function (input, separator) {
            var pad = function (input) { return input < 10 ? "0" + input : input; };
            return [
                pad(Math.floor(input / 3600)),
                pad(Math.floor(input % 3600 / 60)),
                pad(Math.floor(input % 60)),
            ].join(typeof separator !== 'undefined' ? separator : ':');
        }

        var time = convertTime(restMilisecond, ':');
      
        //console.log(number);
        //alert(number);

        $.ajax({
            url: deleteEntryUrl,
            data: "id=" + button.attr("data-customer-id"),
            method: "POST",
            success: function () {
                //Remove clicked entry
                entry.remove();

                //Lowering entries count
                var oldValue = numberRef.html();

                oldValue--;

                if (oldValue <= 0) {
                    //If there are no enties to hold, remove header
                    headerContainer.remove();
                } else {
                    //Lower value
                    numberRef.html(oldValue);
                    timeRef.html(time);
                }

              
            }
        });

    });

});

//Update time entry if user change entry descprition
function descriptionUpdate(e) {

    var dropDownRef = $(e).parent().parent().parent().find(".CustomDropDown");

    dropDownRefGlobal = dropDownRef;

    updateTimeEntry(dropDownRef);
}