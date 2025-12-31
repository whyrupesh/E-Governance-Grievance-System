import { Component } from '@angular/core';
import { Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Navbar} from './shared/components/navbar/navbar';
import { Sidebar} from './shared/components/sidebar/sidebar';
import { NgIf } from '@angular/common';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    MatSidenavModule,
    Navbar,
    Sidebar,
    NgIf                // ✅ THIS FIXES THE ERROR
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {

  showLayout = true;

  constructor(private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.showLayout = !['/login', '/register']
          .includes(event.urlAfterRedirects);
      });
  }
}
