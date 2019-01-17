import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest
} from '@angular/common/http';
import { ResourceServer } from './const';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    // Get the auth token from the service.
    /* if (req.url.endsWith('connect/authorize')
      || req.url.endsWith('connect/token')
      || req.url.endsWith('connect/userinfo')
      || req.url.endsWith('.well-known/openid-configuration')
      || req.url.endsWith('.well-known/openid-configuration/jwks')
    ) {
      return next.handle(req);
    } */
    if (!req.url.startsWith(ResourceServer)) {
      return next.handle(req);
    }

    const access_token = localStorage.getItem('access_token');
    const authReq = req.clone({
      headers: req.headers
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + access_token)
    });
    return next.handle(authReq);
  }
}
