function login() {
    var account = $('#account').val();
    var password = $('#password').val();
    if (!account) {
        alert('用户名不可为空！');
        return;
    }
    if (!password) {
        alert('密码不可为空');
        return;
    }
    var url = "/Account/Check";
    var data = { 'account': account, 'password': password };
    $.post(url, data, function (result) {
        if (result.status === "1") { 
            //登陆成功
            window.location.href = "/Admin";
        }
        if (result.status === "0") {
            //登陆失败
            alert(result.message);
        }
    }, "JSON");
}

$(function () {
    $("#account").focus();

    var PageHeight = $(window).height();

    $("#title").css("margin-top", (PageHeight - 350) / 2);

    $("#password").keydown(function (e) {
           var e = e || window.event || arguments.callee.caller.arguments[0];
           if (e && e.keyCode == 13) {/*按下Enter*/
               login();
           }
    });
    var ul = document.getElementById("personalinfo");
    if (ul != null) { 
        var a = ul.getElementsByTagName("a")[0];
        a.onclick = updateUserInfo;
    }
    ul = document.getElementById("safety");
    if (ul != null) {
        var li = ul.getElementsByClassName("pic-mail")[0];
        var btn_a = li.getElementsByTagName("a")[0];
        btn_a.onclick = updateMail;
        li = ul.getElementsByClassName("pic-password")[0];
        btn_a = li.getElementsByTagName("a")[0];
        btn_a.onclick = updatePassword;
    }
    var mailType = document.getElementById("mailType");
    if (mailType != null) {
        var spans = mailType.getElementsByTagName("span");
        spans[0].onclick = sendMessage;
        spans[1].onclick = mailNotRead;
        spans[2].onclick = mailHasRead;
        spans[3].onclick = mailHasSent;
        spans[1].click();
    }

});

function mailNotRead() {
    var spans = document.getElementById("mailType").getElementsByTagName("span");
    for (var i = 1; i < spans.length; i++) {
        $(spans[i]).removeClass("check");
    }
    $(this).addClass("check");
    var url = "/Account/GetUserMessage";
    var data = {"msgType":0};
    $.post(url, data, function (res) {
        var ul = document.getElementById("PersonalMail");
        ul.innerHTML = "";
        var tmp = '';
        for (var j = 0; j < res.length; j++) {
            tmp += '<li><span>来自：<a name="sendTo" href="javascript:;">' + res[j].from + '</a>';
            tmp += '</span><span class="title">主题：' + res[j].msg.Subject;
            tmp += '</span><br/><p class="content">内容：' + res[j].msg.Content;
            tmp += '<span></span></p></li>';
        }
        ul.innerHTML = tmp;
        var lis = ul.getElementsByTagName("li");
        for (var j = 0; j < lis.length; j++) {
            var a = lis[j].getElementsByTagName("a")[0];
            a.onclick = sendMessageTo;
        }
    }, "JSON");
}

function mailHasSent() {
    var spans = document.getElementById("mailType").getElementsByTagName("span");
    for (var i = 1; i < spans.length; i++) {
        $(spans[i]).removeClass("check");
    }
    $(this).addClass("check");
    var url = "/Account/GetUserMessage";
    var data = { "msgType": 1 };
    $.post(url, data, function (res) {
        var ul = document.getElementById("PersonalMail");
        ul.innerHTML = "";
        var tmp = '';
        for (var j = 0; j < res.length; j++) {
            tmp += '<li><span>来自：' + res[j].from;
            tmp += '</span><span class="title">主题：' + res[j].msg.Subject;
            tmp += '</span><br/><p class="content">内容：' + res[j].msg.Content;
            tmp += '<span></span></p></li>';
        }
        ul.innerHTML = tmp;
    }, "JSON");
}

function mailHasRead() {
    var spans = document.getElementById("mailType").getElementsByTagName("span");
    for (var i = 1; i < spans.length; i++) {
        $(spans[i]).removeClass("check");
    }
    $(this).addClass("check");
    var url = "/Account/GetUserMessage";
    var data = { "msgType": 2 };
    $.post(url, data, function (res) {
        var ul = document.getElementById("PersonalMail");
        ul.innerHTML = "";
        var tmp = '';
        for (var j = 0; j < res.length; j++) {
            tmp += '<li><span>来自：' + res[j].from;
            tmp += '</span><span class="title">主题：' + res[j].msg.Subject;
            tmp += '</span><br/><p class="content">内容：' + res[j].msg.Content;
            tmp += '<span></span></p></li>';
        }
        ul.innerHTML = tmp;
    }, "JSON");
}

function logout() {
    window.location.href = "/Account/Logout";
}

function updateUserInfo() {
    var ul = document.getElementById("personalinfo");
    var li_text = ul.getElementsByTagName("li")[0].innerText;
    var number = li_text.substring(li_text.length - 8);
    var inputs = ul.getElementsByTagName("input");
    var name = '';
    var qq = '';
    var sex = '';
    for (var j = 0; j < inputs.length; j++) {
        if (inputs[j].type == "text") {
            if (inputs[j].name == "name") {
                name = inputs[j].value;
            }
            if (inputs[j].name == "QQ") {
                qq = inputs[j].value;
            }
        }
        if (inputs[j].type == "radio" && inputs[j].checked) {
            sex = inputs[j].value;
        }
    }
    if (name == "") {
        dialog.error("姓名不可为空", "提醒");
        return;
    }
    var url = '/Account/UpdateUserInfo';
    var data = { "number": number, "name": name, "qq": qq, "sex": sex };
    $.post(url, data, function (res) {
        if(res == "success") {
            dialog.success("更新成功",null,null,null);
        }
        else {
            dialog.error("更新个人信息失败，错误原因为：" + res,"出错啦");
        }
    },"JSON");
}

function updateMail() {
    layer.open({
        type: 2,
        title: '更新邮箱',
        shadeClose: true,
        shade: 0.7,
        area: ['600px', '300px'],
        content: '/Account/UpdateMail'
    });
}

function updatePassword() {
    layer.open({
        type: 2,
        title: '更改密码',
        shadeClose: true,
        shade: 0.7,
        area: ['600px', '360px'],
        content: '/Account/UpdatePassword'
    });
}

function sendMessage() {
    layer.open({
        type: 2,
        title: '发送消息',
        shadeClose: true,
        shade: 0.7,
        area: ['600px', '400px'],
        content: '/Account/SendMessage'
    });
}

function sendMessageTo(event) {
    var target_a = event.target;
    layer.open({
        type: 2,
        title: '发送消息',
        shadeClose: true,
        shade: 0.7,
        area: ['600px', '400px'],
        content: '/Account/SendMessage/' + target_a.innerText
    });
}