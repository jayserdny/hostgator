﻿// #region                                                              
var noteToSelectText;
var _loadArraysInProgress = false;  // flag to ignore calls from SessionNotesDDL.change
var reportComments = [];  // empty array
var reportLinks = []; // empty array 
var reportIds=[];  // contains Id url of currently selected report
var tutorSessionNotes = []; // empty array
var tutorNoteIds = []; 
var associateTutorEmails = [];
var _studentId; // for parameter passing
var _latestStudentFirstName;
var _latestParentEmail;
var _latestTutorNote_Id;
var _latestAuthor_Email;
var _latestPrimaryTutor_Email;
var _latestTeacher_Email;
var _latestCaseManager_Email;
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
    $("#associateTutor0Row").hide();
    $("#associateTutor1Row").hide();
    $("#associateTutor2Row").hide();
    $("#teacherRow").hide();
    $("#caseManagerRow").hide();
    $("#Save-Button").hide();
    var studentId = $(this).val();
    _studentId = studentId;
                                                                                  
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
            if (data.Parent.CellPhone !== null && data.Parent.CellPhone !== "") {
                phone = data.Parent.CellPhone;
                phoneLabel = " Phone: ";
            }
            else if (data.Parent.HomePhone !== null && data.Parent.HomePhone !== "") {
                phone = data.Parent.HomePhone;
                phoneLabel = " Phone: ";
            }
            else {
                phone = "";
                phoneLabel = "";
            }
            $("#parentPhoneLabel").text(phoneLabel);
            $("#parentPhone").text(phone);

            var gradeLevelLabel = "";
            var gradeLevel = "";
            if (data.GradeLevel > 0) {
                //gradeLevelLabel = "Grade Level: ";
                gradeLevelLabel = "Grade: ";
                gradeLevel = data.GradeLevel;
            }
            if (data.GradeLevel === 0) {
                gradeLevelLabel = "Grade: ";
                gradeLevel = "Kindergarten";
            }
            if (data.GradeLevel === -1) {
                gradeLevelLabel = "Grade: ";
                gradeLevel = "Pre-K";
            }
            $("#gradeLevelLabel").text(gradeLevelLabel);
            $("#gradeLevel").text(gradeLevel);

            var specialClassLabel = "";
            var specialClass = "";
            if (data.SpecialClass) {
                specialClassLabel = "In Special Class: ";
                specialClass="\u221A";   // tick mark
            }
            $("#specialClassLabel").text(specialClassLabel);
            $("#specialClass").text(specialClass);

            var schoolLabel = "School: ";
            var school = data.School.Name;
            $("#schoolLabel").text(schoolLabel);
            $("#schoolName").text(school);

            var parentEmailLabel;
            var parentEmail;
            if (data.Parent.Email !== null && data.Parent.Email !== "")
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
                    $("#primaryTutorEmail").text("");
                }

                $("#primaryTutorName").text(data.PrimaryTutor.FirstName + " " + data.PrimaryTutor.LastName);
                $("#primaryTutorRow").show();
            }
            else
            {
                $("#primaryTutorRow").hide();
            }

            if (data.Teacher !== null)
            {
                $("#teacherName").text(data.Teacher.FirstName + " " + data.Teacher.LastName);
                $("#teacherPhone").text(data.Teacher.WorkPhone);
                if (data.Teacher.CellPhone !== null)
                {
                    $("#teacherPhone").text(data.Teacher.CellPhone);
                }
                if (data.Teacher.Email !== null) {
                    _latestTeacher_Email = data.Teacher.Email;
                    $("#teacherEmail").text(data.Teacher.Email);
                }
                else
                {
                    $("#teacherEmail").text("");
                }
                $("#teacherRow").show();
            }

            if (data.Parent !== null) {
                if (data.Parent.CaseManagerUser !== null) {
                    phone = "";
                    phoneLabel = "";
                    if (data.Parent.CaseManagerUser.PhoneNumber !== null) {
                        phone = data.Parent.CaseManagerUser.PhoneNumber;
                        phoneLabel = "Phone: ";
                    }
                    else {
                        phone = "";          
                    }                                                                              
                    $("#caseManagerPhone").text(phone);
                    
                    if (data.Parent.CaseManagerUser.Email !== null) {
                        _latestCaseManager_Email = data.Parent.CaseManagerUser.Email;
                        $("#caseManagerEmail").text(data.Parent.CaseManagerUser.Email);
                    }
                    else {
                        $("#caseManagerEmail").text("");
                    }

                    $("#caseManagerName").text(data.Parent.CaseManagerUser.FirstName + " " + data.Parent.CaseManagerUser.LastName);
                    $("#caseManagerRow").show();
                    }
                else {
                    $("#caseManagerRow").hide();
                }
            }
            
            associateTutorEmails = [];       
            if (data.AssociateTutors !== null && data.AssociateTutors.length !== 0)
            { 
                for (var index in data.AssociateTutors)           
                {
                    if (data.AssociateTutors.hasOwnProperty(index)) {
                        var tutorName = data.AssociateTutors[index].FirstName +
                            ' ' +
                            data.AssociateTutors[index].LastName;
                        var tutorPhone = data.AssociateTutors[index].PhoneNumber;
                        tutorPhone = tutorPhone.replace(/\D/g, "");
                        if (tutorPhone.length === 10) {
                            tutorPhone = '(' +
                                tutorPhone.substring(0, 3) +
                                ') ' +
                                tutorPhone.substring(3, 6) +
                                '-' +
                                tutorPhone.substring(6, 10);
                        } else {
                            tutorPhone = "";
                        }

                        var tutorEmail = data.AssociateTutors[index].Email;
                        associateTutorEmails.push(tutorEmail);

                        if (index === "0") {
                            $("#associateTutor0Name").text(tutorName);
                            $("#associateTutor0Phone").text(tutorPhone);
                            $("#associateTutor0Email").text(tutorEmail);
                            $("#associateTutor0Row").show();
                        } else if (index === "1") {
                            $("#associateTutor1Name").text(tutorName);
                            $("#associateTutor1Phone").text(tutorPhone);
                            $("#associateTutor1Email").text(tutorEmail);
                            $("#associateTutor1Row").show();
                        } else if (index === "2") {
                            $("#associateTutor2Name").text(tutorName);
                            $("#associateTutor2Phone").text(tutorPhone);
                            $("#associateTutor2Email").text(tutorEmail);
                            $("#associateTutor2Row").show();
                        }
                    }
                }
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
            $("#authorEmail").hide();   
           
            $("#reportsDDL").append('<option value = "' + '">' + "--Select Report (" +  data.Reports.length  +  ")--" + '</option > ');
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
          //var dummy = "";       
        }
    });
    tutorSessionNotes = []; tutorNoteIds = [];
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
                    // var xx = new Date(Date.parse(note.Date)); 
                    // var dow = xx.getDay();    
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
                    Error: function ()
                {
                    $("#DisplayTutorNotesDiv").hide();
                }
    });   // $.ajax({
} 

function UpdateDocumentLink() {    
    var rpts = $("#reportsDDL option:selected");     
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

function UpdateSessionNote() {
    if (!_loadArraysInProgress) {
        var nts = $("#SessionNotesDDL option:selected");
        if (nts[0].index === 0) {
            $("#SessionNoteLabel").hide();
            $("#SessionNoteText").hide();
            $("#SessionNoteSaveEdits").hide();
            $("#EmailAuthorLabel").hide();
            $("#authorEmail").hide();
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
                    // var x = $("#SessionUserId").text();
                    $("#SessionNoteText").val(note.SessionNote);
                    _latestAuthor_Email = note.ApplicationUser.Email;
                    $("#SessionNoteLabel").show();
                    $("#SessionNoteText").show();
                    if (note.UpdateAllowed)
                    {
                        $("#SessionNoteSaveEdits").show();
                    }  
                    $("#EmailAuthorLabel").hide();
                    $("#authorEmail").hide();
                    if (note.ApplicationUser.Id !== $("#SessionUserId").text())
                    { /*Don't show authors's email if it's the same person as the user:*/
                        $("#EmailAuthorLabel").show();
                        $("#authorEmail").text(note.ApplicationUser.Email);
                        $("#authorEmail").show();
                    }
                },
                Error: function() {
                    //var yyy = "dummy";
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
            date: $("#SessionDate").val(),
            sessionNote: $("#NewSessionNote").val()
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
            tutorSessionNotes.forEach(function(tNote)
            {
                var x = tNote.Date.substring(0, 10);
                var dt = x.slice(0, 10).split("-");
                j += 1;                    
                if (tNote.Id === _latestTutorNote_Id) {      
                    noteToSelect = j;
                    noteToSelectText = tNote.SessionNote;
                }                                           
                // var xx = new Date(Date.parse(tNote.Date));
                // var dow = xx.getDay();
                $("#SessionNotesDDL").append('<option value = "' + '">'
                    //+ weekday[dow] + " "
                    + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                    + "Tutor: " + tNote.ApplicationUser.FirstName + " "
                    + tNote.ApplicationUser.LastName
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
            //var tt = "dummy";
        },
        Error: function()
        {
            //var dummy = "";
        }
    });
}  

function SaveEditedSessionNote(text)
{
    var nts = $("#SessionNotesDDL option:selected");
    var noteToEditId = tutorNoteIds[nts[0].index - 1];
    var noteText = text[0].value;      
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
        success: function() {
            _latestTutorNote_Id = noteToEditId;                                                                     
            $("#SessionNotesDDL").empty();
            $("#TutorNotesSelectLabel").text("Previous Tutor Session Notes:");
            $("#SessionNoteLabel").text("  Session Note: ");

            tutorSessionNotes = []; tutorNoteIds = [];
            LoadTutorNoteArrays(function() { 
                $("#SessionNotesDDL").append('<option value = "' + '">' + "--Select Note--" + '</option > ');   
                tutorSessionNotes.forEach(function (tNote) {   
                        var x = tNote.Date.substring(0, 10);
                        var dt = x.slice(0, 10).split('-');
                        j += 1;                      
                        if (tNote.Id === _latestTutorNote_Id) {      
                            noteToSelect = j;
                        }
                        // var xx = new Date(Date.parse(_note.Date));
                        // var dow = xx.getDay();
                        $("#SessionNotesDDL").append('<option value = "' + '">'
                            //+ weekday[dow] + " "
                            + dt[1] + "/" + dt[2] + "/" + dt[0] + " "
                            + "Tutor: " + tNote.ApplicationUser.FirstName + " "
                            + tNote.ApplicationUser.LastName + '</option>');
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
            //var xx = "dummy";
        },
        Error: function ()
        {
             //var yyy = "dummy";
        }
     });
}

function EmailToParent()
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

function EmailToAssociateTutor(index) {
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + associateTutorEmails[index] + subject;
    window.open(url, '_blank');
}

function EmailToTeacher() {
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + _latestTeacher_Email + subject;
    window.open(url, '_blank');
}

function EmailToCaseManager(latestCaseManagerEmail) {
    var subject = "?subject=Student" + "%20" + _latestStudentFirstName + " - SHEP";
    var url = "mailto:" + latestCaseManagerEmail + subject;
    window.open(url, '_blank');
}


