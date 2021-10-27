$(function () {
    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let res = await fetch(`/api/employee`);
            if (res.ok) {
                let payload = await res.json();
                buildEmployeeList(payload);
                msg === ""
                    ? $("#status").text("Employees Loaded")
                    : $("#status").text(`${msg} - Employees Loaded`);
            } else if (res.status !== 404) {
                let problemJson = await res.json();
                error_rtn(problemJson, res.status);
            } else {
                $("#status").text("no such path on server");
            }

            res = await fetch("/api/department");
            if (res.ok) {
                let divs = await res.json();
                sessionStorage.setItem("allDepartments", JSON.stringify(divs));
            } else if (res.status !== 404) {
                let problem = await res.json();
                error_rtn(problem, res.status);
            } else {
                $("#status").text("no such path on server");
            }

            res = await fetch("/api/problem");
            if (res.ok) {
                let divs = await res.json();
                sessionStorage.setItem("allProblems", JSON.stringify(divs));
            } else if (res.status !== 404) {
                let problem = await res.json();
                error_rtn(problem, res.status);
            } else {
                $("#status").text("no such path on server");
            }
        }
        catch (error) {
            $("#status").text(error.message);
        }
    };

    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update employee</h4>");
        $("#deletebutton").show();

        clearModalFields();
        data.map(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee["title"]);
                $("#TextBoxFirstname").val(employee["firstname"]);
                $("#TextBoxLastname").val(employee["lastname"]);
                $("#TextBoxPhone").val(employee["phoneno"]);
                $("#TextBoxEmail").val(employee["email"]);
                sessionStorage.setItem("id", employee["id"]);
                sessionStorage.setItem("departmentId", employee["departmentId"]);
                sessionStorage.setItem("departmentName", employee["departmentName"]);
                sessionStorage.setItem("timer", employee["timer"]);
                $("#modalstatus").text("update data");
                loadDepartmentDDL(employee["departmentId"]);
                $("#myModal").modal("toggle");
            }
        });


    };

    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employees");
        $("#myModalLabel").text("Add");
        $("#deletebutton").hide();
    };

    const clearModalFields = () => {
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("departmentId");
        sessionStorage.removeItem("departmentName");
        sessionStorage.removeItem("timer");
        $("#myModal").modal("toggle");
        loadDepartmentDDL(-1);
    };

    const add = async () => {
        try {
            // set up a new client side instance of employee
            employee = new Object();

            // populate the properties of the employee
            employee["title"] = $("#TextBoxTitle").val();
            employee["firstname"] = $("#TextBoxFirstname").val();
            employee["lastname"] = $("#TextBoxLastname").val();
            employee["phoneno"] = $("#TextBoxPhone").val();
            employee["email"] = $("#TextBoxEmail").val();

            // stored earlier, numbers needed for Ids or we get http 401
            employee["id"] = -1;
            employee["departmentId"] = 100;
            employee["timer"] = null;
            employee["isTech"] = false;
            employee["staffPicture64"] = null;

            let response = await fetch(`/api/employee`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(employee)
            });

            if (response.ok) {
                let payload = await response.json();
                getAll(payload["msg"]);
            } else if (response.status !== 404) {
                let problem = await response.json();
                error_rtn(problem, response.status);
            } else {
                $("#status").text("no such path on server");
            }
        }
        catch (error) {
            $("#status").text(error.message);
        }
        $("#myModal").modal("toggle");
    };

    const update = async () => {
        try {
            // set up a new client side instance of employee
            employee = new Object();

            // populate the properties of the employee
            employee["title"] = $("#TextBoxTitle").val();
            employee["firstname"] = $("#TextBoxFirstname").val();
            employee["lastname"] = $("#TextBoxLastname").val();
            employee["phoneno"] = $("#TextBoxPhone").val();
            employee["email"] = $("#TextBoxEmail").val();

            // stored earlier, numbers needed for Ids or we get http 401
            employee["id"] = parseInt(sessionStorage.getItem("id"));
            employee["departmentId"] = parseInt($("#ddlDepartments").val());
            employee["departmentName"] = null;
            employee["timer"] = sessionStorage.getItem("timer");
            employee["picture64"] = null;

            let response = await fetch(`/api/employee`, {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(employee)
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
        }
        catch (error) {
            $("#status").text("problem client side, see browser console");
        }
        $("#theModal").modal("toggle");
    };

    const _delete = async () => {
        try {
            // don't forget to add the first slash this took hours to find
            let response = await fetch(`/api/employee/${sessionStorage.getItem(`id`)}`, {
                method: `DELETE`,
                headers: { "Content-Type": "application/json; charset=utf-8" }
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $("#status").text(`Status - ${response.status}, Problem on delete server side, see server console`);
            }
            $("#myModal").modal('toggle');
        } catch (error) {
            $("#status").text(error.message);
        }
    };

    const loadDepartmentDDL = (empdiv) => {
        let html = '';
        $("#ddlDepartments").empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('allDepartments'));
        alldepartments.map(div => { html += `<option value="${div["id"]}">${div["name"]}</option>` });
        $("#ddlDepartments").append(html);
        $("#ddlDepartments").val(empdiv);
    };

    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    $("#deletebutton").click(() => {
        if (window.confirm("Are you sure"))
            _delete();
    });

    $("#employeeList").click(e => {
        clearModalFields();
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        }

        if (id !== "status" && id !== "heading") {
            let employees = JSON.parse(sessionStorage.getItem("allEmployees"));
            id === "0" ? setupForAdd() : setupForUpdate(id, employees);
        } else {
            return false;
        }
    });

    const buildEmployeeList = (data) => {
        $("#employeeList").empty();
        div = $(
            `<div class="list-group-item text-white bg-secondary row d-flex" id="status">Employee info</div>
             <div class= "list-group-item row d-flex text-center" id="heading">
                <div class="col-4 h4">Title</div>
                <div class="col-4 h4">First</div>
                <div class="col-4 h4">Last</div>
             </div>`);
        div.appendTo($("#employeeList"));
        sessionStorage.setItem("allEmployees", JSON.stringify(data));
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add employee</button>`)
        btn.appendTo($("#employeeList"));
        data.map(employee => {
            btn = $(`<button class="list-group-item row d-flex" id="${employee["id"]}">`);
            btn.html(
                `<div class="col-4" id="employeetitle${employee["id"]}">${employee["title"]}</div>
                 <div class="col-4" id="employeefirstname${employee["id"]}">${employee["firstname"]}</div>
                 <div class="col-4" id="employeelastname${employee["id"]}">${employee["lastname"]}</div>`
            );
            btn.appendTo($("#employeeList"));
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
