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

$(function() {
    var PageHeight = $(window).height();

    $("#main").css("margin-top", (PageHeight - 300) / 2);
});

function logout() {
    window.location.href = "/Account/Logout";
}