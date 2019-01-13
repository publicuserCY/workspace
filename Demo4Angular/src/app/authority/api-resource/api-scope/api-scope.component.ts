import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiScope } from '../../models/api-resource.model';
import { ApiScopeRequestModel } from '../../models/api-resource-request.model';
import { BaseService } from 'src/app/shared/base.service';
import { AuthorityInteractionService } from '../../services/authority-Interaction.service';
import { EntityState } from 'src/app/shared/const';

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
        private apiScopeService: BaseService<ApiScopeRequestModel, ApiScope>,
        private authorityInteractionService: AuthorityInteractionService
    ) { }

    ngOnInit() {
        this.mainForm = this.fb.group({
            id: [this.apiScope.id],
            name: [this.apiScope.name, Validators.required],
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
        const requestModel = new ApiScopeRequestModel();
        requestModel.apiScope = this.apiScope;
        this.apiScopeService.submit(requestModel).pipe(
            finalize(() => this.isSpinning = false)
        ).subscribe(
            result => {
                if (result.isSuccess) {
                    if (this.apiScope.state === EntityState.Added) {
                        this.nzMessageService.info('ApiScope 新增完成');
                    } else if (this.apiScope.state === EntityState.Modified) {
                        this.nzMessageService.info('ApiScope 更新完成');
                    }
                    Object.assign(this.apiScope, result.data);
                    this.reset();
                    this.apiScope.state = EntityState.Modified;
                    this.authorityInteractionService.apiScopeChanged(this.apiScope);
                    this.isEdit = false;
                } else {
                    this.nzMessageService.error(result.message);
                }
            }
        );
        /* if (this.apiScope.state === EntityState.Added) {
            this.apiScopeService.add(requestModel).pipe(
                finalize(() => this.isSpinning = false)
            ).subscribe(
                result => {
                    if (result.isSuccess) {
                        Object.assign(this.apiScope, result.data);
                        this.reset();
                        this.apiScope.state = EntityState.Modified;
                        this.authorityInteractionService.apiScopeChanged(this.apiScope);
                        this.edit = false;
                        this.nzMessageService.info('ApiScope 新增完成');
                    } else {
                        this.nzMessageService.error(result.message);
                    }
                }
            );
        } else {
            this.apiScopeService.modify(requestModel).pipe(
                finalize(() => this.isSpinning = false)
            ).subscribe(
                result => {
                    if (result.isSuccess) {
                        Object.assign(this.apiScope, result.data);
                        this.reset();
                        this.apiScope.state = EntityState.Modified;
                        this.authorityInteractionService.apiScopeChanged(this.apiScope);
                        this.edit = false;
                        this.nzMessageService.info('ApiScope 更新完成');
                    } else {
                        this.nzMessageService.error(result.message);
                    }
                }
            );
        } */
    }

    edit() {
        this.apiScope.state = EntityState.Modified;
        this.isEdit = true;
    }

    delete() {
        this.apiScope.state = EntityState.Deleted;
        const requestModel = new ApiScopeRequestModel();
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
