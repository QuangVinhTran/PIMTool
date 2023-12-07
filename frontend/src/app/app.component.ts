import { Component } from '@angular/core';
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'PIM Tool';
  constructor(public translate: TranslateService) {
    translate.addLangs(['en', 'vn', 'fr', 'jp'])
    const lang = localStorage.getItem('lang') || 'en'
    localStorage.setItem('lang', lang)
    translate.setDefaultLang(lang)
  }
}
