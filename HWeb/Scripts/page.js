var $ = layui.$;
var noticeData;
//layui自定义验证规则
layui.form.verify({
    title: function (value) {
        if (value.length < 5) {
            return '标题至少得5个字符啊';
        }
    }
  , pass: [/(.+){6,12}$/, '密码必须6到12位']
  , loginName: function (value) {
      if (value.length < 1) {
          return '账号不能为空';
      }
  }
});

//通用监听提交
//obj.form layui.form
//obj.msg 提交等待的提示消息
//obj.falidFunc 失败处理事件
//obj.okFunc 成功处理事件
//obj.beforeSub 提交之前执行的方法
function formSub(obj) {
    //监听提交
    obj.form.on('submit(' + obj.btn + ')', function (data) {
        if (obj.beforeSub) {
            if (!obj.beforeSub()) {
                return false;
            }
        }
        var _that = this;
        $(_that).attr("disabled", "disabled");
        var _t = layer.msg(obj.msg, {
            icon: 16, shade: 0.01
        });
        ajaxForm(_that, data, function (result) {
            $(_that).removeAttr("disabled");
            layer.close(_t);
            if (!isOk(result)) {
                layer.alert(result.Message || "操作失败！", { icon: 5 });
                obj.falidFunc && obj.falidFunc();
            }
            else {
                obj.layer.msg(result.Message || "操作成功！", { icon: 1 });
                obj.okFunc && obj.okFunc(result);
            }
        });
        return false;
    });
}

//ajax提交表单数据
//submitBtn 提交按钮,  form.on('submit(btn_sub)' 中的data，提交成功后的方法
function ajaxForm(submitBtn, data, func) {
    var _form = $(submitBtn).parents("form");
    var _url = $(_form).attr("action");//获取表单地址
    var _method = $(_form).attr("method");//获取表单提交方式
    if (_method == "get") {
        $.get(_url, function (result) {
            func(result);
        });
    } else {
        $.post(_url, data.field, func);
    }
}
//统一验证返回是否正确
function isOk(res) {
    return res.State == 0;
}

function getUserName() {
    return $('#hid_name', parent.document).val();
}

//获取系统公告
function getNotice(func) {
    if (noticeData) {
        func(noticeData);
        return;
    }
    $.post("/Other/GetLastNotice", {}, function (result) {
        if (isOk(result)) {
            noticeData = result;
            func(result);
        } else {
            layer.msg("获取系统公告失败", { icon: 5 });
        }
    });
}
//获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//获取当前时间
function getDate(hour) {
    if (!hour)
        hour = 0;
    var myDate = new Date();//获取系统当前时间
    return myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + (myDate.getHours() + hour) + ":" + myDate.getMinutes() + ":" + myDate.getSeconds();
}

function getDateZero() {
    var myDate = new Date();//获取系统当前时间
    return myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " 00:00:00";
}
//根据utc时间获取当前时间
function getNowDate(date) {
    var d = new Date(date);
    var localTime = d.getTime();
    var localOffset = d.getTimezoneOffset() * 60000;   //getTimezoneOffset()返回是以分钟为单位，需要转化成ms
    var utc = localTime + localOffset;
    offset = 8;
    korean = utc + (3600000 * offset);
    nd = new Date(korean);
    return nd.toLocaleString();
}
//清除缓存
function clearCache() {
    window.sessionStorage.clear();
    window.localStorage.clear();
}
//处理ajax返回的数据
function proResult(result) {
    if (typeof result == "string") result = eval('(' + result + ')');
    return result;
}
function openTab(t) {
    parent.window.addTab($(t));
}
//修改用户头像
function changeHeadImg(id) {
    var src = "/Other/GetImg?id=" + id + "&rand=" + Math.random();
    $("#head_simg", parent.document).attr("src", src);
    $("#head_bimg", parent.document).attr("src", src);
}

function getUserType() {
    if (window.parent.document)
        return $("#hid_utype", window.parent.document).val();
    else return $("#hid_utype").val();
}

function getDevices(doc, page, limit) {
    //加载设备下拉
    $.post("/Device/AjaxGetDeviceList", { page: 1, limit: 1000 }, function (result) {
        $.each(result.data, function (index, item) {
            if (!item.UserName) item.UserName = item.Imei.substring(item.Imei.length - 6, item.Imei.length);

            if (getUserType() == 4) {
                var _o = '<option value="' + item.DeviceId + '" ' + ((curId == item.DeviceId) && "selected") + ' >' + item.Imei + '</option>';
                $(doc).html(_o);
            }
            else {
                var _o = '<option value="' + item.DeviceId + '" ' + ((curId == item.DeviceId) && "selected") + ' >' + item.UserName + '</option>';
                $(doc).append(_o);
            }
            form.render();
        });
    });
}
