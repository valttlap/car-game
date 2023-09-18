import { TemplateRef } from '@angular/core';

export interface ToastOptions {
  className?: string;
  delay?: number;
  autohide?: boolean;
}

export interface Toast {
  textOrTpl: string | TemplateRef<unknown>;
  options?: ToastOptions;
}
