import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { SmtService, Board } from '../smt.service';

@Component({
    selector: 'app-board-list',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './board-list.component.html',
    styleUrls: ['./board-list.component.css']
})
export class BoardListComponent implements OnInit {
    boards: Board[] = [];
    filteredBoards: Board[] = [];
    searchTerm: string = '';

    constructor(private smtService: SmtService, private router: Router) { }

    ngOnInit(): void {
        this.loadBoards();
    }

    loadBoards(): void {
        this.smtService.getBoards().subscribe(data => {
            this.boards = data;
            this.filterBoards();
        });
    }

    filterBoards(): void {
        if (!this.searchTerm) {
            this.filteredBoards = this.boards;
        } else {
            const term = this.searchTerm.toLowerCase();
            this.filteredBoards = this.boards.filter(b =>
                b.name.toLowerCase().includes(term) ||
                b.description.toLowerCase().includes(term)
            );
        }
    }

    createBoard(): void {
        this.router.navigate(['/boards/create']);
    }

    editBoard(id: number): void {
        this.router.navigate(['/boards/edit', id]);
    }

    deleteBoard(id: number): void {
        if (confirm('Are you sure you want to delete this board?')) {
            this.smtService.deleteBoard(id).subscribe(() => {
                this.boards = this.boards.filter(b => b.id !== id);
                this.filterBoards();
            });
        }
    }
}
