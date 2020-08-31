import { MessageBoxService } from './../shared-components-module/message-box/message-box.service';
import { Book } from './models/book.model';
import { BooksService } from './books.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../authentication/authentication.service';
import { MessageBoxButton } from '../shared-components-module/message-box/enums/message-box-button.enum';
import { MessageBoxResult } from '../shared-components-module/message-box/enums/message-box-result.enum';

@Component({
    selector: 'books',
    templateUrl: './books.component.html',
    styleUrls: ['./books.component.scss']
})
export class BooksComponent implements OnInit {

    books: Book[];
    displayedColumns: string[] = ['bookId', 'name', 'price', 'userBookSubscription'];
    constructor(private booksService: BooksService,
        private router: Router,
        private authService: AuthenticationService,
        private messageBox: MessageBoxService) { }

    ngOnInit(): void {
        this.booksService.getBooks().subscribe(data =>
            this.books = data
        );
    }

    buy(book: Book): void {

        if (!this.authService.isAuthenticated()) {
            this.router.navigateByUrl('/login');
            return;
        }

        this.messageBox
            .show(`Buy book: ${book.name}? Price - ${book.price}`, 'Attention', MessageBoxButton.YesNo)
            .subscribe(result => {
                if (result === MessageBoxResult.Yes) {
                    this.booksService.buySubscription(book).subscribe((data) => book.userBookSubscription = data);
                }
            });
    }

    cancelSubscription(book: Book): void {
        this.booksService.cancelSubscription(book.bookId).subscribe(() => book.userBookSubscription = null);
    }
}
