import { Component, OnInit, OnDestroy, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { SmtService, Order } from '../smt.service';
import { Subscription, interval, startWith, switchMap } from 'rxjs';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './home.component.html',
    styles: []
})
export class HomeComponent implements OnInit, OnDestroy {
    orderList = signal<Order[]>([]);

    completedOrders = computed(() =>
        this.orderList().filter(o => o.status === 'Completed').length
    );

    private pollSubscription: Subscription | null = null;

    constructor(private smtService: SmtService, private router: Router) { }

    ngOnInit(): void {
        this.startPolling();
    }

    ngOnDestroy(): void {
        this.stopPolling();
    }

    startPolling(): void {
        this.pollSubscription = interval(5000) // Increased interval slightly
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

    navigateToCreateOrder(): void {
        this.router.navigate(['/orders/create']);
    }
}
