'use client';
import { useState } from 'react';
import { cn } from '~/infrastructure/lib/utils';

const Footer = () => {
   const [isOpen, setIsOpen] = useState(false);
   console.log(isOpen);
   return (
      <footer className={cn('w-full h-full')}>
         <div className={cn('max-w-[1680px] w-[87.5vw] mx-auto font-SFProText mb-5')}>
            <h2 className={cn('mb-[53px] text-[56px] font-[600] leading-[60px]')}>iPhone</h2>
            <div className={cn('flex flex-wrap lg:flex-row ')}>
               <div className={cn('w-full mb-14 lg:w-auto lg:mb-0 pr-[88px]')}>
                  <h3 className={cn('mb-[14px] text-[#6E6E73] text-[17px] font-normal leading-[21px]')}>Khám Phá iPhone</h3>
                  <ul className={cn('text-[28px] font-semibold leading-[32px]')}>
                     <li className={cn('mb-[11px]')}>Khám Phá Tất Cả iPhone</li> 
                     <li className={cn('mb-[11px]')}>iPhone 16 Pro</li>
                     <li className={cn('mb-[11px]')}>iPhone 16</li>
                     <li className={cn('mb-[11px]')}>iPhone 16e</li>
                     <li className={cn('mb-[11px]')}>iPhone 15</li>
                     <li className={cn('mb-[14px] mt-[30px] text-[17px] leading-[21px]')}>So Sánh iPhone</li>
                     <li className={cn('text-[17px] leading-[21px]')}>Chuyển Từ Android</li>
                  </ul>
               </div>
               <div className={cn('w-full mb-14 md:w-auto md:mb-0 pr-[44px]')}>
                  <h3 className={cn('mb-[14px] text-[#6E6E73] text-[17px] font-normal leading-[21px]')}>Mua iPhone</h3>
                  <ul className={cn('text-[17px] font-semibold leading-[21px]')}>
                     <li className={cn('mb-[14px]')}>Mua iPhone</li>
                     <li className={cn('mb-[14px]')}>Phụ Kiện iPhone</li>
                     <li className={cn('mb-[14px]')}>Apple Trade In</li>
                     <li className={cn('')}>Tài Chính</li>
                  </ul>
               </div>
               <div className={cn('w-full mb-14 md:w-auto md:mb-0 pr-[44px]')}>
                  <h3 className={cn('mb-[14px] text-[#6E6E73] text-[17px] font-normal leading-[21px]')}>Tìm Hiểu Thêm Về iPhone</h3>
                  <ul className={cn('text-[17px] font-semibold leading-[21px]')}>
                     <li className={cn('mb-[14px]')}>Hỗ Trợ iPhone</li>
                     <li className={cn('mb-[14px]')}>AppleCare+ cho iPhone</li>
                     <li className={cn('mb-[14px]')}>iOS 18</li>
                     <li className={cn('mb-[14px]')}>Apple Intelligence</li>
                     <li className={cn('mb-[14px]')}>Các Ứng Dụng Của Apple</li>
                     <li className={cn('mb-[14px]')}>Quyền Riêng Tư Trên iPhone</li>
                     <li className={cn('mb-[14px]')}>iCloud+</li>
                     <li className={cn('mb-[14px]')}>Ví, Pay</li>
                  </ul>
               </div>
   
            </div>
         </div>
         <div className={cn('w-full bg-[#FAFAFC]')}>
            <div className={cn('w-[980px] mx-auto py-[17px] flex flex-col gap-2 text-[11px] font-light font-SFProText')}>
               <div className={cn('w-full flex flex-col gap-2 text-[#0000008F]')}>
                  <p>
                     ∆ Dịch vụ thương mại được cung cấp bởi các đối tác trao đổi của Apple. Báo giá giá trị trao đổi chỉ mang tính ước tính và giá trị thực tế có thể thấp hơn ước tính. Giá trị đổi cũ lấy mới sẽ khác nhau tùy theo tình trạng, năm và cấu hình của thiết bị đủ điều kiện đổi của bạn. Không phải tất cả các thiết bị đều đủ điều kiện nhận điểm tín dụng. Bạn phải ít nhất ở độ tuổi thành niên để đủ điều kiện giao dịch tín dụng. Giá trị đổi cũ lấy mới có thể sẽ được áp dụng cho thiết bị đủ điều kiện mới bạn mua. Ước tính giá trị thực tế được dựa trên việc có nhận được thiết bị đủ điều kiện khớp với thông tin bạn đã mô tả hay không. Thuế giá trị gia tăng (GTGT) có thể sẽ được tính trên toàn bộ giá trị của thiết bị mới bạn mua. Bạn phải xuất trình giấy tờ tùy thân hợp lệ (luật địa phương có thể yêu cầu lưu thông tin này). Các đối tác trao đổi của Apple có quyền từ chối, hủy bỏ hoặc giới hạn số lượng của bất kỳ giao dịch đổi cũ lấy mới nào vì bất kỳ lý do gì. Bạn có thể tìm hiểu thêm thông tin từ đối tác đổi cũ lấy mới của Apple về việc đổi cũ lấy mới và đối tác tái chế về việc tái chế các thiết bị đủ điều kiện. Các hạn chế và giới hạn có thể được áp dụng. Có thể sẽ áp dụng Điều khoản và giới hạn về việc tái chế thiết bị đủ điều kiện.
                  </p>
                  <p>
                     * Trả góp theo tháng với lãi suất 1,67%, sau khi thanh toán lần đầu 20%. Có thêm tùy chọn thanh toán khi hoàn tất giao dịch.
                  </p>
                  <p>
                     Chương trình Trả Góp Hàng Tháng Với MoMo do đối tác tín dụng cung cấp qua ứng dụng MoMo của Công Ty Cổ Phần Dịch Vụ Di Động Trực Tuyến (“MoMo”) chứ không phải Apple. Chỉ cư dân Việt Nam đủ điều kiện mới có thể mua sản phẩm đủ điều kiện qua chương trình này. Tất cả sản phẩm được mua qua hình thức Trả Góp Hàng Tháng Với MoMo đều cần có tài khoản ví điện tử MoMo và phải được đối tác tín dụng của MoMo phê duyệt. Nếu bạn có câu hỏi về điều kiện tín dụng của mình, vui lòng liên hệ với MoMo để nhận câu trả lời từ đối tác tín dụng của MoMo. Ngoài ra, vui lòng tham khảo ứng dụng MoMo hoặc đối tác tín dụng của MoMo để biết điều kiện, phí và phụ phí.
                  </p>
                  <p>
                     Apple có toàn quyền quyết định sản phẩm nào đủ điều kiện hưởng ưu đãi trả góp vào bất cứ lúc nào. Mọi thay đổi về việc lựa chọn sản phẩm, kỳ hạn trả góp và lãi suất đều sẽ làm thay đổi ưu đãi trả góp hàng tháng. Lãi suất quy định là lãi suất tính theo phần trăm hàng tháng. Áp dụng số tiền mua tối thiểu 3.000.000đ cho sản phẩm đủ điều kiện và cần phải thanh toán trước 20% cho mọi sản phẩm bạn mua.
                  </p>
                  <p>
                     Đơn hàng phải được đặt trên Apple Store Trực Tuyến apple.com/vn.
                  </p>
               </div>
               <div className={cn('w-full border border-[#ccc]')} />
               <div className={cn('w-full flex flex-row')}>
                  <div className={cn('flex-1 flex flex-col gap-5')}>
                     <div className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Mua Sắm Và Tìm Hiểu</p>
                        <ul className={cn('flex flex-col gap-1')}>
                           <li>Cửa Hàng</li>
                           <li>Mac</li>
                           <li>iPad</li>
                           <li>iPhone</li>
                           <li>Watch</li>
                           <li>AirPods</li>
                           <li>TV & Nhà</li>
                           <li>AirTag</li>
                           <li>Phụ Kiện</li>
                        </ul>
                     </div>
                     <div className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Ví Apple</p>
                        <ul className={cn('flex flex-col gap-1')}>
                           <li>Ví</li>
                           <li>Apple Pay</li>
                        </ul>
                     </div>
                  </div>
      
                  <div className={cn('flex-1 flex flex-col gap-5')}>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Tài Khoản</p>
                        <li>Quản Lý Tài Khoản Apple Của Bạn</li>
                        <li>Tài Khoản Apple Store</li>
                        <li>iCloud.com</li>
                     </ul>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Giải Trí</p>
                        <li>Apple One</li>
                        <li>Apple TV+</li>
                        <li>Apple Music</li>
                        <li>Apple Arcade</li>
                        <li>Apple Podcasts</li>
                        <li>Apple Books</li>
                     </ul>
                  </div>
      
                  <div className={cn('flex-1 flex flex-col gap-5')}>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Apple Store</p>
                        <li>Ứng Dụng Apple Store</li>
                        <li>Apple Trade In</li>
                        <li>Tài Chính</li>
                        <li>Tình Trạng Đơn Hàng</li>
                        <li>Hỗ Trợ Mua Hàng</li>
                     </ul>
                  </div>
      
                  <div className={cn('flex-1 flex flex-col gap-5')}>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Dành Cho Doanh Nghiệp</p>
                        <li>Apple Và Doanh Nghiệp</li>
                        <li>Mua Hàng Cho Doanh Nghiệp</li>
                     </ul>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p>Cho Giáo Dục</p>
                        <li>Apple Và Giáo Dục</li>
                        <li>Mua Hàng Cho Bậc Đại Học</li>
                     </ul>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Cho Chăm Sóc Sức Khỏe</p>
                        <li>Apple Trong Chăm Sóc Sức Khỏe</li>
                        <li>Mac Trong Chăm Sóc Sức Khỏe</li>
                        <li>Sức Khỏe Trên Apple Watch</li>
                     </ul>
                  </div>
      
                  <div className={cn('flex-1 flex flex-col gap-5')}>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Giá Trị Cốt Lõi Của Apple</p>
                        <li>Trợ Năng</li>
                        <li>Môi Trường</li>
                        <li>Quyền Riêng Tư</li>
                        <li>Chuỗi Cung Ứng</li>
                     </ul>
                     <ul className={cn('flex flex-col gap-1')}>
                        <p className={cn('font-medium')}>Về Apple</p>
                        <li>Newsroom</li>
                        <li>Lãnh Đạo Của Apple</li>
                        <li>Nhà Đầu Tư</li>
                        <li>Đạo Đức & Quy Tắc</li>
                        <li>Sự Kiện</li>
                        <li>Liên Hệ Apple</li>
                     </ul>
                  </div>
               </div>
            </div>
         </div>
      </footer>
   );
};

export default Footer;
