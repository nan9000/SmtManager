import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

import { BoardEdit } from './board-edit';

describe('BoardEdit', () => {
  let component: BoardEdit;
  let fixture: ComponentFixture<BoardEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoardEdit],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(BoardEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
