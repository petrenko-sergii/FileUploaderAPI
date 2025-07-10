import { HttpClient, HttpEventType } from '@angular/common/http';
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
  uploadedFileInfo: string = '';
  uploadProgress = 0;
  uploadedMB = 0;
  totalMB = 0;

  readonly CHUNK_SIZE = 100 * 1024 * 1024; // 100 MB

  constructor(private http: HttpClient) { }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.selectedFileName = this.selectedFile.name;
      this.uploadProgress = 0;
      this.uploadedMB = 0;
      this.totalMB = +(this.selectedFile.size / (1024 * 1024)).toFixed(2);
      this.uploadedFileInfo = '';
    } else {
      this.selectedFile = null;
      this.selectedFileName = '';
      this.uploadProgress = 0;
      this.uploadedMB = 0;
      this.totalMB = 0;
      this.uploadedFileInfo = '';
    }
  }

  async uploadFile() {
    if (!this.selectedFile) return;

    this.isUploading = true;
    this.uploadProgress = 0;
    this.uploadedMB = 0;
    this.totalMB = +(this.selectedFile.size / (1024 * 1024)).toFixed(2);
    this.uploadedFileInfo = '';

    const file = this.selectedFile;
    const totalChunks = Math.ceil(file.size / this.CHUNK_SIZE);

    let uploadedBytes = 0;

    for (let chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++) {
      const start = chunkIndex * this.CHUNK_SIZE;
      const end = Math.min(start + this.CHUNK_SIZE, file.size);
      const chunk = file.slice(start, end);

      const formData = new FormData();
      formData.append('file', chunk, file.name);
      formData.append('chunkIndex', chunkIndex.toString());
      formData.append('totalChunks', totalChunks.toString());

      try {
        const response: any = await this.http.post('/api/upload', formData, {
          reportProgress: true,
          observe: 'events'
        }).toPromise();

        if (response && response.type === HttpEventType.Response && response.body && typeof response.body.message === 'string') {
          this.uploadedFileInfo = response.body.message + ', size is ' + this.totalMB + ' MB.';
        }
      } catch (error) {
        this.isUploading = false;
        this.uploadedFileInfo = 'Upload failed.';
        return;
      }

      uploadedBytes = end;
      this.uploadedMB = +(uploadedBytes / (1024 * 1024)).toFixed(2);
      this.uploadProgress = Math.round((uploadedBytes / file.size) * 100);
    }

    this.isUploading = false;
    this.selectedFile = null;
    this.selectedFileName = '';
  }

  title = 'fileuploaderapi.client';
}
