import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Technicians } from './technicians';

describe('Technicians', () => {
  let component: Technicians;
  let fixture: ComponentFixture<Technicians>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Technicians],
    }).compileComponents();

    fixture = TestBed.createComponent(Technicians);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
