


//Delete running entry
$(document).on('click',
    '#Timer__deleteRunningEntry',
    function () {
        $.ajax({
            type: "DELETE",
            processData: false,
            cache: false,
            url: deleteRunningEntryUrl,
            success: function () {

                $('#TimeEntryPartial').load(timerPartialViewUrl,
                    function () {
                        //Loading color to active timer mode
                        setManAutTimerColors();
                    });
            }
        });
    });



//Turn On/Off timer
function SwitchTimerStatus() {

    var timRef = $('#Timer__OnOff');

    var model;

    if ($('#Timer__OnOff').attr("value") == "on") {

        timRef.attr('src', playImageUrl);
        model = getTimerData(true);

    } else {

        timRef.attr('src', stopImageUrl);

        model = getTimerData(false);



    }
    // Pass data to controller
    sendTimerData(model);
}

/*
Sending data to controler
Depends on bool -> save or edit entry
*/
function sendTimerData(model) {

    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: sendTimeDataUrl,
        contentType: 'application/json',
        success: function () {

            $('#TimeEntryPartial').load(timerPartialViewUrl,
                function () {

                    //Loading color to active timer mode
                    setManAutTimerColors();

                    var projectId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) == 'undefined') ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");
                    var elWhere = $("#Task");

                    //If is timer started and project selected fill tasks
                    fillTaskData(projectId, elWhere);
                });

            //Reload all TimeEntries when add a new time entry
            if (model['saveTimeEntry'] === true) {
                $('#TimeEntriesPartial').load(timeEntriesPartialViewUrl);
            }
        }
    });
}


// If project changed, replace task with default folder
function checkIfProjectChanged(change, onPlace) {
    if (change)
        onPlace.find(".placeToReplace").replaceWith("<span class = 'placeToReplace' id='projectDropDownIcon' > <span> <i class='fa fa-tasks' id ='Timer__selectProject'></span></i></span>");

}

/*Return data to save in DB when timer is started
  If timer is started, saveTimeEntry is false -> Create db row with bool as not saved
  IF timer is stoped, saveTimeEtry is true -> Update existing db row with bool not saved on false to true and update data
*/
function getTimerData(saveTimeEntry) {

    var timerDescription = $('#TimeForm__timeEntryDescriptionId').val();
    var projectId = $('#TimeEntryPartial').find(".placeToReplace").find(".Project").attr("data-id");
    var taskId = $('#TimeEntryPartial').find(".placeToReplace").find(".Task").attr("data-id");
    var billingId = $('#TimeEntryPartial').find(".placeToReplace").find(".Billing").attr("data-id");

    var d = new Date();

    // Data
    var model = { 'SpendedTime': "", 'StartedDate': d, 'EndedDate': d, 'Description': timerDescription, 'IsFinished': false, 'saveTimeEntry': saveTimeEntry, 'ProjectId': projectId, 'ProjectTaskId': taskId, 'BillingsId': billingId };

    //Add list of tags if entry is started
    if (!saveTimeEntry) {
        var selectedTagsParent = $("#SelectedTagIdContainer");

        var data = [];

        selectedTagsParent.find("div").each(function () {
            var id = $(this).attr("data-id");

            data.push(id);

        });

        console.log(data);

        model["tagsId"] = data;
    }


    return model;
}
/* If user change project, task, billing this method is call and gather all data.
   Pass  it to the controller and edit actual time entry
*/
function somethingHasChanged() {
    var timerDescription = $('#TimeForm__timeEntryDescriptionId').val();
    var projectId = $('#TimeEntryPartial').find(".placeToReplace").find(".Project").attr("data-id");
    var taskId = $('#TimeEntryPartial').find(".placeToReplace").find(".Task").attr("data-id");
    var billingId = $('#TimeEntryPartial').find(".placeToReplace").find(".Billing").attr("data-id");



    var d = new Date();

    // Data
    var model = { 'SpendedTime': "", 'StartedDate': d, 'EndedDate': d, 'Description': timerDescription, 'IsFinished': false, 'saveTimeEntry': false, 'ProjectId': projectId, 'ProjectTaskId': taskId, 'BillingsId': billingId };

    //If timer is turned On
    if ($('#Timer__OnOff').attr("value") == "on")
        sendTimerData(model);

}

// Toggle all time entries for the day
$(document).on('click',
    '#TimeEntry__ProjectNumber',
    function (event) {
        var parentDiv = $(this).parent().parent();

        $(this).toggleClass('TimeEntry__ProjectCloseNumber');

        parentDiv.find('.TimeEntry__Item').toggleClass('TimeEntry__HideTimeEntry');
    });

//Update TimeEntry if user change something
function updateTimeEntry(dropDownRef) {

    var curParent = dropDownRef.parent().find(".placeToReplace");
    var description = dropDownRef.parent().find(".TimeEntry__InputDescription").val();
    var timeEntryId = dropDownRef.parent().attr('data-id');
    var projectId = curParent.find(".Project").attr("data-id");
    var taskId = curParent.find(".Task").attr("data-id");
    var billingId = curParent.find(".Billing").attr("data-id");

    var model = { 'Id': timeEntryId, 'Description': description, 'ProjectId': projectId, 'ProjectTaskId': taskId, 'BillingsId': billingId };


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: updateEntryUrl,
        contentType: 'application/json',
        success: function () {
            // Data are refreshed
            dropDownRefGlobal = null;
        }

    });

}
/* Repeat already finished time entry */
$(document).on("click",
    ".RepeatTimeEntry",
    function (e) {
        RepeatTimeEntry($(this).closest(".TimeEntry__Item"));
    });
function RepeatTimeEntry(parent) {
  
    var entryDescrition = parent.find(".TimeEntry__InputDescription").val();
    var projectId = parent.find(".ProjectHolder").find(".Project__Name").attr("data-id"); 
    var taskId = parent.find(".TaskHolder").find(".Project__Name").attr("data-id"); 
    var billingsId = parent.find(".BillingsHolder").find(".Project__Name").attr("data-id"); 

    var d = new Date();

    if (projectId == 0)
        projectId = null;
    if (taskId == 0)
        taskId = null;
    if (billingsId == 0)
        billingsId = null;
     //Data
    var model = { 'SpendedTime': "", 'StartedDate': d, 'EndedDate': d, 'Description': entryDescrition, 'IsFinished': false, 'saveTimeEntry': false, 'ProjectId': projectId, 'ProjectTaskId': taskId, 'BillingsId': billingsId };

   // Add tags
    var selectedTagsParent = parent.find(".SelectedTagsContainer");

    var data = [];

    selectedTagsParent.find("div").each(function () {
        var id = $(this).attr("data-id");

        data.push(id);

    });

   


    model["tagsId"] = data;


    // Pass data to controller
    sendTimerData(model);
}

