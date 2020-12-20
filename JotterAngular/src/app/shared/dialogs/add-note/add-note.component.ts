import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Note } from '../../classes/note';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.scss']
})
export class AddNoteComponent implements OnInit {
  _name: string = '';
  description: string = '';
  edit: boolean;
  oldName: string;

  constructor(
    public dialogRef: MatDialogRef<AddNoteComponent>,
    @Inject(MAT_DIALOG_DATA) data: Note,
    private formBuilder: FormBuilder
  ) { 
    if (data != null) {
      this.edit = true;
      this.oldName = data.name;
      this._name = data.name;
      this.description = data.description;  
    }    
  }
  getTitle(): string {
    return this.edit ? `Edit ${this.oldName}` : "Create new note";
  }

  addForm: FormGroup;

  get name(): AbstractControl {
    return this.addForm.get('name');
  }

  ngOnInit(): void {
    this.addForm = this.formBuilder.group({
      name: [this._name, [Validators.required, Validators.minLength(3)]],
      description: [this.description],
    });
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  get note(): any {
    return this.addForm.value;
  }
}
