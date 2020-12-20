import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Response } from 'src/app/shared/classes/response';
import { Category } from 'src/app/shared/classes/category';
import { Note } from 'src/app/shared/classes/note';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
  ) { }

  getCategories(): Observable<Response> {
    return this.http.get<Response>(
      `${this.apiUrl}/categories`);
  }

  addCategory(category: Category): Observable<Response> {
    return this.http.post<Response>(
      `${this.apiUrl}/categories`,
      { ...category });
  }

  getNote(id: string): Observable<Response> {
    return this.http.get<Response>(
      `${this.apiUrl}/notes/${id}`);
  }

  getNotesList(id: string, password: string): Observable<Response> {
    return this.http.get<Response>(
      `${this.apiUrl}/notes/category?categoryId=${id}&categoryPassword=${password}`);
  }

  addNote(note: Note): Observable<Response> {
    return this.http.post<Response>(
      `${this.apiUrl}/notes`,
      { ...note });
  }

  editNote(note: Note): Observable<Response> {
    return this.http.put<Response>(
      `${this.apiUrl}/notes`,
      { ...note });
  }

  deleteNote(id: string): Observable<Response> {
    return this.http.delete<Response>(
      `${this.apiUrl}/notes/${id}`);
  }

  getFiles(id: string): Observable<Response> {
    return this.http.get<Response>(
      `${this.apiUrl}/files/${id}`);
  }

  addFiles(note: Note): Observable<Response> {
    return this.http.post<Response>(
      `${this.apiUrl}/files`,
      { ...note });
  }

  deleteFiles(id: string): Observable<Response> {
    return this.http.delete<Response>(
      `${this.apiUrl}/files/${id}`);
  }
}
