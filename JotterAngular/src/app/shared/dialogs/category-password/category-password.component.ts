import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-category-password',
  templateUrl: './category-password.component.html',
  styleUrls: ['./category-password.component.scss']
})
export class CategoryPasswordComponent {
  password: string;

  constructor(
    public dialogRef: MatDialogRef<CategoryPasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { categoryName: string }
  ) { }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  get category(): any {
    return { password: this.password };
  }
}
