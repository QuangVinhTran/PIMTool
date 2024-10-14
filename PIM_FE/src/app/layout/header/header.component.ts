import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/shared/services/auth.service';

@Component({
  selector: 'layout-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  lang : string = '';
  constructor(private translateService : TranslateService, private authService : AuthService, private router : Router) {

  }  
  ngOnInit(): void {
      this.lang = localStorage.getItem('lang') || 'en';
  }
  changeLang(element : any) {
    const selectedLang = element.getAttribute("value");
    if(selectedLang === "en"){
      this.lang = "en"
    }else{
      this.lang = "fr"
    }

    localStorage.setItem('lang', this.lang);
    this.translateService.use(this.lang);
  }
  public logout() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
