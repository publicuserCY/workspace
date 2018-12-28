import { Component, OnInit } from '@angular/core';
declare var BMap: any;
declare var BMapLib: any;
declare const BMAP_ANCHOR_TOP_RIGHT: any;
@Component({
  selector: 'app-baidu-map',
  templateUrl: './baidu-map.component.html',
  styleUrls: ['./baidu-map.component.css']
})
export class BaiduMapComponent implements OnInit {
  map: any;
  overlays = [];

  constructor() { }

  ngOnInit() {
    this.map = new BMap.Map('map');
    const point = new BMap.Point(113.00361, 28.164098);
    this.map.centerAndZoom(point, 19);
    this.map.enableScrollWheelZoom(true);
  }

  // 定位城市
  localCity() {
    const myCity = new BMap.LocalCity();
    myCity.get(result => {
      const cityName = result.name;
      this.map.setCenter(cityName);
    });
  }

  // 标注点(默认图标)
  defaultMarker() {
    // 向地图添加标注
    const point = new BMap.Point(113.00361, 28.164098);
    const marker = new BMap.Marker(point);
    this.map.addOverlay(marker);
    this.map.centerAndZoom(point, 19);
    this.overlays.push(marker);
  }

  // 标注点(自定义图标)
  customMarker() {
    const point = new BMap.Point(113.00361, 28.164098);
    // 定义标注图标
    const myIcon = new BMap.Icon('assets/location_48px.png', new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
    myIcon.setImageSize(new BMap.Size(32, 32));
    const marker = new BMap.Marker(point, { icon: myIcon, title: '万博汇一期' });
    this.map.addOverlay(marker);
    this.map.centerAndZoom(point, 19);
    // 设置动画
    const BMAP_ANIMATION_BOUNCE: any = '2';
    marker.setAnimation(BMAP_ANIMATION_BOUNCE);
    // 设置事件
    marker.addEventListener('click', (e): void => {
      const p = marker.getPosition();
      window.alert('marker的位置是' + p.lng + ',' + p.lat);
    });
    this.overlays.push(marker);
  }

  // 鼠标绘制工具
  customDraw() {
    const styleOptions = {
      strokeColor: 'red',
      fillColor: 'red',
      strokeWeight: 3,
      strokeOpacity: 0.8,
      fillOpacity: 0.6,
      strokeStyle: 'solid'
    };
    // 实例化鼠标绘制工具
    const drawingManager = new BMapLib.DrawingManager(this.map, {
      isOpen: false, // 是否开启绘制模式
      enableDrawingTool: true, // 是否显示工具栏
      drawingToolOptions: {
        anchor: BMAP_ANCHOR_TOP_RIGHT, // 位置
        offset: new BMap.Size(5, 5), // 偏离值
      },
      circleOptions: styleOptions, // 圆的样式
      polylineOptions: styleOptions, // 线的样式
      polygonOptions: styleOptions, // 多边形的样式
      rectangleOptions: styleOptions // 矩形的样式
    });
    // 添加鼠标绘制工具监听事件，用于获取绘制结果
    drawingManager.addEventListener('overlaycomplete', (e) => {
      this.overlays.push(e.overlay);
    });
  }

  clearOverlays() {
    // this.map.clearOverlays();
    for (let i = 0; i < this.overlays.length; i++) {
      this.map.removeOverlay(this.overlays[i]);
    }
    this.overlays.length = 0;
  }

  test() {
    const overlays = this.map.getOverlays();
    overlays.forEach(overlay => {
      console.log(overlay.toString());
      // if (overlay.point) {
      //   const icon = overlay.getIcon();
      //   const offset = icon.anchor;
      //   offset.width += 1;
      //   offset.height += 1;
      //   icon.setAnchor(offset);
      // }
    });
  }
}
