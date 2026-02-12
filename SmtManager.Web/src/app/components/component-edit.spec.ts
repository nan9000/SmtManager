import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentEdit } from './component-edit';

describe('ComponentEdit', () => {
  let component: ComponentEdit;
  let fixture: ComponentFixture<ComponentEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentEdit]
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
