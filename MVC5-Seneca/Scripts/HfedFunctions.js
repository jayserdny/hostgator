var hfedClients = [];
var hfedClientIds = [];

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

function LoadClients()
{
    var each = function() { throw new Error("Not implemented"); };   
    var locationId = $(this).val();                                                   
    $.ajax({
        url: "/HfedClients/GetClients",
        data: { id: locationId },
        cache: false,
        type: "POST",
        dataType: "JSON",
        success: function (data) {      
            $('#clientsDDL').empty();
           
            var items = "<option selected value=\"\"></option>";   
            for (var i = 0; i < data.length; i++)
            {                     
                items += "<option value=\"" + data[i].Value + "\">" + data[i].Text + "</option>";
                //items += "<option value=\"" + data[i].Value + "\">" + data[i].Text + "\" selected=1" + "</option>";
            }
            $('#Asterisk').hide(); // Hide red asterisk and footnote when Location changes
            $('#Footnote').hide();
            $('#clientsDDL').html(items);
            $("#clientsDDL").trigger("chosen:updated");
            $("#clientsDDL").change();      
        },
        error: function () {
            var dummy = "";       
        }
    });                                                    
}

function LoadRecipients() {
    var each = function () { throw new Error("Not implemented"); };
    var locationId = $(this).val();
    $.ajax({
        url: "/HfedClients/GetClients",
        data: { id: locationId },
        cache: false,
        type: "POST",
        dataType: "JSON",
        success: function (data) {
            $('#clientsDDL').empty();

            var items = "<option selected value=\"\"></option>";
            for (var i = 0; i < data.length; i++) {
                items += "<option value=\"" + data[i].Value.toString + "\">" + data[i].Text + "</option>";
                //items += "<option value=\"" + data[i].Value.toString + "\">" + data[i].Text + "\" selected=1" + "</option>";
            }
            $('#clientsDDL').html(items);
            $("#clientsDDL").trigger("chosen:updated");
            $("#clientsDDL").change();
        },
        error: function () {
            var dummy = "";
        }
    });
}

function UpdateReminderDate(reminderDate) {
    $.ajax({
        url: '/HfedEmail/EmailReminder',
        data: { reminderDate: reminderDate },
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
