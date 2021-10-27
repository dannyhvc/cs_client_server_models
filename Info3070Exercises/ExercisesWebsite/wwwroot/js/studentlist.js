$(function () {
    const getAll = async (msg) => {
        try {
            $("#studentList").text("Finding Student Information");
            let res = await fetch(`/api/student`);
            if (res.ok) {
                let payload = await res.json();
                buildStudentList(payload);
                msg === ""
                    ? $("#status").text("Students Loaded")
                    : $("#status").text(`${msg} - Students Loaded`);
            }
        }
        catch (error) {
            $("#status").text(error.message);
        }
    };

    const clearModalFields = () => {
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("divisionId");
        sessionStorage.removeItem("timer");
    };

    $("#studentList").click(e => {
        clearModalFields();
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "studentList" || id === "") id = e.target.id;

        if (id !== "status" && id !== "heading") {
            let students = JSON.parse(sessionStorage.getItem("allStudents"));
            students.map(student => {
                if (student.id === parseInt(id)) {
                    $("#TextBoxTitle").val(student["title"]);
                    $("#TextBoxFirstname").val(student["firstname"]);
                    $("#TextBoxLastname").val(student["lastname"]);
                    $("#TextBoxPhone").val(student["phoneno"]);
                    $("#TextBoxEmail").val(student["email"]);
                    sessionStorage.setItem("id", student["id"]);
                    sessionStorage.setItem("divisionId", student["divisionId"]);
                    sessionStorage.setItem("timer", student["timer"]);
                    $("#modalstatus").text("update data");
                    $("#myModal").modal("toggle");
                }
            });
        } else {
            return false;
        }
    });

    $("#updatebutton").click(async e => {
        try {
            // set up a new client side instance of student
            student = new Object();

            // populate the properties of the student
            student["title"] = $("#TextBoxTitle").val();
            student["firstname"] = $("#TextBoxFirstname").val();
            student["lastname"] = $("#TextBoxLastname").val();
            student["phoneno"] = $("#TextBoxPhone").val();
            student["email"] = $("#TextBoxEmail").val();

            // stored earlier, numbers needed for Ids or we get http 401
            student["id"] = parseInt(sessionStorage.getItem("id"));
            student["divisionId"] = parseInt(sessionStorage.getItem("divisionId"));
            student["timer"] = sessionStorage.getItem("timer");
            student["picture64"] = null;

            let response = await fetch(`/api/student`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(student)
            });

            if (response.ok) {
                let payload = await response.json();
                getAll(payload["msg"]);
            } else if (response.status !== 404) {
                let problem = await response.json();
                error_rtn(problem, response.status);
            } else {
                $("#status").text(error.message);
            }
            $("#myModal").modal("toggle");
        }
        catch (error) {
            $("#status").text("problem client side, see browser console");
            console.log(error.message);
        }
    });

    const buildStudentList = (data) => {
        $("#studentList").empty();
        div = $(
            `<div class="list-group-item text-white bg-secondary row d-flex" id="status">Student info</div>
             <div class= "list-group-item row d-flex text-center" id="heading">
                <div class="col-4 h4">Title</div>
                <div class="col-4 h4">First</div>
                <div class="col-4 h4">Last</div>
             </div>`);
        div.appendTo($("#studentList"));
        sessionStorage.setItem("allStudents", JSON.stringify(data));
        data.map(student => {
            btn = $(`<button class="list-group-item row d-flex" id="${student["id"]}">`);
            btn.html(
                `<div class="col-4" id="studenttitle${student["id"]}">${student["title"]}</div>
                 <div class="col-4" id="studentfirstname${student["id"]}">${student["firstname"]}</div>
                 <div class="col-4" id="studentlastname${student["id"]}">${student["lastname"]}</div>`
            );
            btn.appendTo($("#studentList"));
        });
    };

    getAll("");
});

const error_rtn = (problem_json, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    }
    else {
        let keys = Object.keys(problem_json.errors);
        problem = {
            status: status,
            statusText: problem_json.errors[keys[0]][0],
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    }
};
