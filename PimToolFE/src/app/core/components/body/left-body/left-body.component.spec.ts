import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftBodyComponent } from './left-body.component';

describe('LeftBodyComponent', () => {
  let component: LeftBodyComponent;
  let fixture: ComponentFixture<LeftBodyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftBodyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeftBodyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
