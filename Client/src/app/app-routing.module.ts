import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SightingComponent } from './views/sighting/sighting.component';

const routes: Routes = [
  {
    path: '',
    component: SightingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
