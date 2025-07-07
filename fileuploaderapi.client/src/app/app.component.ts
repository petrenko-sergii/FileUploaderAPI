import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface File {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public files: File[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getFiles();
  }

  getFiles() {
    this.http.get<File[]>('/api/files').subscribe(
      (result) => {
        this.files = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  title = 'fileuploaderapi.client';
}
