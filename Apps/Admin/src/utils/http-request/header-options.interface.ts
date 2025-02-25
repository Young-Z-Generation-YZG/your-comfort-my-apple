type ContentType =
   | 'application-json'
   | 'multipart/form-data'
   | 'application/x-www-form-urlencoded';

export interface IHeaderOptions {
   'Content-Type'?: ContentType;
}
