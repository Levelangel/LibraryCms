function GetBook2() {
    var strSearch = $('#txtSearch').val();
    if (strSearch === '') {
        dialog.error('请输入查找的条件！');
        return;
    }
    strSearch = $.trim(strSearch);
    var url = "/Admin/GetBook";
    var data = { 'strSearch': strSearch };

    $.post(url, data, function (res) {
        var result = document.getElementById('result');
        var ul = result.getElementsByTagName('ul')[0];
        ul.innerHTML = '';
        if (res !== "" && res !== "No Rights") {
            console.log(res);
        } else {
            if (res === "") { //没有查找到结果              
                dialog.worning('没有查找到符合的结果！');
            }
        }
    }, "JSON");
}

$(document).ready(function () {
    $('#fil').change(function () {
        var str = $(this).val();
        var arr = str.split('\\');//注split可以用字符或字符串分割 
        var fileName = arr[arr.length - 1];//这就是要取得的文件名称
        if (fileName != "") {
            $('#filename').text(fileName);
            $('#filename').addClass("filenamehover");
        } else {
            $('#filename').text("未选择任何文件");
            $('#filename').removeClass("filenamehover");
        }
        
    });
});