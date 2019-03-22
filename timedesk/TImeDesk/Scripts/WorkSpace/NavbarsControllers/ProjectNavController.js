var projectId = $("#ProjectIdHolder").attr("data-id");

//Workspace body -> Render users, projects, settings, ... 
var body = $("#ContentArea");

// Show all assigned users to project
$(document).on("click",
    ".ProjectShowUsers",
    function (e) {
        getUsersWorkingOnProject();
    });

function getUsersWorkingOnProject() {
    $.ajax({
        url: UsersProjectPartialViewUrl,
        type: "GET",
        data: { projectId: projectId, workspaceId: getCurrentWorkspaceId() },
        success: function (res) {
            body.html(res);          
        }

    });
}


//Show available users to assign
$(document).on("click",
    ".ProjectAddUser",
    function (e) {
        showUsersToAddOnProject();
    });
function showUsersToAddOnProject() {
    $.ajax({
        url: ShowUsersToAddOnProjectPartialViewUrl,
        type: "GET",
        data: { projectId: projectId, workspaceId: getCurrentWorkspaceId() },
        success: function (res) {
            body.html(res);
        }

    });
}

function getCurrentWorkspaceId() {
    return $("#WorkspaceMainSign").attr("data-id");
}



// Function logic

//Assign user to project
$(document).on("click", ".addUserToProject", function(e) {
    AddUserToProject($(this), $(this).attr("user-id"));
});

function AddUserToProject(clickedArea, userId) {

    $.ajax({
        url: AddUserToProjectUrl,
        type: "POST",
        data: { projectId: projectId, workspaceId: getCurrentWorkspaceId(), userId: userId },
        success: function () {
            clickedArea.closest("tr").remove();
        }

    });
}

//Remove user from project
$(document).on("click", ".removeUserFromProject", function (e) {
    RemoveUserFromProject($(this), $(this).attr("user-id"));
});

function RemoveUserFromProject(clickedArea, userId) {

    $.ajax({
        url: RemoveUserFromProjectUrl,
        type: "POST",
        data: { projectId: projectId, workspaceId: getCurrentWorkspaceId(), userId: userId },
        success: function () {
            clickedArea.closest("tr").remove();
        }

    });
}

// Show project statistic
$(document).on("click",
    ".ShowProjectStatistic",
    function(e) {
        GetProjectStatistic();
    });

function GetProjectStatistic() {
    $.ajax({
        url: ProjectDetailStatisticPartialViewUrl,
        type: "GET",
        data: { projectId: projectId, workspaceId: getCurrentWorkspaceId() },
        success: function(res) {
            body.html(res);
        }
    });
}

// Show project tasks
$(document).on("click",
    ".ShowProjectTasks",
    function (e) {
        GetProjectTasks();
    });

function GetProjectTasks() {
    $.ajax({
        url: ProjectTaskPartialViewUrl,
        type: "GET",
        data: { projectId: projectId, workspaceId: getCurrentWorkspaceId() },
        success: function (res) {
            body.html(res);
        }
    });
}