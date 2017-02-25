function SearchDepartment() {
    var strSearch = $('#txtSearch').val();
    strSearch = $.trim(strSearch);
    var url = "/Admin/GetDepartment";
    var data = { 'strSearch': strSearch };

    $.post(url, data, function (res) {
        var result = document.getElementById('result');
        var ul = result.getElementsByTagName('ul')[0];
        ul.innerHTML = '';
        if (res !== "" && res !== "No Rights") {
            var resArr = res.split("|");
            for (var i = 0; i < resArr.length; i++)
            {
                var obj = strToJson(resArr[i]); //Json字符串转Json对象
                var tmpStr = '';
                tmpStr += '<li><span>' + obj.DepartmentName + '</span>属于：';
                tmpStr += '<input class="RadioX" value="X" name="Department' + i + '" type="radio"';
                if (obj.DepartmentType === 0) {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>系<input class="RadioB" value="B" name="Department' + i + '" type="radio"';
                if (obj.DepartmentType === 1) {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>部<input class="RadioA" value="A" name="Department' + i + '" type="radio"';
                if (obj.DepartmentType === 2) {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>管理员<a name="btnRight" href="javascript:;">修改</a>';
                tmpStr += '<a name="btnLeft" href="javascript:;">删除</a></li>';
                ul.innerHTML += tmpStr;
                
            }
            for (var i = 0; i < resArr.length; i++) {
                var li = ul.getElementsByTagName("li")[i];
                li.getElementsByTagName("a")[0].onclick = modifyDept;//right
                li.getElementsByTagName("a")[1].onclick = deleteDept;//left
            }
            
        } else {
            if (res === "") { //没有查找到结果
                dialog.worning('没有查找到符合的结果！');
            }
        }
    }, "JSON");
}

var isAddingDepartment = false;

function addDepartment() {
    if (isAddingDepartment === true) {
        return;
    }
    isAddingDepartment = true;
    $('.add').removeClass("display");
    var i = 0;
    var time = setInterval(function () {
        i += 0.2;
        $(".add").css("opacity", i);
        $(".add").css("filter", 'alpha(opacity = ' + i*100 + ')');
        $(".add").css("-moz-opacity", i);       
        if (i >= 1) {
            clearInterval(time);
        }
    }, 50);    
}

function checkAdd() {
    isAddingDepartment = false;
    var departmentName = $('input[name = "DepartmentName"]').val();
    if (departmentName == "") {
        dialog.worning("部门名不能为空", "提醒");
        return;
    }
    var result = document.getElementById("result");
    var divAdd = result.lastElementChild;
    var rbuttons = divAdd.childNodes;
    var departmentType = '';
   
    for (var i = 0; i < rbuttons.length; i++) {
        if (rbuttons[i].nodeName = "INPUT" && rbuttons[i].checked) {
            departmentType = rbuttons[i].value;
        }
    }
    if (departmentType == "") {
        dialog.worning("部门类型不能为空", "提醒");
        return;
    }
    var url = "/Admin/GetDepartment";
    var Searchdata = { 'strSearch': departmentName };
    $.post(url, Searchdata, function (res) {
        if (res !== "" && res !== "No Rights") {
            dialog.error("您所添加的部门已经存在。", "提醒");
        } else {
            url = "/Admin/AddDepartment";
            var data = { "DepartmentName": departmentName, "DepartmentType": departmentType };
            $.post(url, data, function (res) {
                if (res == "success") {
                    dialog.success("添加成功", null, null, function () { });
                }
                else {
                    dialog.error("添加失败","错误");
                }
                var i = 1;
                var time = setInterval(function () {
                    $(".add").css("opacity", i);
                    $(".add").css("filter", 'alpha(opacity = ' + i * 100 + ')');
                    $(".add").css("-moz-opacity", i);
                    i -= 0.2;
                    if (i <= 0) {
                        clearInterval(time);
                        $(".add").css("opacity", 0);
                        $(".add").css("filter", 'alpha(opacity = 0)');
                        $(".add").css("-moz-opacity", 0);
                        $('.add').addClass("display");
                        var result = document.getElementById("result");
                        var divAdd = result.lastElementChild;
                        var rbuttons = divAdd.childNodes;

                        for (var j = 0; j < rbuttons.length; j++) {
                            if (rbuttons[j].nodeName = "INPUT" && rbuttons[j].checked) {
                                rbuttons[j].checked = false;
                            }
                        }
                        $('input[name = "DepartmentName"]').val("");
                    }
                }, 50);
            });
        }
    },"JSON");

}

function cancelAdd() {
    isAddingDepartment = false;
    var i = 1;
    var time = setInterval(function () {
        $(".add").css("opacity", i);
        $(".add").css("filter", 'alpha(opacity = ' + i * 100 + ')');
        $(".add").css("-moz-opacity", i);
        i -= 0.2;
        if (i <= 0) {
            clearInterval(time);
            $(".add").css("opacity", 0);
            $(".add").css("filter", 'alpha(opacity = 0)');
            $(".add").css("-moz-opacity", 0);
            $('.add').addClass("display");
            var result = document.getElementById("result");
            var divAdd = result.lastElementChild;
            var rbuttons = divAdd.childNodes;

            for (var j = 0; j < rbuttons.length; j++) {
                if (rbuttons[j].nodeName = "INPUT" && rbuttons[j].checked) {
                    rbuttons[j].checked = false;
                }
            }
            $('input[name = "DepartmentName"]').val("");
        }
    }, 50);
}

var oriDeptName = '';
var oriDeptType = '';
var modifyStatus = false;
function modifyDept(event) {
    if (modifyStatus == true) {
        dialog.worning("请先完成上一个修改", "提醒");
        return;
    }
    modifyStatus = true;
    var result = document.getElementById('result');
    var ul = result.getElementsByTagName('ul')[0];
    var li = event.target.parentNode;
    var span = li.getElementsByTagName('span')[0];
    oriDeptName = span.innerText;
    var rbuttons = li.getElementsByTagName('input');
    for (var j = 0; j < rbuttons.length; j++) {
        if (rbuttons[j].nodeName = "INPUT" && rbuttons[j].checked) {
            oriDeptType = rbuttons[j].value;
        }
    }
    span.innerHTML = '<input type="text" name="deptNameToModify" value="' + oriDeptName + '" placeholder="部门名称" />';
    li.getElementsByTagName('a')[0].onclick = cancelModifyDept;//right
    li.getElementsByTagName('a')[0].innerText = "取消";
    li.getElementsByTagName('a')[1].onclick = checkModifyDept; //left
    li.getElementsByTagName('a')[1].innerText = "保存";
}

function deleteDept(event) {
    dialog.worning_question("正在进行的操作将是不可逆的，确定要继续吗？", "警告",
        function yes() {
            var url = '/Admin/DeleteDepartment';
            var li = event.target.parentNode;
            var span = li.getElementsByTagName('span')[0];
            var deptName = span.innerText;
            var rbuttons = li.getElementsByTagName('input');
            var deptType = '';
            for (var j = 0; j < rbuttons.length; j++) {
                if (rbuttons[j].nodeName = "INPUT" && rbuttons[j].checked) {
                    deptType = rbuttons[j].value;
                }
            }
            var data = { "DepartmentName": deptName, "DepartmentType": deptType };
            $.post(url, data, function (res) {
                if (res == "success") {
                    dialog.success("部门删除成功");
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
                    dialog.error("部门删除失败，原因是：" + res,"出错啦");
                }
            },"JSON");
        },
        function no() {
            return;
        }
    );
}

function cancelModifyDept(event) {
    var result = document.getElementById('result');
    var ul = result.getElementsByTagName('ul')[0];
    var li = event.target.parentNode;
    var span = li.getElementsByTagName('span')[0];
    span.innerHTML = '';
    span.innerText = oriDeptName;
    li.getElementsByTagName('a')[0].onclick = modifyDept;//right
    li.getElementsByTagName('a')[0].innerText = "修改";
    li.getElementsByTagName('a')[1].onclick = deleteDept; //left
    li.getElementsByTagName('a')[1].innerText = "删除";
    modifyStatus = false;
}

function checkModifyDept() { //确认修改部门

    var result = document.getElementById('result');
    var ul = result.getElementsByTagName('ul')[0];
    var li = event.target.parentNode;
    var span = li.getElementsByTagName('span')[0];
    var deptName = span.getElementsByTagName('input')[0].value;
    span.innerHTML = '';
    var rbuttons = li.getElementsByTagName('input');
    var deptType = '';
    for (var j = 0; j < rbuttons.length; j++) {
        if (rbuttons[j].nodeName = "INPUT" && rbuttons[j].checked) {
            deptType = rbuttons[j].value;
        }
    }
    span.innerText = deptName;
    var url = '/Admin/ModifyDepartment';
    var data = { "oriDeptName": oriDeptName, "oriDeptType": oriDeptType, "deptName": deptName, "deptType": deptType };
    $.post(url, data, function (res) {
        if (res == "success") {
            dialog.success("部门修改成功");
        }
        else {
            dialog.error("部门修改失败，原因是：" + res, "出错啦");
        }
    },"JSON");
    li.getElementsByTagName('a')[0].onclick = modifyDept;//right
    li.getElementsByTagName('a')[0].innerText = "修改";
    li.getElementsByTagName('a')[1].onclick = deleteDept; //left
    li.getElementsByTagName('a')[1].innerText = "删除";
    modifyStatus = false;
}