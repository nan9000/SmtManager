import { Component, OnInit, OnDestroy, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SmtService, Order } from '../smt.service';
import { Subscription, interval, startWith, switchMap } from 'rxjs';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './home.component.html',
    styles: []
})
export class HomeComponent implements OnInit, OnDestroy {
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

    private pollSubscription: Subscription | null = null;

    constructor(private smtService: SmtService) { }

    ngOnInit(): void {
        this.startPolling();
    }

    ngOnDestroy(): void {
        this.stopPolling();
    }

    startPolling(): void {
        this.pollSubscription = interval(2000)
            .pipe(
                startWith(0),
                switchMap(() => this.smtService.getOrders())
            )
            .subscribe({
                next: (data) => this.orderList.set(data),
                error: (err) => {
                    console.error('Error loading orders:', err);
                }
            });
    }

    stopPolling(): void {
        if (this.pollSubscription) {
            this.pollSubscription.unsubscribe();
            this.pollSubscription = null;
        }
    }

    loadOrders(): void {
        // Manual load, though polling handles it mostly.
        this.smtService.getOrders().subscribe({
            next: (data) => this.orderList.set(data),
            error: (err) => console.error('Error loading orders:', err)
        });
    }

    createOrder(): void {
        if (!this.newOrderNumber?.trim() || !this.newOrderDesc?.trim()) {
            alert('Order number and description are required.');
            return;
        }

        this.smtService.addOrder(this.newOrderNumber, this.newOrderDesc).subscribe({
            next: () => {
                this.loadOrders(); // Refresh immediately
                this.closeNewOrderModal();
            },
            error: (err) => alert(`Failed to create order: ${err.message || 'Unknown error'}`)
        });
    }

    updateOrder(): void {
        if (!this.selectedOrderId || !this.updateOrderNumber?.trim() || !this.updateOrderDesc?.trim()) {
            alert('Please fill all fields.');
            return;
        }

        this.smtService.updateOrder(
            this.selectedOrderId,
            this.updateOrderNumber,
            this.updateOrderDesc,
            this.updateOrderStatus
        ).subscribe({
            next: () => {
                this.loadOrders(); // Refresh immediately
                this.closeUpdateOrderModal();
            },
            error: (err) => alert(`Failed to update order: ${err.message || 'Unknown error'}`)
        });
    }

    deleteOrder(id: number): void {
        if (!confirm('Remove this order?')) return;

        this.smtService.deleteOrder(id).subscribe({
            next: () => this.loadOrders(), // Refresh immediately
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
}
