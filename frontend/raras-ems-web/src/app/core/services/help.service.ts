import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';

export interface HelpStep {
  number: number;
  text: string;
}

export interface HelpResponse {
  moduleKey: string;
  pageKey: string;
  functionalityKey: string;
  title: string;
  steps: HelpStep[];
}

@Injectable({
  providedIn: 'root'
})
export class HelpService {
  private apiUrl = 'http://localhost:5000/api/help';

  constructor(private http: HttpClient) {}

  getHelp(moduleKey: string, pageKey: string, functionalityKey: string): Observable<HelpResponse> {
    const params = new HttpParams()
      .set('moduleKey', moduleKey)
      .set('pageKey', pageKey)
      .set('functionalityKey', functionalityKey);

    return this.http.get<HelpResponse>(this.apiUrl, { params }).pipe(
      catchError(error => {
        console.error('Error fetching help context from API:', error);
        return of({
          moduleKey,
          pageKey,
          functionalityKey,
          title: 'Quick steps',
          steps: []
        });
      })
    );
  }
}
