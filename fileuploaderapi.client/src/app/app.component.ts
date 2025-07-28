import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy {
  selectedFile: File | null = null;
  selectedFileName: string = '';
  isUploading = false;
  uploadSuccess: boolean | null = null;
  uploadedFileInfo: string = '';
  uploadProgress = 0;
  uploadedMB = 0;
  totalMB = 0;
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7024/api/uploadProgressHub', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err =>
      console.error('SignalR Connection Error: ', err)
    );

    this.hubConnection.on('UploadProgress', (data: any) => {
      if (typeof data.progress === 'number') {
        this.uploadProgress = data.progress;
      }
    });
  }

  ngOnDestroy() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

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

    const formData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);

    try {
      const response: any = await this.http.post('/api/upload', formData, {
        reportProgress: true,
        observe: 'events'
      }).toPromise();

      if (response && response.type === HttpEventType.Response) {
        this.uploadedFileInfo = 'Upload complete: ' + this.selectedFile.name + ', size is ' + this.totalMB + ' MB.';
        this.uploadSuccess = true;
      }
    } catch (error) {
      this.isUploading = false;
      this.uploadedFileInfo = 'Upload failed.';
      this.uploadSuccess = false;
      return;
    }

    this.isUploading = false;
    this.selectedFile = null;
    this.selectedFileName = '';
  }

  title = 'fileuploaderapi.client';
}
