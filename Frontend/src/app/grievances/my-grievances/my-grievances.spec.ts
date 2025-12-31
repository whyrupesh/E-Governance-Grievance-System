import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyGrievancesComponent } from './my-grievances';

describe('MyGrievances', () => {
  let component: MyGrievancesComponent;
  let fixture: ComponentFixture<MyGrievancesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyGrievancesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyGrievancesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
