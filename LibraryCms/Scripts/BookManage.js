function bookUpload() {
    layer.open({
        type: 2,
        title: '书籍上传',
        shadeClose: true,
        shade: 0.7,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '600px'],
        content: '/Admin/BookUpload'
    });
}

function GetBook() {
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
    },"JSON");
}