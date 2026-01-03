import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GrievancesById } from './grievances-by-id';

describe('GrievancesById', () => {
  let component: GrievancesById;
  let fixture: ComponentFixture<GrievancesById>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GrievancesById]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GrievancesById);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
