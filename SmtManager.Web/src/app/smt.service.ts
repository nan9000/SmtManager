import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Order {
  id: number;
  orderNumber: string;
  description: string;
  orderDate: Date;
  status?: 'Pending' | 'Production' | 'Completed';
  orderBoards?: { boardId: number; quantityRequired: number; board?: Board }[];
}

export interface Board {
  id: number;
  name: string;
  description: string;
  length: number;
  width: number;
  boardComponents?: { componentId: number; placementCount: number; component?: Component }[];
}

export interface Component {
  id: number;
  name: string;
  description: string;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class SmtService {
  private apiUrl = 'http://localhost:5000/api/orders';
  private boardsUrl = 'http://localhost:5000/api/boards';
  private componentsUrl = 'http://localhost:5000/api/components';

  constructor(private http: HttpClient) { }

  // Orders
  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  addOrder(order: any): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order);
  }

  updateOrder(id: number, order: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, order);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  downloadOrderFile(id: number): void {
    window.open(`${this.apiUrl}/${id}/download`, '_blank');
  }

  // Boards
  getBoards(): Observable<Board[]> {
    return this.http.get<Board[]>(this.boardsUrl);
  }

  getBoard(id: number): Observable<Board> {
    return this.http.get<Board>(`${this.boardsUrl}/${id}`);
  }

  addBoard(board: any): Observable<Board> {
    return this.http.post<Board>(this.boardsUrl, board);
  }

  updateBoard(id: number, board: any): Observable<void> {
    return this.http.put<void>(`${this.boardsUrl}/${id}`, board);
  }

  deleteBoard(id: number): Observable<void> {
    return this.http.delete<void>(`${this.boardsUrl}/${id}`);
  }

  // Components
  getComponents(): Observable<Component[]> {
    return this.http.get<Component[]>(this.componentsUrl);
  }

  getComponent(id: number): Observable<Component> {
    return this.http.get<Component>(`${this.componentsUrl}/${id}`);
  }

  addComponent(component: any): Observable<Component> {
    return this.http.post<Component>(this.componentsUrl, component);
  }

  updateComponent(id: number, component: any): Observable<void> {
    return this.http.put<void>(`${this.componentsUrl}/${id}`, component);
  }

  deleteComponent(id: number): Observable<void> {
    return this.http.delete<void>(`${this.componentsUrl}/${id}`);
  }
}
