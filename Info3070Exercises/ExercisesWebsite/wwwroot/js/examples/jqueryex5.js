$(function () {
    let data;

    $("#loadbutton").on("click", async e => {
        if (sessionStorage.getItem("studentData") === null) {
            const url = "https://raw.githubusercontent.com/elauersen/info3070/master/jqueryex5.json";
            $("#results")
                .text("Location student data on github please wait...");

            try {
                let res = await fetch(url);
                if (!res.ok)
                    throw new Error(`${res.status} ${res.statusText}`);
                data = await res.json();
                sessionStorage.setItem("studentData", JSON.stringify(data));
                $("#results").text("Student data loaded");
            }
            catch (error) {
                $("#results").text(error.message);
            }
        }
        else {
            data = JSON.parse(sessionStorage.getItem("studentData"));
        }

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
