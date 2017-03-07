function GetUser() {
    var strSearch = $('#txtSearch').val();
    if (strSearch === '') {
        dialog.error('请输入查找的条件！');
        return;
    }
    strSearch = $.trim(strSearch);
    var url = "/Admin/GetUser";
    var data = { 'strSearch': strSearch };
    var RoleName;
    $.post(url, data, function (res) {
        var result = document.getElementById('result');
        var ul = result.getElementsByTagName('ul')[0];
        ul.innerHTML = '';
        if (res !== "" && res !== "No Rights") {
            var obj = res;//strToJson(res); //Json字符串转Json对象
            var tmpStr = '';
            RoleName = obj.Role.RoleName;
            tmpStr += '<li><span>学号：</span>' + obj.Number;
            tmpStr += '</li><li><span>用户组：</span><select>';
            tmpStr += ' </select></li><li><span>密码：</span><input id="txtPwd" type="password" value="';
            tmpStr += obj.Password;
            tmpStr += '" /></li><li><span>姓名：</span><input id="txtName" type="text" value="';
            tmpStr += obj.Name;
            tmpStr += '" /></li><li><span>性别：</span><input name="Sex" value="M" id="RadioMan" type="radio"';
            if (obj.Sex === 'M') {
                tmpStr += 'checked="checked" ';
            }
            tmpStr += '/>男<input name="Sex" value="W" id="RadioWoman" type="radio"';
            if (obj.Sex === 'W') {
                tmpStr += 'checked="checked" ';
            }
            tmpStr += '/>女</li><li><span>邮箱：</span><input id="txtMail" type="text" value="';
            tmpStr += obj.Mail;
            tmpStr += '" /></li><li><span>手机：</span><input id="txtPhone" type="text" value="';
            tmpStr += obj.Phone;
            tmpStr += '" /></li><li><span>QQ：</span><input id="txtQQ" type="text" value="';
            tmpStr += obj.QQ;
            tmpStr += '"/></li><li><a href="javascript:;" onclick="saveUserInfo()">保存修改</a></li>';
            ul.innerHTML = tmpStr;
            var txtPwd = document.getElementById("txtPwd");
            txtPwd.onfocus = function () {
                showTip("修改密码需要清空该文本框", $('#txtPwd'), 1);
            };
            GetRoles(RoleName);
        } else {
            if (res === "") { //没有查找到结果              
                dialog.worning('没有查找到符合的结果！');
            }
        }
    }, "JSON");
}

function GetRoles(RoleName) {
    var url = '/Admin/GetRole';
    var data = { 'strSearch': '' };
    $.post(url, data, function (res) {
        var select = document.getElementsByTagName("select")[0];
        var resArrs = res.split("|");
        var tmpStr = '';
        for (var i = 0; i < resArrs.length; i++) {
            var resArr = strToJson(resArrs[i]);
            tmpStr += '<option value="' + resArr.RoleId + '" ';
            if (resArr.RoleName === RoleName) {
                tmpStr += 'selected="selected"';
            }
            tmpStr += '>' + resArr.RoleName + '</option>';
        }
        select.innerHTML = tmpStr;
    }, "JSON");
}

function saveUserInfo() {
    var select = document.getElementsByTagName("select")[0];
    var options = select.getElementsByTagName("option");
    var roleId = '';
    for (var i = 0; i < options.length; i++) {
        if (options[i].selected) {
            roleId = options[i].value;
            break;
        }
    }
    var ul = document.getElementById("result").getElementsByTagName("ul")[0];
    var li = ul.getElementsByTagName("li")[0];
    var rbtnInputs = ul.getElementsByTagName("input");
    var sex = '';
    for (var j = 0; j < rbtnInputs.length; j++) {
        if (rbtnInputs[j].type == 'radio' && rbtnInputs[j].checked) {
            sex = rbtnInputs[j].value;
            break;
        }
    }
    var strTmp = li.innerText;
    var number = strTmp.substring(strTmp.length - 8);
    var url = '/Admin/UpdateUser';
    var data = {
        "number": number,
        "roleId": roleId,
        "password": $('#txtPwd').val(),
        "name": $('#txtName').val(),
        "sex": sex,
        "mail": $('#txtMail').val(),
        "phone": $('#txtPhone').val(),
        "qq": $('#txtQQ').val()
    };
    if ($('#txtPwd').val().trim() == "") {
        showTip("密码不能为空", $('#txtPwd'), 1);
        return;
    }
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    if ($('#txtMail').val().trim()!= "" && !reg.test($('#txtMail').val().trim())) {//邮箱不合法
        showTip("邮箱格式不正确", $('#txtMail'), 1);
        return;
    }
    reg = /^1[0-9]{10}$/;
    if ($('#txtPhone').val().trim() != "" && !reg.test($('#txtPhone').val().trim())) {//手机不合法
        showTip("手机号码格式不正确", $('#txtPhone'), 1);
        return;
    }
    $.post(url, data, function (res) {
        if (res == "success") {
            dialog.success("修改成功", '/Admin/AdvanceManage/UserManage');
        }
    }, "JSON");
}

function showTip(msg, input, cor) { 
    layer.tips(msg, input, {
        tips: [cor, '#FF5722'],
        time: 3000
    });
}