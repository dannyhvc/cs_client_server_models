$(function () {
    const stringData =
        `[{"id": 123, "firstname": "Teachers", "lastname": "Pet"},
        {"id": 234, "firstname": "Brown", "lastname": "Nose"},
        {"id": 345, "firstname": "Always", "lastname": "Late"}]`;

    $("#loadbutton").on("click", e => {
        const studentData = JSON.parse(stringData);

        let html = "";
        studentData.map(stu => {
            html += `<div class="list-group-item">
                        ${stu.id}, ${stu.firstname}, ${stu.lastname}
                    </div>`;
        });

        $("#studentList").html(html);
        $("#loadbutton").hide();
    });
});
