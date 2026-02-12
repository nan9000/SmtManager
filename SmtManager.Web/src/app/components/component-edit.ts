import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { SmtService, Component as SmtComponent } from '../smt.service';

@Component({
  selector: 'app-component-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './component-edit.html',
  styleUrl: './component-edit.css',
})
export class ComponentEdit implements OnInit {
  component: any = { name: '', description: '', quantity: 0 };
  isEditMode = false;
  id: number | null = null;

  constructor(
    private smtService: SmtService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.isEditMode = true;
      this.loadComponent(this.id);
    }
  }

  loadComponent(id: number): void {
    this.smtService.getComponent(id).subscribe(data => {
      this.component = data;
    });
  }

  save(): void {
    if (this.isEditMode && this.id) {
      this.smtService.updateComponent(this.id, this.component).subscribe(() => {
        this.router.navigate(['/components']);
      });
    } else {
      this.smtService.addComponent(this.component).subscribe(() => {
        this.router.navigate(['/components']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/components']);
  }
}
