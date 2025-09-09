import { Component, inject, OnInit } from '@angular/core';
import { LeafletDirective } from '@bluehalo/ngx-leaflet';
import 'leaflet/dist/leaflet.css';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { Order } from '../../../core/models/Order';
import { OrderService } from '../../../core/services/orderService/order-service';
import { DateFormatPipe } from '../../../shared/pipes/dateFormat/date-format-pipe';
//import { latLng, map, marker, tileLayer } from 'leaflet';
import * as L from 'leaflet';

@Component({
  selector: 'app-orders-list-component',
  imports: [
    CardModule,
    DateFormatPipe,
    DialogModule,
    ButtonModule,
    LeafletDirective,
  ],
  templateUrl: './orders-list-component.html',
  styleUrl: './orders-list-component.scss',
})
export class OrdersListComponent implements OnInit {
  ordersService = inject(OrderService);
  orders: Order[] | null = null;
  visible = false;
  private map!: L.Map;
  maps: Record<number, L.Map> = {};
  /*options: L.MapOptions = {
    layers: [
      /*L.tileLayer(
        'https://stamen-tiles-{s}.a.ssl.fastly.net/toner/{z}/{x}/{y}{r}.png',
        {
          subdomains: 'abcd',
          minZoom: 1,
          maxZoom: 16,
        },
      ),
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: 'Map data © OpenStreetMap contributors',
      }),
      //L.circle([46.95, -122], { radius: 5000 }),
    ],
    zoom: 5,
    center: [46.879966, -121.726909],
  };*/

  onMapReady(map: L.Map) {
    this.map = map;
  }

  /* onDialogShow() {
    // bardzo ważne - dajemy małe opóźnienie, aby dialog miał czas się wyrenderować
    setTimeout(() => {
      if (this.map) {
        this.map.invalidateSize(true);
      }
    }, 1000);
  }*/

  onDialogShow(orderId: number) {
    const mapContainer = document.getElementById(`map-${orderId}`);
    if (!mapContainer) return;

    if (!this.maps[orderId]) {
      const map = L.map(`map-${orderId}`, {
        center: [50.870508516, 22.435692269],
        zoom: 14,
      });
      /* coordinates":[[[[22.435275112,50.869216386],[22.436489883,50.871800645],[22.436133243,50.871860415],[22.434861495,50.869136603],[22.435275112,50.869216386]]]]},"area":7907.223388543238,"details":{"id":"060501_2.0001.459","number":"459","sheet":null,"card":null},"address":{"teryt":"0605012","precinct":"ALEKSANDRÓWKA","precinctNumber":"0001","cadastralUnit":"Batorz - gmina wiejska","province":"powiat janowski","state":"lubelskie"},"globalId":"njeoapckntgddgyixnsmnvgfwg"}]*/
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
      }).addTo(map);

      const polygon = L.polygon(
        [
          [50.869216386, 22.435275112],
          [50.871800645, 22.436489883],
          [50.871860415, 22.436133243],
          [50.869136603, 22.434861495],
          [50.869216386, 22.435275112],
        ],
        {
          color: 'green',
          weight: 3,
          fillColor: 'lime',
          fillOpacity: 0.4,
        },
      ).addTo(map);

      polygon.bindPopup('Pole nr 1');

      L.marker([50.870508516, 22.435692269]).addTo(map);

      this.maps[orderId] = map;
    } else {
      setTimeout(() => this.maps[orderId].invalidateSize(), 200);
    }
  }

  ngOnInit() {
    this.ordersService.getAllOrders().subscribe({
      next: (res) => {
        this.orders = res;
      },
      error: (err) => {
        console.log(err);
        this.orders = [];
      },
    });
  }

  showDialog() {
    this.visible = true;
  }
}
