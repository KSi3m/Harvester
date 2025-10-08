import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { FieldDto } from '../../models/dtos/field-dto';
import { Field } from '../../models/Field';
import { GeoJsonDataForFieldResponse } from '../../models/responses/geoJson-data-for-field-response';

@Injectable({
  providedIn: 'root',
})
export class FieldService {
  http: HttpClient = inject(HttpClient);
  url = environment.apiUrl;

  getFields(): Observable<Field[]> {
    return this.http.get<Field[]>(`${this.url}/fields`);
  }

  getFieldById(id: number): Observable<Field> {
    return this.http.get<Field>(`${this.url}/fields/${id}`);
  }

  getGeoJsonDataForField(id: string): Observable<GeoJsonDataForFieldResponse> {
    const encodedId = encodeURIComponent(id);
    return this.http.get<GeoJsonDataForFieldResponse>(
      `${this.url}/fields/${encodedId}/geoJsonData`,
    );
  }

  createField(dto: FieldDto): Observable<void> {
    return this.http.post<void>(`${this.url}/fields`, dto);
  }

  updateField(id: number, dto: FieldDto): Observable<void> {
    return this.http.put<void>(`${this.url}/fields/${id}`, dto);
  }

  deleteField(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/fields/${id}`);
  }
}
