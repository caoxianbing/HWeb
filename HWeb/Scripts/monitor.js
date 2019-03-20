var deviceList = new Array(), markerList = new Array();
var curId = 0, timer = 20, hisStart, lockTimer, w2;
$().ready(function () {
    var _id = GetQueryString("id");
    _id && (curId = _id);
    initMap("m_all")
    loadDevices(1, 20);
    layer.open({
        type: 1
      , title: '设备列表'
      , area: ['260px', '460px']
      , shade: 0
      , maxmin: true
      , offset: [20, 20]
      , closeBtn: false
      , content: $("#ma_div").html()
      , zIndex: layer.zIndex //重点1
      , success: function (layero) {
          layer.setTop(layero); //重点2
      }
        , min: function () {
            $("#layui-layer1 .layui-layer-max").show();
        }
        , restore: function () {
            $("#layui-layer1 .layui-layer-max").hide();
        }
    });
    $("#layui-layer1 .layui-layer-max").hide();

    dataReflsh();
    $("#sp_con").click(function () {
        if ($(this).attr("data-val") == "0") {
            clearInterval(lockTimer);
            $(this).html('<i class="layui-icon">&#xe651;</i>');
            layer.msg("停止自动刷新");
            $(this).attr("data-val", "1");
        } else {
            dataReflsh();
            $(this).html('<i class="layui-icon">&#xe652;</i>');
            layer.msg("开始自动刷新");
            $(this).attr("data-val", "0");

        }
    });
});

function dataReflsh() {
    //定时刷新，20s刷一次
    lockTimer = setInterval(function () {
        timer--;
        $("#m_timer").text(timer + " S");
        if (timer <= 0) {
            timer = 20;
            loadDevices(1, 20);
        }
    }, 1000);
}

function loadDevices(pi, pc) {
    $.post("/Device/AjaxGetMonitorList", { pi: pi, pc: pc }, function (result) {
        if (isOk(result)) {
            var _ul = $("#mal_list>ul");
            _ul.html("");
            var isCenter = false;
            deviceList = new Array();
            markerList = new Array();
            map.clearOverlays();
            layer.close();
            $.each(result.Items, function (index, item) {
                if (!item.UserName) item.UserName = item.IMEI.substring(item.IMEI.length - 6, item.IMEI.length);
               
                var _sexHtml = "";
                if (item.Sex == 1) {
                    _sexHtml= "<i class='layui-icon' style='color:#1E9FFF;font-size:14px;'>&#xe662;</i>";
                }
                else if (item.Sex == 0) {
                    _sexHtml = "<i class='layui-icon' style='color:#f69ed1;font-size:14px;'>&#xe661;</i>";
                }
                var _unHtml = "<span style='font-size:13px;margin-left:0px;'>" + item.UserName + "</span>";
                if (item.UserId <= 0) {
                    _unHtml = "<span class='li_imei'>未绑定</span>";
                }
                var _li = "<li class='ma_dli dli_" + item.Id + "' onclick='clickDevice(" + item.Id + ")'>" + _unHtml+"&nbsp;&nbsp;&nbsp;&nbsp;" + _sexHtml+"<br/>" + "<span class='li_imei'>"+item.IMEI + "</span>" + getStatus(item.Status) + "</li>";
                _ul.append(_li);
                //添加标注
                markerList["m" + item.Id] = addMark(item.Longitude, item.Latitude, getIcon(item.Status, item.Icon));
                markerList["m" + item.Id].addEventListener("click", function () {
                    clickDevice(item.Id);
                });
                deviceList["d" + item.Id] = item;
                if (!isCenter && item.Latitude > 0) {
                    isCenter = true;
                    map.centerAndZoom(new BMap.Point(item.Longitude, item.Latitude), 11);
                }
                if (getUserType() == 4)
                    curId = item.Id;
            });
            if (curId > 0) {
                clickDevice(curId);
            }
        } else {
            $("#mal_list").html("<span style='color:red;'><i class='layui_icon'>&#xe664;</i> 加载失败！</span>");
        }
    });
}
//设备列表单击事件
function clickDevice(id) {
    $(".ma_dli").removeClass("ma_dli_on");
    $(".dli_" + id).addClass("ma_dli_on");
    curId = id;
    var _device = deviceList["d" + id];
    var _marker = markerList["m" + id];
    var point = _marker.getPosition()
    map.centerAndZoom(point, 12);
    addMapWindow(_marker, _device.UserName, getHtml(_device));
    getAddress(point, "#a_spana");
}

function openHistory(t) {
    parent.window.addTab($(t));
}

function getHtml(d) {
    if (!d) return "";
    //设置同步
    $.ajaxSetup({
        async: false
    });
    var _html = $("#ma_dtemple").html();
    _html = _html.replace("{status}", getStatus(d.Status))
        .replace("{positiontime}", d.DeviceUtcDate)
        .replace(/{id}/g, d.Id).replace('{imei}', d.UserName || d.IMEI);
    $.post("/Device/AjaxGetHealthInfo", { deviceId: d.Id }, function (result) {
        if (!isOk(result)) {

        } else {
            var data = result.Item;
            _html = _html.replace("{heartbate}", data.HeartRate)
                .replace("{distance}", data.Distance)
                .replace("{step}", data.Step)
                .replace("{datatime}", data.LastUpdateTime);
        }
    });
    //设回异步
    $.ajaxSetup({
        async: true
    });
    return _html;
}

function showUser(id, imei) {
    var _l2 = layer.load();
    $.post("/User/AJaxGetUserInfo", { deviceId: id }, function (result) {
        layer.close(_l2);
        if (isOk(result)) {
            w2 = layer.open({
                title: imei + ' 的信息',
                type: 1,
                offset: ['50px', '320px'],
                content: $("#d_detail"),
                area: ['700px', '380px;']
            });
            $(".dd_img").attr("src", "/Other/GetImg?id=" + result.Data.HeadImgId);
            $("#sp_ln").text(result.Data.LoginName);
            $("#sp_phone").text("Tel:" + result.Data.PhoneNum);
            $("#txt_un").val(result.Data.UserName);
            $("#txt_email").val(result.Data.Email);
            $("#txt_sex").val(result.Data.Sex == "1" ? "男" : "女");
            $("#txt_bd").val(result.Data.BirthDate);
            $("#txt_info").text(result.Data.UserInfo);

        } else {
            layer.msg(result.Message);
        }
    });
}

function getIcon(s, icon) {
    var str = "";
    if (s == 1 || s == 2) {
        str = icon;
    }
    else {
        str = icon + "_off";
    }
    return str = "../../Content/icon/" + str + ".png";
}

//在线状态
function getStatus(s) {
    var str = "";
    if (s == 0) {
        str = "<span style='color:#c2c2c2;'>未启用</span>";
    }
    else if (s == 1) {
        str = "<span style='color:#009688;'>在线</span>";
    }
    else if (s == 2) {
        str = "<span style='color:#009688;'>在线</span>";
    }
    else if (s == 3) {
        str = "<span style='color:#c2c2c2;'>离线</span>";
    } else {
        str = "<span style='color:#c2c2c2;'>未知</span>";
    }
    return str;
}

function loadHistory() {
    var _id = curId;
    map.clear
    var load1 = layer.load();
    $.post("/Device/AjaxGetHistory", { deviceId: _id, start: getDateZero(), end: getDate() }, function (result) {
        layer.close(load1);
        if (isOk(result)) {
            if (result.data.length == 0) {
                layer.msg("今天没有运动轨迹！");
            } else {
                var arrPois = [];
                $.each(result.data, function (index, item) {
                    arrPois[index] = new BMap.Point(item.Lng, item.Lat);
                });
                hisStart = result.data[0];
                baiduHistory(arrPois);
            }
        } else {
            layer.alert("获取轨迹失败", { icon: 5 });
        }
    });
}

function baiduHistory(arrPois) {
    //  map.addOverlay(new BMap.Polyline(arrPois, { strokeColor: '#555' }));

    drawPolyline(arrPois);
    var sMark = addMark(arrPois[0].lng, arrPois[0].lat, "../../Content/icon/start.png");

    var hisLabel = new BMap.Label("", { offset: new BMap.Size(-60, -70) });
    hisLabel.setStyle({
        border: "1px solid #8d8d8d",
        padding: "8px",
        fontSize: "13px",
        borderRadius: "5px"
    });
    sMark.setLabel(hisLabel);
    hisLabel.setContent("<b>起点</b><br/>经纬度：" + hisStart.Lat.toString().substring(0, 7) + "," + hisStart.Lng.toString().substring(0, 8) + "<br/>" + "时间：" + hisStart.Time);
}
