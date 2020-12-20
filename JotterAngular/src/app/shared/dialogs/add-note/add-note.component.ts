import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Note } from '../../classes/note';

@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.scss']
})
export class AddNoteComponent {
  name: string;
  description: string;
  edit: boolean;
  oldName: string;

  constructor(
    public dialogRef: MatDialogRef<AddNoteComponent>,
    @Inject(MAT_DIALOG_DATA) data: Note
  ) { 
    if (data != null) {
      this.edit = true;
      this.oldName = data.name;
      this.name = data.name;
      this.description = data.description;  
    }    
  }

  getTitle(): string {
    return this.edit ? `Edit ${this.oldName}` : "Create new note";
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  get note(): any {
    return { name: this.name, description: this.description };
  }
}
