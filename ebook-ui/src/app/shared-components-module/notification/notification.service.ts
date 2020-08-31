import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {

    snackbarConfig = new MatSnackBarConfig();

    constructor(private snackBar: MatSnackBar) { }

    show(message: string, title: string, state: string, duration?: number): void {
        this.snackBar.open(message, title, { duration });
    }
}