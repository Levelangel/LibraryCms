function strToJson(str) {
    var json = eval('(' + str + ')');
    return json;
}