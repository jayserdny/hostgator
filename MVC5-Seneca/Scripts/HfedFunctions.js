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
            //var markup = "<option value='0'>-- Select --</option>";
            var markup = "";
            for (var i = 0; i < data.length; i++)
            {
                markup += "<option value=" + data[i].Id.toString + ">" + data[i].FullName + "</option>";
                markup += data[i].Id.toString;               
                //$("#clientsDDL").append("<option value=" + data[i].Id.toString + "," + data[i].FullName + "</option>"); 
                $("#clientsDDL").append( data[i].Id.toString + "," + data[i].FullName ); 
            }
            $("#clientsDDL").trigger("chosen:updated");
            $('#ClientsDiv').show();
            //$("#clientsDDL").html(markup).show();

            //data.forEach(function (client) {
            //    hfedClients.push(client);
            //    hfedClientIds.push(client.Id);
            // });
            //waitForMe();
        },
        error: function () {
            var dummy = "";       
        }
    });
    hfedClients = []; hfedClientIds = [];
}
