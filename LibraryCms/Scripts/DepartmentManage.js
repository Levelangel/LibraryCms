﻿function SearchDepartment() {
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
                tmpStr += '<input class="RadioX" name="Department' + i + '" type="radio"';
                if (obj.DepartmentType === 0) {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>系<input class="RadioB" name="Department' + i + '" type="radio"';
                if (obj.DepartmentType === 1) {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>部<input class="RadioA" name="Department' + i + '" type="radio"';
                if (obj.DepartmentType === 2) {
                    tmpStr += 'checked="checked"';
                }
                tmpStr += '/>管理员<a href="javascript:;">修改</a><a href="javascript:;">删除</a></li>';
                ul.innerHTML += tmpStr;
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