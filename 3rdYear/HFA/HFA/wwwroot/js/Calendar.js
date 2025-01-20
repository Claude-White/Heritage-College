const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

let getEvents = async function () {
    // Get the events through a fetch
    // @todo: switch to ajax and jQuery for consistency
    var RESTResources = JSON.parse(document.getElementById('RESTResources').getAttribute('data-resources'));
    const response = await fetch(RESTResources["get"]);
    const events = await response.json();
    return events;
}//getEvents

let formatDate = function (date) {
    return monthNames[date.getMonth()] + " " + date.getDate() + ", " + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes().toString().padStart(2, '0');
}

let formatEvent = function (event) {
    let date = new Date(event.start)
    return "<strong>" + event.title + "</strong>: " + formatDate(date);
}

let formatJSONEvents = function (events) {
    let formattedEvents = new Array();
    events.forEach(event => {
        let newEvent = {
            title: event.subject,
            start: event.start.dateTime,
            end: event.end.dateTime,
            color: "#ff0000"
        };
        formattedEvents.push(newEvent);
    });
    return formattedEvents;
}

document.addEventListener('DOMContentLoaded', async function () {

    //create the calendar object
    const calendarEl = document.getElementById('calendar');
    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        displayEventTime: false
    });

    //display events in the calendar
    var events = formatJSONEvents(await getEvents());
    events.forEach(event => {
        calendar.addEvent(event)
    });

    //display the events in the list
    const listOfEvents = document.getElementById("listOfEvents");
    events.forEach(event => {
        var eventListItem = document.createElement("li");
        eventListItem.innerHTML = formatEvent(event);
        listOfEvents.appendChild(eventListItem);
    });

    //render the calendar object created
    calendar.render()
})

