//using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastTerm_Draft.Models;
using TheLastTerm_Draft.Service;

namespace TheLastTerm_Draft.Services
{
    public class HoaDonService
    {
        public List<HoaDon> hoaDons = new List<HoaDon>();

        private readonly GenericCrudService<HoaDon> _hoaDonService;
        private readonly GenericCrudService<ChiTietHoaDon> _chiTietHoaDonService;

        public required SanPhamService sps { get; set; }
        public required NhanVienService nvs { get; set; }
        public HoaDonService(
            GenericCrudService<HoaDon> hoaDonService,
            GenericCrudService<ChiTietHoaDon> chiTietHoaDonService)
        {
            _hoaDonService = hoaDonService
                ?? throw new ArgumentNullException(nameof(hoaDonService));

            _chiTietHoaDonService = chiTietHoaDonService
                ?? throw new ArgumentNullException(nameof(chiTietHoaDonService));
        }

        public void LoadData()
        {
            hoaDons.Clear();
            // Lấy danh sách hóa đơn từ database
            hoaDons = _hoaDonService.LayTatCa()
            .Select(x => HoaDon.FromDatabase(
                x.MaHoaDon,
                x.MaNhanVien,
                x.NgayGioTao,
                x.TongTien,
                x.TrangThai
            ))
            .ToList();

            var tatCaChiTiet = _chiTietHoaDonService.LayTatCa();

            // Với mỗi hóa đơn, load các chi tiết hóa đơn
            foreach (var hoaDon in hoaDons)
            {
                var chiTietHoaDons = tatCaChiTiet
                    .Where(x => x.MaHoaDon == hoaDon.MaHoaDon)
                    .Select(x => new ChiTietHoaDon(
                        x.MaHoaDon,
                        x.MaSanPham,
                        x.SoLuong,
                        x.GiaBan
                    ))
                    .ToList();

                hoaDon.ChiTietHoaDons = chiTietHoaDons;
            }
        }


        public HoaDon? TimHoaDon(string maHoaDon)
        {
            return hoaDons.FirstOrDefault(x => x.MaHoaDon == maHoaDon);
        }

        // Xác nhận hóa đơn
        // trừ số lượng trong sản phẩm kho

        // Nhân viên phụ trách hóa đơn sẽ làm hóa đơn theo rolecode là 4 nhân viên bán hàng
        //string NhanVien.rolecode = 4;
        public NhanVien? TimNhanVien(string maNhanVien)
        {
            return nvs.nhanViens.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
        }

        public void ThemHoaDon(HoaDon hoaDon, string MaNhanVienphuTrach)
        {
            var nhanVien = TimNhanVien(MaNhanVienphuTrach);
            if(nhanVien == null)
                throw new ArgumentException("Nhân viên không tồn tại.");

            // Kiểm tra xem nhân viên có quyền tạo hóa đơn không rolecode = 4
            if (nhanVien.RoleCode != (RoleCode)4)
                throw new ArgumentException("Nhân viên không có quyền tạo hóa đơn.");

            //Kiểm tra nhân viên đã được phân vào kho chưa
            if (string.IsNullOrEmpty(nhanVien.MaKho))
                throw new ArgumentException("Nhân viên chưa được phân vào kho.");

            if (TimHoaDon(hoaDon.MaHoaDon) != null)
                throw new ArgumentException("Hóa đơn đã tồn tại.");

            hoaDon.MaNhanVien = MaNhanVienphuTrach;

            _hoaDonService.Them(hoaDon);
            LoadData();
        }

        public void ThemChiTietHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            // Kiểm tra hóa đơn đã tồn tại chưa
            var hoaDon = TimHoaDon(chiTietHoaDon.MaHoaDon);
            if (hoaDon == null)
                throw new ArgumentException("Hóa đơn không tồn tại.");

            // Chỉ hóa đơn nháp mới được phép sửa.
            if (hoaDon.TrangThai != TrangThai.Nhap)
                throw new ArgumentException("Hóa đơn đã được xác nhận, không thể cập nhật.");

            // Kiểm tra chi tiết đã tồn tại chưa
            if (hoaDon.ChiTietHoaDons.Any(ct => ct.MaSanPham == chiTietHoaDon.MaSanPham))
                throw new ArgumentException("Chi tiết hóa đơn đã tồn tại.");

            _chiTietHoaDonService.Them(chiTietHoaDon);
            hoaDon.ChiTietHoaDons.Add(chiTietHoaDon);
            hoaDon.TongTien = hoaDon.ChiTietHoaDons.Sum(x => x.SoLuong * x.GiaBan);
            _hoaDonService.Sua(hoaDon);
            LoadData();
        }

        public void CapNhatHoaDon(HoaDon hoaDon)
        {
            var hoaDonHienTai = TimHoaDon(hoaDon.MaHoaDon);

            // Kiểm tra hóa đơn đã tồn tại chưa
            if (hoaDonHienTai == null)
                throw new ArgumentException("Hóa đơn không tồn tại.");

            // Chỉ hóa đơn nháp mới được phép sửa.
            if (hoaDonHienTai.TrangThai != TrangThai.Nhap)
                throw new ArgumentException("Hóa đơn đã được xác nhận, không thể cập nhật.");

            var chiTietMoi = hoaDon.ChiTietHoaDons.ToList();

            if (chiTietMoi.Any(ct => ct.MaHoaDon != hoaDon.MaHoaDon))
                throw new ArgumentException("Mã hóa đơn trong chi tiết không khớp với hóa đơn cần cập nhật.");

            if (chiTietMoi.GroupBy(ct => ct.MaSanPham).Any(nhom => nhom.Count() > 1))
                throw new ArgumentException("Một sản phẩm chỉ được xuất hiện một lần trong hóa đơn.");

            // MaSanPham là một phần của khóa chính ChiTietHoaDon. Vì vậy thay thế
            // toàn bộ chi tiết sẽ xử lý được cả đổi sản phẩm, số lượng và giá bán.
            // Đọc lại từ DB vì caller có thể đã thay ChiTietHoaDons của
            // hoaDonHienTai trước khi gọi hàm này.
            var chiTietCu = _chiTietHoaDonService.LayTatCa()
                .Where(ct => ct.MaHoaDon == hoaDon.MaHoaDon)
                .ToList();

            foreach (var chiTiet in chiTietCu)
            {
                _chiTietHoaDonService.Xoa(chiTiet);
            }

            foreach (var chiTiet in chiTietMoi)
            {
                _chiTietHoaDonService.Them(chiTiet);
            }

            hoaDonHienTai.ChiTietHoaDons = chiTietMoi;

            hoaDonHienTai.TongTien = chiTietMoi.Sum(ct => ct.SoLuong * ct.GiaBan);

            _hoaDonService.Sua(hoaDonHienTai);

            LoadData();
        }

        public void XacNhanHoaDon(HoaDon hoaDon)
        {
            //Kiểm tra hóa đơn đã tồn tại chưa
            if (TimHoaDon(hoaDon.MaHoaDon) == null)
                throw new ArgumentException("Hóa đơn không tồn tại.");

            //Kiểm tra tráng thái của hóa đơn, nếu trạng thái là Nháp thì được cập nhật
            if (hoaDon.TrangThai != TrangThai.Nhap)
                throw new ArgumentException("Hóa đơn đã được xác nhận, không thể cập nhật.");

            //lấy má kho của nhân viên phụ trách hóa đơn
            var nhanVien = TimNhanVien(hoaDon.MaNhanVien);
            if (nhanVien == null || string.IsNullOrEmpty(nhanVien.MaKho))
                throw new ArgumentException("Nhân viên phụ trách hóa đơn không tồn tại hoặc chưa được phân vào kho.");

            // Xóa thiêts bị khỏi kho khi đã xác nhận
            if (hoaDon.ChiTietHoaDons != null)
            {
                foreach (var chiTiet in hoaDon.ChiTietHoaDons)
                {
                    sps.XoaThietBi(chiTiet.MaSanPham, nhanVien.MaKho, chiTiet.SoLuong);
                }
            }

            // Cập nhật trạng thái hóa đơn thành Xác nhận
            hoaDon.TrangThai = TrangThai.XacNhan;

            _hoaDonService.Sua(hoaDon);
            LoadData();
        }

        public void HuyHoaDon(HoaDon hoaDon)
        {
            //Kiểm tra hóa đơn đã tồn tại chưa
            if (TimHoaDon(hoaDon.MaHoaDon) == null)
                throw new ArgumentException("Hóa đơn không tồn tại.");

            //Kiểm tra tráng thái của hóa đơn, nếu trạng thái là Nháp thì được cập nhật
            if (hoaDon.TrangThai != TrangThai.Nhap)
                throw new ArgumentException("Hóa đơn đã được xác nhận, không thể cập nhật.");

            // Cập nhật trạng thái hóa đơn thành Hủy
            hoaDon.TrangThai = TrangThai.Huy;
            _hoaDonService.Sua(hoaDon);
            LoadData();
        }

        public List<HoaDon> LayTatCaHoaDon()
        {
            return hoaDons;
        }
    }
}
