// #region Declarations
var reportComments = [];  // empty array
var reportLinks = []; // empty array 
var reportIds=[];  // contains Id url of currently selected report
var tutorSessionNotes = []; // empty array
var tutorNoteIds = []; 
var _student_Id; // for parameter passing
var _latestStudentFirstName;
var _latestParentEmail;
var _latestTutorNote_Id;
var _latestAuthor_Email;
var _latestPrimaryTutor_Email; 
var weekday = new Array(7);
weekday[0] = "Sun"; weekday[1] = "Mon";weekday[2] = "Tue";weekday[3] = "Wed";weekday[4] = "Thu";weekday[5] = "Fri";weekday[6] = "Sat";
// #endregion

function UpdateStudentDetails(event)
{
    $("#DocumentComment").text("");
    $("#SessionNoteText").text("");
    $("#NewSessionNote").text("");
    var student_Id = $(this).val();
    _student_Id = student_Id;
  
    $.ajax({
        url: "/DisplayStudentInfo/GetStudentDetails",
        data: { id: student_Id },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            $("#NewSessionNote").text("");
            $("#EnterTutorNotesDiv").show();
            $("#EnterTutorNotesLabel").text("Enter New Tutor Session Note");

            _latestStudentFirstName = data.FirstName;

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
            $("#EditSessionNote").hide();
            $("#EmailAuthorLabel").hide();
            $("#AuthorEmail").hide();   
           
            $("#reportsDDL").append('<option value = "' + '">' + "--Select Report--" + '</option > ');
            reportComments = [];
            reportIds = [];
            reportLinks = [];            
            $.each(data.Reports, function (i, Report) {
                var x = Report.DocumentDate.substring(0, 10);               
                var dt =x.slice(0, 10).split('-');              
                reportLinks.push(Report.DocumentLink);
                reportComments.push(Report.Comments);                 
                reportIds.push(Report.Id);
               
                $("#reportsDDL").append('<option value = "' + '">' 
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " " // How to reformat date to MM/dd/yyyy?
                    + Report.DocumentType.Name
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
        error: function (data) {
            $("#DocumentsDiv").hide();
        }  
    }); // $.ajax({ 
    GetTutorNotes();
}  

function GetTutorNotes()
{   
    $.ajax({ 
        url: "/TutorNotes/GetTutorComments",
        data: { id: _student_Id },
        type: "GET",
        dataType: "JSON",
        success: function (comments) {   
            $("#SessionNotesDDL").empty();
            tutorSessionNotes = [];
            tutorNoteIds = [];
            $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');
            $.each(comments, function (i, note) {
                var x = note.Date.substring(0, 10);
                var dt = x.slice(0, 10).split('-');
                tutorSessionNotes.push(note.SessionNote);
                tutorNoteIds.push(note.Id);
                var xx = new Date(Date.parse(note.Date));
                var dow = xx.getDay();
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    + weekday[dow] + " "
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + note.ApplicationUser.FirstName + " "
                    + note.ApplicationUser.LastName
                    + '</option>');   
            });
            if (tutorSessionNotes.length !== 0) {
                $("#DisplayTutorNotesDiv").show();
                $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes");
                $("#SessionNoteLabel").text("  Session Note: ");
            }
            else {
                $("#DisplayTutorNotesDiv").hide();
            }
        },
        error: function (data) {
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
    var nts = $("#SessionNotesDDL option:selected");
    if (nts[0].index === 0) {
        $("#SessionNoteLabel").hide();
        $("#SessionNoteText").hide();
        $("#EditSessionNote").hide();
        $("#EmailAuthorLabel").hide();
        $("#AuthorEmail").hide();     
    }
    else
    {
            $("#SessionNoteText").text(tutorSessionNotes[nts[0].index - 1]);
            $.ajax({
            url: "/TutorNotes/GetTutorNote",
            data:
            {
                Id: tutorNoteIds[nts[0].index - 1]
            },
            type: "POST",
            dataType: "JSON",
            success: function (note) {
                var x = $("#SessionUserId").text();
                _latestAuthor_Email = note.ApplicationUser.Email;
                $("#SessionNoteLabel").show();
                $("#SessionNoteText").show();
                $("#EditSessionNote").show();
                $("#EmailAuthorLabel").hide();
                $("#AuthorEmail").hide(); 
                if (note.ApplicationUser.Id !== $("#SessionUserId").text()) { /*Don't show authors's email if it's the same person as the user:*/
                    $("#EmailAuthorLabel").show();
                    $("#AuthorEmail").text(note.ApplicationUser.Email);
                    $("#AuthorEmail").show();                     
                }
            },
            Error: function (response) {
                var yyy = "dummy";
            }
        });
    }
}    

function SaveSessionNote(stId)
{                                                                   
    $.ajax({
        url: "/TutorNotes/SaveTutorNote",
        data:
        {
            Student_Id: stId,
            Date: $("#SessionDate").val(), SessionNote: $("#NewSessionNote").val()
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

            tutorSessionNotes = [];
            tutorNoteIds = [];
            var j = 0;
            var noteToSelect = 0;
            $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');
            $.each(note.TutorNotes, function (i, _note) {
                var x = _note.Date.substring(0, 10);
                var dt = x.slice(0, 10).split('-');
                j += 1;
                var selOption = "";
                if (_note.Id === _latestTutorNote_Id) {
                    //noteToSelect = j - 1; 
                    noteToSelect = j;
                }             
                tutorSessionNotes.push(_note.SessionNote);
                tutorNoteIds.push(_note.Id);
                var xx = new Date(Date.parse(_note.Date));
                var dow = xx.getDay();
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    + weekday[dow] + " "
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + _note.ApplicationUser.FirstName + " "
                    + _note.ApplicationUser.LastName
                    + '</option>');      
            });
            // Reset selected item of tutor notes dropdownlist:
            $("#SessionNotesDDL")[0].selectedIndex = noteToSelect;
            if (noteToSelect !== 0) {
                $("#SessionNoteText").text(note.SessionNote);
                $("#SessionNoteText").show();
                $("#SessionNoteLabel").show();
                //$("#EditSessionNote").show();
                $("#DisplayTutorNotesDiv").show();
                $("#NewSessionNote").val("");
            }
            var tt = "dummy";
        },
        Error: function(response)
        {
            var dummy = "";
        }
    });
}

function EditSessionNote(Id)
{
    var nts = $("#SessionNotesDDL option:selected");
    var noteToEditId = tutorNoteIds[nts[0].index - 1];
    var noteText = tutorSessionNotes[nts[0].index - 1];
    $("#EditSessionNoteLabel").text("Edit:");
    $("#EditSessionNoteText").val(noteText);
    $("#EditSessionNoteDiv").show();
    var dummy = "";
}

function SaveEditedSessionNote(text)
{
    var nts = $("#SessionNotesDDL option:selected");
    var noteToEditId = tutorNoteIds[nts[0].index - 1];
    var noteText = text[0].value;
    var dummy = "";
     $.ajax({
        url: "/TutorNotes/EditTutorSessionNote",
        data:
        {
            Id:noteToEditId, sessionNote:noteText
        },
        type: "POST",
        dataType: "JSON",
        success: function (note)
        {                                                                 
            $("#EditSessionNoteDiv").hide();
            _latestTutorNote_Id = noteToEditId;

            $("#SessionNotesDDL").empty();
            $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes:");
            $("#SessionNoteLabel").text("  Session Note: ");

            tutorSessionNotes = [];
            tutorNoteIds = [];
            var j = 0;
            var noteToSelect = 0;
            $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');
            $.each(note.TutorNotes, function (i, _note) {
                var x = _note.Date.substring(0, 10);
                var dt = x.slice(0, 10).split('-');
                j += 1;
                var selOption = "";
                if (_note.Id === _latestTutorNote_Id) {
                    //noteToSelect = j - 1;
                    noteToSelect = j;
                }
                tutorSessionNotes.push(_note.SessionNote);
                tutorNoteIds.push(_note.Id);
                var xx = new Date(Date.parse(_note.Date));
                var dow = xx.getDay();
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    + weekday[dow] + " "
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + _note.ApplicationUser.FirstName + " "
                    + _note.ApplicationUser.LastName
                    + '</option>');   
            });
            // Reset selected item of tutor notes dropdownlist:
            $("#SessionNotesDDL")[0].selectedIndex = noteToSelect;
            $("#SessionNoteText").text(note.SessionNote);
            //$("#SessionNotesDDL").prop('selected', true);  // didn't work
            //$("#SessionNotesDDL").attr('selected', 'selected');  // didn't work

            $("#DisplayTutorNotesDiv").show();
            $("#NewSessionNote").val("");
            var xxx = "dummy";
        },
        Error: function (response) {
            var yyy = "dummy";
        }
    });
}

function EmailToParent(Id)
{       
    var subject = "?subject=Student" + "%20" +  _latestStudentFirstName;
    var url = "mailto:" + _latestParentEmail + subject;
    window.open(url, '_blank');   
}

function EmailToAuthor(_latestAuthor_Email)
{
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + _latestAuthor_Email + subject;    
    window.open(url, '_blank');   
}

function EmailToPrimaryTutor(_latestPrimaryTutor_Email) {
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + _latestPrimaryTutor_Email + subject;
    window.open(url, '_blank');
}