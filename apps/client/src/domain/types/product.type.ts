export interface ProductItem {
     id: string;
     productId: string;
     sku: string;
     model: string;
     color: string;
     storage: number;
     imageUrls: string[];
     imageIds: string[];
     price: number;
     quantityInStock: number;
     createdAt: Date;
     updatedAt: Date;
}

export interface Product {
     id: string;
     categoryId: string;
     promotionId: string;
     name: string;
     description: string;
     averageRating: any;
     imageUrls: string[];
     imageIds: string[];
     createdAt: Date;
     updatedAt: Date;
}