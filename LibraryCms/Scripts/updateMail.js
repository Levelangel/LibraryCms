function closeThis() { //关闭当前层
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}

$(function () {
    var div = document.getElementById("newMail");
    var input = div.getElementsByTagName("input")[0];
    var a = div.getElementsByTagName("a")[0];
    a.onclick = function () {
        if (input.value.trim() == "") {
            showTip('邮箱地址不能为空', input);
            return;
        }
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (reg.test(input.value.trim())) {//合法
            a.innerText = "正在发送确认邮件...";
            var url = '/Account/UpdateMail';
            var data = { "newMail": input.value.trim() };
            $.post(url, data, function (res) {
                if (res == "success") {
                    dialog.success("我们已经往您的邮箱发送了邮件，请按照邮件内容完成剩下的操作", null, "成功", closeThis);
                    a.innerText = "发送成功";
                }
                else {
                    dialog.error("发送邮件失败",null);
                }
            },"JSON");
        } else {
            showTip('邮箱地址格式不正确', input);
        }
    };
});
function showTip(msg, input) {
    layer.tips(msg, input, {
        tips: [1, '#FF5722'],
        time: 3000
    });
}