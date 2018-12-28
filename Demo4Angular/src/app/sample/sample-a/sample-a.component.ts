import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-sample-a',
  templateUrl: './sample-a.component.html',
  styleUrls: ['./sample-a.component.css']
})
export class SampleAComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {  }

  goback() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  B() {
    this.router.navigate(['../b'], { relativeTo: this.route });
  }
}
