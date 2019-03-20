var form = layui.form, laydate = layui.laydate, slider = layui.slider, element = layui.element;
var curId = 0, start, end, playTime, hisData, hisMarker, timer, hisI, hisLabel;
$().ready(function () {
    initMap("h_all");
    element.progress("pro_his", "0%");
    element.render();
    var _id = GetQueryString("id");
    _id && (curId = _id);
    ////加载设备下拉
    //$.post("/Device/AjaxGetDeviceList", { page: 1, limit: 1000 }, function (result) {
    //    $.each(result.data, function (index, item) {
    //        if (!item.UserName) item.UserName = item.Imei.substring(item.Imei.length - 6, item.Imei.length);
    //        var _o = '<option value="' + item.DeviceId + '" ' + ((curId == item.DeviceId) && "selected") + ' >' + item.UserName + '</option>';
    //        $("#sel_device").append(_o);
    //        form.render();
    //    });
    //});

    getDevices("#sel_device", 1, 1000);

    start = getDateZero();
    end = getDate();
    //日期时间范围
    laydate.render({
        elem: '#txt_time'
      , type: 'datetime'
      , range: '~'
      , value: getDateZero() + " ~ " + getDate()
      , max: getDate()
      , done: function (value, date, endDate) {
          start = value.split('~')[0];
          end = value.split('~')[1];
      }
    });

    playTime = 700;
    //垂直滑块
    slider.render({
        elem: '#slispeed'
      , type: 'vertical' //垂直滑块
      , step: 50,
        min: 50,
        max: 1050,
        value: 700,
        tips: false,
        change: function (value) {
            playTime = 1100 - value;
        }
    });
    form.render();
});

function stopOrStart(t) {
    if (isPlay()) {
        layer.msg('已暂停播放');
        clearInterval(timer);
        $(t).html("继&nbsp;&nbsp;续");
    } else {
        layer.msg('重新播放');
        $(t).html("暂&nbsp;&nbsp;停");
        movePoint(hisI);
    }
}

function isPlay() {
    return $("#btn_ss").text().indexOf("停") >= 0
}

function proClick(t) {
    if (hisData) {
        var _max = $(t).width();
        var _x = event.clientX;
        var _bf = parseInt(_x / _max * 100);
        element.progress("pro_his", _bf + "%");
        clearInterval(timer);
        hisI = parseInt(_x / _max * hisData.length);
        movePoint(hisI);
    }
}

function loadHistory() {
    var _id = $("#sel_device").val();
    if (_id <= 0) {
        layer.alert("请选择设备！", { icon: 5 });
        return;
    }
    $("#btn_ss").html("暂&nbsp;&nbsp;停");
    element.progress("pro_his", "0%");
    map.clearOverlays();
    clearInterval(timer);
    var load1 = layer.load();
    $.post("/Device/AjaxGetHistory", { deviceId: _id, start: start, end: end }, function (result) {
        layer.close(load1);
        if (isOk(result)) {
            if (result.data.length == 0) {
                layer.alert("该时间段暂无数据！", { icon: 5 });
            } else {
                var arrPois = [];
                hisData = result.data;
                $.each(result.data, function (index, item) {
                    arrPois[index] = new BMap.Point(item.Lng, item.Lat);
                    // hisData[index].Time = getNowDate(hisData[index].Time);
                });
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

    sMark.addEventListener("click", function () {
        map.openInfoWindow(getInfoWin(hisData[0], "起点"), new BMap.Point(arrPois[0].lng, arrPois[0].lat)); //开启信息窗口
    });
    var eMark = addMark(arrPois[arrPois.length - 1].lng, arrPois[arrPois.length - 1].lat, "../../Content/icon/end.png");
    eMark.addEventListener("click", function () {
        map.openInfoWindow(getInfoWin(hisData[hisData.length - 1], "终点"), new BMap.Point(arrPois[hisData.length - 1].lng, arrPois[hisData.length - 1].lat)); //开启信息窗口
    });
    hisMarker = addMark(arrPois[0].lng, arrPois[0].lat, "../../Content/icon/1.png");
    setLabel(hisMarker);
    movePoint(0);
}

function setLabel(marker) {
    hisLabel = new BMap.Label("", { offset: new BMap.Size(-60, -70) });
    hisLabel.setStyle({
        border: "1px solid #009688",
        padding: "8px",
        fontSize: "13px",
        borderRadius: "5px"
    });
    marker.setLabel(hisLabel);
}


function getInfoWin(d, title) {
    var _html = $("#ma_dtemple").html();
    _html = _html.replace("{latlng}", d.Lat.toString().substring(0, 7) + "," + d.Lng.toString().substring(0, 8))
                 .replace("{time}", d.Time)
        .replace("{speed}", d.Speed);
    var opts = {
        width: 100,     // 信息窗口宽度
        height: 85,     // 信息窗口高度
        title: title, // 信息窗口标题
        enableMessage: true,//设置允许信息窗发送短息
        message: ""
    }
    map.centerAndZoom(new BMap.Point(d.Lng, d.Lat), 13);
    var infoWindow = new BMap.InfoWindow(_html, opts);
    return infoWindow;
}

function movePoint(i) {
    hisI = i;
    if (i >= hisData.length) {
        layer.msg('轨迹播放完成');
        return;
    }
    var _point = new BMap.Point(hisData[i].Lng, hisData[i].Lat);
    hisMarker.setPosition(_point);
    hisLabel.setContent("经纬度：" + hisData[i].Lat.toString().substring(0, 7) + "," + hisData[i].Lng.toString().substring(0, 8) + "<br/>" + "时间：" + hisData[i].Time + "<br/>" + "速度：" + hisData[i].Speed + " KM/H");

    var _num = parseInt(((i + 1) / hisData.length).toFixed(2) * 100);
    element.progress("pro_his", _num + "%");
    if (isPlay())
        timer = setTimeout(function () {
            i++;
            movePoint(i);
        }, playTime);
}