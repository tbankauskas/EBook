import { HttpService } from './../core/http/http.service';
import { UserBookSubscription } from './models/user-book-subscription.model';
import { Book } from './models/book.model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class BooksService extends HttpService {

    getBooks(): Observable<Book[]> {
        return this.get<Book[]>(`${this.apiUrl}/books`);
    }

    buySubscription(book: Book): Observable<UserBookSubscription> {
        return this.post<UserBookSubscription>(`${this.apiUrl}/subscriptions/`, book);
    }

    cancelSubscription(bookId: number): Observable<void> {
        return this.delete<void>(`${this.apiUrl}/subscriptions/${bookId}`);
    }
}