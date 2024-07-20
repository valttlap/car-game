import {
  Directive,
  EventEmitter,
  HostBinding,
  HostListener,
  Input,
  Output,
} from '@angular/core';

export type SortColumn = 'country' | 'date' | 'diplomatNumber' | '';
export type SortDirection = 'asc' | 'desc' | '';
export const rotate: { [key: string]: SortDirection } = {
  asc: 'desc',
  desc: '',
  '': 'asc',
};

export interface SortEvent {
  column: SortColumn;
  direction: SortDirection;
}

@Directive({
  selector: '[appSortHeader]',
  standalone: true,
})
export class SortHeaderDirective {
  @Input() sortable: SortColumn = '';
  @Input() direction: SortDirection = '';
  @Output() sort = new EventEmitter<SortEvent>();

  @HostBinding('class.asc')
  get isAsc() {
    return this.direction === 'asc';
  }

  @HostBinding('class.desc')
  get isDesc() {
    return this.direction === 'desc';
  }

  @HostListener('click')
  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({ column: this.sortable, direction: this.direction });
  }
}
