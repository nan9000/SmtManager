import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SmtService, Component as SmtComponent } from '../smt.service';

@Component({
    selector: 'app-component-list',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './component-list.component.html',
    styleUrls: ['./component-list.component.css']
})
export class ComponentListComponent implements OnInit {
    components: SmtComponent[] = [];
    filteredComponents: SmtComponent[] = [];
    searchTerm: string = '';

    constructor(private smtService: SmtService, private router: Router) { }

    ngOnInit(): void {
        this.loadComponents();
    }

    loadComponents(): void {
        this.smtService.getComponents().subscribe(data => {
            this.components = data;
            this.filterComponents();
        });
    }

    filterComponents(): void {
        if (!this.searchTerm) {
            this.filteredComponents = this.components;
        } else {
            const term = this.searchTerm.toLowerCase();
            this.filteredComponents = this.components.filter(c =>
                c.name.toLowerCase().includes(term) ||
                c.description.toLowerCase().includes(term)
            );
        }
    }

    editComponent(id: number): void {
        this.router.navigate(['/components/edit', id]);
    }

    createComponent(): void {
        this.router.navigate(['/components/create']);
    }

    deleteComponent(id: number): void {
        if (confirm('Are you sure you want to delete this component?')) {
            this.smtService.deleteComponent(id).subscribe(() => {
                this.components = this.components.filter(c => c.id !== id);
                this.filterComponents();
            });
        }
    }
}
