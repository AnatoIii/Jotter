import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { NoteService } from '../core/services/note.service';
import { Category } from '../shared/classes/category';
import { Note } from '../shared/classes/note';
import { NoteDetailsComponent } from '../shared/dialogs/note-details/note-details.component';
import { AddCategoryComponent } from '../shared/dialogs/add-category/add-category.component';
import { AddNoteComponent } from '../shared/dialogs/add-note/add-note.component';
import { AuthService } from '../core/services/auth.service';
import { User } from '../shared/classes/user';
import { CategoryPasswordComponent } from '../shared/dialogs/category-password/category-password.component';
import { ConfirmationModalComponent } from '../shared/dialogs/confirmation-modal/confirmation-modal.component';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  user: User;

  categories: Category[] = null;
  selectedCategory: Category;
  showLoader: boolean = false;
  notes: Note[];

  constructor(
    private dialog: MatDialog,
    private toastr: ToastrService,
    private noteService: NoteService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.authService.saveUser()
      .subscribe(user => {
        this.user = user;
      });

    this.getAllCategories();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  isAuthentificated() {
    return this.user != null;
  }

  isCategorySelected(id: string): boolean {
    return this.selectedCategory && id == this.selectedCategory.id;
  }

  getAllCategories() {
    this.showLoader = true;
    this.noteService.getCategories()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(this.categories);
          this.categories = response.responseResult.categories;
          this.showLoader = false;
        },
        error => {
          this.showLoader = false;
          this.showError(error.message.error || error.message);
        }
      );
  }

  getCategoryNotes(id: string): void {
    this.selectedCategory = this.categories.find(category => category.id === id);
    this.notes = undefined;
    if (this.selectedCategory.isLocked) {
      const dialogRef = this.dialog.open(CategoryPasswordComponent, {
        width: '400px',
        data: { categoryName: this.selectedCategory.name }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result == null) {
          this.selectedCategory = null;
          return;
        }

        this.getCategoryNotesData(result.password);
      });
    } else {
      this.getCategoryNotesData();
    }
  }

  private getCategoryNotesData(password: string = null) {
    this.noteService.getNotesList(this.selectedCategory.id, password)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          if (!response.isSuccessful) {
            this.showError(response.error);
            this.selectedCategory = null;
            return;
          }
          this.notes = response.responseResult.notes;
        },
        error => {
          this.showLoader = false;
          this.showError(error.message.error || error.message);
        }
      );
  }

  getNoteDetails(id: string): void {
    this.noteService.getNote(id)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          this.notes = response.responseResult;
        },
        error => {
          this.showLoader = false;
          this.showError(error.message.error || error.message);
        }
      );
  }

  addCategory(): void {
    const dialogRef = this.dialog.open(AddCategoryComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(category => {
      console.log(category);

      if (category != null) {
        this.noteService.addCategory(category)
          .pipe(takeUntil(this.unsubscribe))
          .subscribe(
            response => {
              this.categories.push(response.responseResult);
            },
            error => {
              this.showError(error.message.error || error.message);
            }
          );
      };
    });
  }

  noCategories(): boolean {
    return this.categories == null || this.categories.length == 0;
  }

  addNote(): void {
    const dialogRef = this.dialog.open(AddNoteComponent, {
      width: '400px'
    });
    
    dialogRef.afterClosed().subscribe((note: Note) => {
      if (!note) {
        return;
      }

      note.categoryID = this.selectedCategory.id;

      this.noteService.addNote(note)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(
          response => {
            if (!response.isSuccessful) {
              this.showError(response.error);
              return;
            }
            this.notes.push(response.responseResult);
          },
          error => {
            this.showError(error.message.error || error.message);
          }
        );
    });
  }

  openDialog(note: Note): void {
    const dialogRef = this.dialog.open(NoteDetailsComponent, {
      width: '600px',
      data: note
    });

    dialogRef.afterClosed().subscribe(res => {
      if (!res) {
        return;
      }
      if (res.deleted) {
        this.notes = this.notes.filter(n => n.id != note.id);
      }
      else if (res.note) {
        this.notes = this.notes.map(n => n.id == res.note.id ? res.note : n);
      }
    });
  }

  showError(error: string) {
    this.toastr.error(error, 'Error');
  }

  canShowCategoryNotes(): boolean {
    if (!this.selectedCategory) {
      return false;
    }

    if (!this.selectedCategory.isLocked) {
      return true;
    }

    if (this.selectedCategory.isLocked && this.notes == undefined) {
      return false;
    }

    return true;
  }
}

const categories = [
  {
    id: '1',
    name: 'Work',
    isLocked: true
  },
  {
    id: '2',
    name: 'University',
    isLocked: false
  },
  {
    id: '3',
    name: 'Personal',
    isLocked: false
  },
  {
    id: '4',
    name: 'Foood',
    isLocked: false
  },
];

const notes = [
  {
    id: '4',
    name: 'Buy the book',
    description: 'author Dan Brown',
    files: []
  },
  {
    id: '4',
    name: 'Dinner',
    description: 'Make lasagna for dinner',
    files: []
  },
  {
    id: '4',
    name: 'Milk',
    description: 'Buy milk for pancakes',
    files: []
  }
];