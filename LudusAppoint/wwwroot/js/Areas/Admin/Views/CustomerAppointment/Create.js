
document.addEventListener('DOMContentLoaded', function ()
{
    const branchSelect = document.getElementById('branchSelect');
    const ageGroupSelect = document.getElementById('ageGroupSelect');
    const genderSelect = document.getElementById('genderSelect');
    const preliminaryInformationNextButton = document.getElementById('preliminaryInformationNextButton');

    function enablePreliminaryInformationNextButton()
    {
        if (branchSelect.value && ageGroupSelect.value && genderSelect.value)
        {
            preliminaryInformationNextButton.removeAttribute('disabled');
        }
    }

    branchSelect.addEventListener('change', enablePreliminaryInformationNextButton);
    ageGroupSelect.addEventListener('change', enablePreliminaryInformationNextButton);
    genderSelect.addEventListener('change', enablePreliminaryInformationNextButton);
});



document.addEventListener('DOMContentLoaded', function ()
{
    const branchSelect = document.getElementById('branchSelect');
    const ageGroupSelect = document.getElementById('ageGroupSelect');
    const genderSelect = document.getElementById('genderSelect');
    const offeredServicesAccordion = document.getElementById('offeredServicesAccordion');
    const offeredServicesButton = document.getElementById('offeredServicesButton');
    const preliminaryInformationNextButton = document.getElementById('preliminaryInformationNextButton'); // New button for triggering the check

    function checkPreliminarySelections()
    {
        if (branchSelect.value && ageGroupSelect.value && genderSelect.value)
        {
            // Enable and show Hairdressing Services Accordion
            offeredServicesAccordion.classList.remove('disabled'); // Enable accordion
            offeredServicesButton.click();
        }
    }

    preliminaryInformationNextButton.addEventListener('click', checkPreliminarySelections); // Bind click event to the button

    checkPreliminarySelections()
});



document.addEventListener('DOMContentLoaded', function ()
{
    const offeredServicesTableBody = document.querySelector('#offeredServicesTable tbody');
    const offeredServicesNextButton = document.getElementById('offeredServicesNextButton');

    let selectedofferedServiceIds = new Set(); // To track selected services

    function updateNextButtonState()
    {
        if (selectedofferedServiceIds.size > 0)
        {
            offeredServicesNextButton.removeAttribute('disabled');
        } else
        {
            offeredServicesNextButton.setAttribute('disabled', 'true');
        }
    }

    function bindCheckboxEvents()
    {
        const checkboxes = offeredServicesTableBody.querySelectorAll('input[type="checkbox"]');
        checkboxes.forEach(checkbox =>
        {
            checkbox.addEventListener('change', function ()
            {
                if (this.checked)
                {
                    selectedofferedServiceIds.add(this.value);
                } else
                {
                    selectedofferedServiceIds.delete(this.value);
                }
                updateNextButtonState();
            });
        });

        // Update button state on initial binding (in case there are preselected checkboxes)
        updateNextButtonState();
    }

    function fetchOfferedServices()
    {
        const ageGroupId = document.getElementById('ageGroupSelect').value;
        const genderValue = document.getElementById('genderSelect').value;

        if (ageGroupId && genderValue)
        {
            $.ajax({
                url: '/Admin/CustomerAppointment/GetOfferedServices', // Update with your controller's route
                type: 'GET',
                data: { genderValue, ageGroupId },
                beforeSend: function ()
                {
                    // Show the spinner before making the request
                    document.getElementById('offeredServicesCard').style.display = 'none';
                    document.getElementById('offeredServicesloadingSpinner').style.display = 'block';
                },
                success: function (data)
                {
                    document.getElementById('offeredServicesCard').style.display = 'block';
                    // Clear existing rows
                    offeredServicesTableBody.innerHTML = '';

                    if (Array.isArray(data) && data.length > 0)
                    {
                        document.getElementById('approximateDurationInputDiv').style.display = 'block';
                        // Populate table with new data

                        document.getElementById('ApproximateDuration').value = 0;
                        document.getElementById('Price').value = 0;
                        let totalDuration = 0;
                        let totalPrice = 0;

                        data.forEach(service =>
                        {
                            const isChecked = selectedofferedServiceIds.has(service.id.toString()) ? 'checked' : '';
                            const durationParts = service.approximateDuration.split(':'); // Split the duration string
                            const formattedDuration = `${durationParts[0]}:${durationParts[1]}`; // Format as HH:MM
                            const row = `
										<tr>
											<td>
												<div class="form-check d-flex justify-content-center align-items-center">
													<input class="form-check-input" type="checkbox" name="offeredServiceIds" value=${service.id} id="offeredService-${service.id}" ${isChecked}>
												</div>
											</td>
											<td>${service.name}</td>
											<td class="text-end pe-5" id="serviceApproximateDuration">${formattedDuration}</td>
											<td class="pe-5 text-end" id="servicePrice">${service.price}</td>
										</tr>
									`;
                            offeredServicesTableBody.insertAdjacentHTML('beforeend', row);

                            //setDurationAndPrice

                            if (isChecked == 'checked')
                            {
                                if (formattedDuration)
                                {
                                    const [hours, minutes] = formattedDuration.split(':').map(Number);
                                    totalDuration += (hours * 60) + minutes; // Convert to total minutes
                                    totalPrice += Number(service.price);
                                }
                            }
                        });

                        const totalHours = Math.floor(totalDuration / 60);
                        const totalMinutes = totalDuration % 60;
                        document.getElementById('ApproximateDuration').value = `${String(totalHours).padStart(2, '0')}:${String(totalMinutes).padStart(2, '0')}`;
                        document.getElementById('Price').value = totalPrice;

                        attachCheckboxListeners();
                        // Re-bind change event to checkboxes for tracking selections
                        bindCheckboxEvents();
                    } else
                    {
                        // Display a "No Data Available" message
                        document.getElementById('approximateDurationInputDiv').style.display = 'none';
                        offeredServicesTableBody.innerHTML = `
									<tr>
										<td colspan="4" class="text-center">@Localizer["NoOfferedServicesAvailable."] @Localizer["PleaseTryWithDiffrentAgeGroupOrGender."]</td>
									</tr>
								`;
                    }
                },
                error: function (xhr, status, error)
                {
                    document.getElementById('offeredServicesCard').style.display = 'block';
                    offeredServicesTableBody.innerHTML = `<tr>
																	   <td colspan="4" class="text-center">An error occurred while fetching data. Please try again later.</td>
																	   </tr>`;
                    console.error('Error fetching data:', error);
                },
                complete: function ()
                {
                    // Hide the spinner after the request is complete
                    document.getElementById('offeredServicesloadingSpinner').style.display = 'none';
                }
            });
        }
    }

    function attachCheckboxListeners()
    {
        // Get the table body and all dynamically added checkboxes
        const serviceCheckboxes = offeredServicesTableBody.querySelectorAll('input[type="checkbox"]');
        const approximateDurationInput = document.getElementById('ApproximateDuration');
        const priceInput = document.getElementById('Price');

        // Function to update the approximate duration
        function updateApproximateDuration()
        {
            let totalDuration = 0;
            let totalPrice = 0;

            serviceCheckboxes.forEach(checkbox =>
            {
                if (checkbox.checked)
                {
                    // Find the parent row of the checkbox
                    const row = checkbox.closest('tr');
                    // Find the cell containing the approximate duration
                    const durationCell = row.cells.namedItem("serviceApproximateDuration").innerHTML;
                    const priceCell = row.cells.namedItem("servicePrice").innerHTML;

                    if (durationCell)
                    {
                        const [hours, minutes] = durationCell.split(':').map(Number);
                        totalDuration += (hours * 60) + minutes; // Convert to total minutes
                        totalPrice += Number(priceCell);
                    }
                }
            });

            // Convert totalDuration back to HH:mm format
            const totalHours = Math.floor(totalDuration / 60);
            const totalMinutes = totalDuration % 60;
            approximateDurationInput.value = `${String(totalHours).padStart(2, '0')}:${String(totalMinutes).padStart(2, '0')}`;
            priceInput.value = totalPrice;

            document.getElementById('appointmentDateTimeAccordion').classList.add('disabled');
            document.getElementById('appointmentSummaryAccordion').classList.add('disabled');
        }

        // Attach event listeners to each checkbox
        serviceCheckboxes.forEach(checkbox =>
        {
            checkbox.addEventListener('change', updateApproximateDuration);
        });
    }

    // Bind the service fetching to changes in selects
    document.getElementById('ageGroupSelect').addEventListener('change', fetchOfferedServices);
    document.getElementById('genderSelect').addEventListener('change', fetchOfferedServices);

    // Initial fetch and binding
    fetchOfferedServices();
    attachCheckboxListeners();
});



document.addEventListener('DOMContentLoaded', function ()
{
    const offeredServicesNextButton = document.getElementById('offeredServicesNextButton');
    const appointmentInfoButton = document.getElementById('appointmentInfoButton');
    const appointmentInfoAccordion = document.getElementById('appointmentInfoAccordion');

    function checkOfferedServicesSelections()
    {
        appointmentInfoAccordion.classList.remove('disabled');
        appointmentInfoButton.click();
    }

    offeredServicesNextButton.addEventListener('click', checkOfferedServicesSelections);
});



branchSelect.addEventListener('change', function ()
{
    const selectedBranchId = branchSelect.value;
    const selectedServiceIds = Array.from(document.querySelectorAll('input[name="offeredServiceIds"]:checked')).map(input => parseInt(input.value)); // Convert to integers

    if (selectedBranchId && selectedServiceIds.length > 0)
    {
        $.ajax({
            url: '/Admin/CustomerAppointment/GetEmployees',
            type: 'GET',
            traditional: true,
            data: { branchId: selectedBranchId, offeredServiceIds: JSON.stringify(selectedServiceIds) },
            success: function (data)
            {
                employeeSelect.innerHTML = '<option selected hidden value="">@Localizer["SelectEmployee"]</option>';
                data.forEach(employee =>
                {
                    employeeSelect.insertAdjacentHTML('beforeend', `<option value="${employee.Id}">${employee.Name} ${employee.Surname}</option>`);
                });
            },
            error: function (xhr, status, error)
            {
                employeeSelect.innerHTML = `<option>${xhr.responseJSON.message}</option>`;
                console.error('Error fetching employees:', error);
            }
        });
    }
});



document.getElementById('offeredServicesTable').addEventListener('change', function ()
{
    const selectedBranchId = branchSelect.value;
    const selectedServiceIds = Array.from(document.querySelectorAll('input[name="offeredServiceIds"]:checked')).map(input => parseInt(input.value));

    if (selectedBranchId && selectedServiceIds.length > 0)
    {
        $.ajax({
            url: '/Admin/CustomerAppointment/GetEmployees',
            type: 'GET',
            traditional: true,
            data: { branchId: selectedBranchId, offeredServiceIds: JSON.stringify(selectedServiceIds) },
            success: function (data)
            {
                document.getElementById('appointmentInfoNextButton').disabled = true;
                employeeSelect.innerHTML = '<option selected hidden value="">@Localizer["SelectEmployee"]</option>';
                data.forEach(employee =>
                {
                    employeeSelect.insertAdjacentHTML('beforeend', `<option value="${employee.Id}">${employee.Name} ${employee.Surname}</option>`);
                });
            },
            error: function (xhr, status, error)
            {
                employeeSelect.innerHTML = `<option>${xhr.responseJSON.message}</option>`;
                console.error('Error fetching employees:', error);
            }
        });
    }
});



$(document).ready(function ()
{
    $('#offeredServicesTable').DataTable({
        columnDefs: [{
            "orderable": false,
            "targets": [0]
        }],
        responsive: true,
        paging: true,
        searching: true,
        ordering: true,
        lengthChange: true,
        language: {
            search: "@Localizer["Search"] : ",
            lengthMenu: "@Localizer["Show _MENU_ entries"]",
            info: "@Localizer["Showing _START_ to _END_ of _TOTAL_ entries"]",
            paginate: {
                previous: "@Localizer["Previous"]",
                next: "@Localizer["Next"]"
					}
        }
    });
});



document.addEventListener('DOMContentLoaded', function ()
{
    const appointmentInfoNextButton = document.getElementById('appointmentInfoNextButton');
    const collapseAppointmentInfo = document.getElementById('collapseAppointmentInfo');
    const inputs = collapseAppointmentInfo.querySelectorAll('input:not([type="email"]), select');

    function enableappointmentInfoNextButton()
    {
        const allFilled = Array.from(inputs).every(input => input.value);
        appointmentInfoNextButton.disabled = !allFilled;
    }

    inputs.forEach(input =>
    {
        input.addEventListener('input', enableappointmentInfoNextButton);
    });

    // Also check when employee options are populated
    const employeeSelect = document.getElementById('employeeSelect');
    employeeSelect.addEventListener('change', enableappointmentInfoNextButton);
});



document.addEventListener('DOMContentLoaded', function ()
{
    const appointmentInfoNextButton = document.getElementById('appointmentInfoNextButton');
    const appointmentDateTimeButton = document.getElementById('appointmentDateTimeButton');
    const appointmentDateTimeAccordion = document.getElementById('appointmentDateTimeAccordion');

    function appointmentDateTimeAccordionShow()
    {
        appointmentDateTimeAccordion.classList.remove('disabled');
        appointmentDateTimeButton.click();
    }

    appointmentInfoNextButton.addEventListener('click', appointmentDateTimeAccordionShow);
});


const apiUrlTemplate = `/Admin/CustomerAppointment/GetReservedDaysTimes?employeeId={employeeId}&branchId={branchId}`;

function parseTimeToMinutes(timeString) 
{
    const [hours, minutes] = timeString.split(":").map(Number);
    return hours * 60 + minutes;
}
// Fetch reserved data and update UI
function fetchReservedData()
{
    const employeeId = document.getElementById("employeeSelect").value;
    const branchId = document.getElementById("branchSelect").value;
    const approximateDurationValue = document.getElementById("ApproximateDuration").value;
    const approximateDuration = parseTimeToMinutes(approximateDurationValue);

    if (!employeeId || !branchId || approximateDuration <= 0)
    {
        disableCalendar();
        clearTimeSlots();
        return;
    }

    const apiUrl = apiUrlTemplate
        .replace("{employeeId}", employeeId)
        .replace("{branchId}", branchId);

    fetch(apiUrl)
        .then(response => response.json())
        .then(data =>
        {
            enableCalendar(data.reservationInAdvanceDayLimit);
            setupTimeSlots(data, approximateDuration);
        });
}

// Enable calendar and set date limits
function enableCalendar(dayLimit)
{
    const calendar = document.getElementById("calendar");
    const today = new Date().toISOString().split("T")[0];
    const limitDate = new Date();
    limitDate.setDate(limitDate.getDate() + dayLimit);

    calendar.disabled = false;
    calendar.min = today;
    calendar.max = limitDate.toISOString().split("T")[0];
    calendar.addEventListener("change", () => updateTimeSlots());
}

// Disable calendar and clear time slots
function disableCalendar()
{
    const calendar = document.getElementById("calendar");
    calendar.disabled = true;
    calendar.removeEventListener("change", updateTimeSlots);
    calendar.value = "";
}

// Clear time slots
function clearTimeSlots()
{
    const timeSlotsContainer = document.getElementById("time-slots");
    timeSlotsContainer.innerHTML = "";
    document.getElementById("appointmentDateTimeNextButton").setAttribute('disabled', 'true');
}

// Re-generate time slots when the selected date changes
function updateTimeSlots() 
{
    const employeeId = document.getElementById("employeeSelect").value;
    const branchId = document.getElementById("branchSelect").value;
    const approximateDurationValue = document.getElementById("ApproximateDuration").value;
    const approximateDuration = parseTimeToMinutes(approximateDurationValue);

    if (!employeeId || !branchId || approximateDuration <= 0)
    {
        clearTimeSlots();
        return;
    }

    // Fetch the reserved data again (you can cache it instead of re-fetching for optimization)
    const apiUrl = apiUrlTemplate
        .replace("{employeeId}", employeeId)
        .replace("{branchId}", branchId);

    fetch(apiUrl)
        .then(response => response.json())
        .then(data =>
        {
            setupTimeSlots(data, approximateDuration);
        });
}

// Generate time slots based on reserved data
function setupTimeSlots(data, approximateDuration)
{
    const calendar = document.getElementById("calendar");
    if (!calendar.value) return;

    const selectedDate = new Date(calendar.value);
    const timeSlotsContainer = document.getElementById("time-slots");
    clearTimeSlots();

    const reservedSlots = data.reservedDaysTimes.filter(slot =>
        new Date(slot.startDateTime).toDateString() === selectedDate.toDateString()
    );

    const leaves = data.employeeLeaves.filter(leave =>
        new Date(leave.startDateTime).toDateString() === selectedDate.toDateString()
    );

    const minimumServiceDuration = data.minimumServiceDuration; // Get the duration from JSON

    for (let hour = 0; hour < 24; hour++)
    {
        for (let minute = 0; minute < 60; minute += minimumServiceDuration)
        {
            const startTime = new Date(selectedDate);
            startTime.setHours(hour, minute, 0, 0);

            const endTime = new Date(startTime);
            endTime.setMinutes(endTime.getMinutes() + approximateDuration);

            if (!isTimeSlotAvailable(startTime, endTime, reservedSlots, leaves)) continue;

            const button = document.createElement("button");
            button.type = "button";
            button.className = "btn btn-outline-light";
            button.textContent = startTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
            button.dataset.time = `${startTime.toLocaleDateString()} ${startTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
            button.setAttribute("data-bs-toggle", "button"); // Bootstrap toggle

            // Add event listener to handle selection logic if needed
            button.addEventListener("click", (event) => 
            {
                const previouslySelected = timeSlotsContainer.querySelector(".active");
                if (previouslySelected && previouslySelected !== button)
                {
                    previouslySelected.classList.remove("active");
                }
                button.classList.toggle("active");
                document.getElementById("appointmentDateTimeNextButton").removeAttribute('disabled');
                if (event.target.tagName === "BUTTON" && event.target.dataset.time) 
                {
                    const selectedTime = event.target.dataset.time; // Get the ISO string from the button
                    document.getElementById("StartDateTime").value = selectedTime; // Set it in the hidden input
                }
            });

            timeSlotsContainer.appendChild(button);
        }
    }
}

// Check if a time slot is available
function isTimeSlotAvailable(startTime, endTime, reservedSlots, leaves)
{
    for (const slot of reservedSlots)
    {
        const reservedStart = new Date(slot.startDateTime);
        const reservedEnd = new Date(slot.endDateTime);

        if (
            (startTime >= reservedStart && startTime < reservedEnd) ||
            (endTime > reservedStart && endTime <= reservedEnd) ||
            (startTime <= reservedStart && endTime >= reservedEnd)
        )
        {
            return false;
        }
    }

    for (const leave of leaves)
    {
        const leaveStart = new Date(leave.startDateTime);
        const leaveEnd = new Date(leave.endDateTime);

        if (
            (startTime >= leaveStart && startTime < leaveEnd) ||
            (endTime > leaveStart && endTime <= leaveEnd) ||
            (startTime <= leaveStart && endTime >= leaveEnd)
        )
        {
            return false;
        }
    }

    return true;
}

// Attach event listeners for input changes
document.getElementById("employeeSelect").addEventListener("change", fetchReservedData);
document.getElementById("branchSelect").addEventListener("change", fetchReservedData);
document.getElementById("ApproximateDuration").addEventListener("input", fetchReservedData);


document.addEventListener('DOMContentLoaded', function ()
{
    const appointmentDateTimeNextButton = document.getElementById('appointmentDateTimeNextButton');
    const appointmentSummaryButton = document.getElementById('appointmentSummaryButton');
    const appointmentSummaryAccordion = document.getElementById('appointmentSummaryAccordion');

    function appointmentSummaryAccordionShow()
    {
        appointmentSummaryAccordion.classList.remove('disabled');
        appointmentSummaryButton.click();
    }

    appointmentDateTimeNextButton.addEventListener('click', appointmentSummaryAccordionShow);
});


document.addEventListener('DOMContentLoaded', function ()
{
    const summaryBranch = document.getElementById('summaryBranch');
    const summaryGender = document.getElementById('summaryGender');
    const summaryAgeGroup = document.getElementById('summaryAgeGroup');
    const summaryNameSurname = document.getElementById('summaryNameSurname');
    const summaryServices = document.getElementById('summaryServices');
    const summaryDuration = document.getElementById('summaryDuration');
    const summaryPrice = document.getElementById('summaryPrice');
    const summaryEmployee = document.getElementById('summaryEmployee');
    const summaryDateTime = document.getElementById('summaryDateTime');

    function updateSummary()
    {
        summaryBranch.textContent = document.getElementById('branchSelect').selectedOptions[0].text;
        summaryGender.textContent = document.getElementById('genderSelect').selectedOptions[0].text;
        summaryAgeGroup.textContent = document.getElementById('ageGroupSelect').selectedOptions[0].text;
        summaryNameSurname.textContent = document.getElementById('Name').value + " " + document.getElementById('Surname').value;

        const selectedServices = document.querySelectorAll('input[name="offeredServiceIds"]:checked');
        summaryServices.innerHTML = '';
        selectedServices.forEach(service =>
        {
            const serviceName = service.closest('tr').querySelector('td:nth-child(2)').textContent;
            const li = document.createElement('li');
            li.textContent = serviceName;
            summaryServices.appendChild(li);
        });

        summaryDuration.textContent = document.getElementById('ApproximateDuration').value;
        summaryPrice.textContent = document.getElementById('Price').value;
        summaryEmployee.textContent = document.getElementById('employeeSelect').selectedOptions[0].text;
        summaryDateTime.textContent = document.querySelector('#time-slots .active')?.dataset.time || '';
    }

    document.getElementById('appointmentDateTimeNextButton').addEventListener('click', updateSummary);
});


document.querySelector("form").addEventListener("submit", function (e)
{
    console.log("submit : " + emailInput.validity.valid);
    const emailInput = document.getElementById("EMail");
    if (emailInput.validity.valid === false)
    {
        console.log("emailInput.validity.valid : " + emailInput.validity.valid);
        const accordionItem = emailInput.closest(".accordion-item");
        if (accordionItem)
        {
            const button = accordionItem.querySelector("button[data-bs-toggle='collapse']");
            if (button && button.getAttribute("aria-expanded") === "false")
            {
                // Open the accordion section if it is collapsed
                button.click();
            }
        }
    }
});

