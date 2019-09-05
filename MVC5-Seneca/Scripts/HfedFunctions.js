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
    hfedClients = []; hfedClientIds = [];   
    var locationId = $(this).val();                                                   
    $.ajax({
        url: "/HfedClients/GetClients",
        data: { id: locationId },
        cache: false,
        type: "POST",
        dataType: "JSON",
        success: function (data) {     
            $('#ClientsDiv').hide();
            $('#clientsDDL').empty();
           
            var items = "<option selected value=\"\"></option>";   
            for (var i = 0; i < data.length; i++)
            {                     
                items += "<option value=\"" + data[i].Value.toString + "\">" + data[i].Text + "</option>";
            }
            $('#clientsDDL').html(items);
            $("#clientsDDL").trigger("chosen:updated");
            $("#clientsDDL").change();
            $('#ClientsDiv').show();   
        },
        error: function () {
            var dummy = "";       
        }
    });
    hfedClients = []; hfedClientIds = [];
}
