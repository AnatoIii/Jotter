import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NoteService } from 'src/app/core/services/note.service';
import { ToastrService } from 'ngx-toastr';
import { Note } from '../../classes/note';
import { AddNoteComponent } from '../add-note/add-note.component';
import { ConfirmationModalComponent } from '../confirmation-modal/confirmation-modal.component';

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
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: Note,
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

  onEdit() {
    const dialogRef = this.dialog.open(AddNoteComponent, {
      width: '400px',
      data: this.note
    });  

    dialogRef.afterClosed().subscribe((result: Note) => {
      if (result) {
        result.id = this.note.id;
        this.noteService.editNote(result)
          .pipe(takeUntil(this.unsubscribe))
          .subscribe(response => {
            if (!response.isSuccessful) {
              this.showError(response.error);
              return;
            }
            
            this.dialogRef.close({ deleted: false, note: response.responseResult });
          });
      }
    });
  }

  onDelete() {
    const dialogRef = this.dialog.open(ConfirmationModalComponent, {
      width: '400px',
      data: { title: `Do you want to delete '${this.note.name}'?`, yes: 'Yes', no: 'No' }
    });  

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.noteService.deleteNote(this.note.id)
          .pipe(takeUntil(this.unsubscribe))
          .subscribe(response => {});
        this.dialogRef.close({ deleted: true });
      }
    });
  }

  showError(error: string) {
    this.toastr.error(error, 'Error');
  }
}
