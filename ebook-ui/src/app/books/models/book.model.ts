import { UserBookSubscription } from './user-book-subscription.model';
export class Book {
    bookId: number;
    name: string;
    price: number;
    userBookSubscription: UserBookSubscription;
}