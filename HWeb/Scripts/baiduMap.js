var map;
//初始化地图
function initMap(id) {
    // 百度地图API功能
    map = new BMap.Map(id);    // 创建Map实例
    map.centerAndZoom(new BMap.Point(121.4391047490,31.2054593808), 14);  // 初始化地图,设置中心点坐标和地图级别
    //添加地图类型控件
    map.addControl(new BMap.MapTypeControl({
        mapTypes: [
            BMAP_NORMAL_MAP,
            BMAP_HYBRID_MAP
        ]
    }));
    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
}
//获取地址
function getAddress(point, doc) {
    var geoc = new BMap.Geocoder();
    var address = "";
    geoc.getLocation(point, function (rs) {
        var addComp = rs.addressComponents;
        $(doc).html(address = addComp.province + addComp.city + addComp.district + addComp.street + addComp.streetNumber);
    });
}
function addMapWindow(marker, title, content) {
    //创建检索信息窗口对象
    var searchInfoWindow = null;
    searchInfoWindow = new BMapLib.SearchInfoWindow(map, content, {
        title: title,      //标题
        width: 220,             //宽度
        height: 142,              //高度
        panel: "panel",         //检索结果面板
        enableAutoPan: true,     //自动平移
        searchTypes: [

        ]
    });
    searchInfoWindow.open(marker);
}

//添加图标
function addMark(lat, lng, icon) {
    var pt = new BMap.Point(lat, lng);
    var myIcon = new BMap.Icon(icon, new BMap.Size(25, 30));
    var marker2 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
    map.addOverlay(marker2);
    return marker2;
}

//画线
function drawPolyline(points,icon) {
    // 创建polyline对象
    var pois = points;

    var options = {
        enableEditing: false,//是否启用线编辑，默认为false
        enableClicking: true,//是否响应点击事件，默认为true
        strokeWeight: '6',//折线的宽度，以像素为单位
        strokeOpacity: 0.8,//折线的透明度，取值范围0 - 1
        strokeColor: "#01AAED" //折线颜色
    };
    if (icon) {//带箭头的折线图
        var sy = new BMap.Symbol(BMap_Symbol_SHAPE_BACKWARD_OPEN_ARROW, {
            scale: 0.6,//图标缩放大小
            strokeColor: '#fff',//设置矢量图标的线填充颜色
            strokeWeight: '2',//设置线宽
        });
        options.icons = [new BMap.IconSequence(sy, '10', '30')];
    }
    var polyline = new BMap.Polyline(pois, options);

    map.addOverlay(polyline);          //增加折线
    map.setViewport(points);
}