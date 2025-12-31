import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OverdueGrievances } from './overdue-grievances';

describe('OverdueGrievances', () => {
  let component: OverdueGrievances;
  let fixture: ComponentFixture<OverdueGrievances>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OverdueGrievances]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OverdueGrievances);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
