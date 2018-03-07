// #region                                                              
var noteToSelectText;
var _loadArraysInProgress = false;  // flag to ignore calls from SessionNotessDDL.change
var reportComments = [];  // empty array
var reportLinks = []; // empty array 
var reportIds=[];  // contains Id url of currently selected report
var tutorSessionNotes = []; // empty array
var tutorNoteIds = []; 
var _studentId; // for parameter passing
var _latestStudentFirstName;
var _latestParentEmail;
var _latestTutorNote_Id;
var _latestAuthor_Email;
var _latestPrimaryTutor_Email; 
var weekday = new Array(7);
weekday[0] = "Sun"; weekday[1] = "Mon"; weekday[2] = "Tue"; weekday[3] = "Wed"; weekday[4] = "Thu"; weekday[5] = "Fri"; weekday[6] = "Sat"; 
// #endregion

function UpdateStudentDetails()
{
    $("#DocumentComment").text("");
    $("#SessionNoteText").val("");
    $("#NewSessionNote").val("");
    $("#motherFather").text("");
    $("#parentFirstName").text("");
    $("#parentPhoneLabel").text("");
    $("#parentPhone").text("");
    $("#parentEmailLabel").text("");
    $("#parentEmail").text("");
    $("#primaryTutorRow").hide();
    $("#Save-Button").hide();
    var studentId = $(this).val();
    _studentId = studentId;

    var updateAllowed = $("#UpdateAllowed").text();
    var t = updateAllowed;
   

    $.ajax({
        url: "/DisplayStudentInfo/GetStudentDetails",
        data: { id: studentId },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            $("#NewSessionNote").val("");
            $("#EnterTutorNotesDiv").show();
            $("#EnterTutorNotesLabel").text("Enter New Tutor Session Note"); 

            _latestStudentFirstName = data.FirstName;

            if ($("#UpdateAllowed").text() === "true") {
                $("#Save-Button").show();
            }
            var parent;
            if (data.Parent.MotherFather === "M")
                parent = "Mother: ";
            else
                parent = "Father: ";
            $("#motherFather").text(parent);
            $("#parentFirstName").text(data.Parent.FirstName);

            var phone = "";
            var phoneLabel = "";
            if (data.Parent.CellPhone !== null) {
                phone = data.Parent.CellPhone;
                phoneLabel = "   Cell Phone: ";
            }
            else if (data.Parent.HomePhone !== null) {
                phone = data.Parent.HomePhone;
                phoneLabel = "   Home Phone: ";
            }
            else {
                phone = "";
                phoneLabel = "";
            }
            $("#parentPhoneLabel").text(phoneLabel);
            $("#parentPhone").text(phone);

            var parentEmailLabel;
            var parentEmail;
            if (data.Parent.Email !== null)
                parentEmailLabel = "   Email: ",
                    parentEmail = data.Parent.Email,
                    _latestParentEmail = parentEmail;
            else
                parentEmailLabel = "",
                    parentEmail = "",    // Don't show labels or 'null' if no email
                    _latestParentEmail = "";
            $("#parentEmailLabel").text(parentEmailLabel);
            $("#parentEmail").text(parentEmail);

            if (data.PrimaryTutor !== null) {
                if (data.PrimaryTutor.PhoneNumber !== null) {
                    $("#primaryTutorPhone").text(data.PrimaryTutor.PhoneNumber);
                }
                else {
                    $("#primaryTutorPhone").text("");
                }
                if (data.PrimaryTutor.Email !== null) {
                    _latestPrimaryTutor_Email = data.PrimaryTutor.Email;
                    $("#primaryTutorEmail").text(data.PrimaryTutor.Email);
                }
                else {
                    $("#primaryTutoEmail").text("");
                }

                $("#primaryTutorName").text(data.PrimaryTutor.FirstName + " " + data.PrimaryTutor.LastName);
                $("#primaryTutorRow").show();
            }
            else
            {
                $("#primaryTutorRow").hide();
            }

            $("#reportsDDL").empty();
            $("#DocumentCommentLabel").hide();
            $("#DocumentComment").hide();
            $("#DocumentLinkLabel").hide();
            $("#GetDocumentPDF").hide();
            $("#DocumentsDiv").hide();

            $("#SessionNoteLabel").hide();
            $("#SessionNoteText").hide();
            $("#SessionNoteSaveEdits").hide();
            $("#EmailAuthorLabel").hide();
            $("#AuthorEmail").hide();   
           
            $("#reportsDDL").append('<option value = "' + '">' + "--Select Report--" + '</option > ');
            reportComments = [];
            reportIds = [];
            reportLinks = [];            
            $.each(data.Reports, function (i, report) {
                var x = report.DocumentDate.substring(0, 10);               
                var dt =x.slice(0, 10).split("-");              
                reportLinks.push(report.DocumentLink);
                reportComments.push(report.Comments);                 
                reportIds.push(report.Id);
               
                $("#reportsDDL").append('<option value = "' + '">' 
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " " 
                    + report.DocumentType.Name
                    + '</option>');
            }); 
            if (reportLinks.length !== 0) {
                $("#DocumentSelectLabel").text("Documents");
                $("#DocumentsDiv").show();
                $("#DocumentLinkLabel").text("  PDF Report: ");  
                $("#DocumentCommentLabel").text("  Comments: ");               
            }
            else {
                $("#DocumentsDiv").hide();
            }
        },
        error: function () {
            $("#DocumentsDiv").hide();
        }  
    }); // $.ajax({ 
    GetTutorNotes();  // in UpdateStudentDetails
}

function LoadTutorNoteArrays(waitForMe) {    
    tutorSessionNotes = []; tutorNoteIds = [];
    $.ajax({
        url: "/TutorNotes/GetTutorComments",
        data: { id: _studentId },
        type: "GET",
        dataType: "JSON",                                
        success: function (data) {
            data.forEach(function(note) {
                    tutorSessionNotes.push(note);
                    tutorNoteIds.push(note.Id);
                });
            waitForMe();
        },
        error: function () {
          var dummy = "";
        }
    });
    tutorSessionNotes = []; tutorNoteIds = [];
    var xx = "dummy";                                   
}

function GetTutorNotes()
{   
    $.ajax({ 
        url: "/TutorNotes/GetTutorComments",
        data: { id: _studentId },
        type: "GET",
        dataType: "JSON",
        success: function () {   
            $("#SessionNotesDDL").empty();
            $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');
            LoadTutorNoteArrays(function() {
                tutorSessionNotes.forEach(function(note) {
                    var x = note.Date.substring(0, 10);
                    var dt = x.slice(0, 10).split('-');
                    var xx = new Date(Date.parse(note.Date));
                    var dow = xx.getDay();
                    $("#SessionNotesDDL").append('<option value = "' + '">'
                        //+ weekday[dow] + " "
                        + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                        + "Tutor: " + note.ApplicationUser.FirstName + " "
                        + note.ApplicationUser.LastName + '</option>');
                });
                if (tutorSessionNotes.length !== 0) {
                    $("#DisplayTutorNotesDiv").show();
                    $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes");
                    $("#SessionNoteLabel").text("  Session Note: ");
                }
            });
            $("#DisplayTutorNotesDiv").hide();
        },
                    Error: function (data)
                {
                    $("#DisplayTutorNotesDiv").hide();
                }
    });   // $.ajax({
} 

function UpdateDocumentLink(event) {    
    var rpts = $("#reportsDDL option:selected");
    var xx = rpts[0].index;
    if (rpts[0].index === 0) {
        $("#DocumentCommentLabel").hide();
        $("#DocumentComment").hide();
        $("#GetDocumentPDF").hide();
        $("#DocumentLinkLabel").hide();
        $("#GetDocumentPDF").hide();      
    }
    else {
        $("#DocumentComment").text(reportComments[rpts[0].index - 1]);   
        $("#GetDocumentPDF").attr("href", "/StudentReports/ViewReport/" + reportIds[rpts[0].index - 1]);
        $("#DocumentComment").show();        
        $("#GetDocumentPDF").show();
        $("#DocumentLinkLabel").show();
        $("#GetDocumentPDF").show();
    }
}

function UpdateSessionNote(event) {
    if (!_loadArraysInProgress) {
        var nts = $("#SessionNotesDDL option:selected");
        if (nts[0].index === 0) {
            $("#SessionNoteLabel").hide();
            $("#SessionNoteText").hide();
            $("#SessionNoteSaveEdits").hide();
            $("#EmailAuthorLabel").hide();
            $("#AuthorEmail").hide();
        } else {
            $("#SessionNoteText").text(tutorSessionNotes[nts[0].index - 1]);
            $.ajax({
                url: "/TutorNotes/GetTutorNote",
                data:
                {
                    Id: tutorNoteIds[nts[0].index - 1]
                },
                type: "POST",
                dataType: "JSON",
                success: function(note) {
                    var x = $("#SessionUserId").text();
                    $("#SessionNoteText").val(note.SessionNote);
                    _latestAuthor_Email = note.ApplicationUser.Email;
                    $("#SessionNoteLabel").show();
                    $("#SessionNoteText").show();
                    if (note.UpdateAllowed)
                    {
                        $("#SessionNoteSaveEdits").show();
                    }  
                    $("#EmailAuthorLabel").hide();
                    $("#AuthorEmail").hide();
                    if (note.ApplicationUser.Id !== $("#SessionUserId").text())
                    { /*Don't show authors's email if it's the same person as the user:*/
                        $("#EmailAuthorLabel").show();
                        $("#AuthorEmail").text(note.ApplicationUser.Email);
                        $("#AuthorEmail").show();
                    }
                },
                Error: function(response) {
                    var yyy = "dummy";
                }
            });
        }
    }
}    

function SaveSessionNote(stuId)
{ 
    
    $.ajax({
        url: "/TutorNotes/SaveTutorNote",
        data:
        {
            studentId: stuId,
            date: $("#SessionDate").val(), sessionNote: $("#NewSessionNote").val()
        },
        type: "POST",
        dataType: "JSON",
        success: function (note)
        {
            // After insert, reload $SessionNotesDDL and select this record.            
            _latestTutorNote_Id = note.Id;

            $("#SessionNotesDDL").empty();      
            $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes:");
            $("#SessionNoteLabel").text("  Session Note: ");

            LoadTutorNoteArrays(function() {     
            var j = 0;
            var noteToSelect = 0;
            noteToSelectText = "";
            $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');
            tutorSessionNotes.forEach(function(_note)
            {
                var x = _note.Date.substring(0, 10);
                var dt = x.slice(0, 10).split("-");
                j += 1;                    
                if (_note.Id === _latestTutorNote_Id) {      
                    noteToSelect = j;
                    noteToSelectText = _note.SessionNote;
                }                                           
                var xx = new Date(Date.parse(_note.Date));
                var dow = xx.getDay();
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    //+ weekday[dow] + " "
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + _note.ApplicationUser.FirstName + " "
                    + _note.ApplicationUser.LastName
                    + '</option>');      
            });

            // Reset selected item of tutor notes dropdownlist:
            $("#SessionNotesDDL")[0].selectedIndex = noteToSelect;
            if (noteToSelect !== 0) {
                $("#SessionNoteText").val(noteToSelectText);
                $("#SessionNoteText").show();
                $("#SessionNoteLabel").show();
                $("#SessionNoteSaveEdits").show();
                $("#DisplayTutorNotesDiv").show();
                $("#NewSessionNote").val("");
            }
        });
            var tt = "dummy";
        },
        Error: function(response)
        {
            var dummy = "";
        }
    });
}  

function SaveEditedSessionNote(text)
{
    var nts = $("#SessionNotesDDL option:selected");
    var noteToEditId = tutorNoteIds[nts[0].index - 1];
    var noteText = text[0].value;
    var dummy = "";
    var j = 0;
    var noteToSelect = 0;
     $.ajax({
        url: "/TutorNotes/EditTutorSessionNote",
        data:
        {
            Id:noteToEditId, sessionNote:noteText
        },
        type: "POST",
        dataType: "JSON",
        success: function(note) {
            _latestTutorNote_Id = noteToEditId; 
            $("#SessionNotesDDL").empty();
            $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes:");
            $("#SessionNoteLabel").text("  Session Note: ");

            tutorSessionNotes = []; tutorNoteIds = [];
            LoadTutorNoteArrays(function() { 
                $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');   
                tutorSessionNotes.forEach(function (_note) {   
                        var x = _note.Date.substring(0, 10);
                        var dt = x.slice(0, 10).split('-');
                        j += 1;                      
                        if (_note.Id === _latestTutorNote_Id) {      
                            noteToSelect = j;
                        }
                        var xx = new Date(Date.parse(_note.Date));
                        var dow = xx.getDay();
                        $("#SessionNotesDDL").append('<option value = "' + '">'
                            //+ weekday[dow] + " "
                            + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                            + "Tutor: " + _note.ApplicationUser.FirstName + " "
                            + _note.ApplicationUser.LastName + '</option>');
                });

                if (noteToSelect === 0) // note was deleted
                {
                    $("#SessionNoteLabel").hide();
                    $("#SessionNoteText").hide();
                    $("#SessionNoteSaveEdits").hide();
                }
                else
                { // Reset selected item of tutor notes dropdownlist:
                    $("#SessionNotesDDL")[0].selectedIndex = noteToSelect;
                    $("#SessionNoteText").text(tutorSessionNotes[noteToSelect]);
                    $("#DisplayTutorNotesDiv").show();
                    $("#NewSessionNote").val("");
                }
            });
            var xxx = "dummy";
        },
        Error: function ()
        {
             var yyy = "dummy";
        }
     });
}

function EmailToParent(id)
{       
    var subject = "?subject=Student" + "%20" +  _latestStudentFirstName;
    var url = "mailto:" + _latestParentEmail + subject;
    window.open(url, '_blank');   
}

function EmailToAuthor(latestAuthorEmail)
{
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + latestAuthorEmail + subject;    
    window.open(url, '_blank');   
}

function EmailToPrimaryTutor(latestPrimaryTutorEmail) {
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + latestPrimaryTutorEmail + subject;
    window.open(url, '_blank');
}