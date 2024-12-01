import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from './service/shared.service';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Project_Management_ELCA_FE';
  siteLanguage = 'English';
  siteCodeLang = 'en';
  languageList = [
    { code: 'en', label: 'English' },
    { code: 'fr', label: 'French' },
  ];

  constructor(
    private router: Router,
    public sharedService: SharedService,
    private translate: TranslateService
  ) {}

  changeSiteLanguage(localeCode: string): void {
    const selectedLanguage = this.languageList
      .find((language) => language.code === localeCode)
      ?.label.toString();

    if (selectedLanguage) {
      this.siteCodeLang = localeCode;
      this.siteLanguage = selectedLanguage;
      this.translate.use(this.siteCodeLang);
    }

    console.log(localeCode);

    const currentLanguage = this.translate.currentLang;
    console.log('current language is: ', currentLanguage);
  }

  isActiveRoute(route: string): boolean {
    return this.router.url === route;
  }

  navigateToProjectDetail() {
    this.router.navigate(['/project', '']);
  }

  changeIsUpdate(val: boolean) {
    this.sharedService.setIsUpdate(val);
  }
}
