export interface IReviewPayload {
   sku_id: string;
   order_id: string;
   order_item_id: string;
   content: string;
   rating: number;
}

export interface IUpdateReviewPayload {
   rating: number;
   content: string;
}

export interface IGetProductModelsQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _textSearch?: string | null;
}

export interface IGetProductModelsByCategorySlugQueryParams {
   _page?: number | null;
   _limit?: number | null;
   _colors?: string[] | null;
   _storages?: string[] | null;
   _models?: string[] | null;
   _minPrice?: number | null;
   _maxPrice?: number | null;
   _priceSort?: 'ASC' | 'DESC' | null;
}

export interface ChatbotMessagesRequest {
   chatbot_messages: ChatbotMessage[];
}

export interface ChatbotMessage {
   role: 'user' | 'assistant';
   content: string;
}


