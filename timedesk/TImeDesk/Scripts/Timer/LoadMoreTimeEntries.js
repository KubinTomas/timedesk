/* 
This class is responsible for loading old time entries and append it
*/
var timeEntryLoadHistory = 1;
var maxLoadedHistory = 5;

$('#loadMoreTimeEntries').on("click",
    function (e) {
        //ToDo Historie pomocí append a ne dalšího load time entry
        //$('#loadMoreTimeEntries').append('<div>Test</div>');

        if (timeEntryLoadHistory >= maxLoadedHistory)
            window.location.href = dashboardUrl;

        //Increas week number
        timeEntryLoadHistory++;


        $.ajax({
            url: increaLoadedHistoryUrl,
            data: "historyValue=" + timeEntryLoadHistory,
            type: "post",
            cachce: false,
            success: function (data) {
                //Reload timeentries
                $('#TimeEntriesPartial').load(timeEntriesPartialViewUrl);
            }

        });

        if (timeEntryLoadHistory >= maxLoadedHistory) {
            $('#loadMoreTimeEntries').html("Redirect to Dashboard");
            $('#loadMoreTimeEntries').width(150);
           
        }

});