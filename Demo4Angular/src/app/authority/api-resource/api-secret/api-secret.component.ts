import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ApiSecretRequestModel } from '../../models/api-resource-request.model';
import { AuthorityService } from '../../services/authority.service';
import { AuthorityInteractionService } from '../../services/authority-Interaction.service';

@Component({
    selector: 'app-api-secret',
    templateUrl: './api-secret.component.html',
    styleUrls: ['./api-secret.component.css']
})
export class ApiSecretComponent implements OnInit, OnDestroy {
    @Input() apiSecret: ApiSecretRequestModel;
    edit = false;
    subscription: Subscription;
    mainForm: FormGroup;
    constructor(
        private fb: FormBuilder,
        private authorityService: AuthorityService,
        private authorityInteractionService: AuthorityInteractionService
    ) { }

    ngOnInit() {
        this.mainForm = this.fb.group({
            id: [this.apiSecret.id],
            description: [this.apiSecret.description],
            value: [this.apiSecret.value, Validators.required],
            expiration: [this.apiSecret.expiration],
            type: [this.apiSecret.type, Validators.required],
            created: [this.apiSecret.created]
        });
    }

    save(id: number) {

    }

    delete(id: number) {
        /* const model: BookRequestModel = { Id: this.book.Id };
        this.http.deleteBook(model).subscribe(
            (result: OperationResult<Book>) => {
                if (result.IsSuccess) {
                    this.bookInteractionService.itemDelete(this.book);
                }
            }
        ); */
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}
