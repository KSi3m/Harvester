import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Combine } from '../../../core/models/Combine';
import { CombineService } from '../../../core/services/combineService/combine-service';

@Component({
  selector: 'app-combine-list-component',
  imports: [CardModule, ButtonModule, ConfirmDialogModule],
  templateUrl: './combine-list-component.html',
  styleUrl: './combine-list-component.scss',
  providers: [ConfirmationService],
})
export class CombineListComponent implements OnInit {
  combineService = inject(CombineService);
  confirmationService = inject(ConfirmationService);
  combines: Combine[] | null = null;
  router = inject(Router);
  messageService = inject(MessageService);

  ngOnInit() {
    this.combineService.getCombines().subscribe({
      next: (res) => {
        this.combines = res;
      },
      error: (err) => {
        console.log(err);
        this.combines = [];
      },
    });
  }

  confirmDelete(combineId: number) {
    this.confirmationService.confirm({
      message: 'Do you want to delete this combine?',
      header: 'Danger Zone',
      icon: 'pi pi-info-circle',
      rejectLabel: 'Cancel',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger',
      },

      accept: () => {
        this.deleteCombine(combineId);
      },
      reject: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Rejected',
          detail: 'You have rejected',
        });
      },
    });
  }

  deleteCombine(id: number) {
    this.combineService.deleteCombine(id).subscribe({
      next: () => {
        this.combines = this.combines?.filter((x) => x.id !== id) as Combine[];
        this.messageService.add({
          severity: 'info',
          summary: 'Confirmed',
          detail: 'Combine deleted',
        });
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to delete combine',
        });
      },
    });
  }
  updateCombine(id: number) {
    this.router.navigate(['/combines', id, 'edit']);
  }
}
