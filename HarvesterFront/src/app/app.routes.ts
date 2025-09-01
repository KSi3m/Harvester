import { Routes } from '@angular/router';
import { AddFieldFormComponent } from './features/pages/add-field-form-component/add-field-form-component';
import { AddOrderComponent } from './features/pages/add-order-component/add-order-component';

export const routes: Routes = [
  { path: 'fields/add', component: AddFieldFormComponent },
  { path: 'orders/add', component: AddOrderComponent },
];
