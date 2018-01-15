var _latestUser_Id;

function GetUserResetInfo(event) {
    var user_Id = $(this).val();
    $.ajax({
        url: "/ResetAnyPassword/GetUserInfo",
        data: { id: user_Id },
        type: "GET",
        dataType: "JSON",
        success: function (user) {
            _latestUser_Id = user_Id;            
            $("#userName").text("User:" + " " + user.LastName);
            $("#userEmail").text(user.Email);
            $("#newPassword").text("");
            $("#EnterNewPasswordDiv").show();
            if (user.CellPhone !== null) 
                $("#userPhone").text("Cell:" + " " + user.CellPhone);              
            else            
                if (user.HomePhone !== null)
                {
                    $("#userPhone").text("Cell:" + " " + user.CellPhone);
                }                      
        },
        error: function (data) {
            $("#Main").hide();
        }  
    }); 
}  // function GetUserResetInfo(event)

function SaveNewPassword(event){
    var user_Id = $(this).val();
    var x = user_Id;

    $.ajax({
        url: "/ResetAnyPassword/Reset",
        data: { id: user_Id },
        type: "GET",
        dataType: "JSON",
        success: function (user) {
            _latestUser_Id = user_Id;
            $("#EnterNewPasswordDiv").show();
        },
        error: function (data) {
            $("#Main").hide();
        }
    });
}  //  function SaveNewPassword(event)
