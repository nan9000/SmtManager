import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

import { ComponentEdit } from './component-edit';

describe('ComponentEdit', () => {
  let component: ComponentEdit;
  let fixture: ComponentFixture<ComponentEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentEdit],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ComponentEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
