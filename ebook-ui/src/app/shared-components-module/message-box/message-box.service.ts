import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MessageBoxComponent } from './message-box.component';
import { MessageBoxButton } from './enums/message-box-button.enum';
import { MessageBoxResult } from './enums/message-box-result.enum';
import { MatDialog } from '@angular/material/dialog';

@Injectable({ providedIn: 'root' })
export class MessageBoxService {

    constructor(private dialog: MatDialog) { }

    show(message: string, title = 'Alert',
        messageBoxButton: MessageBoxButton,
        width?: string): Observable<MessageBoxResult> {
        const dialogRef = this.dialog.open(MessageBoxComponent, {
            data: {
                title: title || 'Alert',
                message,
                button: messageBoxButton,
                allow_outside_click: false
            },
            width
        });

        return dialogRef.afterClosed();
    }
}