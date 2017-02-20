$(function() {
    var PageHeight = $(window).height();

    $("#frame").css("min-height", PageHeight - 40);
});

function strToJson(str) {
    var json = eval('(' + str + ')');
    return json;
}