import { Component } from '@angular/core';
import {faCircleNotch, faSpinner} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-page-loader',
  templateUrl: './page-loader.component.html',
  styleUrls: ['./page-loader.component.scss']
})
export class PageLoaderComponent {

  protected readonly faSpinner = faSpinner;
  protected readonly faCircleNotch = faCircleNotch;
}
