import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiSecret } from '../../model/api-resource.model';
import { ApiSecretRequestModel } from '../../model/api-resource-request.model';
import { AuthorityInteractionService } from '../../service/authority-Interaction.service';
import { BaseService } from 'src/app/shared/base.service';
import { EntityState, Uris } from 'src/app/shared/const';


@Component({
    selector: 'app-api-secret',
    templateUrl: './api-secret.component.html',
    styleUrls: ['./api-secret.component.css']
})
export class ApiSecretComponent implements OnInit {
    @Input() apiSecret: ApiSecret;
    isSpinning = false;
    isEdit = false;
    mainForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private nzMessageService: NzMessageService,
        private apiSecretService: BaseService<ApiSecretRequestModel, ApiSecret>,
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
        if (this.apiSecret.state === EntityState.Added) {
            this.isEdit = true;
        } else {
            this.apiSecret.state = EntityState.Modified;
        }
    }

    submit() {
        this.isSpinning = true;
        this.apiSecret.description = this.mainForm.get('description').value;
        this.apiSecret.value = this.mainForm.get('value').value;
        this.apiSecret.expiration = this.mainForm.get('expiration').value;
        this.apiSecret.type = this.mainForm.get('type').value;
        this.apiSecret.created = this.mainForm.get('created').value;
        let requestModel: ApiSecretRequestModel;
        switch (this.apiSecret.state) {
            case EntityState.Added:
                requestModel = new ApiSecretRequestModel(Uris.AddApiSecret);
                break;
            case EntityState.Modified:
                requestModel = new ApiSecretRequestModel(Uris.ModifyApiSecret);
                break;
        }
        requestModel.apiSecret = this.apiSecret;
        this.apiSecretService.submit(requestModel).pipe(
            finalize(() => this.isSpinning = false)
        ).subscribe(
            result => {
                if (result.isSuccess) {
                    Object.assign(this.apiSecret, result.data);
                    this.reset();
                    this.authorityInteractionService.apiSecretChanged(this.apiSecret);
                    if (this.apiSecret.state === EntityState.Added) {
                        this.nzMessageService.info('ApiSecret 新增完成');
                    } else if (this.apiSecret.state === EntityState.Modified) {
                        this.nzMessageService.info('ApiSecret 更新完成');
                    }
                    this.apiSecret.state = EntityState.Modified;
                    this.isEdit = false;
                } else {
                    this.nzMessageService.error(result.message);
                }
            }
        );
    }

    edit() {
        this.apiSecret.state = EntityState.Modified;
        this.isEdit = true;
    }

    delete() {
        this.apiSecret.state = EntityState.Deleted;
        const requestModel = new ApiSecretRequestModel(Uris.DeleteApiSecret);
        requestModel.apiSecret = this.apiSecret;
        this.apiSecretService.submit(requestModel).pipe(
            finalize(() => this.isSpinning = false)
        ).subscribe(
            result => {
                if (result.isSuccess) {
                    this.authorityInteractionService.apiSecretChanged(this.apiSecret);
                    this.nzMessageService.info('ApiSecret 删除完成');
                } else {
                    this.nzMessageService.error(result.message);
                }
            }
        );
    }

    cancel() {
        if (this.apiSecret.state === EntityState.Added) {
            this.apiSecret.state = EntityState.Deleted;
            this.authorityInteractionService.apiSecretChanged(this.apiSecret);
        } else {
            this.reset();
            this.isEdit = false;
        }
    }

    reset() {
        const initialMap = {
            id: this.apiSecret.id,
            description: this.apiSecret.description,
            value: this.apiSecret.value,
            expiration: this.apiSecret.expiration,
            type: this.apiSecret.type,
            created: this.apiSecret.created
        };
        this.mainForm.reset(initialMap);
    }
}
