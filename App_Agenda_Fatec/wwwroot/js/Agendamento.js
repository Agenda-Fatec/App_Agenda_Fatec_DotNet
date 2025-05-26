// Constants and Variables:

var times = {

    morning: ["07:45:00", "08:35:00", "09:30:00", "10:20:00", "11:20:00", "12:10:00", "13:00:00"],

    night: ["17:20:00", "18:10:00", "19:00:00", "19:50:00", "20:50:00", "21:40:00", "22:30:00"]

}

// Functions:

function Define_Date()
{

    const months = {

        "Jan": "01",
        "Feb": "02",
        "Mar": "03",
        "Apr": "04",
        "May": "05",
        "Jun": "06",
        "Jul": "07",
        "Aug": "08",
        "Sep": "09",
        "Oct": "10",
        "Nov": "11",
        "Dec": "12"

    }

    const today = new Date(Date.now()).toString().split(" ");

    const date = `${today[3]}-${months[today[1]]}-${today[2]}`;

    document.getElementById("data_utilizacao").value = date;

    document.getElementById("data_utilizacao").min = date;

}

function Add_Times()
{

    let start_times = "";

    let end_times = "";

    const times_list = (document.getElementById("period").value === "morning") ? times.morning : times.night;

    for(let i = 0; i < times_list.length - 1; i++)
    {

        start_times = start_times.concat(`<option value="${times_list[i]}"> ${times_list[i]} </option>`);

        end_times = end_times.concat(`<option value="${times_list[i + 1]}"> ${times_list[i + 1]} </option>`);

    }

    document.getElementById("hora_inicio_utilizacao").innerHTML = "".concat(start_times);

    document.getElementById("hora_fim_utilizacao").innerHTML = "".concat(end_times);

}

// Events:

window.addEventListener("load", function() {

    Define_Date();

    Add_Times();

    document.querySelector("div").querySelectorAll("input").forEach(element => {

        if(element.type === "radio")
        {
    
            element.onchange = function() {
    
                document.getElementById("period").value = element.value;
        
                Add_Times();
        
            };
    
        }
    
    });

});

document.querySelector("form").onreset = function() {

    document.getElementById("period").value = "morning";

    Add_Times();

}