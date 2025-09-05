import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CreateOrderDto } from '../../models/dtos/create-order-dto';
import { Order } from '../../models/Order';
import { CreateOrderResponse } from '../../models/responses/create-order-response';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  http: HttpClient = inject(HttpClient);
  url = environment.apiUrl;

  createOrder(dto: CreateOrderDto): Observable<CreateOrderResponse> {
    return this.http.post<CreateOrderResponse>(`${this.url}/orders`, dto);
  }

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.url}/orders`);
  }
}
