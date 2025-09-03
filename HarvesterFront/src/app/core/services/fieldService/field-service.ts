import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { FieldDto } from '../../models/dtos/field-dto';
import { Field } from '../../models/Field';
import { AreaResponse } from '../../models/responses/area-response';

@Injectable({
  providedIn: 'root',
})
export class FieldService {
  http: HttpClient = inject(HttpClient);
  url = environment.apiUrl;

  getFields(): Observable<Field[]> {
    return this.http.get<Field[]>(`${this.url}/fields`);
  }

  getAreaForField(id: string): Observable<AreaResponse> {
    const encodedId = encodeURIComponent(id);
    return this.http.get<AreaResponse>(`${this.url}/fields/${encodedId}/area`);
  }

  createField(dto: FieldDto): Observable<void> {
    return this.http.post<void>(`${this.url}/fields`, dto);
  }
}
