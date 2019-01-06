import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-sample-a-detail',
  template: `<p>{{id$ | async}}</p>
  <a (click)="click()">navigage to id:2</a>`,
  styles: [``]
})
export class SampleADetailComponent implements OnInit {
  id$: Observable<number>;
  id: number;
  constructor(
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.id$ = this.route.paramMap.pipe(
      map(params => +params.get('id'))
    );
    this.id = +this.route.snapshot.paramMap.get('id');
  }

  goback() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  click() {
    this.router.navigate(['../', 2], { relativeTo: this.route });
  }
}
