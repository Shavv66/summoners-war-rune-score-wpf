import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RunesGridComponent } from './runes-grid.component';

describe('RunesGridComponent', () => {
  let component: RunesGridComponent;
  let fixture: ComponentFixture<RunesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RunesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RunesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
