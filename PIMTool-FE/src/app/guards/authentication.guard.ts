import {CanActivateChildFn, Router} from '@angular/router';
import {LocalStorageService} from "../service/local-storage.service";
import {inject} from "@angular/core";

export const authenticationGuard: CanActivateChildFn = (route, state) => {
  const localStorageService = inject(LocalStorageService);
  const router = inject(Router);
  if (localStorageService.getDataFromLocal("token") === null) {
    router.navigateByUrl("/login").then();
    return false;
  } else {
    return true;
  }
};
