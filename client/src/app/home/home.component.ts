import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  products: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.http.get('https://localhost:7059/api/products').subscribe({
      next: response => this.products = response,
      error: error => console.log(error),
      complete: () => console.log('request has completed')
    })
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

}
