import { Injectable, TemplateRef } from '@angular/core';
import { Toast, ToastOptions } from '../interfaces/toast';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  toasts: Toast[] = [];

  show(textOrTpl: string | TemplateRef<unknown>, options: ToastOptions = {}) {
    this.toasts.push({ textOrTpl, options });
  }

  showError(
    textOrTpl: string | TemplateRef<unknown>,
    options: ToastOptions = {}
  ) {
    this.toasts.push({
      textOrTpl,
      options: { ...options, className: 'bg-danger text-light', delay: 10000 },
    });
  }

  showSuccess(
    textOrTpl: string | TemplateRef<unknown>,
    options: ToastOptions = {}
  ) {
    this.toasts.push({
      textOrTpl,
      options: { ...options, className: 'bg-success text-light' },
    });
  }

  showInfo(
    textOrTpl: string | TemplateRef<unknown>,
    options: ToastOptions = {}
  ) {
    this.toasts.push({
      textOrTpl,
      options: { ...options, className: 'bg-info text-light' },
    });
  }

  showWarning(
    textOrTpl: string | TemplateRef<unknown>,
    options: ToastOptions = {}
  ) {
    this.toasts.push({
      textOrTpl,
      options: { ...options, className: 'bg-warning text-light' },
    });
  }

  remove(toast: Toast) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }

  clear() {
    this.toasts.splice(0, this.toasts.length);
  }
}
