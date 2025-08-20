import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Field } from '../../models/Field';

export interface FieldDto {
  name: string;
  areaHectares: number;
  terrainCoeff: number;
  shapeCoeff: number;
  cropType: string;
}

@Injectable({
  providedIn: 'root',
})
export class FieldService {
  http: HttpClient = inject(HttpClient);
  url = environment.apiUrl;

  getFields(): Observable<Field[]> {
    return this.http.get<Field[]>(`${this.url}/fields`);
  }

  getAreaForField(id: string): Observable<number> {
    return this.http.get<number>(`${this.url}/fields/${id}/area`);
  }

  createField(dto: FieldDto): Observable<void> {
    return this.http.post<void>(`${this.url}/fields`, dto);
  }
}
