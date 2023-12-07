import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RightBodyComponent } from './right-body.component';

describe('RightBodyComponent', () => {
  let component: RightBodyComponent;
  let fixture: ComponentFixture<RightBodyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RightBodyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RightBodyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
