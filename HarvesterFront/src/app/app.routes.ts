import { Routes } from '@angular/router';
import { AddFieldFormComponent } from './features/pages/add-field-form-component/add-field-form-component';
import { AddOrderComponent } from './features/pages/add-order-component/add-order-component';
import { OrdersListComponent } from './features/pages/orders-list-component/orders-list-component';
import { CombineListComponent } from './features/pages/combine-list-component/combine-list-component';

export const routes: Routes = [
  { path: 'fields/add', component: AddFieldFormComponent },
  { path: 'orders/add', component: AddOrderComponent },
  { path: 'orders', component: OrdersListComponent },
  { path: 'combines', component: CombineListComponent },
];
