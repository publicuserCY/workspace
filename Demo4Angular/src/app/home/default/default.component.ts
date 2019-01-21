import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.css']
})
export class DefaultComponent implements OnInit {
  isCollapsed = true;
  openMap = {
    identity: true,
    login: false,
    other: false
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {

  }

  openHandler(value: string): void {
    for (const key in this.openMap) {
      if (key !== value) {
        this.openMap[key] = false;
      }
    }
  }

  navToClients() {
    this.router.navigate(['authority', 'clients']);
  }

  navToUsers() {
    this.router.navigate(['authority', 'users']);
  }
}
