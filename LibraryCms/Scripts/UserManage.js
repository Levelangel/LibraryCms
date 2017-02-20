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
            tmpStr += '" /></li><li><span>性别：</span><input name="Sex" id="RadioMan" type="radio"';
            if (obj.Sex === '男') {
                tmpStr += 'checked="checked" ';
            }
            tmpStr += '/>男<input name="Sex" id="RadioWoman" type="radio"';
            if (obj.Sex === '女') {
                tmpStr += 'checked="checked" ';
            }
            tmpStr += '/>女</li><li><span>邮箱：</span><input id="txtMail" type="text" value="';
            tmpStr += obj.Mail;
            tmpStr += '" /></li><li><span>手机：</span><input id="txtPhone" type="text" value="';
            tmpStr += obj.Phone;
            tmpStr += '" /></li><li><span>QQ：</span><input id="txtQQ" type="text" value="';
            tmpStr += obj.QQ;
            tmpStr += '"/></li><li><a href="javascript:;">保存修改</a></li>';
            ul.innerHTML = tmpStr;
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
        var resArr = res.split("|");
        var tmpStr = '';
        for (var i = 0; i < resArr.length; i++) {
            var resArr = strToJson(resArr[i]);
            tmpStr += '<option value="value" ';
            if (resArr.RoleName === RoleName) {
                tmpStr += 'selected="selected"';
            }
            tmpStr += '>' + resArr.RoleName + '</option>';
        }
        select.innerHTML = tmpStr;
    }, "JSON");
}