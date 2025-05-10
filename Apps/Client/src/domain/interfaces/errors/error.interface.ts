export interface HttpErrorResponse {
   status: number;
   data: ServerErrorResponse;
}

export interface ServerErrorResponse {
   title: string;
   path: string;
   status: number;
   error: {
      code: string;
      message: string;
   };
   trace_id: string;
   type: string;
}
