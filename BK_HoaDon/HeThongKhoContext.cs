using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TheLastTerm_Draft.Models;

namespace TheLastTerm_Draft.Data
{
    public class HeThongKhoContext : DbContext
    {
        public HeThongKhoContext(DbContextOptions<HeThongKhoContext> options)
          : base(options)
        {
        }
        public DbSet<Kho> Kho { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<SanPhamKho> SanPhamKhos { get; set; }
        public DbSet<ThietBi> ThietBis { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }   // <-- Đã thêm DbSet Nhân viên
        public DbSet<TaiKhoan> TaiKhoans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // =========================================================
            // 1. ÁNH XẠ TÊN BẢNG TRONG CSDL (UPPERCASE_WITH_UNDERSCORE)
            // =========================================================
            modelBuilder.Entity<Kho>().ToTable("KHO");
            modelBuilder.Entity<LoaiSanPham>().ToTable("LOAI_SAN_PHAM");
            modelBuilder.Entity<ViTri>().ToTable("VI_TRI");
            modelBuilder.Entity<NhanVien>().ToTable("NHAN_VIEN");
            modelBuilder.Entity<TaiKhoan>().ToTable("TAI_KHOAN");
            modelBuilder.Entity<SanPham>().ToTable("SAN_PHAM");
            modelBuilder.Entity<SanPhamKho>().ToTable("SAN_PHAM_KHO");
            modelBuilder.Entity<ThietBi>().ToTable("THIET_BI");
            modelBuilder.Entity<HoaDon>().ToTable("HOA_DON");
            modelBuilder.Entity<ChiTietHoaDon>().ToTable("CHI_TIET_HOA_DON");
            //        // 1. Chỉ định tên bảng trong SQL Server
            //        modelBuilder.Entity<HoaDon>().ToTable("Hoa_Don");
            //        modelBuilder.Entity<ChiTietHoaDon>().ToTable("Chi_Tiet_Hoa_Don");
            //        modelBuilder.Entity<SanPham>().ToTable("San_Pham");
            //        modelBuilder.Entity<ThietBi>().ToTable("Thiet_Bi");
            //        modelBuilder.Entity<SanPhamKho>().ToTable("San_Pham_Kho");

            //        // 2. Khai báo Khóa chính đơn (THÊM CÁC DÒNG NÀY)
            //        modelBuilder.Entity<SanPham>().HasKey(sp => sp.MaSanPham);
            //        modelBuilder.Entity<HoaDon>().HasKey(hd => hd.MaHoaDon);
            //        modelBuilder.Entity<ThietBi>().HasKey(tb => tb.MaThietBi);

            //        // 3. Khai báo Khóa chính gộp
            //        modelBuilder.Entity<SanPhamKho>()
            //            .HasKey(spk => new { spk.MaSanPham, spk.MaKho }); 

            //modelBuilder.Entity<ChiTietHoaDon>()
            //    .HasKey(ct => new { ct.MaHoaDon, ct.MaSanPham });

            //        // 4. Bỏ qua thuộc tính không lưu trong DB
            //        modelBuilder.Entity<SanPhamKho>()
            //            .Ignore(spk => spk.ThietBis);

            // =========================================================
            // 1. KHO
            // =========================================================


            // =========================================================
            // 2. NHAN VIEN
            // =========================================================

            modelBuilder.Entity<NhanVien>()
                .HasKey(k => k.MaNhanVien);


            // NHAN_VIEN.MaKho -> KHO.MaKho
            modelBuilder.Entity<NhanVien>()
                .HasOne<Kho>()
                .WithMany()
                .HasForeignKey(k => k.MaKho)
                .IsRequired(false);


            // =========================================================
            // 3. TAI KHOAN
            // =========================================================

            modelBuilder.Entity<TaiKhoan>()
                .HasKey(k => k.MaNhanVien);


            // TAI_KHOAN.MaNhanVien -> NHAN_VIEN.MaNhanVien
            modelBuilder.Entity<TaiKhoan>()
                .HasOne<NhanVien>()
                .WithOne()
                .HasForeignKey<TaiKhoan>(k => k.MaNhanVien);

            modelBuilder.Entity<Kho>()
                .HasKey(k => k.MaKho);


            // KHO.MaKhoChiuTrachNhiem -> KHO.MaKho
            modelBuilder.Entity<Kho>()
                .HasOne<Kho>()
                .WithMany()
                .HasForeignKey(k => k.MaKhoChiuTrachNhiem)
                .IsRequired(false);



            // =========================================================
            // 5. LOAI SAN PHAM
            // =========================================================

            modelBuilder.Entity<LoaiSanPham>()
                .HasKey(k => k.TenLoaiSanPham);

            // =========================================================
            // 6. VI TRI
            // =========================================================

            modelBuilder.Entity<ViTri>()
                .HasKey(k => k.MaViTri);



            // =========================================================
            // 7. SAN PHAM
            // =========================================================

            modelBuilder.Entity<SanPham>()
                .HasKey(k => k.MaSanPham);

            // SAN_PHAM.TenLoaiSanPham -> LOAI_SAN_PHAM.TenLoaiSanPham
            modelBuilder.Entity<SanPham>()
                .HasOne<LoaiSanPham>()
                .WithMany()
                .HasForeignKey(k => k.TenLoaiSanPham);


            // =========================================================
            modelBuilder.Entity<SanPham>()
                .HasOne<LoaiSanPham>()
                .WithMany()
                .HasForeignKey(k => k.TenLoaiSanPham);

            // =========================================================
            // 7.1 HOA DON VA CHI TIET HOA DON
            // =========================================================

            modelBuilder.Entity<HoaDon>()
                .HasKey(hd => hd.MaHoaDon);

            modelBuilder.Entity<ChiTietHoaDon>()
                .ToTable("CHI_TIET_HOA_DON")
                .HasKey(ct => new { ct.MaHoaDon, ct.MaSanPham });

            modelBuilder.Entity<ChiTietHoaDon>()
                .HasOne<HoaDon>()
                .WithMany(hd => hd.ChiTietHoaDons)
                .HasForeignKey(ct => ct.MaHoaDon);

            modelBuilder.Entity<ChiTietHoaDon>()
                .HasOne<SanPham>()
                .WithMany()
                .HasForeignKey(ct => ct.MaSanPham);


            // =========================================================


            // =========================================================
            // 8. SAN PHAM KHO
            // =========================================================

            modelBuilder.Entity<SanPhamKho>()
                .HasKey(k => new
                {
                    k.MaSanPham,
                    k.MaKho,
                });

            // SAN_PHAM_KHO.MaSanPham -> SAN_PHAM.MaSanPham
            modelBuilder.Entity<SanPhamKho>()
                .HasOne<SanPham>()
                .WithMany()
                .HasForeignKey(k => k.MaSanPham);

            // SAN_PHAM_KHO.MaKho -> KHO.MaKho
            modelBuilder.Entity<SanPhamKho>()
                .HasOne<Kho>()
                .WithMany()
                .HasForeignKey(k => k.MaKho);

            // SAN_PHAM_KHO.MaViTri -> VI_TRI.MaViTri
            modelBuilder.Entity<SanPhamKho>()
                .HasOne<ViTri>()
                .WithMany()
                .HasForeignKey(k => k.MaViTri);


            // =========================================================
            // 9. THIET BI
            // =========================================================

            modelBuilder.Entity<ThietBi>()
                .HasKey(k => new
                {
                    k.MaThietBi,
                    k.MaSanPham,
                    k.MaKho
                });

            // THIET_BI -> SAN_PHAM_KHO
            modelBuilder.Entity<ThietBi>()
                .HasOne<SanPhamKho>()
                .WithMany(x => x.ThietBis)
                .HasForeignKey(k => new
                {
                    k.MaSanPham,
                    k.MaKho
                });

            // THIET_BI.MaSanPham -> SAN_PHAM.MaSanPham
            modelBuilder.Entity<ThietBi>()
                .HasOne<SanPham>()
                .WithMany()
                .HasForeignKey(k => k.MaSanPham);

            // THIET_BI.MaKho -> KHO.MaKho
            modelBuilder.Entity<ThietBi>()
                .HasOne<Kho>()
                .WithMany()
                .HasForeignKey(k => k.MaKho);
        }
    }
}
