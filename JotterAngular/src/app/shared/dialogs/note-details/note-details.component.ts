import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NoteService } from 'src/app/core/services/note.service';

import { Note } from '../../classes/note';
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
    public dialogRef: MatDialogRef<NoteDetailsComponent>,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: Note,
    private noteService: NoteService
  ) { }

  ngOnInit(): void {
    this.note = this.data;
    this.note.files = [{ id: '1', fileName: 'First attach' }];
  }

  onClick(): void {
    this.dialogRef.close();
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
        this.dialogRef.close(true);
      }
    });
  }
}
