import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

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
  private apiUrl = 'api/orders';
  private orders: Order[] = [
    { id: 1, orderNumber: 'ORD-2023-001', description: 'Mainboard Batch A', orderDate: new Date() },
    { id: 2, orderNumber: 'ORD-2023-002', description: 'Power Supply Unit', orderDate: new Date() },
    { id: 3, orderNumber: 'ORD-2023-003', description: 'Power Supply Unit B', orderDate: new Date() }
  ];

  constructor(private http: HttpClient) {}

  getOrders(): Observable<Order[]> {
    return of(this.orders);
  }

  addOrder(orderNumber: string, description: string): Observable<Order> {
    if (!orderNumber?.trim() || !description?.trim()) {
      throw new Error('Order number and description are required.');
    }

    const newOrder: Order = {
      id: this.getNextId(),
      orderNumber: orderNumber.trim(),
      description: description.trim(),
      orderDate: new Date(),
      status: 'Pending'
    };
    this.orders.push(newOrder);
    return of(newOrder);
  }

  updateOrder(id: number, orderNumber: string, description: string, status: string): Observable<Order> {
    if (id <= 0) throw new Error('Invalid order ID.');
    if (!orderNumber?.trim() || !description?.trim()) throw new Error('Order number and description are required.');

    const order = this.orders.find(o => o.id === id);
    if (order) {
      order.orderNumber = orderNumber.trim();
      order.description = description.trim();
      order.status = status as 'Pending' | 'Production' | 'Completed';
    }
    return of(order!);
  }

  deleteOrder(id: number): Observable<boolean> {
    this.orders = this.orders.filter(o => o.id !== id);
    return of(true);
  }

  downloadOrderFile(id: number): void {
    const order = this.orders.find(o => o.id === id);
    if (!order) return;

    const jsonData = JSON.stringify(order, null, 2);
    const blob = new Blob([jsonData], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = `production_${order.orderNumber}.json`;
    anchor.click();
    window.URL.revokeObjectURL(url);
  }

  private getNextId(): number {
    return this.orders.length > 0 ? Math.max(...this.orders.map(o => o.id)) + 1 : 1;
  }
}
