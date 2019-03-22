/* Delete note */
$(document).on("click", ".DeleteNote", function (e) {
    DeleteNote($(this).attr("data-id"), $(this).parent().parent().parent().parent().parent());
});

function DeleteNote(delId, note) {

    $.ajax({
        url: deleteNoteUrl,
        data: "id=" + delId,
        method: "POST",
        success: function () {
            //Remove clicked note
            note.remove();
        }
    });
}
/* Create note */
$(document).on("click",
    "#AddNoteArea",
    function (e) {

        console.log($(this));

        var privateNote = isNotePrivate($(this).attr("note-type"));

        console.log(privateNote);

        AddNote(privateNote, $(this));
    });

function isNotePrivate(type) {

    if (type === "private")
        return true;

    return false;

}

function AddNote(privateNote, before) {

    $.ajax({
        type: "GET",
        url: noteUrl,
        data: { privateNote: privateNote },
        success: function(res) {
            before.before(res);
        }
    });
}
/* Update note */
function UpdateNote(e) {

    var note = $(e).closest('div[class^="ToDoList__StickNoteWrapper"]');

    var headerText = note.find(".ToDoList__HeaderTitle").val();
    var bodyContentText = note.find(".ToDoList__TextArea").val();
    var noteId = note.find(".DeleteNote").attr("data-id");

    $.ajax({
        type: "POST",
        url: updateNoteUrl,
        data: { id: noteId, header: headerText, content: bodyContentText },
        success: function () {
        }

    });
}
/* Call color cover to choose color */
$(document).on("click", ".ToggleColorChangeDiv", function (e) {   

    var parent = $(this).parent().parent().parent().parent().parent();
    var cover = parent.find(".ToDoList__ChangeColorWrapper");

    //if (glCover != null && cover == glCover)
    //    return;

      colorChangeCover(cover);
});
var glCover = null;
function colorChangeCover(cover) {

    if (glCover != null)
        closeOpenedCover();


    glCover = cover;

    cover.animate({
        height: '+=80px'
    });
}

$(document).click(function (e) {
    if (!$(e.target).hasClass('ToggleColorChangeDiv') && glCover != null)
        closeOpenedCover();

});

function closeOpenedCover() {

    glCover.animate({
        height: '-=80px'
    });

    glCover = null;
}

/* Select color */
$(document).on("click", ".ToDoList__ColorWrapper", function () {
    changeColor($(this));
});
function changeColor(colorDiv) {

    var note = colorDiv.parent().parent();

    var colorId = colorDiv.attr("data-id");
    var noteId = note.find(".DeleteNote").attr("data-id");


    $.ajax({
        type: "GET",
        url: changeNoteColorUrl,
        data: { colId: colorId, noteId: noteId },
        success: function (res) {
            note.find(".ToDoList__StickNoteHeader").css("background-color", res["Header"]);
            note.find(".ToDoList__StickNoteBody").css("background-color", res["Body"]);
            note.find(".ToDoList__ChangeColorWrapper").css("background-color", res["Header"]);

        }

    });
}
