import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonModule, ToastModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  providers: [MessageService],
})
export class App {
  protected readonly title = signal('HarvesterFront');
}
