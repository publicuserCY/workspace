import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiScope } from '../../model/api-resource.model';
import { ApiScopeRequestModel } from '../../model/api-resource-request.model';
import { ApiScopeService } from '../../service/api-scope.service';
import { AuthorityInteractionService } from '../../service/authority-Interaction.service';
import { EntityState, Uris } from 'src/app/shared/const';
import { uniqueApiScopeNameValidatorFn } from '../../validator/api-scope-name.validator';


@Component({
    selector: 'app-api-scope',
    templateUrl: './api-scope.component.html',
    styleUrls: ['./api-scope.component.css']
})
export class ApiScopeComponent implements OnInit {
    @Input() apiScope: ApiScope;
    isSpinning = false;
    isEdit = false;
    mainForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private nzMessageService: NzMessageService,
        private apiScopeService: ApiScopeService,
        private authorityInteractionService: AuthorityInteractionService
    ) { }

    ngOnInit() {
        this.mainForm = this.fb.group({
            id: [this.apiScope.id],
            name: [this.apiScope.name,
            {
                validators: [Validators.required],
                asyncValidators: [uniqueApiScopeNameValidatorFn(this.apiScopeService, this.apiScope.id)],
                updateOn: 'blur'
            }],
            displayName: [this.apiScope.displayName],
            description: [this.apiScope.description],
            required: [this.apiScope.required],
            emphasize: [this.apiScope.emphasize, Validators.required],
            showInDiscoveryDocument: [this.apiScope.showInDiscoveryDocument, Validators.required]
        });
        if (this.apiScope.state === EntityState.Added) {
            this.isEdit = true;
        } else {
            this.apiScope.state = EntityState.Modified;
        }
    }

    submit() {
        this.isSpinning = true;
        this.apiScope.name = this.mainForm.get('name').value;
        this.apiScope.displayName = this.mainForm.get('displayName').value;
        this.apiScope.description = this.mainForm.get('description').value;
        this.apiScope.required = this.mainForm.get('required').value;
        this.apiScope.emphasize = this.mainForm.get('emphasize').value;
        this.apiScope.showInDiscoveryDocument = this.mainForm.get('showInDiscoveryDocument').value;

        let requestModel: ApiScopeRequestModel;
        switch (this.apiScope.state) {
            case EntityState.Added:
                requestModel = new ApiScopeRequestModel(Uris.AddApiResource);
                break;
            case EntityState.Modified:
                requestModel = new ApiScopeRequestModel(Uris.ModifyApiResource);
                break;
        }
        requestModel.apiScope = this.apiScope;
        this.apiScopeService.submit(requestModel).pipe(
            finalize(() => this.isSpinning = false)
        ).subscribe(
            result => {
                if (result.isSuccess) {
                    Object.assign(this.apiScope, result.data);
                    this.mainForm.get('name').setAsyncValidators(uniqueApiScopeNameValidatorFn(this.apiScopeService, this.apiScope.id));
                    this.reset();
                    this.authorityInteractionService.apiScopeChanged(this.apiScope);
                    if (this.apiScope.state === EntityState.Added) {
                        this.nzMessageService.info('ApiScope 新增完成');
                    } else if (this.apiScope.state === EntityState.Modified) {
                        this.nzMessageService.info('ApiScope 更新完成');
                    }
                    this.apiScope.state = EntityState.Modified;
                    this.isEdit = false;
                } else {
                    this.nzMessageService.error(result.message);
                }
            }
        );
    }

    edit() {
        this.apiScope.state = EntityState.Modified;
        this.isEdit = true;
    }

    delete() {
        this.apiScope.state = EntityState.Deleted;
        const requestModel = new ApiScopeRequestModel(Uris.DeleteApiResource);
        requestModel.apiScope = this.apiScope;
        this.apiScopeService.submit(requestModel).pipe(
            finalize(() => this.isSpinning = false)
        ).subscribe(
            result => {
                if (result.isSuccess) {
                    this.authorityInteractionService.apiScopeChanged(this.apiScope);
                    this.nzMessageService.info('ApiScope 删除完成');
                } else {
                    this.nzMessageService.error(result.message);
                }
            }
        );
    }

    cancel() {
        if (this.apiScope.state === EntityState.Added) {
            this.apiScope.state = EntityState.Deleted;
            this.authorityInteractionService.apiScopeChanged(this.apiScope);
        } else {
            this.reset();
            this.isEdit = false;
        }
    }

    reset() {
        const initialMap = {
            id: this.apiScope.id,
            name: this.apiScope.name,
            displayName: this.apiScope.displayName,
            description: this.apiScope.description,
            required: this.apiScope.required,
            emphasize: this.apiScope.emphasize,
            showInDiscoveryDocument: this.apiScope.showInDiscoveryDocument
        };
        this.mainForm.reset(initialMap);
    }
}