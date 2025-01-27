
const calendarContainer = $('#calendar');
const timeSelectorContainer = $('#time-selector-container');
const warningText = $('#warning-text');
const selectedDatetimeInput = $('#selected-datetime');
const dateTimeInput = $('#DateTime'); // Hidden input for selected date and time

// Function to load the calendar
const loadCalendar = () => {
    $.getJSON('/CustomerAppointment/GetCalendarDays', function (days) {
        if (!days || days.length === 0) {
            return;
        }

        calendarContainer.empty(); // Clear existing content

        days.forEach(day => {
            const date = new Date(day.date); // Ensure correct parsing of date
            const formattedDate = date.toLocaleDateString(); // Format the date for display

            const dateDiv = $('<div>')
                .addClass('calendar-day col-3 p-2 mb-2 text-center')
                .text(formattedDate)
                .css({
                    backgroundColor: day.isAvailable ? 'green' : 'red',
                    color: 'white',
                    cursor: day.isAvailable ? 'pointer' : 'default',
                })
                .attr('data-date', date.toISOString().split('T')[0]) // Use ISO format
                .toggleClass('selectable', day.isAvailable)
                .click(function () {
                    if ($(this).hasClass('selectable')) {
                        $('.calendar-day').removeClass('bg-primary'); // Reset previously selected
                        $(this).addClass('bg-primary'); // Highlight selected day
                        loadTimeSelector(date.toISOString().split('T')[0]);
                    }
                });

            calendarContainer.append(dateDiv);
        });
    }).fail(function (jqxhr, textStatus, error) {
        warningText.text("Failed to load calendar data.");
    });
};

// Function to load the time selector
const loadTimeSelector = (selectedDate) => {
    const timeSelector = $('#time-selector');
    const warningText = $('#warning-text');

    // Show the time selector container
    timeSelectorContainer.css('display', 'block');

    // Initialize available time slots (10:00 to 21:00, 20-minute increments)
    const startHour = 10;
    const endHour = 21;
    const timeSlots = [];
    for (let hour = startHour; hour < endHour; hour++) {
        for (let minute = 0; minute < 60; minute += 20) {
            const time = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
            timeSlots.push(time);
        }
    }

    // Fetch reserved times for the selected date
    $.getJSON(`/CustomerAppointment/GetReservedTimes?date=${selectedDate}`, function (reservedTimes) {

        // Filter out reserved times from the available slots
        const availableTimes = timeSlots.filter(slot => {
            const slotTime = new Date(`1970-01-01T${slot}:00`);
            return !reservedTimes.some(reserved => {
                const reservedStart = new Date(`1970-01-01T${reserved.startTime}:00`);
                const reservedEnd = new Date(`1970-01-01T${reserved.endTime}:00`);
                return slotTime >= reservedStart && slotTime < reservedEnd;
            });
        });

        // Render the available times in the selector
        timeSelector.empty();
        if (availableTimes.length === 0) {
            warningText.text("No available time slots for the selected date.");
        } else {
            warningText.text(""); // Clear warning
            availableTimes.forEach(time => {
                const timeOption = $('<div>')
                    .addClass('time-slot card p-2 mb-2')
                    .text(time)
                    .css({
                        backgroundColor: 'green',
                        color: 'white',
                        cursor: 'pointer'
                    })
                    .click(function () {
                        // Set the selected datetime in the hidden input
                        dateTimeInput.val(`${selectedDate} ${time}`);
                        $('.time-slot').css('backgroundColor', 'green'); // Reset others
                        $(this).css('backgroundColor', 'blue'); // Highlight selected time
                    });
                timeSelector.append(timeOption);
            });
        }
    }).fail(function (jqxhr, textStatus, error) {
        warningText.text("Failed to load reserved times.");
    });
};

// Initialize the calendar on page load
$(document).ready(function () {
    loadCalendar();
});
