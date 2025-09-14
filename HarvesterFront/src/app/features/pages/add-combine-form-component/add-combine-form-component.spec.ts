import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCombineFormComponent } from './add-combine-form-component';

describe('AddCombineFormComponent', () => {
  let component: AddCombineFormComponent;
  let fixture: ComponentFixture<AddCombineFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCombineFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCombineFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
