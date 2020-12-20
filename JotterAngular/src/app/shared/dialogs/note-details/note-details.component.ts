import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Note } from '../../classes/note';

@Component({
  selector: 'app-note-details',
  templateUrl: './note-details.component.html',
  styleUrls: ['./note-details.component.scss']
})
export class NoteDetailsComponent implements OnInit {

  note: Note;

  constructor(
    public dialogRef: MatDialogRef<NoteDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Note
  ) { }

  ngOnInit(): void {
    this.note = this.data;
    this.note.files = [{ id: '1', fileName: 'First attach' }];
  }

  onClick(): void {
    this.dialogRef.close();
  }

}
