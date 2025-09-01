import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Combine } from '../../models/Combine';

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
}
