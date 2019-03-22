var wNavbar = $("#NavigationBarWrapper");

//Workspace body -> Render users, projects, settings, ... 
var body = $("#ContentArea");

/* Switching user worksapce */
$(document).on("click",
    ".SwitchWorkSpace",
    function (e) {

        var element = $(this);

        if (element.hasClass("ActiveWorkspace"))
            return;

        switchCurrentWorkspace(element);

    });

function switchCurrentWorkspace(element) {


    var actualWorkspaceElement = $("#WorkSpaceHolder").find(".ActiveWorkspace");



    var id = element.attr("data-id");

    $.ajax({
        url: switchWorkspaceUrl,
        type: "POST",
        data: { id: id },
        success: function () {
            actualWorkspaceElement.removeClass("ActiveWorkspace fa fa-toggle-on");
            actualWorkspaceElement.addClass("fa fa-toggle-off");

            element.removeClass("fa fa-toggle-off");
            element.addClass("ActiveWorkspace fa fa-toggle-on");

           // ReloadLayout();
            location.reload();
        }

    });

}

function ReloadLayout() {
    $.ajax({
        url: GetWorkspaceIndexView,
        type: "GET",
        success: function (res) {
            console.log(res);
            console.log($(res));
            //var sideBar = res.text();


            //console.log(sideBar);
        }
    });
}

/* Handling invitations */
$(document).on("click",
    ".Invitation",
    function(e) {
        HandleInvitation($(this));
    });

function HandleInvitation(element) {

    var parent = element.closest(".InvitationWrapper");

    var type = element.attr("type");

    var accept = InviteResponse(type);
    var id = element.attr("data-id");


    $.ajax({
        url: invHandlerUrl,
        type: "POST",
        data: { usWorkspaceId: id, acceptInvitation: accept },
        success: function() {
            parent.remove();

            if (accept)
                AddNewWorkspace(id);
        }
    });


}
function InviteResponse(type) {

    if (type === "accept") {
        return true;
    }
    return false;
}
function AddNewWorkspace(id) {

    $.ajax({
        url: addNewWorkspaceUrl,
        type: "GET",
        data: { invitationId: id },
        success: function (res) {
      
             $("#WorkspaceEntries").append(res);

        }
    });
}




/* Workspace navigation menu */

/* Show dashboard */
$(document).on("click",
    ".Dashboard",
    function (e) {
        GetUsersPartialView();    
    });

function GetUsersPartialView() {

    var id = getCurrentWorkspaceId();

    $.ajax({
        url: UsersPartialViewUrl,
        type: "GET",
        data: { workspaceId: id },
        success: function (res) {
            body.html(res);
            getWorkspaceNavbar();
        }

    });
}
/* Show user profil */
$(document).on("click", ".userWorkspaceProfil", function (e) {
    showUserDetail($(this).attr("data-user-id"));
});

function showUserDetail(userId) {

    var workspaceId = getCurrentWorkspaceId();

    $.ajax({
        url: ShowUserDetailUrl,
        type: "GET",
        data: { userId: userId, workspaceId: workspaceId},
        success: function (res) {
            body.html(res);
        }

    });
}


/* Show invitation panel */
$(document).on("click",
    ".InviteContainer",
    function (e) {
        GetInvitePartialView();
    });

function GetInvitePartialView() {

    var id = getCurrentWorkspaceId();

    $.ajax({
        url: InvitePartialViewUrl,
        type: "GET",
        data: { workspaceId: id },
        success: function (res) {
            body.html(res);
        }

    });
}

/* Show workspace projects */
$(document).on("click",
    ".ShowProjects",
    function (e) {
         GetProjectsPartialView();
    });

function GetProjectsPartialView() {

    var id = getCurrentWorkspaceId();

    $.ajax({
        url: ProjectsPartialViewUrl,
        type: "GET",
        data: { workspaceId: id },
        success: function (res) {
            body.html(res);
        }

    });
}

/* Show project detail */

$(document).on("click",
    ".projectDetail",
    function(e) {
        GetProjectDetailPartialView($(this).attr("data-id"));
    });
function GetProjectDetailPartialView(projectId) {

    var id = getCurrentWorkspaceId();

    $.ajax({
        url: ProjectDetailPartialViewUrl,
        type: "GET",
        data: { projectId: projectId, workspaceId: id },
        success: function (res) {
            body.html(res);
            getProjectNavbar();
        }

    });
}

/* Show all permissions */
$(document).on("click", ".ShowPermissions", function(e) {
    GetUsersPermissions();
});
function GetUsersPermissions() {
    var id = getCurrentWorkspaceId();

    $.ajax({
        url: PermissionsPartialViewUrl,
        type: "GET",
        data: { workspaceId: id },
        success: function (res) {
            body.html(res);
        }

    });
}


/* Show workspace statistic */
$(document).on("click", ".CanManageStatistic", function (e) {
   ShowWorkspaceStatistic();
   
});
function ShowWorkspaceStatistic() {
    var id = getCurrentWorkspaceId();

    $.ajax({
        url: StatisticPartialViewUrl,
        type: "GET",
        data: { workspaceId: id },
        success: function (res) {
            body.html(res);
        }

    });
}

/* Save permissions for that row */
$(document).on("click",
    ".savePermissions",
    function(e) {
        saveUserPermissions($(this).attr("user-id"), $(this));
    });

function saveUserPermissions(userId, entry) {

    var data = {};
    var parent = entry.closest("tr");

    parent.find('td').each(function () {
        var chckBox = $(this).children();

        if (chckBox.is(":checkbox")) {
            data[chckBox.val()] = chckBox.is(":checked");
        }

    });


    var id = getCurrentWorkspaceId();
    data["userId"] = userId;
    data["workspaceId"] = getCurrentWorkspaceId();
    //var data = { CanManageRoles: true, CanManageProjects: true,CanManageBudget: true, CanManageStatistic: true, CanManageSettings: false, CanInviteUser: false,  userId: userId, workspaceId: id};

    $.ajax({
        url: SavePermissionsUrl,
        type: "POST",
        data: JSON.stringify(data),    
        contentType: 'application/json',
        success: function () {
            //Changing navbar when save roles -> Just test
            getWorkspaceNavbar();
        }

    });
}

//Redirect to project creation
$(document).on("click",
    ".CreateNewProject, .editProject",
    function () {
        var projectId = $(this).attr("project-id");
        GetCreateProjectPartailView(projectId);
    });
function GetCreateProjectPartailView(projectId) {

    var id = getCurrentWorkspaceId();

    $.ajax({
        url: CreateProjectUrl,
        type: "GET",
        data: { workspaceId: id, projectId : projectId },
        success: function (res) {
            body.html(res);
        }
    });   
}


function getCurrentWorkspaceId() {
    return $("#WorkspaceMainSign").attr("data-id");
}

/* Workspace navigation bars */

// Return basic dahboard nav bar
function getWorkspaceNavbar() {

    var id = getCurrentWorkspaceId();

    $.ajax({
        url: WorkspaceNavbarPartialViewUrl,
        type: "GET",
        data: { workspaceId: id },
        success: function (res) {
            wNavbar.html(res);
        }
    });   
}
// Return project nav bar
function getProjectNavbar() {

    $.ajax({
        url: ProjectNavbarPartialViewUrl,
        type: "GET",
        success: function (res) {
            wNavbar.html(res);
        }
    });
}

$(document).on("click",
    ".UserWorkspaceDetailBillingItem",
    function (e) {
       
        var billing = $(this);
        var billingId = billing.attr("data-id");

        var billingParent = billing.parent();

        var removeBilling;

        var newBillingPlace;
        
        //Check if billing should be add or removed

        //Remove billing
        if (billingParent.hasClass("AssignedCurrency")) {

            newBillingPlace = billingParent.parent().find(".AvailableCurrency");

            removeBilling = true;

            billing.remove();
            newBillingPlace.append(billing);

        } else { //Add tag

             newBillingPlace = billingParent.parent().find(".AssignedCurrency");

           
            removeBilling = false;

            billing.remove();
            newBillingPlace.append(billing);
        }

        var workspaceId = getCurrentWorkspaceId();
        var userId = $("#UserDetailIdHolder").attr("user-id");

        if (removeBilling) {
                $.ajax({
                    url: RemoveBillingFromUserUrl,
                    type: "POST",
                    data: { workspaceId: workspaceId, userId: userId, billingId: billingId },
                    success: function () {

                    }
                });
            } else {
                $.ajax({
                    url: AddBillingToUserUrl,
                    type: "POST",
                    data: { workspaceId: workspaceId, userId: userId, billingId: billingId },
                    success: function () {

                    }
                });
            }


    });




$(document).on("click",
    ".KickUserOutOfWorkspaceBtn, .LeaveWorkspace",
    function (e) {
        e.stopImmediatePropagation();

        var msg = GetBootBoxMessage($(this));


        var userId = $(this).attr("user-id");



        var userWasKicked = UserWasKickced($(this));

     

        ManageKickOrLeaveUser(userId, msg, userWasKicked);
    });

function ManageKickOrLeaveUser(userId, msg, userWasKicked) {
    bootbox.confirm(msg,
        function (result) {
            if (result) {
            
                $.ajax({
                    url: RemoveUserFormWorkspaceUrl,
                    type: "POST",
                    data: { userId: userId, workspaceId: getCurrentWorkspaceId(), userWasKicked: userWasKicked },
                    success: function (response) {
                        

                            if (response.result == "Redirect")
                                window.location = response.url;
                    }

                });
            }
        });
}
function GetBootBoxMessage(btn) {
    if (btn.hasClass("KickUserOutOfWorkspaceBtn")) {
        return "Do you really want to kick this user?";
    } else {
        return "Do you really want to leave this workspace?";
    }
}
function UserWasKickced(btn) {
    if (btn.hasClass("KickUserOutOfWorkspaceBtn")) {
        return true;
    } else {
        return false;
    }
}