import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, delay, map } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiSecret } from '../../models/api-resource.model';
import { AuthorityService } from '../../services/authority.service';
import { AuthorityInteractionService } from '../../services/authority-Interaction.service';
import { ApiSecretRequestModel } from '../../models/api-resource-request.model';
import { EntityState } from 'src/app/shared/const';

@Component({
    selector: 'app-api-secret',
    templateUrl: './api-secret.component.html',
    styleUrls: ['./api-secret.component.css']
})
export class ApiSecretComponent implements OnInit {
    @Input() apiSecret: ApiSecret;
    isSpinning = false;
    edit = false;
    mainForm: FormGroup;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private fb: FormBuilder,
        private nzMessageService: NzMessageService,
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
            created: [this.apiSecret.created, Validators.required]
        });
        if (this.apiSecret.state === EntityState.Added) {
            this.edit = true;
        }
    }

    submit() {
        this.isSpinning = true;
        this.apiSecret.description = this.mainForm.get('description').value;
        this.apiSecret.value = this.mainForm.get('value').value;
        this.apiSecret.expiration = this.mainForm.get('expiration').value;
        this.apiSecret.type = this.mainForm.get('type').value;
        this.apiSecret.created = this.mainForm.get('created').value;
        const requestModel = new ApiSecretRequestModel();
        requestModel.apiSecret = this.apiSecret;
        if (this.apiSecret.state === EntityState.Added) {
            this.authorityService.addApiSecret(requestModel).pipe(
                finalize(() => this.isSpinning = false)
            ).subscribe(
                result => {
                    if (result.isSuccess) {
                        Object.assign(this.apiSecret, result.data);
                        this.apiSecret.state = EntityState.Modified;
                        this.authorityInteractionService.apiSecretModified(this.apiSecret);
                        this.edit = false;
                        this.nzMessageService.info('ApiSecret 新增完成');
                    } else {
                        this.nzMessageService.error(result.message);
                    }
                }
            );
        } else {
            this.authorityService.modifyApiSecret(requestModel).pipe(
                finalize(() => this.isSpinning = false)
            ).subscribe(
                result => {
                    if (result.isSuccess) {
                        Object.assign(this.apiSecret, result.data);
                        this.apiSecret.state = EntityState.Modified;
                        this.authorityInteractionService.apiSecretModified(this.apiSecret);
                        this.edit = false;
                        this.nzMessageService.info('ApiSecret 更新完成');
                    } else {
                        this.nzMessageService.error(result.message);
                    }
                }
            );
        }
    }

    delete() {
        const requestModel = new ApiSecretRequestModel();
        requestModel.apiSecret = this.apiSecret;
        this.authorityService.deleteApiSecret(requestModel).pipe(
            finalize(() => this.isSpinning = false)
        ).subscribe(
            result => {
                if (result.isSuccess) {

                    this.nzMessageService.info('ApiSecret 删除完成');
                } else {
                    this.nzMessageService.error(result.message);
                }
            }
        );
    }

    cancel() {
        if (this.apiSecret.state === EntityState.Added) {
            this.authorityInteractionService.apiSecretDeleted(this.apiSecret);
        } else {
            const initialMap = {
                id: this.apiSecret.id,
                description: this.apiSecret.description,
                value: this.apiSecret.value,
                expiration: this.apiSecret.expiration,
                type: this.apiSecret.type,
                created: this.apiSecret.created
            };
            this.mainForm.reset(initialMap);
            for (const key of Object.keys(this.mainForm.controls)) {
                this.mainForm.controls[key].markAsPristine();
                this.mainForm.controls[key].updateValueAndValidity();
            }
            this.edit = false;
        }
    }
}
