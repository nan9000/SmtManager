import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Order {
  id: number;
  orderNumber: string;
  description: string;
  orderDate: Date;
  status?: 'Pending' | 'Production' | 'Completed';
}

@Injectable({
  providedIn: 'root'
})
export class SmtService {
  private apiUrl = 'http://localhost:5000/api/orders';

  constructor(private http: HttpClient) { }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  addOrder(orderNumber: string, description: string): Observable<Order> {
    const payload = { orderNumber, description };
    return this.http.post<Order>(this.apiUrl, payload);
  }

  updateOrder(id: number, orderNumber: string, description: string, status: string): Observable<void> {
    const payload = { orderNumber, description, status };
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  downloadOrderFile(id: number): void {
    window.open(`${this.apiUrl}/${id}/download`, '_blank');
  }
}
