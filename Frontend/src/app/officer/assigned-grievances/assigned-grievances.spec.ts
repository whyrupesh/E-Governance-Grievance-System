import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedGrievancesComponent } from './assigned-grievances';

describe('AssignedGrievances', () => {
  let component: AssignedGrievancesComponent;
  let fixture: ComponentFixture<AssignedGrievancesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignedGrievancesComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AssignedGrievancesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
