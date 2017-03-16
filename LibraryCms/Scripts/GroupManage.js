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