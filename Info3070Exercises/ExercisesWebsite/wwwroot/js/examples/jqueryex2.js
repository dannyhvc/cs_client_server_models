$(function() {
    const studentData = JSON.parse(
        `[{"id": 123, "firstname": "Teachers", "lastname": "Pet"},
        {"id": 234, "firstname": "Brown", "lastname": "Nose"},
        {"id": 345, "firstname": "Always", "lastname": "Late"}]`
    );

    $("#loadbutton").on("click", e => {
        let html = "";
        studentData.map(stu => {
            html += `<div class="list-group-item" id="${stu.id}">
                        ${stu.firstname}, ${stu.lastname}
                    </div>`;
        });

        $("#studentList").html(html);
        $("#loadbutton").hide();
    }); //load button

    $("#studentList").click(e => {
        const student = studentData.find(s => s.id == parseInt(e.target.id));
        $("#results").text(
            `you selected ${student.firstname}, ${student.lastname}`
        );
    }); //student list

});
