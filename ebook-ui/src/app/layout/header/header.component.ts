import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})

export class HeaderComponent implements OnInit {

  userName: string;

  constructor(private authService: AuthenticationService,
    private router: Router) { }

  ngOnInit(): void {
    this.authService.authChanged.subscribe(a => {
      if (a.event === 'login') {
        this.userName = this.authService.userName;
      } else {
        this.userName = '';
      }
    });
  }

  logout(): void {
    this.authService.logout(false);
    this.router.navigateByUrl('/login');
  }
}
