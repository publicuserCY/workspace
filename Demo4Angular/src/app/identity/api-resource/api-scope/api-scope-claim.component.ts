import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd';

import { ApiScopeClaim } from '../../model/api-resource.model';
import { AuthorityInteractionService } from '../../service/authority-Interaction.service';
import { EntityState, Uris } from 'src/app/shared/const';

@Component({
    selector: 'app-api-scope-claim',
    templateUrl: './api-scope-claim.component.html',
    styleUrls: ['./api-scope-claim.component.css']
})
export class ApiScopeClaimComponent implements OnInit {
    @Input() mainForm: FormGroup;
    @Input() apiScopeClaim: ApiScopeClaim;
    @Input() isEdit: boolean;
    // mainForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private nzMessageService: NzMessageService,
        private authorityInteractionService: AuthorityInteractionService
    ) { }

    ngOnInit() {
        /* this.mainForm = this.fb.group({
            id: [this.apiScopeClaim.id],
            type: [this.apiScopeClaim.type, Validators.required]
        }); */
        // this.authorityInteractionService.apiScopeClaimChanged(this.mainForm);
    }

    deleteScopeClaim() {
        this.apiScopeClaim.state = EntityState.Deleted;
    }
}
