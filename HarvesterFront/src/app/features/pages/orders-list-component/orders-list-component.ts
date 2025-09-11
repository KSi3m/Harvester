import { Component, inject, OnInit } from '@angular/core';
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
  imports: [CardModule, DateFormatPipe, DialogModule, ButtonModule],
  templateUrl: './orders-list-component.html',
  styleUrl: './orders-list-component.scss',
})
export class OrdersListComponent implements OnInit {
  ordersService = inject(OrderService);
  orders: Order[] | null = null;
  visible = false;
  private map!: L.Map;
  maps: Record<number, L.Map | null> = {};
  visibles: Record<number, boolean> = {};
  onMapReady(map: L.Map) {
    this.map = map;
  }

  onDialogShow(orderId: number) {
    const mapContainer = document.getElementById(`map-${orderId}`);
    if (!mapContainer) return;

    if (!this.maps[orderId]) {
      const order = this.orders?.find((x) => x.id == orderId) as Order;
      const map = L.map(`map-${orderId}`, {
        center: [
          order.field.centerPoint.coordinates[1],
          order.field.centerPoint.coordinates[0],
        ],
        zoom: 14,
      });

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
      }).addTo(map);

      const latLngs = order.field.boundary.coordinates.map((polygon) =>
        polygon.map((ring) =>
          ring.map(([lng, lat]) => [lat, lng] as [number, number]),
        ),
      );

      const polygon = L.polygon(latLngs);
      polygon.addTo(map);
      polygon.bindPopup(order.field.identifierName);

      L.marker([
        order.field.centerPoint.coordinates[1],
        order.field.centerPoint.coordinates[0],
      ]).addTo(map);

      this.maps[orderId] = map;
    } else {
      setTimeout(() => this.maps[orderId]!.invalidateSize(), 200);
    }
  }

  ngOnInit() {
    this.ordersService.getAllOrders().subscribe({
      next: (res) => {
        this.orders = res;

        this.maps = {};
        this.visibles = {};

        for (const order of this.orders) {
          this.maps[order.id] = null;
          this.visibles[order.id] = false;
        }
      },
      error: (err) => {
        console.log(err);
        this.orders = [];
      },
    });
  }

  showDialog(id: number) {
    this.visibles[id] = true;
  }
  onDialogHide(order: Order) {
    this.visibles[order.id] = false;
    if (this.maps[order.id]) {
      this.maps[order.id]!.remove();
      this.maps[order.id] = null;
    }
  }

  deleteOrder(id: number) {
    this.ordersService.deleteOrder(id).subscribe({
      next: () => {
        this.orders = this.orders?.filter((x) => x.id !== id) as Order[];
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
