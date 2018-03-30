import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RunesPageComponent } from './runes-page.component';

describe('RunesPageComponent', () => {
  let component: RunesPageComponent;
  let fixture: ComponentFixture<RunesPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RunesPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RunesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
