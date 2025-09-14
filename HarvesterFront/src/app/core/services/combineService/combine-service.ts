import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Combine } from '../../models/Combine';
import { CreateCombineDto } from '../../models/dtos/create-combine-dto';

@Injectable({
  providedIn: 'root',
})
export class CombineService {
  http: HttpClient = inject(HttpClient);
  url = environment.apiUrl;

  getCombineById(id: number): Observable<Combine> {
    return this.http.get<Combine>(`${this.url}/combines/${id}`);
  }

  getCombines(): Observable<Combine[]> {
    return this.http.get<Combine[]>(`${this.url}/combines`);
  }

  createCombine(dto: CreateCombineDto): Observable<void> {
    return this.http.post<void>(`${this.url}/combines`, dto);
  }

  updateCombine(id: number, dto: CreateCombineDto): Observable<void> {
    return this.http.put<void>(`${this.url}/combines/${id}`, dto);
  }
  deleteCombine(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/combines/${id}`);
  }
}
