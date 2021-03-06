﻿
$(document).ready(function () {
    $('#fil').change(function () {
        var str = $(this).val();
        var arr = str.split('\\');//注split可以用字符或字符串分割 
        var fileName = arr[arr.length - 1];//这就是要取得的文件名称
        if (fileName != "") {
            $('#filenameText').text(fileName);
            $('#filename').addClass("filenamehover");
        } else {
            $('#filenameText').text("未选择任何文件");
            $('#filename').removeClass("filenamehover");
        }
    });
});

function startUpload() {
    strSearch = $.trim($('#filenameText').text());
    if (strSearch == "未选择任何文件") {
        dialog.worning("未选择任何文件，请先选择要上传的书籍", "提示");
        return;
    }
    if ($('input[name="bookName"]').val() == "") {
        $("#status").text("书籍名称不能为空");
        return;
    }
    if ($('input[name="author"]').val() == "") {
        $("#status").text("作者名不能为空");
        return;
    }
    if ($('input[name="pages"]').val() == "") {
        $("#status").text("书籍页数不能为空");
        return;
    }
    if ($('#format').val() == "") {
        $("#status").text("格式不能为空");
        return;
    }
    var url = "/Admin/GetBook";
    var data = { 'strSearch': strSearch };

    $.post(url, data, function (res) {
        if (res !== "") {
            dialog.question("已经有同名书籍存在，是否还要添加？", "确认", function () { }, function () { return; })
        }
        fileUpload(document.getElementById("fil").files[0]);
    }, "JSON");
}

function fileUpload(file) {
    var params = new FormData();
    params.append("tpUploadFile", file);
    params.append("bookName", $('input[name="bookName"]').val());
    params.append("author", $('input[name="author"]').val());
    params.append("publisher", $('input[name="publisher"]').val());
    params.append("pages", $('input[name="pages"]').val());
    if ($('input[name="publicTime"]').val() == "") {
        var date = new Date();
        params.append("publicTime", date.getFullYear() + "-" + date.getMonth() + 1);
    } else {
        params.append("publicTime", $('input[name="publicTime"]').val());
    }  
    params.append("format", ($('#format').val() == "txt" || $('#format').val() == "pdf") ? $('#format').val() : "txt");
    $.ajax({
        type: "POST",
        url: "/Admin/AjaxUpload",
        data: params,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result === "no file") {
                dialog.error("上传书籍丢失，请重新上传", "出错啦");
            } else if (result === "error") {
                dialog.error("上传过程中出错，请重新上传", "出错啦");
            } else {
                dialog.success("上传成功", null, null, function () { });
            }
        },
        error: function () {
            dialog.error("连接服务器出错","出错啦");
        },
        dataType: 'json'
    });
    $("#percent").css("width", 0);
    var timer = setInterval(function () {
        $.ajax({
            url: '/Admin/AjaxUploadPersent',
            type: 'POST',
            success: function (percent) {
                //console.log(percent);
                if (percent == "100") {
                    clearInterval(timer);
                    $("#percent").css("width", 520);
                    $("#filenameText").text("100%");
                    return;
                }
                $("#filenameText").text(percent.toFixed(2) + "%");
                $("#percent").css("width", percent.toFixed(2) * 520.0 / 100);
            },
            error: function () {
                clearInterval(timer);
                console.log('ajax error');
            }
        })
    }, 500);
}