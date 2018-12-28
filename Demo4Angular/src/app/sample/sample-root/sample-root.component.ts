import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-sample-root',
  templateUrl: './sample-root.component.html',
  styleUrls: ['./sample-root.component.css']
})
export class SampleRootComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() { }

  A() {
    this.router.navigate(['a'], { relativeTo: this.route });
  }

  B() {
    this.router.navigate(['b'], { relativeTo: this.route });
  }
}
