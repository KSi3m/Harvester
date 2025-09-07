import { Component, inject, OnInit } from '@angular/core';
import { Order } from '../../../core/models/Order';
import { OrderService } from '../../../core/services/orderService/order-service';
import { CardModule } from 'primeng/card';
import { DateFormatPipe } from '../../../shared/pipes/dateFormat/date-format-pipe';

@Component({
  selector: 'app-orders-list-component',
  imports: [CardModule, DateFormatPipe],
  templateUrl: './orders-list-component.html',
  styleUrl: './orders-list-component.scss',
})
export class OrdersListComponent implements OnInit {
  ordersService = inject(OrderService);
  orders: Order[] | null = null;

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
}
