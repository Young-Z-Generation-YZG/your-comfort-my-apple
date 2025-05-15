import { HttpErrorResponse } from '~/src/domain/interfaces/errors/error.interface';

const isServerErrorResponse = (error: unknown): error is HttpErrorResponse => {
   return typeof error === 'object' && error !== null;
};

export default isServerErrorResponse;
