function closeThis() { //关闭当前层
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}

$(function () {
    var div = document.getElementById("newPassword");
    var inputs = div.getElementsByTagName("input");
    var pwd1 = inputs[0];
    var pwd2 = inputs[1];
    var a = div.getElementsByTagName("a")[0];
    a.onclick = function () {
        if (pwd1.value.trim() == "") {
            showTip('密码不能为空', pwd1, 1);
            return;
        }
        if (pwd2.value.trim() !== pwd1.value.trim()) {
            showTip('密码不相同', pwd2, 3);
            return;
        }
        var url = '/Account/UpdatePassword';
        var data = { "newPassword": pwd1.value.trim() };
        $.post(url, data, function (res) {
            if (res == "success") {
                dialog.success("密码修改成功", null, "成功", closeThis);
            }
            else {
                dialog.error("密码修改失败，失败原因是："+res, null);
            }
        }, "JSON");
    };

    $(pwd1).keydown(function (e) {
        if (e.keyCode == 32) {
            event.returnValue = false;
        }
        pwd1.value = pwd1.value.trim();
    });
    $(pwd2).keydown(function (e) {
        if (e.keyCode == 32) {
            event.returnValue = false;
        }
        pwd2.value = pwd2.value.trim();
    });
});
function showTip(msg, input, cor) {
    layer.tips(msg, input, {
        tips: [cor, '#FF5722'],
        time: 3000
    });
}