import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { SmtService, Board, Component as ComponentModel } from '../smt.service';

@Component({
  selector: 'app-board-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './board-edit.html',
  styleUrl: './board-edit.css',
})
export class BoardEdit implements OnInit {
  board: any = { name: '', description: '', length: 0, width: 0, boardComponents: [] };
  isEditMode = false;
  id: number | null = null;

  // Component Selection
  availableComponents: ComponentModel[] = [];
  selectedComponentId: number | null = null;
  componentPlacementCount: number = 1;
  currentBoardComponents: { componentId: number, name: string, placementCount: number }[] = [];

  constructor(
    private smtService: SmtService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadComponents();
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.isEditMode = true;
      this.loadBoard(this.id);
    }
  }

  loadBoard(id: number): void {
    this.smtService.getBoard(id).subscribe(data => {
      this.board = data;
      // Map existing board components to local structure for editing
      if (data.boardComponents) {
        this.currentBoardComponents = data.boardComponents.map(bc => ({
          componentId: bc.componentId,
          // We might not have the name directly on boardComponent, but let's see if the backend returns included component
          name: bc.component ? bc.component.name : `Component ${bc.componentId}`,
          placementCount: bc.placementCount
        }));
      }
    });
  }

  loadComponents(): void {
    this.smtService.getComponents().subscribe(data => {
      this.availableComponents = data;
      // Update names if we loaded components after board
      this.updateComponentNames();
    });
  }

  updateComponentNames(): void {
    this.currentBoardComponents.forEach(bc => {
      const comp = this.availableComponents.find(c => c.id === bc.componentId);
      if (comp) {
        bc.name = comp.name;
      }
    });
  }

  addComponentToBoard(): void {
    if (!this.selectedComponentId || this.componentPlacementCount <= 0) return;

    const component = this.availableComponents.find(c => c.id === Number(this.selectedComponentId));
    if (!component) return;

    if (this.currentBoardComponents.some(c => c.componentId === component.id)) {
      alert('Component already added to board. You can update the quantity if needed.');
      return;
    }

    this.currentBoardComponents.push({
      componentId: component.id,
      name: component.name,
      placementCount: this.componentPlacementCount
    });

    this.selectedComponentId = null;
    this.componentPlacementCount = 1;
  }

  removeComponentFromBoard(componentId: number): void {
    this.currentBoardComponents = this.currentBoardComponents.filter(c => c.componentId !== componentId);
  }

  save(): void {
    const boardToSend = {
      ...this.board,
      boardComponents: this.currentBoardComponents.map(c => ({
        componentId: c.componentId,
        placementCount: c.placementCount
      }))
    };

    if (this.isEditMode && this.id) {
      this.smtService.updateBoard(this.id, boardToSend).subscribe(() => {
        this.router.navigate(['/boards']);
      });
    } else {
      this.smtService.addBoard(boardToSend).subscribe(() => {
        this.router.navigate(['/boards']);
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/boards']);
  }
}
