var routeURL = location.protocol + "//" + location.host;

$(document).ready(function () {
    $("#appointmentDate").kendoDateTimePicker({
        format: "yyyy/MM/dd hh:mm tt",
        value: new Date()

    });

    InitializeCalendar();
});

var calendar;

function InitializeCalendar() {

    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next,today',
                    center: 'title',
                    right: ''
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay:'block',
                events: function (fecthInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeURL + '/api/Appointment/GetCalendarData?supplierId=' + $("#supplierId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            var events = [];
                            if (response.status == 1) {
                                $.each(response.dataenum, function (i, d) {
                                    events.push({
                                        title: d.title,
                                        description: d.description,
                                        start: d.startDate,
                                        end: d.endDate,
                                        allDay: true,
                                        backgroundColor: d.supplierApprovedApt ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: d.id,
                                        role: d.role
                                    });
                                })
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
                }
            });
            calendar.render();
        }
    }
    catch (e) {
        alert(e);
    }
}

function onShowModal(obj, isEventDetail) {

    if (isEventDetail != null) {

        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#appointmentDate").val(obj.startDate);
        $("#duration").val(obj.duration);
        $("#supplierId").val(obj.supplierId);
        $("#coupleId").val(obj.coupleId);
        $("#id").val(obj.id);
        $("#lblCoupleName").html(obj.coupleName);
        $("#lblSupplierName").html(obj.supplierName);

        $("#lblStatus").parent().removeClass("d-none");

        if (obj.role !== "Admin") {
            $("#supplierId").parent().addClass("d-none");
            $(".appointmentDateAndTime").parent().addClass("d-none");
            $("#duration").parent().addClass("d-none");
        } else {
            $("#lblSupplierName").parent().removeClass("d-none");
            $("#supplierId").parent().removeClass("d-none");
            $("#btnSubmit").removeClass("d-none");
        }

        if (obj.role === "Supplier") {
            $('#btnConfirm').addClass("d-none");
        }


        if (obj.supplierApprovedApt) {
            $("#lblStatus").html('Approved');
            $('#btnConfirm').addClass("d-none");

            if (obj.role === 'Admin') {
                $('#btnSubmit').removeClass("d-none");
            } else {
                $('#btnSubmit').addClass("d-none");
            }
        }
        else {
            $("#lblStatus").html('Pending');
            $('#btnConfirm').removeClass("d-none");
            $('#btnSubmit').removeClass("d-none");
        }

        $("#btnDelete").removeClass("d-none");
    }
    else {
        $("#appointmentDate").val(obj.startStr + " " + new moment().format("hh:mm A"));
        $("#id").val(0);
        $("#btnDelete").addClass("d-none");
        $('#btnSubmit').removeClass("d-none");

        if (obj.role !== "Supplier") {
            $("#supplierId").parent().removeClass("d-none");
            $(".appointmentDateAndTime").parent().removeClass("d-none");
            $("#duration").parent().removeClass("d-none");
        }

    }
     $("#apptInput").modal("show");
 }

function onCloseModal() {
    $('#appointmentform')[0].reset();
    $('#id').val(0);
    $("#title").val('');
    $("#description").val('');
    $("#appointmentDate").val('');
    $("#apptInput").modal("hide");
}

function onSubmitForm() {

    if (checkValidation()) {

        var requestData = {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#appointmentDate").val(),
            Duration: $("#duration").val(),
            SupplierId: $("#supplierId").val(),
            CoupleId: $("#coupleId").val(),
        };


        $.ajax({
            url: routeURL + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status !== 0) {
                    calendar.refetchEvents();
                    $.notify(response.message, "success");
                    onCloseModal();
                } else {
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
}


function checkValidation() {
    var isValid = true;
    if ($('#title').val() === undefined || $('#title').val() === "") {
        isValid = false;
        $('#title').addClass('error');
    } else {
        $('#title').removeClass('error');
    }

    if ($('#appointmentDate').val() === undefined || $('#appointmentDate').val() === "") {
        isValid = false;
        $('#appointmentDate').addClass('error');
    } else {
        $('#appointmentDate').removeClass('error');
    }

    return isValid;
}

function getEventDetailsByEventId(info) {
    $.ajax({
        url: routeURL + '/api/Appointment/GetCalendarDataById/' + info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1 && response.dataenum != undefined) {
                onShowModal(response.dataenum, true); 
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onSupplierChange() {
    calendar.refetchEvents();
}

function onDeleteAppointment() {
    var id = parseInt($("#id").val());
;    $.ajax({
    url: routeURL + '/api/Appointment/DeleteAppointment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                onCloseModal();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onConfirm() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeURL + '/api/Appointment/ConfirmEvent/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                onCloseModal();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });

}