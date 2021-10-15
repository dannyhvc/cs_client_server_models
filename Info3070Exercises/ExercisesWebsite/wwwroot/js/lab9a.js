$(function () {
    $("#getbutton").click(async (e) => {
        try {
            let lastname = $("#TextBoxFindLastname").val();
            $("#status").text("please wait...");
            $("#myModal").modal("toggle");
            let response = await fetch(`/api/student/${lastname}`);
            if (response.ok) {
                let student = await response.json();
                if (student.lastname !== "not found") {
                    $("#TextBoxEmail").val(student.email);
                    $("#TextBoxTitle").val(student.title);
                    $("#TextBoxFirstname").val(student.firstname);
                    $("#TextBoxLastname").val(student.lastname);
                    $("#TextBoxPhone").val(student.phoneno);
                    $("#status").text("student found!");

                    // return theses non mutated values later
                    sessionStorage.setItem("id", student.id);
                    sessionStorage.setItem("divisionId", student.divisionId);
                    sessionStorage.setItem("timer", student.timer);
                }
                else {
                    $("#TextBoxEmail").val("not found");
                    $("#TextBoxTitle").val("");
                    $("#TextBoxFirstname").val("");
                    $("#TextBoxLastname").val("");
                    $("#TextBoxPhone").val("");
                    $("##status").text("no such student");
                }
            }
            else if (response.status !== 404) {
                let problem = await response.json();
                error_rtn(problem, response.status);
            }
            else {
                $("#status").text("no such path on server");
            }
        } catch (error) {
            $("#status").text("problem client side, see browser console");
            console.log(error.message);
        }
    });

    $("#updatebutton").click(async e => {
        try {
            // set up a new client side instance of student
            student = new Object();

            // populate the properties of the student
            student.title = $("#TextBoxTitle").val();
            student.firstname = $("#TextBoxFirstname").val();
            student.lastname = $("#TextBoxLastname").val();
            student.email = $("#TextBoxEmail").val();
            student.phoneno = $("#TextBoxPhone").val();
            student.divisionName = "";
            student.picture64 = "";

            // stored earlier, numbers needed for Ids or we get http 401
            student.id = parseInt(sessionStorage.getItem("id"));
            student.divisionId = parseInt(sessionStorage.getItem("divisionId"));
            student.timer = sessionStorage.getItem("timer");

            let response = await fetch(`/api/student`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(student)
            });

            if (response.ok) {
                let payload = await response.json();
                $("#status").text(payload["msg"]);
            }
            else if (response.status !== 404) {
                let problem = await response.json();
                error_rtn(problem, response.status);
            }
            else {
                $("#status").text("no such path on server");
            }
        }
        catch (error) {
            $("#status").text("problem client side, see browser console");
            console.log(error.message);
        }
    });

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
