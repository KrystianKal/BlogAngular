import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleSocialsComponent } from './article-socials.component';

describe('ArticleSocialsComponent', () => {
  let component: ArticleSocialsComponent;
  let fixture: ComponentFixture<ArticleSocialsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArticleSocialsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArticleSocialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
