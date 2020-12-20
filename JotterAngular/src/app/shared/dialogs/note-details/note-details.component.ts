import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { NoteService } from 'src/app/core/services/note.service';
import { Note } from '../../classes/note';

@Component({
  selector: 'app-note-details',
  templateUrl: './note-details.component.html',
  styleUrls: ['./note-details.component.scss']
})
export class NoteDetailsComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  note: Note;

  constructor(
    private noteService: NoteService,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<NoteDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Note
  ) { }

  ngOnInit(): void {
    this.note = this.data;
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  loadFile(id: string): void {
    this.noteService.getFile(id)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          const base64 = response.responseResult.base64File;
          window.open("data:application/pdf," + escape(base64));

          //window.open("data:application/octet-stream;charset=utf-16le;base64,"+base64);

          // const a = document.createElement("a");
          // a.href = "data:application/octet-stream;base64,"+base64;
          // a.download = "documentName.pdf"
          // a.click();
        },
        error => {
          this.showError(error.message.error || error.message);
        }
      );
  }

  attachFile(event): void {
    const file: File = event.target.files[0];
    //this.note.files.push({ id: '', name: file.name });

    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      const uploadFile = {
        fileName: file.name,
        base64File: reader.result,
        noteId: this.note.id
      };

      this.noteService.addFiles(uploadFile)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(
          response => {
            console.log(response);
          },
          error => {
            this.showError(error.message.error || error.message);
          }
        );
    };
  }

  onClick(): void {
    this.dialogRef.close();
  }

  showError(error: string) {
    this.toastr.error(error, 'Error');
  }

}
