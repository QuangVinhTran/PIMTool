import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {TranslateService} from "@ngx-translate/core";
import {Store} from "@ngrx/store";
import {selectAllowMultiLanguages} from "../../core/store/setting/setting.selectors";
import {getLocalLanguage} from "../../core/utils/localStorage.util";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  constructor(
    private router: Router,
    private translate: TranslateService,
    private store: Store
  ) {}

  allowMultiLanguages!: boolean
  currentLanguage!: string

  ngOnInit() {
    this.store.select(selectAllowMultiLanguages).subscribe(value => this.allowMultiLanguages = value)
    this.setLang()
  }

  setLang() {
    const lang = getLocalLanguage()
    if (!lang || !['en', 'fr'].includes(lang)) {
      this.switchLanguage('en')
      this.currentLanguage = 'en'
    } else {
      this.currentLanguage = lang
    }
  }

  logout() {
    localStorage.removeItem('access_token')
    localStorage.removeItem('refresh_token')
    this.router.navigate(['login'])
  }

  displayLanguageOptions = false

  switchLanguage(langCode: string) {
    this.translate.use(langCode)
    localStorage.setItem('lang', langCode)
    this.displayLanguageOptions = false
  }
}
