function UpdateStudentDropdownList()
{                             
    var tutorId = $(this).val();
    $.ajax({
        url: "/AssociateTutors/GetStudents",
        data: { id: tutorId }, type: "GET", dataType: "JSON",
        success: function (data) {
            $("#studentsDDL").append('<option value = >' + "-Select Student-" + '</option > ');
            $.each(data.Students, function (i, student) {
                $("#studentsDDL").append('<option value = "' + '">' + student.FirstName + '</option>');
            }
        }
      }); // $.ajax({ 
}