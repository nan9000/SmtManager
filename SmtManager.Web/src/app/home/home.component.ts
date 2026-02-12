import { Component, OnInit, OnDestroy, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SmtService, Order, Board } from '../smt.service';
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
    updateOrderBoards = signal<{ boardId: number, name: string, quantityRequired: number }[]>([]);

    // Board Selection
    availableBoards = signal<Board[]>([]);
    selectedBoardId = signal<number | null>(null);
    boardQuantity = signal<number>(1);
    currentOrderBoards = signal<{ boardId: number, name: string, quantityRequired: number }[]>([]);

    completedOrders = computed(() =>
        this.orderList().filter(o => o.status === 'Completed').length
    );

    private pollSubscription: Subscription | null = null;

    constructor(private smtService: SmtService) { }

    ngOnInit(): void {
        this.startPolling();
        this.loadBoards();
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

    loadBoards(): void {
        this.smtService.getBoards().subscribe({
            next: (data) => this.availableBoards.set(data),
            error: (err) => console.error('Error loading boards:', err)
        });
    }

    addBoardToOrder(): void {
        const boardId = this.selectedBoardId();
        const quantity = this.boardQuantity();

        if (!boardId || quantity <= 0) return;

        const board = this.availableBoards().find(b => b.id === Number(boardId));
        if (!board) return;

        // Check if already added
        if (this.currentOrderBoards().some(b => b.boardId === board.id)) {
            alert('Board already added to order.');
            return;
        }

        this.currentOrderBoards.update(boards => [
            ...boards,
            { boardId: board.id, name: board.name, quantityRequired: quantity }
        ]);

        // Reset selection
        this.selectedBoardId.set(null);
        this.boardQuantity.set(1);
    }

    removeBoardFromOrder(boardId: number): void {
        this.currentOrderBoards.update(boards => boards.filter(b => b.boardId !== boardId));
    }

    createOrder(): void {
        if (!this.newOrderNumber?.trim() || !this.newOrderDesc?.trim()) {
            alert('Order number and description are required.');
            return;
        }

        const newOrder = {
            orderNumber: this.newOrderNumber,
            description: this.newOrderDesc,
            status: 'Pending',
            orderDate: new Date(),
            orderBoards: this.currentOrderBoards().map(b => ({
                boardId: b.boardId,
                quantityRequired: b.quantityRequired
            }))
        };

        this.smtService.addOrder(newOrder).subscribe({
            next: () => {
                this.loadOrders(); // Refresh immediately
                this.closeNewOrderModal();
            },
            error: (err) => alert(`Failed to create order: ${err.message || 'Unknown error'}`)
        });
    }

    addBoardToUpdateOrder(): void {
        const boardId = this.selectedBoardId();
        const quantity = this.boardQuantity();

        if (!boardId || quantity <= 0) return;

        const board = this.availableBoards().find(b => b.id === Number(boardId));
        if (!board) return;

        if (this.updateOrderBoards().some(b => b.boardId === board.id)) {
            alert('Board already added to order.');
            return;
        }

        this.updateOrderBoards.update(boards => [
            ...boards,
            { boardId: board.id, name: board.name, quantityRequired: quantity }
        ]);

        this.selectedBoardId.set(null);
        this.boardQuantity.set(1);
    }

    removeBoardFromUpdateOrder(boardId: number): void {
        this.updateOrderBoards.update(boards => boards.filter(b => b.boardId !== boardId));
    }

    updateOrder(): void {
        if (!this.selectedOrderId || !this.updateOrderNumber?.trim() || !this.updateOrderDesc?.trim()) {
            alert('Please fill all fields.');
            return;
        }

        const updatedOrder = {
            id: this.selectedOrderId,
            orderNumber: this.updateOrderNumber,
            description: this.updateOrderDesc,
            status: this.updateOrderStatus,
            orderDate: new Date(),
            orderBoards: this.updateOrderBoards().map(b => ({
                boardId: b.boardId,
                quantityRequired: b.quantityRequired
            }))
        };

        this.smtService.updateOrder(this.selectedOrderId, updatedOrder).subscribe({
            next: () => {
                this.loadOrders();
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
        this.currentOrderBoards.set([]);
        this.selectedBoardId.set(null);
        this.boardQuantity.set(1);
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

        // Load existing boards
        const existingBoards = order.orderBoards?.map(ob => ({
            boardId: ob.boardId,
            name: ob.board?.name || `Board #${ob.boardId}`,
            quantityRequired: ob.quantityRequired
        })) || [];
        this.updateOrderBoards.set(existingBoards);

        this.showUpdateOrderModal.set(true);
    }

    closeUpdateOrderModal(): void {
        this.showUpdateOrderModal.set(false);
        this.selectedOrderId = null;
    }
}
