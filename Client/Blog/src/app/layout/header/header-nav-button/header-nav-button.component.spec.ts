import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderNavButtonComponent } from './header-nav-button.component';

describe('HeaderNavButtonComponent', () => {
  let component: HeaderNavButtonComponent;
  let fixture: ComponentFixture<HeaderNavButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderNavButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderNavButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
