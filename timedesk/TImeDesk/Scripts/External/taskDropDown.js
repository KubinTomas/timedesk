function removeOpenFolderClassTask() {
    if (!$("#Task__DropDown").hasClass('show'))
        $('#Task__OpenFolderBox').removeClass('Project__OpenedProject');
}

/* Closing project drop down */
var taskIsOpen = false;



$(document).click(function () {
    if ($("#Task__DropDown").hasClass('show') && taskIsOpen) {
        closeTaskSelect();
    }
    taskIsOpen = true;
    $('#TsFilterInput').val = "";

    removeOpenFolderClassTask();

});


/* Open project and add active class */
$('#TimeEntryPartial').on("click", "#Task__OpenFolderBox", function () {


   
    var prDropDown = document.getElementById('Task__DropDown');
   
    prDropDown.classList.toggle("show");
  
    // Nefunguje -> task sel is undefined
    if ($("#Task__DropDown").hasClass('show') && !$('.Task__SelectedTask')[0]) {
        $('#Task__OpenFolderBox').addClass('Project__OpenedProject');
    } else {
        $('#Task__OpenFolderBox').removeClass('Project__OpenedProject');
    }



    /*  Reset content of input search and all values */
    $('#TsFilterInput').val("");
    filterProjectFunction();

    /* Prevent insta closing*/
    taskIsOpen = false;

    removeOpenFolderClassTask();
});



$('#Task__DropDown').click(function (e) {
    e.stopPropagation();
    removeOpenFolderClassTask();
});

$(document).on('click', '#Task__DropDown', function (e) {

  
   
    document.getElementById('Task__DropDown').classList.toggle("show");

    var taskId = (typeof ($('#taskDropDownIcon .Project__Name').attr("data-id")) === "undefined") ? "0" : $('#taskDropDownIcon .Project__Name').attr("data-id");
  
    if (taskId === '0')
        $('#Task__OpenFolderBox').addClass('Project__OpenedProject');
    else
        $('#Task__OpenFolderBox').removeClass('Project__OpenedProject');

    removeOpenFolderClass();
});


/* Clicking on tasks */
$(document).on('click', '#Tasks a', function () {
    var project = this.innerHTML;

    if (this.id === '0') {
        $('#taskDropDownIcon').replaceWith('<span id ="taskDropDownIcon"><i class="fa fa-tasks" id="Timer__selectTask"></i></span>');

        //ToDo : RemoveOpen folder -> not working
    } else {
        $('#taskDropDownIcon').replaceWith('<span class ="Task__SelectedTask" id ="taskDropDownIcon"><i id="Timer__selectTask"> ' + project + '  </i></span>');
    }
    document.getElementById('Task__DropDown').classList.toggle("show");

    somethingHasChanged();
    removeOpenFolderClassTask();

});
$('#Tasks').on("click", "a", function (event) {

    var project = this.innerHTML;

    if (this.id === '0') {
        $('#taskDropDownIcon').replaceWith('<span id ="taskDropDownIcon"><i class="fa fa-tasks" id="Timer__selectTask"></i></span>');

    } else {
        $('#taskDropDownIcon').replaceWith('<span class ="Task__SelectedTask" id ="taskDropDownIcon"><i id="Timer__selectTask"> ' + project + '  </i></span>');
    }

    somethingHasChanged();
    closeTaskSelect();
    removeOpenFolderClassTask();
});

function closeTaskSelect() {

    if ($("#Task__DropDown").hasClass('show') && taskIsOpen) {
        document.getElementById('Task__DropDown').classList.toggle("show");
        $('#Task__OpenFolderBox').removeClass('Project__OpenedProject');
    }
    removeOpenFolderClassTask();
}
/* Filter task by name */
function filterProjectFunction() {
    var i;
    var filter = document.getElementById("TsFilterInput").value.toUpperCase();
    var dropDown = document.getElementById("Task__DropDown");
    var a = dropDown.getElementsByTagName("a");
    for (i = 0; i < a.length; i++) {

        if (a[i].id !== 0) {
            if (a[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
                $(a[i]).parent().css("display", "");
            } else {
                $(a[i]).parent().css("display", "none");
            }
        }

    }
}