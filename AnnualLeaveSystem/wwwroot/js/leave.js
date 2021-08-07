$(function () {
    //var modelDate = new Date('@Model.StartDate');

    $('#startDate').datepicker({
        format: 'dd.mm.yyyy',
        startDate: '-0d',
        //date: moment(),
        autoclose: true
    }).on("changeDate", function (ev) {
        $('#endDate').datepicker("setStartDate", ev.date);
        startDate = parseDate($('#StartDateInput').val());
        if (startDate > parseDate($('#EndDateInput').val())) {
            $('#EndDateInput').val($('#StartDateInput').val())
        }
        endDate = parseDate($('#EndDateInput').val());

        

        $.get('api/holidays', (data) => {
            ValidateDate(ev, data, 'startDateSpanMessage');
            $('#TotalDays').val(getBusinessDatesCount(startDate, endDate, data));
        });
    });

    //var d = new Date();
    //var now = new Date(d.getFullYear(), d.getMonth(), d.getDate());

    // $('#startDate').datepicker('update', now);

    $('#endDate').datepicker({
        format: 'dd.mm.yyyy',
        autoclose: true,
        //date: moment(),
        startDate: '-0d'
    }).on("changeDate", function (ev) {
        $('#startDate').datepicker("setEndDate", ev.date);
        startDate = parseDate($('#StartDateInput').val());
        endDate = parseDate($('#EndDateInput').val());


        $.get('api/holidays', (data) => {
            ValidateDate(ev, data, 'endDateSpanMessage');
            $('#TotalDays').val(getBusinessDatesCount(startDate, endDate, data));
        });


    });


    var startDate = parseDate($('#StartDateInput').val());
    var endDate = parseDate($('#EndDateInput').val());
    //$('#TotalDays').val(getBusinessDatesCount(startDate, endDate));




});

function ValidateDate(ev, data, span) {
    var startDate = ev.target.children[1].value;
    var isValid = true;

    for (var i = 0; i < data.length; i++) {
        var currentDate = data[i];

        if (currentDate.date == startDate) {
            $('#' + span).text("This date is official holiday (" + currentDate.name + ").")
            $('#submitBtn').attr('disabled', true);
            isValid = false;
            break;
        }
    }

    if (isValid) {
        $('#' + span).text("")
        $('#submitBtn').attr('disabled', false);
    }
}

function getBusinessDatesCount(startDate, endDate, holidays) {
    let count = 0;
    const curDate = new Date(startDate.getTime());
    while (curDate <= endDate) {
        const dayOfWeek = curDate.getDay();
        if (dayOfWeek != 0 && dayOfWeek != 6) count++;
        curDate.setDate(curDate.getDate() + 1);
    }

    holidays.forEach(day => {
        if (parseDate(day.date) >= startDate &&
            parseDate(day.date) <= endDate &&
            (parseDate(day.date).getDay() % 6) != 0) {
            count--;
        }
    })
    return count;
}

function parseDate(input) {
    // Transform date from text to date
    var parts = input.match(/(\d+)/g);
    // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[2], parts[1] - 1, parts[0]); // months are 0-based
}