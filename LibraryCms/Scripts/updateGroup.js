function closeThis() { //关闭当前层
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}

$(function () {
    var a = document.getElementById("addDiv").getElementsByTagName("a")[0];
    a.onclick = a_click;
});

function a_click() {
    var groupName = document.getElementById("addDiv").getElementsByTagName("ul")[0].getElementsByTagName("input")[0].value;
    if ($.trim(groupName) === "") {
        dialog.worning('用户组名不能为空！');
        return;
    }
    var roleId = document.getElementById("roleid").getAttribute("roleid");
    groupName = $.trim(groupName);
    var checkurl = "/Admin/GetRole";
    var checkdata = { 'strSearch': groupName, 'isEx': 'true' };

    $.post(checkurl, checkdata, function (checkres) {
        if (checkres !== "") {
            var resArr = checkres.split("|");
            for (var i = 0; i < resArr.length; i++) {
                var obj = eval('(' + resArr[i] + ')');
                if (roleId != obj.RoleId) {
                    dialog.error('该用户组名已经存在！');
                    return;
                }
            }
        }
        var select = document.getElementsByTagName("select")[0];
        var options = select.getElementsByTagName("option");
        var deptTypeName = "";
        for (var i = 0; i < options.length; i++) {
            if (options[i].selected) {
                deptTypeName = options[i].value;
                break;
            }
        }
        var rightDiv = document.getElementById("addDiv").getElementsByTagName("div")[0];
        var inputs = rightDiv.getElementsByTagName("input");
        var rights = "";
        for (var j = 0; j < inputs.length; j++) {
            if (inputs[j].type == "checkbox" && inputs[j].checked) {
                rights += "1";
            } else {
                rights += "0";
            }
        }
        var data = {
            "roleId": roleId,
            "groupName": groupName,
            "departmentType": deptTypeName[0],
            "departmentId": deptTypeName.substring(1),
            "rights": rights
        };
        var url = '/Admin/UpdateGroup';
        $.post(url, data, function (res) {
            if (res == "success") {
                dialog.success("修改成功", null, "成功", closeThis);
            } else {
                dialog.error("修改失败");
            }
        }, "JSON");
    }, "JSON");
}