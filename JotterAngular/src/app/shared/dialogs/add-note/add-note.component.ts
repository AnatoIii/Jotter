import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.scss']
})
export class AddNoteComponent {

  name: string;
  description: string;

  constructor(
    public dialogRef: MatDialogRef<AddNoteComponent>
  ) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  get note(): any {
    return { name: this.name, description: this.description };
  }

}
