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

type PasswordInputFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: Path<T>;
   className?: string;
};

const PasswordInputField = <T extends FieldValues>({
   form,
   name,
   className,
}: PasswordInputFieldProps<T>) => {
   return (
      <div className={className}>
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
            }) => (
               <input
                  type="password"
                  {...field}
                  className={cn(
                     'file:text-foreground placeholder:text-muted-foreground selection:bg-primary selection:text-primary-foreground dark:bg-input/30 border-input h-9 w-full min-w-0 rounded-md border bg-transparent px-3 py-1 text-base shadow-xs transition-[color,box-shadow] outline-none file:inline-flex file:h-7 file:border-0 file:bg-transparent file:text-sm file:font-medium disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50 md:text-sm',
                     'focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px]',
                     'aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive',
                  )}
               />
            )}
         />
      </div>
   );
};

export default PasswordInputField;
