import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { Field } from '../../../core/models/Field';
import { FieldService } from '../../../core/services/fieldService/field-service';

@Component({
  selector: 'app-fields-list-component',
  imports: [CardModule, ButtonModule, ConfirmDialogModule, DialogModule],
  templateUrl: './fields-list-component.html',
  styleUrl: './fields-list-component.scss',
  providers: [ConfirmationService],
})
export class FieldsListComponent implements OnInit {
  fieldService = inject(FieldService);
  confirmationService = inject(ConfirmationService);
  fields: Field[] | null = null;
  router = inject(Router);
  messageService = inject(MessageService);

  maps: Record<number, L.Map | null> = {};
  visibles: Record<number, boolean> = {};

  ngOnInit() {
    this.fieldService.getFields().subscribe({
      next: (res) => {
        this.fields = res;

        this.maps = {};
        this.visibles = {};

        for (const field of this.fields) {
          this.maps[field.id] = null;
          this.visibles[field.id] = false;
        }
      },
      error: (err) => {
        console.log(err);
        this.fields = [];
      },
    });
  }

  confirmDelete(fieldId: number) {
    this.confirmationService.confirm({
      message: 'Do you want to delete this field?',
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
        this.deleteField(fieldId);
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

  deleteField(id: number) {
    this.fieldService.deleteField(id).subscribe({
      next: () => {
        this.fields = this.fields?.filter((x) => x.id !== id) as Field[];
        this.messageService.add({
          severity: 'info',
          summary: 'Confirmed',
          detail: 'Field deleted',
        });
      },
      error: (err) => {
        console.log(err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to delete field',
        });
      },
    });
  }
  updateField(id: number) {
    this.router.navigate(['/fields', id, 'edit']);
  }

  showDialog(id: number) {
    this.visibles[id] = true;
  }
  onDialogHide(field: Field) {
    this.visibles[field.id] = false;
    if (this.maps[field.id]) {
      this.maps[field.id]!.remove();
      this.maps[field.id] = null;
    }
  }

  onDialogShow(fieldId: number) {
    const mapContainer = document.getElementById(`map-${fieldId}`);
    if (!mapContainer) return;

    if (!this.maps[fieldId]) {
      const field = this.fields?.find((x) => x.id == fieldId) as Field;
      const map = L.map(`map-${fieldId}`, {
        center: [
          field.centerPoint.coordinates[1],
          field.centerPoint.coordinates[0],
        ],
        zoom: 14,
      });

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
      }).addTo(map);

      const latLngs = field.boundary.coordinates.map((polygon) =>
        polygon.map((ring) =>
          ring.map(([lng, lat]) => [lat, lng] as [number, number]),
        ),
      );

      const polygon = L.polygon(latLngs);
      polygon.addTo(map);
      polygon.bindPopup(field.identifierName);

      L.marker([
        field.centerPoint.coordinates[1],
        field.centerPoint.coordinates[0],
      ]).addTo(map);

      this.maps[fieldId] = map;
    } else {
      setTimeout(() => this.maps[fieldId]!.invalidateSize(), 200);
    }
  }
}
