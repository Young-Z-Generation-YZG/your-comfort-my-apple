export interface ICategory {
   id: string;
   name: string;
   description: string;
   order: number;
   slug: string;
   parent_id: string | null;
   created_at: string;
   updated_at: string;
   modified_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
}
