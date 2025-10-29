import {
   type UseFormReturn,
   type Path,
   type FieldValues,
   Controller,
   ControllerRenderProps,
   ControllerFieldState,
   UseFormStateReturn,
} from 'react-hook-form';
import { cn } from '~/src/infrastructure/lib/utils';

type FieldMessageErrorProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: Path<T>;
   className?: string;
};

const FieldMessageError = <T extends FieldValues>({
   form,
   name,
   className,
}: FieldMessageErrorProps<T>) => {
   return (
      <div
         className={cn('text-[0.8rem] font-medium text-destructive', className)}
      >
         <Controller
            control={form.control}
            name={name}
            render={({
               field,
               fieldState,
               formState,
            }: {
               field: ControllerRenderProps<T, Path<T>>;
               fieldState: ControllerFieldState;
               formState: UseFormStateReturn<T>;
            }) => {
               const errorMessage = fieldState.error?.message;

               return (
                  <p className="text-[0.8rem] font-medium text-destructive">
                     {errorMessage}
                  </p>
               );
            }}
         />
      </div>
   );
};

export default FieldMessageError;
