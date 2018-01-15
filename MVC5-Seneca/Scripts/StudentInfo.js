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

function UpdateStudentDetails(event)
{
    $("#DocumentComment").text("");
    $("#SessionNoteText").text("");
    var student_Id = $(this).val();
    _student_Id = student_Id;
    $.ajax({
        url: "/DisplayStudentInfo/GetStudentDetails",
        data: { id: student_Id },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            $("#EnterTutorNotesDiv").show();
            $("#EnterTutorNotesLabel").text("Enter Tutor Session Note");

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
            if (data.Parent.CellPhone !== null)
            {
                phone = data.Parent.CellPhone;
                phoneLabel = "   Cell Phone: ";
            }
            else if (data.Parent.HomePhone !== null)
            {
                phone = data.Parent.HomePhone;
                phoneLabel = "   Home Phone: ";
            }
            else
            {
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

            $("#reportsDDL").empty();
            $("#DocumentsDiv").hide();

            $("#SessionNotesDDL").empty();
            tutorSessionNotes = [];
            $("#DisplayTutorNotesDiv").hide();
           
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
            }
        }); // $.ajax({

    $.ajax({
        url: "/TutorNotes/GetTutorComments",
        data: { id: student_Id },
        type: "GET",
        dataType: "JSON",
        success: function (comments) {
            tutorSessionNotes = [];
            tutorNoteIds = [];
                $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');
                $.each(comments, function (i, note) {
                    var x = note.Date.substring(0, 10);
                    var dt = x.slice(0, 10).split('-');              
                    tutorSessionNotes.push(note.SessionNote);
                    tutorNoteIds.push(note.Id);
                    $("#SessionNotesDDL").append('<option value = "' + '">'                   
                        + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                        + "Tutor: " + note.User.FirstName + " "
                        + note.User.LastName                        
                        + '</option>');
                });                
                if (tutorSessionNotes.length !== 0)
                {
                    $("#DisplayTutorNotesDiv").show();
                    $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes:");                    
                    $("#SessionNoteLabel").text("  Session Note: ");                    
                }
                else
                {
                    $("#DisplayTutorNotesDiv").hide();
                }
        },
        error: function (data)
        {
            $("#DisplayTutorNotesDiv").hide();
        }       

    }); // $.ajax({
   
}  // function UpdateStudentDetails(event)

function f(event) {    
    var rpts = $("#reportsDDL option:selected");     
    $("#DocumentComment").text(reportComments[rpts[0].index - 1]);
    $("#GetDocumentPDF").attr("href", reportLinks[rpts[0].index - 1]);    
}

function UpdateSessionNote(event) {
    var nts = $("#SessionNotesDDL option:selected");
    $("#SessionNoteText").text(tutorSessionNotes[nts[0].index - 1]);
    $.ajax({
        url: "/TutorNotes/GetTutorNote",
        data:
        {
            Id: tutorNoteIds[nts[0].index-1]
        },
        type: "POST",
        dataType: "JSON",
        success: function (note) {
            if (note.User.Id !== Number($("#SessionUserId").text()))
            { /*Don't show authors's email if it's the same person as the user:*/
            _latestAuthor_Email = note.User.Email;
            $("#EmailAuthorLabel").show();
            $("#AuthorEmail").text(note.User.Email);
            $("#AuthorEmail").show();
            }
        },
        Error: function (response) {
            var yyy = "dummy";
        }        
    });
}

function SaveSessionNote(usrId, stId)
{
    $.ajax({
        url: "/TutorNotes/SaveTutorNote",
        data:
        {
            User_Id: usrId, Student_Id: stId,
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
                    noteToSelect = j - 1;         
                }             
                tutorSessionNotes.push(_note.SessionNote);
                tutorNoteIds.push(_note.Id);
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + _note.User.FirstName + " "
                    + _note.User.LastName
                    + '</option>');
            });
            // Reset selected item of tutor notes dropdownlist:
            $("#SessionNotesDDL")[0].selectedIndex = noteToSelect;
            $("#SessionNoteText").text(note.SessionNote);
            //$("#SessionNotesDDL").prop('selected', true);  // didn't work
            //$("#SessionNotesDDL").attr('selected', 'selected');  // didn't work

            $("#DisplayTutorNotesDiv").show();
            $("#NewSessionNote").val("");          

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
            //$("#SessionNoteText").text(noteText);
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
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + _note.User.FirstName + " "
                    + _note.User.LastName
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
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName;
    var url = "mailto:" + _latestAuthor_Email + subject;    
    window.open(url, '_blank');   
}