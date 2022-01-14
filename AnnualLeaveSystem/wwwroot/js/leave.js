$(function () {

    $('#startDate').datepicker({
        format: 'dd.mm.yyyy',
        startDate: '-0d',
        autoclose: true
    }).on("changeDate", function (ev) {
        $('#endDate').datepicker("setStartDate", ev.date);
        startDate = ParseDate($('#StartDateInput').val());
        if (startDate > ParseDate($('#EndDateInput').val())) {
            $('#EndDateInput').val($('#StartDateInput').val())
            ValidateIsHoliday($('#EndDateInput').val(), allHolidays, "endDateSpanMessage")
        }
        endDate = ParseDate($('#EndDateInput').val());


        $.get('api/holidays', (data) => {
            ValidateIsHoliday(ev.target.children[1].value, data, 'startDateSpanMessage');

            $('#TotalDays').val(GetBusinessDatesCount(startDate, endDate, data));
        });
    });


    $('#endDate').datepicker({
        format: 'dd.mm.yyyy',
        autoclose: true,
        startDate: '-0d'
    }).on("changeDate", function (ev) {
        $('#startDate').datepicker("setEndDate", ev.date);
        startDate = ParseDate($('#StartDateInput').val());
        endDate = ParseDate($('#EndDateInput').val());


        $.get('api/holidays', (data) => {
            ValidateIsHoliday(ev.target.children[1].value, data, 'endDateSpanMessage');
            $('#TotalDays').val(GetBusinessDatesCount(startDate, endDate, data));
        });


    });

    var allHolidays = [];
    $.get('api/holidays', (data) => {
        data.forEach(day => allHolidays.push(day));
        var startDate = ParseDate($('#StartDateInput').val());
        var endDate = ParseDate($('#EndDateInput').val());
        ValidateIsHoliday(startDate, data, 'endDateSpanMessage');
        ValidateIsHoliday(endDate, data, 'endDateSpanMessage');
        $('#TotalDays').val(GetBusinessDatesCount(startDate, endDate, data));

    });

});

function ValidateIsHoliday(date, holidays, span) {
    var isNotHoliday = true;
    var currentHolidayName;

    for (var i = 0; i < holidays.length; i++) {
        var currentDate = holidays[i];

        if (currentDate.date == date) {
            currentHolidayName = currentDate.name;
            $('#' + span).text("This date is official holiday (" + currentDate.name + ").")
            $('#submitBtn').attr('disabled', true);
            isNotHoliday = false;
            break;
        }
    }

    if (isNotHoliday) {
        $('#' + span).text().replace("This date is official holiday (" + currentHolidayName + ").", "");
        $('#submitBtn').attr('disabled', false);
    }
}

function GetBusinessDatesCount(startDate, endDate, holidays) {
    let count = 0;
    const curDate = new Date(startDate.getTime());
    while (curDate <= endDate) {
        const dayOfWeek = curDate.getDay();
        if (dayOfWeek != 0 && dayOfWeek != 6) count++;
        curDate.setDate(curDate.getDate() + 1);
    }

    holidays.forEach(day => {
        if (ParseDate(day.date) >= startDate &&
            ParseDate(day.date) <= endDate &&
            (ParseDate(day.date).getDay() % 6) != 0) {
            count--;
        }
    })
    return count;
}

function ParseDate(input) {
    var parts = input.match(/(\d+)/g);
    return new Date(parts[2], parts[1] - 1, parts[0]); // months are 0-based
}