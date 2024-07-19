import { Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: MainComponent, canActivate: [authGuard] },
];
