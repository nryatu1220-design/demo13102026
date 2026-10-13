using System;
using System.Collections.Generic;
using System.Linq;
using TheLastTerm_Draft.Models;
using TheLastTerm_Draft.Service;

namespace TheLastTerm_Draft.Service
{
    public class SanPhamService
    {
        public List<SanPham> sanPhams =
            new List<SanPham>();

        public List<SanPhamKho> sanPhamKhos =
            new List<SanPhamKho>();



        private readonly GenericCrudService<SanPham> _sanPhamService;
        private readonly GenericCrudService<SanPhamKho> _sanPhamKhoService;
        private readonly GenericCrudService<ThietBi> _thietBiService;

        public SanPhamService(
            GenericCrudService<SanPham> sanPhamService,
            GenericCrudService<SanPhamKho> sanPhamKhoService,
            GenericCrudService<ThietBi> thietBiService)
        {
            _sanPhamService = sanPhamService
                ?? throw new ArgumentNullException(nameof(sanPhamService));

            _sanPhamKhoService = sanPhamKhoService
                ?? throw new ArgumentNullException(nameof(sanPhamKhoService));

            _thietBiService = thietBiService
                ?? throw new ArgumentNullException(nameof(thietBiService));
        }

        public void LoadData()
        {
            sanPhams.Clear();
            sanPhamKhos.Clear();

            // =========================
            // Lấy danh sách sản phẩm
            // =========================
            List<SanPham> danhSachSanPham =
                _sanPhamService.LayTatCa();

            foreach (var sp in danhSachSanPham)
            {
                SanPham sanPham = new SanPham(
                    sp.MaSanPham,
                    sp.TenSanPham,
                    sp.TenLoaiSanPham,
                    sp.MoTaSanPham,
                    sp.TienNhapVe,
                    sp.TienBan
                );

                sanPhams.Add(sanPham);
            }

            // =========================
            // Lấy danh sách sản phẩm trong kho
            // =========================
            List<SanPhamKho> danhSachSanPhamKho =
                _sanPhamKhoService.LayTatCa();

            foreach (var spk in danhSachSanPhamKho)
            {
                SanPhamKho sanPhamKho =
                    SanPhamKho.FromDatabase(
                        spk.MaSanPham,
                        spk.MaKho,
                        spk.MaViTri,
                        spk.SoLuongLyThuyet,
                        spk.SoLuongThucTe
                    );

                sanPhamKhos.Add(sanPhamKho);
            }

            // =========================
            // Lấy danh sách thiết bị
            // =========================
            List<ThietBi> danhSachThietBi =
                _thietBiService.LayTatCa();

            foreach (var tb in danhSachThietBi)
            {
                ThietBi thietBi =
                    ThietBi.FromDataBase(
                        tb.MaThietBi,
                        tb.MaSanPham,
                        tb.MaKho,
                        tb.TinhTrang
                    );

                // Tìm SanPhamKho chứa sản phẩm này tại kho tương ứng
                SanPhamKho sanPhamKho = sanPhamKhos.FirstOrDefault(
                    x => x.MaSanPham == thietBi.MaSanPham
                      && x.MaKho == thietBi.MaKho
                );

                // Đưa thiết bị vào SanPhamKho
                if (sanPhamKho != null)
                {
                    sanPhamKho.ThietBis.Add(thietBi);
                }
            }
        }

        public void ThemThietBi(string maSanPham,
                          string maKho,
                          int soLuongThem)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
                throw new ArgumentException(
                    "Mã sản phẩm không được để trống."
                );

            if (string.IsNullOrWhiteSpace(maKho))
                throw new ArgumentException(
                    "Mã kho không được để trống."
                );

            if (soLuongThem <= 0)
                throw new ArgumentException(
                    "Số lượng thêm phải lớn hơn 0."
                );

            // Tìm sản phẩm kho tương ứng
            SanPhamKho sanPhamKho = sanPhamKhos.FirstOrDefault(
                x => x.MaSanPham == maSanPham &&
                     x.MaKho == maKho
            );

            if (sanPhamKho == null)
                throw new ArgumentException(
                    "Sản phẩm chưa tồn tại trong kho."
                );

            // =========================
            // Tạo mã thiết bị mới
            // =========================

            int soThuTu = sanPhamKho.ThietBis.Count + 1;

            for (int i = 0; i < soLuongThem; i++)
            {
                string maThietBi =
                    $"TB_{maKho}_{maSanPham}_{soThuTu + i:D2}";

                var thietBi = new ThietBi(maThietBi, maSanPham, maKho);

                _thietBiService.Them(thietBi);
                sanPhamKho.ThietBis.Add(thietBi);
            }

            sanPhamKho.SoLuongLyThuyet += soLuongThem;
            sanPhamKho.SoLuongThucTe += soLuongThem;
            _sanPhamKhoService.Sua(sanPhamKho);
        }


        public void XoaThietBi(
            string maSanPham,
            string maKho,
            int soLuongXoa)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
                throw new ArgumentException(
                    "Mã sản phẩm không được để trống."
                );

            if (string.IsNullOrWhiteSpace(maKho))
                throw new ArgumentException(
                    "Mã kho không được để trống."
                );

            if (soLuongXoa <= 0)
                throw new ArgumentException(
                    "Số lượng xóa phải lớn hơn 0."
                );

            // Tìm sản phẩm kho tương ứng
            SanPhamKho sanPhamKho = sanPhamKhos.FirstOrDefault(
                x => x.MaSanPham == maSanPham &&
                     x.MaKho == maKho
            );

            if (sanPhamKho == null)
                throw new ArgumentException(
                    "Sản phẩm chưa tồn tại trong kho."
                );
            if (sanPhamKho.SoLuongThucTe < soLuongXoa)
            {
                throw new ArgumentException(
                    $"Số lượng thực tế không đủ để xuất! (Hiện có: {sanPhamKho.SoLuongThucTe}, Cần xuất: {soLuongXoa})"
                );
            }

            // Chỉ lấy thiết bị bình thường
            List<ThietBi> danhSachXoa =
                sanPhamKho.ThietBis
                    .Where(x => x.TinhTrang == TinhTrang.BinhThuong)
                    .Take(soLuongXoa)
                    .ToList();

            if (danhSachXoa.Count < soLuongXoa)
                throw new ArgumentException(
                    "Số lượng thiết bị bình thường không đủ để xóa."
                );

            sanPhamKho.SoLuongLyThuyet -= danhSachXoa.Count;
            sanPhamKho.SoLuongThucTe -= danhSachXoa.Count;

            foreach (ThietBi thietBi in danhSachXoa)
            {
                _thietBiService.Xoa(thietBi);
                sanPhamKho.ThietBis.Remove(thietBi);
            }

            _sanPhamKhoService.Sua(sanPhamKho);
        }

        //public void XoaThietBi(string maSanPham, string maKho, int soLuongXoa)
        //{
        //    if (string.IsNullOrWhiteSpace(maSanPham))
        //        throw new ArgumentException("Mã sản phẩm không được để trống.");

        //    if (string.IsNullOrWhiteSpace(maKho))
        //        throw new ArgumentException("Mã kho không được để trống.");

        //    if (soLuongXoa <= 0)
        //        throw new ArgumentException("Số lượng xóa phải lớn hơn 0.");

        //    // 1. Tìm sản phẩm tồn trong kho
        //    SanPhamKho sanPhamKho = sanPhamKhos.FirstOrDefault(
        //        x => x.MaSanPham == maSanPham && x.MaKho == maKho
        //    );

        //    if (sanPhamKho == null)
        //        throw new ArgumentException("Sản phẩm chưa tồn tại trong kho.");

        //    // 2. Kiểm tra số lượng tồn thực tế có đủ để xuất hay không
        //    if (sanPhamKho.SoLuongThucTe < soLuongXoa)
        //    {
        //        throw new ArgumentException(
        //            $"Số lượng thực tế không đủ để xuất! (Hiện có: {sanPhamKho.SoLuongThucTe}, Cần xuất: {soLuongXoa})"
        //        );
        //    }

        //    // 3. Trừ trực tiếp số lượng lý thuyết và thực tế của kho
        //    sanPhamKho.SoLuongLyThuyet -= soLuongXoa;
        //    sanPhamKho.SoLuongThucTe -= soLuongXoa;

        //    // 4. Tìm các thiết bị sẵn có trong bảng THIET_BI để xóa kèm (nếu thiếu cũng KHÔNG báo lỗi)
        //    List<ThietBi> danhSachXoa = sanPhamKho.ThietBis
        //        .Where(x => x.TinhTrang == TinhTrang.BinhThuong)
        //        .Take(soLuongXoa)
        //        .ToList();

        //    foreach (ThietBi thietBi in danhSachXoa)
        //    {
        //        _thietBiService.Xoa(thietBi);
        //        sanPhamKho.ThietBis.Remove(thietBi);
        //    }

        //    // 5. Cập nhật số lượng tồn kho mới vào Database
        //    _sanPhamKhoService.Sua(sanPhamKho);
        //}
    }
}
