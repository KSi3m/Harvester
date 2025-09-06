import { Component, inject, OnInit } from '@angular/core';
import { CardModule } from 'primeng/card';
import { Combine } from '../../../core/models/Combine';
import { CombineService } from '../../../core/services/combineService/combine-service';

@Component({
  selector: 'app-combine-list-component',
  imports: [CardModule],
  templateUrl: './combine-list-component.html',
  styleUrl: './combine-list-component.scss',
})
export class CombineListComponent implements OnInit {
  combineService = inject(CombineService);
  combines: Combine[] = [];

  ngOnInit() {
    this.combineService.getCombines().subscribe({
      next: (res) => {
        this.combines = res;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
