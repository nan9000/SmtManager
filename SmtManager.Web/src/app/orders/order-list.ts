import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { SmtService, Order } from '../smt.service';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css',
})
export class OrderList implements OnInit {
  orders: Order[] = [];
  filteredOrders: Order[] = [];
  searchTerm: string = '';

  constructor(private smtService: SmtService, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.smtService.getOrders().subscribe((data) => {
      this.orders = data;
      this.filterOrders();
    });
  }

  filterOrders(): void {
    if (!this.searchTerm) {
      this.filteredOrders = this.orders;
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredOrders = this.orders.filter(
        (o) =>
          o.orderNumber.toLowerCase().includes(term) ||
          o.description.toLowerCase().includes(term) ||
          (o.status && o.status.toLowerCase().includes(term))
      );
    }
  }

  createOrder(): void {
    this.router.navigate(['/orders/create']);
  }

  editOrder(id: number): void {
    this.router.navigate(['/orders/edit', id]);
  }

  deleteOrder(id: number): void {
    if (confirm('Are you sure you want to delete this order?')) {
      this.smtService.deleteOrder(id).subscribe(() => {
        this.orders = this.orders.filter((o) => o.id !== id);
        this.filterOrders();
      });
    }
  }

  downloadOrder(id: number): void {
    this.smtService.downloadOrderFile(id);
  }
}
