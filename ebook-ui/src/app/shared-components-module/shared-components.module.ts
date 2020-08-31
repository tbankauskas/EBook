import { MatIconModule } from '@angular/material/icon';
import { MessageBoxComponent } from './message-box/message-box.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { SpinnerComponent } from './spinner/spinner.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

const modules = [
    CommonModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
];

@NgModule({
    declarations: [
        SpinnerComponent,
        MessageBoxComponent
    ],
    imports: [
        ...modules
    ],
    exports: [
        ...modules,
        SpinnerComponent,
        MessageBoxComponent,
    ]
})
export class SharedComponentsModule { }
