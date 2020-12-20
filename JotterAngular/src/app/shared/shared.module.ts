import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ToastrModule } from 'ngx-toastr';

import { MaterialModule } from './app-material-design';
import { NoteDetailsComponent } from './dialogs/note-details/note-details.component';
import { CategoryPasswordComponent } from './dialogs/category-password/category-password.component';
import { AddCategoryComponent } from './dialogs/add-category/add-category.component';
import { AddNoteComponent } from './dialogs/add-note/add-note.component';

@NgModule({
  declarations: [
    NoteDetailsComponent,
    CategoryPasswordComponent,
    AddCategoryComponent,
    AddNoteComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    ToastrModule.forRoot()
  ],
  exports: [
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    ToastrModule
  ]
})
export class SharedModule { }
