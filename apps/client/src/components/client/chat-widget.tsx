'use client';

import React, { useState, useRef, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { MessageCircle, X, Send, Loader2 } from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import { useSendChatMutation } from '~/infrastructure/services/chat.service';
import { ChatbotMessage } from '~/domain/interfaces/catalog.interface';

const ChatWidget = () => {
   const [isOpen, setIsOpen] = useState(false);
   const [inputValue, setInputValue] = useState('');
   const [messages, setMessages] = useState<ChatbotMessage[]>([]);
   const messagesEndRef = useRef<HTMLDivElement>(null);
   const inputRef = useRef<HTMLInputElement>(null);

   const [sendChat, { isLoading }] = useSendChatMutation();

   const scrollToBottom = () => {
      messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
   };

   useEffect(() => {
      scrollToBottom();
   }, [messages]);

   useEffect(() => {
      if (isOpen && inputRef.current) {
         inputRef.current.focus();
      }
   }, [isOpen]);

   const handleSend = async () => {
      if (!inputValue.trim() || isLoading) return;

      const userMessage: ChatbotMessage = {
         role: 'user',
         content: inputValue.trim(),
      };

      setMessages((prev) => [...prev, userMessage]);
      setInputValue('');

      try {
         const response = await sendChat({
            chatbot_messages: [...messages, userMessage],
         }).unwrap();

         const assistantMessage: ChatbotMessage = {
            role: 'assistant',
            content: response.content,
         };

         setMessages((prev) => [...prev, assistantMessage]);
      } catch (error) {
         console.error('Chat error:', error);
         const errorMessage: ChatbotMessage = {
            role: 'assistant',
            content: 'Xin lỗi, đã có lỗi xảy ra. Vui lòng thử lại sau.',
         };
         setMessages((prev) => [...prev, errorMessage]);
      }
   };

   const handleKeyPress = (e: React.KeyboardEvent) => {
      if (e.key === 'Enter' && !e.shiftKey) {
         e.preventDefault();
         handleSend();
      }
   };

   return (
      <>
         {/* Floating Button */}
         <motion.button
            onClick={() => setIsOpen(!isOpen)}
            className={cn(
               'fixed bottom-6 right-6 z-50 flex h-14 w-14 items-center justify-center rounded-full shadow-lg transition-all duration-300',
               'bg-gradient-to-r from-[#0071e3] to-[#00c7be]',
               'hover:scale-110 hover:shadow-xl',
            )}
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.95 }}
         >
            <AnimatePresence mode="wait">
               {isOpen ? (
                  <motion.div
                     key="close"
                     initial={{ rotate: -90, opacity: 0 }}
                     animate={{ rotate: 0, opacity: 1 }}
                     exit={{ rotate: 90, opacity: 0 }}
                  >
                     <X className="h-6 w-6 text-white" />
                  </motion.div>
               ) : (
                  <motion.div
                     key="chat"
                     initial={{ rotate: 90, opacity: 0 }}
                     animate={{ rotate: 0, opacity: 1 }}
                     exit={{ rotate: -90, opacity: 0 }}
                  >
                     <MessageCircle className="h-6 w-6 text-white" />
                  </motion.div>
               )}
            </AnimatePresence>
         </motion.button>

         {/* Chat Panel */}
         <AnimatePresence>
            {isOpen && (
               <motion.div
                  initial={{ opacity: 0, y: 20, scale: 0.95 }}
                  animate={{ opacity: 1, y: 0, scale: 1 }}
                  exit={{ opacity: 0, y: 20, scale: 0.95 }}
                  transition={{ duration: 0.2 }}
                  className={cn(
                     'fixed bottom-24 right-6 z-50 flex h-[500px] w-[380px] flex-col overflow-hidden rounded-2xl shadow-2xl',
                     'bg-white/95 backdrop-blur-xl',
                     'border border-gray-200/50',
                  )}
               >
                  {/* Header */}
                  <div
                     className={cn(
                        'flex items-center gap-3 px-5 py-4',
                        'bg-gradient-to-r from-[#0071e3] to-[#00c7be]',
                     )}
                  >
                     <div className="flex h-10 w-10 items-center justify-center rounded-full bg-white/20 backdrop-blur-sm">
                        <MessageCircle className="h-5 w-5 text-white" />
                     </div>
                     <div>
                        <h3 className="text-base font-semibold text-white">
                           YB Store Assistant
                        </h3>
                        <p className="text-xs text-white/80">
                           Sẵn sàng hỗ trợ bạn 24/7
                        </p>
                     </div>
                  </div>

                  {/* Messages */}
                  <div className="flex-1 overflow-y-auto p-4">
                     {messages.length === 0 ? (
                        <div className="flex h-full flex-col items-center justify-center text-center">
                           <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-gradient-to-r from-[#0071e3]/10 to-[#00c7be]/10">
                              <MessageCircle className="h-8 w-8 text-[#0071e3]" />
                           </div>
                           <h4 className="mb-2 text-lg font-medium text-gray-800">
                              Xin chào! 👋
                           </h4>
                           <p className="text-sm text-gray-500">
                              Tôi là trợ lý ảo của YB Store. Bạn cần tư vấn về
                              sản phẩm Apple nào?
                           </p>
                        </div>
                     ) : (
                        <div className="space-y-4">
                           {messages.map((msg, idx) => (
                              <motion.div
                                 key={idx}
                                 initial={{ opacity: 0, y: 10 }}
                                 animate={{ opacity: 1, y: 0 }}
                                 className={cn(
                                    'flex',
                                    msg.role === 'user'
                                       ? 'justify-end'
                                       : 'justify-start',
                                 )}
                              >
                                 <div
                                    className={cn(
                                       'max-w-[80%] rounded-2xl px-4 py-2.5',
                                       msg.role === 'user'
                                          ? 'bg-gradient-to-r from-[#0071e3] to-[#00c7be] text-white'
                                          : 'bg-gray-100 text-gray-800',
                                    )}
                                 >
                                    <p className="whitespace-pre-wrap text-sm leading-relaxed">
                                       {msg.content}
                                    </p>
                                 </div>
                              </motion.div>
                           ))}
                           {isLoading && (
                              <motion.div
                                 initial={{ opacity: 0 }}
                                 animate={{ opacity: 1 }}
                                 className="flex justify-start"
                              >
                                 <div className="flex items-center gap-2 rounded-2xl bg-gray-100 px-4 py-3">
                                    <Loader2 className="h-4 w-4 animate-spin text-[#0071e3]" />
                                    <span className="text-sm text-gray-500">
                                       Đang trả lời...
                                    </span>
                                 </div>
                              </motion.div>
                           )}
                           <div ref={messagesEndRef} />
                        </div>
                     )}
                  </div>

                  {/* Input */}
                  <div className="border-t border-gray-100 bg-white p-4">
                     <div className="flex items-center gap-2">
                        <input
                           ref={inputRef}
                           type="text"
                           value={inputValue}
                           onChange={(e) => setInputValue(e.target.value)}
                           onKeyPress={handleKeyPress}
                           placeholder="Nhập tin nhắn..."
                           disabled={isLoading}
                           className={cn(
                              'flex-1 rounded-full border border-gray-200 bg-gray-50 px-4 py-2.5 text-sm outline-none transition-all',
                              'focus:border-[#0071e3] focus:bg-white focus:ring-2 focus:ring-[#0071e3]/20',
                              'disabled:opacity-50',
                           )}
                        />
                        <motion.button
                           onClick={handleSend}
                           disabled={!inputValue.trim() || isLoading}
                           whileHover={{ scale: 1.05 }}
                           whileTap={{ scale: 0.95 }}
                           className={cn(
                              'flex h-10 w-10 items-center justify-center rounded-full transition-all',
                              'bg-gradient-to-r from-[#0071e3] to-[#00c7be]',
                              'disabled:opacity-50 disabled:cursor-not-allowed',
                              'hover:shadow-lg',
                           )}
                        >
                           <Send className="h-4 w-4 text-white" />
                        </motion.button>
                     </div>
                  </div>
               </motion.div>
            )}
         </AnimatePresence>
      </>
   );
};

export default ChatWidget;
