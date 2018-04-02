import { TestBed, inject } from '@angular/core/testing';

import { NavBarStateService } from './nav-bar-state.service';

describe('NavBarStateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NavBarStateService]
    });
  });

  it('should be created', inject([NavBarStateService], (service: NavBarStateService) => {
    expect(service).toBeTruthy();
  }));
});
