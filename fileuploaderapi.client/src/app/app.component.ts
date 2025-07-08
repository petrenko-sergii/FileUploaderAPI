import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Component} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent  {
  selectedFile: File | null = null;
  selectedFileName: string = '';
  isUploading = false;

  constructor(private http: HttpClient) { }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.selectedFile = file;
      this.selectedFileName = file.name;
    }
  }

  uploadFile() {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.isUploading = true;

    this.http.post('/api/files/upload', formData, {
      reportProgress: true,
      observe: 'events'
    }).subscribe({
      next: (event: HttpEvent<any>) => {
        if (event.type === HttpEventType.Response) {
          this.isUploading = false;
          this.selectedFile = null;
          this.selectedFileName = '';
          // Optionally, handle success (e.g., show a message or refresh file list)
        }
      },
      error: (error) => {
        this.isUploading = false;
        this.selectedFile = null;
        console.error('File upload error:', error?.message || error);
        // Optionally, handle error (e.g., show an error message)
      }
    });
  }

  title = 'fileuploaderapi.client';
}
