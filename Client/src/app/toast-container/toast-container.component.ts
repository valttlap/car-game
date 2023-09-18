import { Component, HostBinding, TemplateRef } from '@angular/core';
import { NgFor, NgIf, NgTemplateOutlet } from '@angular/common';
import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from '../services/toast.service';
import { Toast } from '../interfaces/toast';

@Component({
  selector: 'app-toast-container',
  standalone: true,
  imports: [NgbToastModule, NgIf, NgTemplateOutlet, NgFor],
  template: `
    <ngb-toast
      *ngFor="let toast of toastService.toasts"
      [class]="toast.options?.className || ''"
      [autohide]="true"
      [delay]="toast.options?.delay || 5000"
      (hidden)="toastService.remove(toast)">
      <ng-template [ngIf]="isTemplate(toast)" [ngIfElse]="text">
        <ng-template [ngTemplateOutlet]="toast.textOrTpl"></ng-template>
      </ng-template>

      <ng-template #text>{{ toast.textOrTpl }}</ng-template>
    </ngb-toast>
  `,
})
export class ToastContainerComponent {
  @HostBinding('class') toasClass =
    'toast-container position-fixed top-0 end-0 p-3';
  @HostBinding('style') toastStyle = 'z-index: 1200';

  constructor(public toastService: ToastService) {}

  isTemplate(toast: Toast): toast is { textOrTpl: TemplateRef<unknown> } {
    return toast.textOrTpl instanceof TemplateRef;
  }
}
