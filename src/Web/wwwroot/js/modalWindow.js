"use strict";
$(function () {
    $.ajaxSetup({ cache: false });
    $(".delete").click(function (e) {
        console.log("feee");
        e.preventDefault();
        $.get(this.href, function (data) {
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        });
    });
})