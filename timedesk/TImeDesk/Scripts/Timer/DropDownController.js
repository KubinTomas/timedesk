
/* Filter project by name */    
function filterProjectFunction() {
    var i;
    //  var filter = document.getElementById("PrFilterInput").value.toUpperCase();
    var filter = dropDownContent.find(".Project__filterContainer").find(".FilterInput").get(0).value.toUpperCase();

    //  var dropDown = document.getElementById("Project__DropDown");
    var dropDown = dropDownContent.get(0);

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


//Store all dropdowns
var dropDownContent;
var curDropDown;
var replacePlace;

var isDropOpen = false;

// Dynamicly select drop downs
$('#TimerWrapper').on("click", ".CustomDropDown", function (e) {
  
    // Close all opened dropDowns
    if (!$(this).find(".Project__DropDownContent").hasClass('show'))
        closeOpenedDropDown();

    // Dynamic find
    var dropDown = $(this).find(".Project__DropDownContent");

    

    dropDownContent = dropDown;
    curDropDown = $(this);

    //Open or close drop down
    if (dropDown.hasClass('show')) {
        isDropOpen = false;
        dropDownContent.removeClass('show');
    }else{
        isDropOpen = true;
        dropDown.addClass('show');
        }
       


    //Prevent closing dropdown, when clicking inside
    dropDownContent.click(function (e) {
        e.stopPropagation();
        // removeOpenFolderClassTask();
    });

    //Remove events, prevent stacking dropdowns
    curDropDown.find(".Project__projectsContainer").off();


    //Manage clicking on tags
    curDropDown.find(".Project__projectsContainer").on("click",
        ".ItemTag",
        function(e) {
            var tag = $(this);
            var tagId = tag.attr("data-id");

            var tagParent = tag.parent();

            var entryId = tag.closest(".TimeEntry__ItemWrapper").attr("data-id");

            var removeTag;

            //Sotre container for added tags and already available tags
            var container = tagParent.parent();

            //Check if tag should be add or removed

            //Remove tag
            if (tagParent.hasClass("SelectedTagsContainer")) {

                var newTagPlace = container.find(".AvailableTagsContainer");

                removeTag = true;

                tag.remove();
                newTagPlace.append(tag);

            } else { //Add tag

                var newTagPlace = container.find(".SelectedTagsContainer");

                removeTag = false;

                tag.remove();
                newTagPlace.append(tag);
            }

            //If Entry exist or is running, then add or remove tag 
            if (entryId != 0) {

                if (removeTag) {
                    $.ajax({
                        url: removeTagFromTimeEntryUrl,
                        type: "POST",
                        data: { entryId: entryId, tagId: tagId },
                        success: function () {

                        }
                    });
                } else {
                    $.ajax({
                        url: addTagToTimeEntryUrl,
                        type: "POST",
                        data: { entryId: entryId, tagId: tagId },
                        success: function () {

                        }
                    });
                }
                
            }
            
        });

    //Select project on click
     curDropDown.find(".Project__projectsContainer").on("click", "a", function (event) {


        //Remove all selected items
        $(this).parent().parent().find('.Project__itemContainer').removeClass("SelectedItemInDropDown");

        //Select new item
        $(this).addClass("SelectedItemInDropDown");


        var project = this.innerHTML;
        var isDefault = this.id === '0';


        

        // If we choose project
        if ($(this).find("span").hasClass("Project")) {
            var onPlace = curDropDown.parent().find(".TaskMainContainer");

            var oldPrId = curDropDown.parent().find(".placeToReplace").find(".Project").attr("data-id");

            var newPrId;

            if ($(this).find("span").attr("data-id") === "0") {
                curDropDown.find(".placeToReplace").replaceWith("<span class = 'placeToReplace' id='projectDropDownIcon' > <span> <i class='fa fa-folder-open' id ='Timer__selectProject'  ></span></i></span>");
                $(this).removeClass("SelectedItemInDropDown");
            } else {
                curDropDown.find(".placeToReplace").replaceWith('<span class ="Task__SelectedTask placeToReplace" id ="projectDropDownIcon"><i id="Timer__selectProject"> ' + project + '  </i></span>');
            }

            // Nevybere nove ID
            //  newPrId = (typeof (curDropDown.find(".placeToReplace .Project__Name").attr("data-id")) == 'undefined') ? "0" : curDropDown.find(".placeToReplace .Project__Name").attr("data-id");
            newPrId = curDropDown.parent().find(".placeToReplace").find(".Project").attr("data-id");


            var res = parseInt(newPrId) != parseInt(oldPrId);


            var projectId = (typeof (curDropDown.find(".placeToReplace .Project__Name").attr("data-id")) == 'undefined') ? "0" : curDropDown.find(".placeToReplace .Project__Name").attr("data-id");
            var elWhere = curDropDown.parent().find(".TasksContainer");



            checkIfProjectChanged(res, onPlace);


            fillTaskData(projectId, elWhere);

        } else if ($(this).find("span").hasClass("Task")) {

            //If we clicked on TASK
            if ($(this).find("span").attr("data-id") === "0") {
                curDropDown.find(".placeToReplace").replaceWith("<span class = 'placeToReplace' id='projectDropDownIcon' > <span> <i class='fa fa-tasks' id ='Timer__selectProject'  ></span></i></span>");
                $(this).removeClass("SelectedItemInDropDown");
            } else {
                curDropDown.find(".placeToReplace").replaceWith('<span class ="Task__SelectedTask placeToReplace" id ="projectDropDownIcon"><i id="Timer__selectProject"> ' + project + '  </i></span>');
            }
        } else if ($(this).find("span").hasClass("Billing")) {

            if ($(this).find("span").attr("data-id") === "0") {
                curDropDown.find(".placeToReplace").replaceWith("<span class = 'placeToReplace' id='projectDropDownIcon' > <span> <i class='fa fa-dollar' id ='Timer__selectProject'  ></span></i></span>");
                $(this).removeClass("SelectedItemInDropDown");
            } else {
                curDropDown.find(".placeToReplace").replaceWith('<span class ="Task__SelectedTask placeToReplace" id ="projectDropDownIcon"><i id="Timer__selectProject"> ' + project + '  </i></span>');
            }
        }else {

            alert("Error");
        }


        //Check for already written time entries and update it
        if (typeof curDropDown.parent().attr('data-id') !== 'undefined') {
            updateTimeEntry(curDropDown);
        }

        somethingHasChanged();
        closeOpenedDropDown();
    });



    /*  Reset content of input search and all values */
    $(this).find(".FilterInput").val("");

    filterProjectFunction();
   
});
function closeOpenedDropDown() {
    if (dropDownContent != null && curDropDown != null) {
        if (dropDownContent != null && isDropOpen) {
            dropDownContent.removeClass('show');
            isDropOpen = false;
        }
            

        if (dropDownContent.hasClass('show')) { 
            curDropDown.addClass('Project__OpenedProject');
        } else {
            curDropDown.removeClass('Project__OpenedProject');
        }
    }
}
$(document).click(function (e) {

    var isItag = $(e.target).is('i');
    
    //Closing dropdown if user click somewhere else expect dropdown
    if (!$(e.target).hasClass('CustomDropDown') && !isItag && !$(e.target).hasClass('Project__ColorHolder') && !$(e.target).hasClass('Project__Name') && !$(e.target).hasClass('placeToReplace') && !$(e.target).hasClass('Project__OpenedProject')) {
        if (dropDownContent != null && isDropOpen) {
            dropDownContent.removeClass('show');
            isDropOpen = false;
        }
    }

    /*Finding element with tag placeToReplace and checking if next tag is I / Span 
    I is for selected project, etc.
    Span is for empty icon (no project, etc.)
    */
    if (dropDownContent != null)
    var path = dropDownContent.prev().children().children();

    if (curDropDown != null) {
        if (dropDownContent.hasClass('show') && path.is("span")) { //
            curDropDown.addClass('Project__OpenedProject');          
        } else {
            curDropDown.removeClass('Project__OpenedProject');
        }
    }
});
// Filling task drop down with data
function fillTaskData(id, place) {
    $.ajax({
        type: "GET",
        cache: false,
        data: { prId: id },
        dataType: 'json',
        url: '/Timer/GetTasksInProjectJsonModel',
        success: function (data) {
            if (place != null) {
                place.empty();

                place.append(' <div class="Project__ItemWrapper"><a class="Project__itemContainer"><div class="Project__ColorHolder" style="background-color:transparent;"></div><span class="Project__Name Task" data-id="0">No Task</span></a></div>');

                for (obj in data) {
                    place.append(" <div class='Project__ItemWrapper'><a class='Project__itemContainer'><div class='Project__ColorHolder' style='background-color:" + data[obj].ColorString + "; '></div><span class='Project__Name Task' data-id='" + data[obj].Id + "' style='color:" + data[obj].ColorString + "; '>" + data[obj].Name + "</span></a></div>");
                }
            }


        }
    });
}
