import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.scss']
})
export class AddCategoryComponent {

  addForm: FormGroup;

  get name(): AbstractControl {
    return this.addForm.get('name');
  }

  constructor(
    public dialogRef: MatDialogRef<AddCategoryComponent>,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.addForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      password: [''],
    });
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  get category(): any {
    return this.addForm.value;
  }
}
