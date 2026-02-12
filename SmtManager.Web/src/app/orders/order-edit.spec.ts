import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';

import { OrderEdit } from './order-edit';

describe('OrderEdit', () => {
  let component: OrderEdit;
  let fixture: ComponentFixture<OrderEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderEdit],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(OrderEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
