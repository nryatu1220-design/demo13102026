using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastTerm_Draft.Data;
using TheLastTerm_Draft.Models;
using TheLastTerm_Draft.Service;
using TheLastTerm_Draft.Services;

namespace TheLastTerm_Draft
{
    class Program
    {
        private const string ConnectionString = "Server=localhost;Database=QUANLYKHO_CUOIKY;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // 1. Khởi tạo Context
                var builder = new DbContextOptionsBuilder<HeThongKhoContext>().UseSqlServer(ConnectionString);
                using var context = new HeThongKhoContext(builder.Options);

                // 2. Khởi tạo & LoadData cho toàn bộ các Service phụ thuộc
                var sps = new SanPhamService(new(context), new(context), new(context));
                sps.LoadData();

                var nvs = new NhanVienService(new(context), new(context));
                nvs.LoadData();

                var hdService = new HoaDonService(new(context), new(context))
                {
                    sps = sps,
                    nvs = nvs
                };
                hdService.LoadData();

                // 3. Khai báo thông tin test
                const string maHoaDon = "HD13";
                const string maNhanVien = "NV07"; // Nhân viên bán hàng (RoleCode hợp lệ, có mã kho)

                var hoaDon = hdService.TimHoaDon(maHoaDon);

                // --- LUỒNG 1: NẾU HÓA ĐƠN CHƯA TỒN TẠI -> TẠO MỚI & THÊM CHI TIẾT ---
                if (hoaDon == null)
                {
                    //Console.WriteLine($"[1] Hóa đơn {maHoaDon} chưa tồn tại. Bắt đầu tạo mới...");

                    //// Bước 1: Tạo hóa đơn rỗng trước
                    //var hoaDonMoi = new HoaDon(maHoaDon, maNhanVien);
                    //hdService.ThemHoaDon(hoaDonMoi, maNhanVien);
                    //Console.WriteLine("--> Tạo hóa đơn nháp thành công!");

                    //// Bước 2: Thêm từng chi tiết sản phẩm 
                    //// (Hàm này tự động lưu CSDL, tính lại TongTien và update bảng HOA_DON)
                    //hdService.ThemChiTietHoaDon(new ChiTietHoaDon(maHoaDon, "SP03", 2, 1200000));
                    //Console.WriteLine("--> Thêm các chi tiết hóa đơn & tự động tính TongTien thành công!");
                }
                // --- LUỒNG 2: NẾU HÓA ĐƠN ĐANG Ở DẠNG NHÁP -> CẬP NHẬT HOẶC XÁC NHẬN ---
                else if (hoaDon.TrangThai == TrangThai.Nhap)
                {
                    //    Console.WriteLine($"[2] Hóa đơn {maHoaDon} đang ở trạng thái Nháp.");

                    //    // TEST CẬP NHẬT HÓA ĐƠN: Đổi số lượng từ 2 thành 1
                    //    Console.WriteLine("Đang thử cập nhật lại danh sách sản phẩm (chỉnh số lượng từ 2 về 1)...");
                    //    hoaDon.ChiTietHoaDons = new List<ChiTietHoaDon>
                    //        {
                    //            new ChiTietHoaDon(maHoaDon, "SP03", 1, 1200000), // Đổi số lượng từ 2 -> 1
                    //        };
                    //    hdService.CapNhatHoaDon(hoaDon);
                    //    Console.WriteLine("--> Cập nhật hóa đơn thành công! (TongTien đã tự động cập nhật lại)");

                    ////BẮT ĐẦU TEST XÁC NHẬN HÓA ĐƠN & TRỪ KHO
                    //Console.WriteLine("Đang tiến hành xác nhận hóa đơn...");
                    //hdService.XacNhanHoaDon(hoaDon);
                    //Console.WriteLine("--> Xác nhận hóa đơn thành công! (Đã tự động trừ kho của nhân viên phụ trách)");

                    // THỰC HIỆN HỦY HÓA ĐƠN
                    Console.WriteLine("Đang tiến hành hủy hóa đơn...");
                    hdService.HuyHoaDon(hoaDon);
                    Console.WriteLine("--> Hủy hóa đơn thành công! (Trạng thái hóa đơn đã chuyển sang TrangThai.Huy)");
                }
                // --- LUỒNG 3: HÓA ĐƠN ĐÃ XÁC NHẬN HOẶC HỦY ---
                else
                {
                    Console.WriteLine($"[3] Hóa đơn {maHoaDon} đang ở trạng thái: {hoaDon.TrangThai} (Không được phép chỉnh sửa).");
                }
            }
            catch (Exception ex)
            {
                // Unwrapping để hiển thị chính xác câu thông báo lỗi thực tế từ SQL Server
                string chiTietLoi = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine("\n[LỖI THAO TÁC]");
                Console.WriteLine($"Chi tiết: {chiTietLoi}");
            }
        }
    }
}
