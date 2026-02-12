import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SmtService, Board, Component as ComponentModel } from '../smt.service';

@Component({
    selector: 'app-board-list',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './board-list.component.html',
    styleUrls: ['./board-list.component.css']
})
export class BoardListComponent implements OnInit {
    boards: Board[] = [];
    newBoard: any = { name: '', description: '', length: 0, width: 0 };

    // Component Selection
    availableComponents: ComponentModel[] = [];
    selectedComponentId: number | null = null;
    componentPlacementCount: number = 1;
    currentBoardComponents: { componentId: number, name: string, placementCount: number }[] = [];

    constructor(private smtService: SmtService) { }

    ngOnInit(): void {
        this.loadBoards();
        this.loadComponents();
    }

    loadBoards(): void {
        this.smtService.getBoards().subscribe(data => {
            this.boards = data;
        });
    }

    loadComponents(): void {
        this.smtService.getComponents().subscribe(data => {
            this.availableComponents = data;
        });
    }

    addComponentToBoard(): void {
        if (!this.selectedComponentId || this.componentPlacementCount <= 0) return;

        const component = this.availableComponents.find(c => c.id === Number(this.selectedComponentId));
        if (!component) return;

        if (this.currentBoardComponents.some(c => c.componentId === component.id)) {
            alert('Component already added to board.');
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

    addBoard(): void {
        const boardToSend = {
            ...this.newBoard,
            boardComponents: this.currentBoardComponents.map(c => ({
                componentId: c.componentId,
                placementCount: c.placementCount
            }))
        };

        this.smtService.addBoard(boardToSend).subscribe(board => {
            this.boards.push(board);
            this.newBoard = { name: '', description: '', length: 0, width: 0 };
            this.currentBoardComponents = [];
        });
    }

    deleteBoard(id: number): void {
        this.smtService.deleteBoard(id).subscribe(() => {
            this.boards = this.boards.filter(b => b.id !== id);
        });
    }
}
