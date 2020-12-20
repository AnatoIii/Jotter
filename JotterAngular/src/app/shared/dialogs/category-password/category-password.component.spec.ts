import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryPasswordComponent } from './category-password.component';

describe('CategoryPasswordComponent', () => {
  let component: CategoryPasswordComponent;
  let fixture: ComponentFixture<CategoryPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryPasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
