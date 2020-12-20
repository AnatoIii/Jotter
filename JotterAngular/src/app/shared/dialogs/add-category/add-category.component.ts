import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.scss']
})
export class AddCategoryComponent {
  name: string;
  password: string;

  constructor(
    public dialogRef: MatDialogRef<AddCategoryComponent>
  ) { }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  get category(): any {
    return { name: this.name, password: this.password };
  }
}
