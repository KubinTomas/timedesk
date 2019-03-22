function removeOpenFolderClass() {
    if (!$("#Project__DropDown").hasClass('show'))
        $('#Project__OpenFolderBox').removeClass('Project__OpenedProject');
}
/* Filter project by name */
function filterProjectFunction() {
    var i;
    var filter = document.getElementById("PrFilterInput").value.toUpperCase();
    var dropDown = document.getElementById("Project__DropDown");
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

/* Closing project drop down */
var projectIsOpen = false;

$(document).click(function () {
    if ($("#Project__DropDown").hasClass('show') && projectIsOpen) {
        closeProjectSelect();
    }
    projectIsOpen = true;
    $('#PrFilterInput').val = "";

    removeOpenFolderClass();

});
/* Open project and add active class */
function projectDropDown() {

    var prDropDown = document.getElementById('Project__DropDown');

    prDropDown.classList.toggle("show");

    if ($("#Project__DropDown").hasClass('show') && !$('.Project__SelectedProject')[0]) {
        $('#Project__OpenFolderBox').addClass('Project__OpenedProject');
    } else {
        $('#Project__OpenFolderBox').removeClass('Project__OpenedProject');
    }



    /*  Reset content of input search and all values */
    $('#PrFilterInput').val("");
    filterProjectFunction();

    /* Prevent insta closing*/
    projectIsOpen = false;

    removeOpenFolderClass();
}
$('#Project__DropDown').click(function (e) {
    e.stopPropagation();
    removeOpenFolderClass();
});

$(document).on('click', '#Project__DropDown', function (e) {
    document.getElementById('Project__DropDown').classList.toggle("show");

    var projectId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) === "undefined") ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");
    console.log(projectId);
    if (projectId === '0')
        $('#Project__OpenFolderBox').addClass('Project__OpenedProject');
    else
        $('#Project__OpenFolderBox').removeClass('Project__OpenedProject');

    removeOpenFolderClass();
});


/* Clicking on projects */
$(document).on('click', '#Projects a', function () {

    var oldPrId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) == 'undefined') ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");
  

//    alert("Cur Id" + currentId + " OldPrId " + oldPrId);

    var project = this.innerHTML;
    

    if (this.id === '0') {
        $('#projectDropDownIcon').replaceWith('<span id ="projectDropDownIcon"><i class="fa fa-folder-open" id="Timer__selectProject"></i></span>');

        //ToDo : RemoveOpen folder -> not working
    } else {
        $('#projectDropDownIcon').replaceWith('<span class ="Project__SelectedProject" id ="projectDropDownIcon"><i id="Timer__selectProject"> ' + project + '  </i></span>');
    }
    document.getElementById('Project__DropDown').classList.toggle("show");

    var currentId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) == 'undefined') ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");

    var res = (parseInt(oldPrId) !== parseInt(currentId));
   
    //Remove selected task
    
 
    checkIfProjectChanged(res);

    somethingHasChanged();
    removeOpenFolderClass();




    var projectId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) == 'undefined') ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");
    fillTaskData(projectId);

});


$('#Projects').on("click", "a", function (event) {

    var oldPrId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) == 'undefined') ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");
    var project = this.innerHTML;

    if (this.id === '0') {
        $('#projectDropDownIcon').replaceWith('<span id ="projectDropDownIcon" onclick=""><i class="fa fa-folder-open" id="Timer__selectProject"></i></span>');

    } else {
        $('#projectDropDownIcon').replaceWith('<span class ="Project__SelectedProject" id ="projectDropDownIcon"><i id="Timer__selectProject"> ' + project + '  </i></span>');
    }

    //Remove selected task
    // $('#taskDropDownIcon').replaceWith('<span id ="taskDropDownIcon"><i class="fa fa-tasks" id="Timer__selectTask"></i></span>');

    var isSame;

    var currentId = $(this).children("span").attr("data-id");
    var res = (parseInt(oldPrId) !== parseInt(currentId));
    
    //alert();
    checkIfProjectChanged(res);


    somethingHasChanged();
    closeProjectSelect();
    removeOpenFolderClass();

    var projectId = (typeof ($('#projectDropDownIcon .Project__Name').attr("data-id")) == 'undefined') ? "0" : $('#projectDropDownIcon .Project__Name').attr("data-id");
    fillTaskData(projectId);

});

function closeProjectSelect() {

    if ($("#Project__DropDown").hasClass('show') && projectIsOpen) {
        document.getElementById('Project__DropDown').classList.toggle("show");
        $('#Project__OpenFolderBox').removeClass('Project__OpenedProject');
    }
    removeOpenFolderClass();
}


setInterval(function () {

}, 5000);
