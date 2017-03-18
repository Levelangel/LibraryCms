function GetRole() {
    var strSearch = $('#txtSearch').val();
    strSearch = $.trim(strSearch);
    var url = "/Admin/GetRole";
    var data = { 'strSearch': strSearch };

    $.post(url, data, function (res) {
        var result = document.getElementById('result');
        var ul = result.getElementsByTagName('ul')[0];
        ul.innerHTML = '';
        if (res !== "" && res !== "No Rights") {
            var resArr = res.split("|");
            for (var i = 0; i < resArr.length; i++) {
                var obj = strToJson(resArr[i]); //Json字符串转Json对象
                var tmpStr = '';
                tmpStr += '<li><span>' + obj.RoleName + '：';
                //if (obj.Department.DepartmentType === 0) {
                //    tmpStr += '系';
                //}
                //if (obj.Department.DepartmentType === 1) {
                //    tmpStr += '部';
                //}
                //if (obj.Department.DepartmentType === 2) {
                //    tmpStr += '管理员';
                //}
                tmpStr += obj.Department.DepartmentName;
                tmpStr += '</span>';
                tmpStr += '<div><input type="checkbox" ';
                if (obj.Rights[0] === '1') {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>书籍管理<input type="checkbox" ';
                if (obj.Rights[1] === '1') {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>书籍类别管理<br/><input type="checkbox" ';
                if (obj.Rights[2] === '1') {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>书籍题库<input type="checkbox" ';
                if (obj.Rights[3] === '1') {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>用户管理<br/><input type="checkbox" ';
                if (obj.Rights[4] === '1') {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>用户组管理<input type="checkbox" ';
                if (obj.Rights[5] === '1') {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>系部管理</div><a href="javascript:;">修改</a><a href="javascript:;">删除</a></li>';
                ul.innerHTML += tmpStr;
            }
            for (var j = 0; j < resArr.length; j++) {
                var li = ul.getElementsByTagName("li")[j];
                var btnModify = li.getElementsByTagName("a")[0];
                var btnDelete = li.getElementsByTagName("a")[1];
                btnDelete.onclick = deleteGroup;
            }
        } else {
            if (res === "") { //没有查找到结果              
                dialog.worning('没有查找到符合的结果！');
            }
        }
    },"JSON");
}

function addGroup() {
    layer.open({
        type: 2,
        title: '添加用户组',
        shadeClose: true,
        shade: 0.7,
        area: ['800px', '420px'],
        content: '/Admin/AddGroup'
    });
}

function deleteGroup(event) {
    dialog.worning_question("正在进行的操作将是不可逆的，而且会删除该用户组下的所有用户，确定要继续吗？", "警告",
        function yes() {
            var url = '/Admin/DeleteGroup';
            var li = event.target.parentNode;
            var span = li.getElementsByTagName('span')[0];
            var groupNameType = span.innerText;
            var groupName = groupNameType.split('：')[0];
            var chkButtons = li.getElementsByTagName('input');
            var rights = '';
            for (var j = 0; j < chkButtons.length; j++) {
                if (chkButtons[j].type = "checkbox" && chkButtons[j].checked) {
                    rights += "1";
                } else {
                    rights += "0";
                }
            }
            var data = { "strSearch": groupName, "isEx": 'true' };
            $.post(url, data, function (res) {
                if (res == "success") {
                    dialog.success("用户组删除成功");
                    var i = 1;
                    var time = setInterval(function () {
                        $(li).css("opacity", i);
                        $(li).css("filter", 'alpha(opacity = ' + i * 100 + ')');
                        $(li).css("-moz-opacity", i);
                        i -= 0.2;
                        if (i <= 0) {
                            clearInterval(time);
                            $(li).css("opacity", 0);
                            $(li).css("filter", 'alpha(opacity = 0)');
                            $(li).css("-moz-opacity", 0);
                            $(li).addClass("display");
                            $(li).remove();
                        }
                    }, 50);
                }
                else {
                    dialog.error("用户组删除失败，原因是：" + res, "出错啦");
                }
            }, "JSON");
        },
        function no() {
            return;
        }
    );
}