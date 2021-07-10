// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




$(function () {
    //$('.datepicker').datepicker({
    //    format: "dd.mm.yyyy",
    //    orientation: "top",
    //    autoclose: true
    //});
    var startDate = parseDate($('#StartDateInput').val());
    var endDate = parseDate($('#EndDateInput').val());
    $('#TotalDays').text(getBusinessDatesCount(startDate, endDate));


    $('#startDate').datepicker({
        format: 'dd.mm.yyyy',
        startDate: '-0d',
        autoclose: true
    }).on("changeDate", function (e) {
        $('#endDate').datepicker("setStartDate", e.date);
        startDate = parseDate($('#StartDateInput').val());
        endDate = parseDate($('#EndDateInput').val());
        $('#TotalDays').val(getBusinessDatesCount(startDate, endDate));
    });

    var d = new Date();
    var now = new Date(d.getFullYear(), d.getMonth(), d.getDate());

    $('#startDate').datepicker('update', now);

    $('#endDate').datepicker({
        format: 'dd.mm.yyyy',
        autoclose: true
    }).on("changeDate", function (e) {
        $('#startDate').datepicker("setEndDate", e.date);
        startDate = parseDate($('#StartDateInput').val());
        endDate = parseDate($('#EndDateInput').val());
        $('#TotalDays').val(getBusinessDatesCount(startDate, endDate));

    });
    $('#endDate').datepicker('update', now);

});



function getBusinessDatesCount(startDate, endDate) {
    let count = 0;
    const curDate = new Date(startDate.getTime());
    while (curDate <= endDate) {
        const dayOfWeek = curDate.getDay();
        if (!(dayOfWeek == 0 || dayOfWeek==6)) count++;
        curDate.setDate(curDate.getDate() + 1);
    }

    return count;
}

function parseDate(input) {
    // Transform date from text to date
    var parts = input.match(/(\d+)/g);
    // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[2], parts[1] - 1, parts[0]); // months are 0-based
}