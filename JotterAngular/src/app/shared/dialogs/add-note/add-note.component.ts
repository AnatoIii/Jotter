import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.scss']
})
export class AddNoteComponent {

  addForm: FormGroup;

  get name(): AbstractControl {
    return this.addForm.get('name');
  }

  constructor(
    public dialogRef: MatDialogRef<AddNoteComponent>,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.addForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
    });
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  get note(): any {
    return this.addForm.value;
  }
}
