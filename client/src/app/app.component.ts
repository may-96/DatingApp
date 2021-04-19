import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Demo App';
  users: any;

  constructor(private http: HttpClient){

  }
  ngOnInit(): void {
    this.http.get("https://localhost:5001/api/appusers").subscribe({
      next: (response) => this.users = response,
      error: (err) => console.log(err)
    })
  }
}
