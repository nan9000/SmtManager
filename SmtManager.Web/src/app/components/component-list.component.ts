import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SmtService, Component as SmtComponent } from '../smt.service';

@Component({
    selector: 'app-component-list',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './component-list.component.html',
    styleUrls: ['./component-list.component.css']
})
export class ComponentListComponent implements OnInit {
    components: SmtComponent[] = [];
    newComponent: any = { name: '', description: '', quantity: 0 };

    constructor(private smtService: SmtService) { }

    ngOnInit(): void {
        this.loadComponents();
    }

    loadComponents(): void {
        this.smtService.getComponents().subscribe(data => {
            this.components = data;
        });
    }

    addComponent(): void {
        this.smtService.addComponent(this.newComponent).subscribe(component => {
            this.components.push(component);
            this.newComponent = { name: '', description: '', quantity: 0 };
        });
    }

    deleteComponent(id: number): void {
        this.smtService.deleteComponent(id).subscribe(() => {
            this.components = this.components.filter(c => c.id !== id);
        });
    }
}
