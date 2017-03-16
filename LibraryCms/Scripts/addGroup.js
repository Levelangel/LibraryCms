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
    groupName = $.trim(groupName);
    var url = "/Admin/GetRole";
    var data = { 'strSearch': groupName, 'isEx':'true' };

    $.post(url, data, function (res) {
        if (res !== "") {       
            dialog.error('即将添加的用户组已经存在！');
            return;
        }
        var options = document.getElementById("addDiv").getElementsByTagName("select")[0].getElementsByTagName("option");
        var DepartmentId = "";
        var DepartmentType = "";
        for (var i = 0; i < options.length; i++) {
            if (options[i].selected) {
                var tmp = options[i].value;
                DepartmentType = tmp[0];
                DepartmentId = tmp.substring(1);
                break;
            }
        }
        var checkboxs = document.getElementById("addDiv").getElementsByTagName("div")[0].getElementsByTagName("input");
        var tmpRight = '';
        for (var j = 0; j < checkboxs.length; j++) {
            if (checkboxs[j].type == "checkbox" && checkboxs[j].checked) {
                tmpRight += "1";
            } else {
                tmpRight += "0";
            }
        }
        var retData = {
            'groupName': groupName,
            'departmentType': DepartmentType,
            'departmentId': DepartmentId,
            'rights': tmpRight
        };
        $.post("/Admin/AddGroup", retData, function (ret) {
            if (ret == "success") {
                dialog.success("添加成功", null, "成功", closeThis);
            } else {
                dialog.error("添加失败");
            }
        }, "JSON");

    }, "JSON");

    //closeThis();
}