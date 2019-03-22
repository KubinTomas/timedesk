//Workspace body -> Render users, projects, settings, ... 
var body = $("#ContentArea");

// Show settings partial view
$(document).on("click",
    ".CanManageSettings",
    function(e) {
        GetWorkspaceSettings();
    });

function GetWorkspaceSettings() {
    $.ajax({
        url: WorkspaceSettingsPartialViewUrl,
        type: "GET",
        data: { workspaceId: getCurrentWorkspaceId() },
        success: function(res) {
            body.html(res);
        }
    });
}

// Dissolve (cancle/delete) workspace

$(document).on("click",
    ".DeleteWorkspace",
    function(e) {
        DeleteWorkspace();
    });
function DeleteWorkspace() {

    var msg = "Do you really want to delete this workspace?";

    bootbox.confirm(msg,
        function(res) {
            if (res) {
                $.ajax({
                    url: DeleteWorkspaceUrl,
                    type: "POST",
                    data: { workspaceId: getCurrentWorkspaceId() },
                    success: function(response) {
                        if (response.result == "Redirect")
                            window.location = response.url;
                    }
                });
            }
        });
}
