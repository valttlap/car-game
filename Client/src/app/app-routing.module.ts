import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SightingComponent } from './views/sighting/sighting.component';
import { canActivate } from './auth.guard';

const routes: Routes = [
  {
    path: '',
    component: SightingComponent,
    canActivate: [canActivate],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
