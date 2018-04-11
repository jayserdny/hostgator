var students = []; // empty array
var studentIds = []; 
var tutorId = "";

function UpdateStudentDropdownList() {  
    $("#studentsDDL").empty();
    $("#studentsDDL").append('<option value = "' + '">' + " Select " + '</option > ');
    tutorId = $(this).val();   
    LoadStudentArrays(function () {
        students.forEach(function (student) {
            $("#studentsDDL").append('<option value = ' + student.Id + '>'
                + student.FirstName + '</option>');
        });
        if (students.length !== 0) {
            $("#StudentsDiv").show();
        }   
    });           
}

function LoadStudentArrays(waitForMe) {
    students = []; studentIds = [];
    $.ajax({
        url: "/AssociateTutors/GetStudents",
        data: { id: tutorId },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            data.forEach(function (student) {
                students.push(student);
                studentIds.push(student.Id);
             });
            waitForMe();
        },
        error: function () {
            var dummy = "";
        }
    });
    students = []; studentIds = [];
}
