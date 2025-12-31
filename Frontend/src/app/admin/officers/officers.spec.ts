import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Officers } from './officers';

describe('Officers', () => {
  let component: Officers;
  let fixture: ComponentFixture<Officers>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Officers]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Officers);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
