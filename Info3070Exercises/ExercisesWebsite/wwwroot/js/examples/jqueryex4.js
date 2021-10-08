$(function() {
    const stringData =
        `[{"id": 123, "firstname": "Teachers", "lastname": "Pet"},
        {"id": 234, "firstname": "Brown", "lastname": "Nose"},
        {"id": 345, "firstname": "Always", "lastname": "Late"}]`

    sessionStorage.getItem("studentData") === null
        ? sessionStorage.setItem("studentData", stringData)
        : null;

    let data = JSON.parse(sessionStorage.getItem("studentData"));

    $("#loadbutton").on("click", e => {
        let html = "";
        data.map(stu => {
            html += `<div class="list-group-item" id="${stu.id}">
                        ${stu.firstname}, ${stu.lastname}
                    </div>`;
        });

        $("#studentList").html(html);
        $("#loadbutton").hide();
        $("#addbutton").show();
        $("#removebutton").show();
    }); //load button

    $("#studentList").on("click", e => {
        const student = studentData.find(s => s["id"] == parseInt(e.target.id));
        $("#results").text(
            `you selected ${student["firstname"]}, ${student["lastname"]}`
        );
    }); //student list

    $("#addbutton").on("click", e => {
        const current_student = data[data.length - 1];
        $("#results").text(`adding student ${current_student["id"] + 101}`);
        data.push({
            "id": current_student["id"] + 101,
            "firstname": "New",
            "lastname": "Student"
        });
        sessionStorage.setItem("studentData", JSON.stringify(data));
        let html = "";
        data.map(stu => {
            html += `<div class="list-group-item" id="${stu.id}">
                        ${stu.firstname}, ${stu.lastname}
                    </div>`;
        });
        $("#studentList").html(html);
    }); //add button

    $("#removebutton").on("click", e => {
        if (data.length > 0) {
            const current_student = data[data.length - 1];
            data.splice(-1, 1);
            $("#results").text(`removing student ${current_student["id"]}`);
            sessionStorage.setItem("studentData", JSON.stringify(data));
            let html = "";
            data.map(stu => {
                html += `<div class="list-group-item" id="${stu.id}">
                            ${stu.firstname}, ${stu.lastname}
                        </div>`;
            });
            $("#studentList").html(html);
        }
        else {
            $("#results").text("no students to remove");
        }
    }); //clear button

});
