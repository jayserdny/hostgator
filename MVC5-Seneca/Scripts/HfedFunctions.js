function UpdateHfedStartDate(startDate)
{
    $.ajax({
        url: '/HfedSchedules/Index',
        data: { startDate: startDate },
        type: "POST",
        success: function (data) { 
            $("body").html(data);  // to refresh the page
            //alert('Ajax hit'); 
        },
        error: function (jqxhr, status, exception) {
            alert('Exception:', exception);
        }
    });  
}

function UpdateHfedEndDate(endDate)
{
    $.ajax({
        url: '/HfedSchedules/Index',
        data: { endDate: endDate },
        type: "POST",
        success: function (data) {
            $("body").html(data);  // to refresh the page
            //alert('Ajax hit'); 
        },
        error: function (jqxhr, status, exception) {
            alert('Exception:', exception);
        }
    });
}
