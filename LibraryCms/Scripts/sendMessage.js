function closeThis() { //关闭当前层
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}


$(function () {
    var a = document.getElementsByTagName("a")[0];
    a.onclick = sendMessage;
});

function sendMessage(event) {
    var sendTo = $('input[name="sendTo"]').val().trim();
    var subject = $('input[name="subject"]').val().trim();
    var detail = $('textarea[name="detail"').val().trim();
    if (sendTo == "") {
        dialog.worning('"发送给"文本框不能为空');
        return;
    }
    if (subject == "") {
        dialog.worning('"主题"文本框不能为空');
        return;
    }
    var data = {
        'sendTo': sendTo,
        'subject': subject,
        'detail': detail
    };
    var url = '/Account/SendTo';
    $.post(url, data, function (res) {
        if (res == "success") {
            dialog.success("消息发送成功", null, null, closeThis);
        } else {
            if (res == "same one") {
                dialog.error("不能给自己发消息呢");
                return;
            }
            dialog.error("发送错误，原因是："+res);
        }
    },"JSON");
}