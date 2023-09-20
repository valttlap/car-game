import { map, take } from 'rxjs';
import { Injectable, inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root',
})
export class PermissionService {
  canActivate(auth: AuthService) {
    return auth.isAuthenticated$.pipe(
      take(1),
      map(isAuthenticated => {
        if (!isAuthenticated) {
          auth.loginWithRedirect();
          return false;
        }
        return true;
      })
    );
  }
}

export const canActivate: CanActivateFn = () => {
  return inject(PermissionService).canActivate(inject(AuthService));
};
