import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyGrievanceDetail } from './my-grievance-detail';

describe('MyGrievanceDetail', () => {
  let component: MyGrievanceDetail;
  let fixture: ComponentFixture<MyGrievanceDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyGrievanceDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyGrievanceDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
