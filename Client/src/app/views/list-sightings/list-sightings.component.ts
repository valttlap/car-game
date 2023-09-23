import {
  Component,
  OnInit,
  QueryList,
  TemplateRef,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {
  SortEvent,
  SortHeaderDirective,
} from 'src/app/dirctives/sort-header.directive';
import { SightingClient, SightingUserDto } from 'src/app/services/api';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-list-sightings',
  templateUrl: './list-sightings.component.html',
  styleUrls: ['./list-sightings.component.scss'],
})
export class ListSightingsComponent implements OnInit {
  compare = (v1: string | Date, v2: string | Date) =>
    v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

  sightings: SightingUserDto[] = [];

  @ViewChildren(SortHeaderDirective)
  headers?: QueryList<SortHeaderDirective>;

  @ViewChild('content') content?: TemplateRef<unknown>;

  constructor(
    private sightingClient: SightingClient,
    private modalService: NgbModal,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.getSightings();
  }

  private getSightings(): void {
    this.sightingClient.getSightings().subscribe({
      next: sightings => {
        this.sightings = sightings;
      },
      error: error => {
        this.toastService.showError(error);
      },
    });
  }

  onSort({ column, direction }: SortEvent) {
    // resetting other headers
    this.headers?.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // sorting countries
    if (direction === '' || column === '') {
      this.getSightings();
    } else {
      let res: 1 | 0 | -1;
      this.sightings = [...this.sightings].sort((a, b) => {
        switch (column) {
          case 'country':
            res = this.compare(a.country, b.country);
            return direction === 'asc' ? res : -res;
          case 'date':
            res = this.compare(a.date, b.date);
            return direction === 'asc' ? res : -res;
          default:
            return 0;
        }
      });
    }
  }

  deleteSighting(id: number): void {
    this.modalService
      .open(this.content, {
        ariaLabelledBy: 'modal-basic-title',
      })
      .result.then(result => {
        if (result) {
          this.sightingClient.deleteSighting(id).subscribe({
            next: () => {
              this.toastService.showSuccess('Havainto poistettu!');
              this.getSightings();
            },
            error: error => {
              this.toastService.showError(error);
            },
          });
        }
      });
  }
}
