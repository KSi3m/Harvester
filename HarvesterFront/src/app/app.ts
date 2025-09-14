import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonModule, ToastModule, MenubarModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  providers: [MessageService],
})
export class App {
  protected readonly title = signal('HarvesterFront');
  items = [
    {
      label: 'Home',
      icon: 'pi pi-home',
      routerLink: '/',
    },
    {
      label: 'Orders',
      icon: 'pi pi-star',
      routerLink: '/orders',
      items: [
        {
          label: 'Add new order',
          icon: 'pi pi-plus',
          routerLink: '/orders/add',
        },
      ],
    },
    {
      label: 'Combines',
      icon: 'pi pi-home',
      routerLink: '/combines',
      items: [
        {
          label: 'Add new combine',
          icon: 'pi pi-plus',
          routerLink: '/combines/add',
        },
      ],
    },
    {
      label: 'Add field',
      icon: 'pi pi-expand',
      routerLink: '/fields/add',
    },
  ];
}
