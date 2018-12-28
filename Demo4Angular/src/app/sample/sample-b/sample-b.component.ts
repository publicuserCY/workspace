import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-sample-b',
  templateUrl: './sample-b.component.html',
  styleUrls: ['./sample-b.component.css']
})
export class SampleBComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() { }

  goback() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  A() {
    this.router.navigate(['../a'], { relativeTo: this.route });
  }

}
