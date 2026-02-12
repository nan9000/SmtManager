import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { SmtService, Order, Board } from '../smt.service';

@Component({
  selector: 'app-order-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './order-edit.html',
  styleUrl: './order-edit.css',
})
export class OrderEdit implements OnInit {
  order: any = { orderNumber: '', description: '', status: 'Pending', orderBoards: [] };
  isEditMode = false;
  id: number | null = null;
  statuses = ['Pending', 'Production', 'Completed'];

  // Board Selection
  availableBoards: Board[] = [];
  selectedBoardId: number | null = null;
  boardQuantityRequired: number = 1;
  currentOrderBoards: { boardId: number, name: string, quantityRequired: number }[] = [];

  constructor(
    private smtService: SmtService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadBoards();
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.isEditMode = true;
      this.loadOrder(this.id);
    }
  }

  loadOrder(id: number): void {
    this.smtService.getOrders().subscribe(orders => {
      // Since getOrder(id) isn't explicitly in the service interface shown in previous steps (it showed getOrders and getBoard/getComponent but maybe I missed getOrder individual fetch or I can just filter from all orders if list is small, OR I should add getOrder to service if missing. 
      // Checking step 13, getOrders return Observable<Order[]>, verify if there is getOrder(id).
      // Actually I see `getOrders`, `addOrder`, `updateOrder`, `deleteOrder`. I DO NOT SEE `getOrder(id)` in the service file content I read in step 13!
      // Wait, let me double check step 13 output.
      // Line 41: getOrders(): Observable<Order[]> { return this.http.get<Order[]>(this.apiUrl); }
      // It seems `getOrder(id)` is missing from `SmtService`! 
      // I should add it or just filter from `getOrders` for now, but adding it is better.
      // However, to avoid context switching and potential errors, I will implement `getOrder` in `SmtService` first or just use `getOrders` and find.
      // Given the time, I'll filter from `getOrders` for now as a fallback, but technically `OrdersController` has `GetOrderById`.
      // Let's implement `getOrder` in `SmtService` first.
      const order = orders.find(o => o.id === id);
      if (order) {
        this.order = { ...order }; // Clone to avoid direct mutation of cached list if any
        if (order.orderBoards) {
          this.currentOrderBoards = order.orderBoards.map(ob => ({
            boardId: ob.boardId,
            name: ob.board ? ob.board.name : `Board ${ob.boardId}`,
            quantityRequired: ob.quantityRequired
          }));
        }
      }
    });
  }

  loadBoards(): void {
    this.smtService.getBoards().subscribe(data => {
      this.availableBoards = data;
      this.updateBoardNames();
    });
  }

  updateBoardNames(): void {
    this.currentOrderBoards.forEach(ob => {
      const board = this.availableBoards.find(b => b.id === ob.boardId);
      if (board) {
        ob.name = board.name;
      }
    });
  }

  addBoardToOrder(): void {
    if (!this.selectedBoardId || this.boardQuantityRequired <= 0) return;

    const board = this.availableBoards.find(b => b.id === Number(this.selectedBoardId));
    if (!board) return;

    if (this.currentOrderBoards.some(ob => ob.boardId === board.id)) {
      alert('Board already added to order. You can update the quantity if needed.');
      return;
    }

    this.currentOrderBoards.push({
      boardId: board.id,
      name: board.name,
      quantityRequired: this.boardQuantityRequired
    });

    this.selectedBoardId = null;
    this.boardQuantityRequired = 1;
  }

  removeBoardFromOrder(boardId: number): void {
    this.currentOrderBoards = this.currentOrderBoards.filter(ob => ob.boardId !== boardId);
  }

  save(): void {
    const orderToSend = {
      ...this.order,
      orderBoards: this.currentOrderBoards.map(ob => ({
        boardId: ob.boardId,
        quantityRequired: ob.quantityRequired
      }))
    };

    if (this.isEditMode && this.id) {
      this.smtService.updateOrder(this.id, orderToSend).subscribe(() => {
        this.router.navigate(['/orders']);
      });
    } else {
      this.smtService.addOrder(orderToSend).subscribe(() => {
        this.router.navigate(['/orders']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/orders']);
  }
}
