var local_lat = "30.646458";
var local_lng = "114.308687";

function getAddressByAddressName(params) {
    var option = {
        address_name: false,
        city: "武汉市",
        type: 'post',
        dataType: 'JSONP',
        successFun: false,
        errorFun: false
    }

    $.extend(option, params);

    var url = "http://api.map.baidu.com/geocoder/v2/?ak=C39a01ae2221d5afc846238539b6bbc2&callback=renderOption&output=json&address=" + option.address_name + "&city=" + option.city;

    $.ajax({
        url: url,
        dataType: option.dataType,
        method: option.type,
        success: function (response) {
            //alert(JSON.stringify(response));
            //return;
            if (response.status == 0) {
                if (option.successFun) {
                    option.successFun(response);
                }
            }
            else {
                $.toastMsg("没有找到对应信息,请输入完整地名!", 1500);
            }
        },
        error: function () {

        }
    });
}

function getAddressByCoordinate(params) {
    var option = {
        lat: false,
        lng: false,
        city: "武汉市",
        type: 'post',
        dataType: 'JSONP',
        successFun: false,
        errorFun: false
    }

    $.extend(option, params);

    var url = "http://api.map.baidu.com/geocoder/v2/?ak=8TN0gC5Rqo6cec2jroKOkNpE&callback=renderReverse&location=" + option.lat + "," + option.lng + "&output=json&pois=1";

    $.ajax({
        url: url,
        dataType: option.dataType,
        method: option.type,
        crossDomain: true,
        success: function (response) {
            //alert(JSON.stringify(response)); return;
            if (response.status == 0) {
                if (option.successFun) {
                    option.successFun(response);
                }
            }
            else {
                $.toastMsg("获取失败", 1500);
            }
        },
        error: function () {

        }
    });
}

function createMap(map) {
    // 百度地图API功能
    map = new BMap.Map("allmap");
    // 创建Map实例
    map.centerAndZoom(new BMap.Point(local_lng, local_lat), 15);
    // 初始化地图,设置中心点坐标和地图级别
    map.enableScrollWheelZoom(true);
    //开启鼠标滚轮缩放
    var point = new BMap.Point(local_lng, local_lat);
    // 关联图片
    var myIcon = new BMap.Icon("/kxd/img/tp1.png", new BMap.Size(38, 53));
    //map.centerAndZoom(point, 15);
    var marker = new BMap.Marker(point, {
        icon: myIcon
    });

    // 创建标注
    map.addOverlay(marker);

    marker.addEventListener("click", function (e) {
        $('#p12-ppux').toggle();
    });

    marker.enableDragging(true);
    marker.addEventListener("dragend", function (e) {
        var lat = e.point.lat;
        var lng = e.point.lng;
        map.panTo(new BMap.Point(lat, lng), 2000);
        //alert(lat + "," + lng);
    });

    return map;
}

function theLocation(map, lng, lat) {
    map.clearOverlays();

    var new_point = new BMap.Point(lng, lat);
    // 关联图片
    var myIcon = new BMap.Icon("/kxd/img/tp1.png", new BMap.Size(38, 53));
    map.centerAndZoom(new_point, 15);
    map.enableScrollWheelZoom(true);
    var marker = new BMap.Marker(new_point, {
        icon: myIcon
    });
    marker.enableDragging(true);
    marker.addEventListener("dragend", function (e) {
        var lat = e.point.lat;
        var lng = e.point.lng;
        map.panTo(new BMap.Point(lat, lng), 2000);
        //alert(lat + "," + lng);
    });

    // 将标注添加到地图中
    map.addOverlay(marker);              

    map.panTo(new_point, 2000);
}

var afterMethod = null;

function locationError(error) {
    var opertion = {
        msg: "坐标获取失败!",
        exitText: "关闭!"
    };

    $.alertbox(opertion)
}

function showPosition(data) {
    if (data.status != 0) {
        alert("地图坐标转换出错");
        return;
    }

    lng = data.result[0].x;
    lat = data.result[0].y;

    if (afterMethod) {
        var params = {
            lat: lat,
            lng: lng,
            successFun: afterMethod
        }

        getAddressByCoordinate(params);
    }  
}

function locationSuccess(position) {
    var currentLat = position.coords.latitude;

    lat = currentLat;
    var currentLon = position.coords.longitude;
    lng = currentLon;

    if (afterMethod) {
        afterMethod();
    }

    var url = 'http://api.map.baidu.com/geoconv/v1/?coords=' + currentLon + ',' + currentLat + '&from=1&to=5&ak=8TN0gC5Rqo6cec2jroKOkNpE&callback=showPosition';

    var script = document.createElement('script');

    script.src = url;

    document.getElementsByTagName("head")[0].appendChild(script);
}

function getLocal(option) {
    afterMethod = option.successFun;

    //手机使用代码
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(locationSuccess, locationError, {
            // 指示浏览器获取高精度的位置，默认为false
            enableHighAcuracy: true,
            // 指定获取地理位置的超时时间，默认不限时，单位为毫秒
            timeout: 2500,
            // 最长有效期，在重复获取地理位置时，此参数指定多久再次获取位置。
            maximumAge: 3000
        });
    } else {
        alert("Your browser does not support Geolocation!");
    }
}

