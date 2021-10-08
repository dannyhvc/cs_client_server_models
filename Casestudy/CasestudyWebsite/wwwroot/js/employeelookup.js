$(function() {
    $("#getButton").click(async (e) => {
        try {
            let email = $("#textboxEmail").val();
            $("#status").text("please wait...");
            let res = await fetch(`/api/employee/${email}`);
            if (res.ok) {
                let data = await res.json();
                if (data.lastname !== "not found") {
                    $("#title").text(data["title"]);
                    $("#firstname").text(data["firstname"]);
                    $("#lastname").text(data["lastname"]);
                    $("#phone").text(data["phoneno"]);
                    $("#status").text("employee found");
                }
                else {
                    $("#title").text("");
                    $("#firstname").text("not found");
                    $("#email").text("");
                    $("#phone").text("");
                    $("#status").text("no such employee");
                }
            }
            else if (res.status !== 404) {
                let problem_json = await res.json();
                error_rtn(problem_json, res.status);
            }
            else
                $("#status").text("no such path on server");
        }
        catch (err) {
            $("#status").text(err.message);
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