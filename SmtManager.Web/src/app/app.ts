import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { SmtService, Order } from './smt.service';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, RouterOutlet],
  templateUrl: 'app.html',
  styleUrls: ['app.component.css']
})
export class App implements OnInit {
  orderList = signal<Order[]>([]);
  showNewOrderModal = signal(false);
  showUpdateOrderModal = signal(false);

  newOrderNumber = '';
  newOrderDesc = '';
  updateOrderNumber = '';
  updateOrderDesc = '';
  updateOrderStatus = 'Pending';
  selectedOrderId: number | null = null;

  completedOrders = computed(() =>
    this.orderList().filter(o => o.status === 'Completed').length
  );

  currentUser$;

  constructor(
    private smtService: SmtService,
    private authService: AuthService
  ) {
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.smtService.getOrders().subscribe({
      next: (data) => this.orderList.set(data),
      error: (err) => {
        console.error('Error loading orders:', err);
        alert('Failed to load orders. Please refresh the page.');
      }
    });
  }

  createOrder(): void {
    if (!this.newOrderNumber?.trim() || !this.newOrderDesc?.trim()) {
      alert('Order number and description are required.');
      return;
    }

    try {
      this.smtService.addOrder(this.newOrderNumber, this.newOrderDesc).subscribe({
        next: () => {
          this.loadOrders();
          this.closeNewOrderModal();
        },
        error: (err) => alert(`Failed to create order: ${err.message || 'Unknown error'}`)
      });
    } catch (error: any) {
      alert(`Error: ${error.message}`);
    }
  }

  updateOrder(): void {
    if (!this.selectedOrderId || !this.updateOrderNumber?.trim() || !this.updateOrderDesc?.trim()) {
      alert('Please fill all fields.');
      return;
    }

    try {
      this.smtService.updateOrder(
        this.selectedOrderId,
        this.updateOrderNumber,
        this.updateOrderDesc,
        this.updateOrderStatus
      ).subscribe({
        next: () => {
          this.loadOrders();
          this.closeUpdateOrderModal();
        },
        error: (err) => alert(`Failed to update order: ${err.message || 'Unknown error'}`)
      });
    } catch (error: any) {
      alert(`Error: ${error.message}`);
    }
  }

  deleteOrder(id: number): void {
    if (!confirm('Remove this order?')) return;

    this.smtService.deleteOrder(id).subscribe({
      next: () => this.loadOrders(),
      error: (err) => {
        console.error('Error deleting order:', err);
        alert('Failed to delete order.');
      }
    });
  }

  downloadOrder(id: number): void {
    this.smtService.downloadOrderFile(id);
  }

  openNewOrderModal(): void {
    this.newOrderNumber = '';
    this.newOrderDesc = '';
    this.showNewOrderModal.set(true);
  }

  closeNewOrderModal(): void {
    this.showNewOrderModal.set(false);
  }

  openUpdateOrderModal(order: Order): void {
    this.selectedOrderId = order.id;
    this.updateOrderNumber = order.orderNumber;
    this.updateOrderDesc = order.description;
    this.updateOrderStatus = order.status || 'Pending';
    this.showUpdateOrderModal.set(true);
  }

  closeUpdateOrderModal(): void {
    this.showUpdateOrderModal.set(false);
    this.selectedOrderId = null;
  }

  logout(): void {
    this.authService.logout();
  }
}