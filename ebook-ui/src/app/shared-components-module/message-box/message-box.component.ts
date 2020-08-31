import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MessageBoxResult } from './enums/message-box-result.enum';
import { MessageBoxButton } from './enums/message-box-button.enum';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'message-box',
    templateUrl: './message-box.component.html',
    styleUrls: ['./message-box.component.scss']
})
export class MessageBoxComponent {

    title: string;
    message: string;
    button: MessageBoxButton;
    allow_outside_click: boolean;

    constructor(public dialogRef: MatDialogRef<MessageBoxComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
        this.title = data.title;
        this.message = data.message;
        this.button = data.button;
        this.dialogRef.disableClose = !data.allow_outside_click || false;
        this.dialogRef.keydownEvents().pipe(filter(key => key.code === 'Escape')).subscribe(() => {
            this.onEscape();
        });
    }

    onOk(): void {
        this.dialogRef.close(MessageBoxResult.Ok);
    }

    onCancel(): void {
        this.dialogRef.close(MessageBoxResult.Cancel);
    }

    onYes(): void {
        this.dialogRef.close(MessageBoxResult.Yes);
    }

    onNo(): void {
        this.dialogRef.close(MessageBoxResult.No);
    }

    onAccept(): void {
        this.dialogRef.close(MessageBoxResult.Accept);
    }

    onReject(): void {
        this.dialogRef.close(MessageBoxResult.Reject);
    }

    onEscape(): void {
        if (this.button === MessageBoxButton.Ok || this.button === MessageBoxButton.OkCancel) {
            this.dialogRef.close(MessageBoxResult.Cancel);
            return;
        }
        if (this.button === MessageBoxButton.YesNo) {
            this.dialogRef.close(MessageBoxResult.No);
            return;
        }
        if (this.button === MessageBoxButton.AcceptReject) {
            this.dialogRef.close(MessageBoxResult.Reject);
            return;
        }
    }
}
