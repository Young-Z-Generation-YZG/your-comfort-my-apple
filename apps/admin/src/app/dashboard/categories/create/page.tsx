'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useRouter } from 'next/navigation';
import { useState, useEffect } from 'react';
import { Button } from '@components/ui/button';
import {
   Form,
   FormControl,
   FormDescription,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import { Input } from '@components/ui/input';
import { Textarea } from '@components/ui/textarea';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { LoadingOverlay } from '@components/loading-overlay';
import { useCategoryService } from '~/src/hooks/api/use-category-service';
import { toast } from '~/src/hooks/use-toast';
import { ArrowLeft } from 'lucide-react';
import { TCategoryItem } from '~/src/infrastructure/services/category.service';

const createCategorySchema = z.object({
   name: z
      .string()
      .min(1, {
         message: 'Name is required.',
      })
      .min(2, {
         message: 'Name must be at least 2 characters.',
      }),
   description: z
      .string()
      .min(1, {
         message: 'Description is required.',
      })
      .min(5, {
         message: 'Description must be at least 5 characters.',
      }),
   parent_id: z.string(),
});

type CreateCategoryFormValues = z.infer<typeof createCategorySchema>;

const CreateCategoryPage = () => {
   const router = useRouter();
   const { createCategoryAsync, getCategoriesAsync, isLoading } =
      useCategoryService();
   const [isSubmitting, setIsSubmitting] = useState(false);
   const [categories, setCategories] = useState<TCategoryItem[]>([]);

   const form = useForm<CreateCategoryFormValues>({
      resolver: zodResolver(createCategorySchema),
      defaultValues: {
         name: '',
         description: '',
         parent_id: '__none__',
      },
   });

   // Fetch categories for parent selection
   useEffect(() => {
      const fetchCategories = async () => {
         const result = await getCategoriesAsync();
         if (result.isSuccess && result.data) {
            setCategories(result.data);
         }
      };
      fetchCategories();
   }, [getCategoriesAsync]);

   const onSubmit = async (data: CreateCategoryFormValues) => {
      setIsSubmitting(true);

      try {
         const payload = {
            name: data.name,
            description: data.description,
            parent_id: data.parent_id === '__none__' ? '' : data.parent_id,
         };

         const result = await createCategoryAsync(payload);

         if (result.isSuccess) {
            toast({
               title: 'Success',
               description: 'Category created successfully!',
            });

            // Navigate back to categories list after a short delay
            setTimeout(() => {
               router.push('/dashboard/categories');
            }, 1000);
         } else {
            toast({
               variant: 'destructive',
               title: 'Error',
               description: result.error
                  ? (result.error as any)?.data?.message ||
                    'Failed to create category'
                  : 'Failed to create category. Please try again.',
            });
         }
      } catch (error) {
         console.error('Error creating category:', error);
         toast({
            variant: 'destructive',
            title: 'Error',
            description: 'An unexpected error occurred. Please try again.',
         });
      } finally {
         setIsSubmitting(false);
      }
   };

   return (
      <div className="p-5">
         <LoadingOverlay isLoading={isLoading || isSubmitting} />

         <div className="mb-6">
            <Button
               variant="ghost"
               onClick={() => router.back()}
               className="mb-4"
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>

            <div>
               <h1 className="text-3xl font-bold tracking-tight">
                  Create New Category
               </h1>
               <p className="text-muted-foreground mt-1">
                  Create a new category with name, description, and optional
                  parent category
               </p>
            </div>
         </div>

         <div className="rounded-lg border bg-card p-6 w-auto">
            <Form {...form}>
               <form
                  onSubmit={form.handleSubmit(onSubmit)}
                  className="space-y-6"
               >
                  <FormField
                     control={form.control}
                     name="name"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Category Name</FormLabel>
                           <FormControl>
                              <Input
                                 placeholder="e.g., Electronics"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Enter the name of the category.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="description"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Description</FormLabel>
                           <FormControl>
                              <Textarea
                                 placeholder="e.g., Electronic devices and accessories..."
                                 className="min-h-[100px]"
                                 {...field}
                                 disabled={isSubmitting}
                              />
                           </FormControl>
                           <FormDescription>
                              Provide a detailed description of the category.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <FormField
                     control={form.control}
                     name="parent_id"
                     render={({ field }) => (
                        <FormItem>
                           <FormLabel>Parent Category</FormLabel>
                           <Select
                              onValueChange={field.onChange}
                              defaultValue={field.value}
                              disabled={isSubmitting}
                           >
                              <FormControl>
                                 <SelectTrigger>
                                    <SelectValue placeholder="Select parent category (optional)" />
                                 </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                 <SelectItem value="__none__">
                                    None (Root Category)
                                 </SelectItem>
                                 {categories.map((category) => (
                                    <SelectItem
                                       key={category.id}
                                       value={category.id}
                                    >
                                       {category.name}
                                    </SelectItem>
                                 ))}
                              </SelectContent>
                           </Select>
                           <FormDescription>
                              Select a parent category if this is a subcategory.
                              Leave empty for a root category.
                           </FormDescription>
                           <FormMessage />
                        </FormItem>
                     )}
                  />

                  <div className="flex justify-end gap-4">
                     <Button
                        type="button"
                        variant="outline"
                        onClick={() => router.back()}
                        disabled={isSubmitting}
                     >
                        Cancel
                     </Button>
                     <Button type="submit" disabled={isSubmitting}>
                        {isSubmitting ? 'Creating...' : 'Create Category'}
                     </Button>
                  </div>
               </form>
            </Form>
         </div>
      </div>
   );
};

export default CreateCategoryPage;
