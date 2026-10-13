using System;
using System.Collections.Generic;
using System.Linq;
using TheLastTerm_Draft.Models;
using TheLastTerm_Draft.Service;

namespace TheLastTerm_Draft.Service
{
    public class NhanVienService
    {
        public List<NhanVien> nhanViens =
            new List<NhanVien>();

        public List<TaiKhoan> taiKhoans =
            new List<TaiKhoan>();

        private readonly GenericCrudService<NhanVien> _nhanVienService;
        private readonly GenericCrudService<TaiKhoan> _taiKhoanService;

        public NhanVienService(
            GenericCrudService<NhanVien> nhanVienService,
            GenericCrudService<TaiKhoan> taiKhoanService)
        {
            _nhanVienService = nhanVienService
                ?? throw new ArgumentNullException(nameof(nhanVienService));

            _taiKhoanService = taiKhoanService
                ?? throw new ArgumentNullException(nameof(taiKhoanService));
        }

        public void LoadData()
        {
            // Lấy danh sách nhân viên từ database
            List<NhanVien> danhSachNhanVien =
                _nhanVienService.LayTatCa();

            foreach (var nv in danhSachNhanVien)
            {
                NhanVien nhanVien = new NhanVien(
                    nv.MaNhanVien,
                    nv.HoTen,
                    nv.GioiTinh,
                    nv.NgaySinh,
                    nv.SoDienThoai,
                    nv.Email,
                    nv.Luong,
                    nv.RoleCode,
                    nv.MaKho
                );

                nhanViens.Add(nhanVien);
            }

            // Lấy danh sách tài khoản từ database
            List<TaiKhoan> danhSachTaiKhoan =
                _taiKhoanService.LayTatCa();

            foreach (var tk in danhSachTaiKhoan)
            {
                TaiKhoan taiKhoan = TaiKhoan.FromDatabase(
                    tk.MaNhanVien,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.TinhTrangLaoDong,
                    tk.TrangThaiHoatDong
                );

                taiKhoans.Add(taiKhoan);
            }
        }
    }
}
