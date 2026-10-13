
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    public enum TinhTrang
    {
        [Display(Name ="Bình thường")]
        BinhThuong =0,
        [Display(Name ="Hư hỏng")]
        HuHong = 1,
        [Display(Name = "Đang đổi trả")]
        DangDoiTra = 2
    }

    [Table("THIET_BI")]
    public class ThietBi
    {
        private string maThietBi;
        private string maSanPham;
        private string maKho;
        private TinhTrang tinhTrang;


        [Column("MaThietBi")]
        public string MaThietBi
        {
            get
            {
                return maThietBi;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã thiết bị không được để trống.");

                //if (!Regex.IsMatch(value, @"^TB\d+$"))
                //    throw new ArgumentException("Mã thiết bị phải có dạng TB + số.");

                //maThietBi = value;
                if (!Regex.IsMatch(value, @"^(?i)TB_[A-Z0-9_]+$"))
                {
                    throw new ArgumentException("Mã thiết bị không đúng định dạng (VD: TB_KH01_SP01_01).");
                }

                maThietBi = value.ToUpper();
            }
        }

        [Column("MaSanPham")]
        public string MaSanPham
        {
            get
            {
                return maSanPham;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mã sản phẩm không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^SP\d+$"))
                {
                    throw new ArgumentException("Mã sản phẩm phải có dạng SP + số.");
                }

                maSanPham = value;
            }
        }

        [Column("MaKho")]
        public string MaKho
        {
            get
            {
                return maKho;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mã kho không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^KH\d+$"))
                {
                    throw new ArgumentException("Mã kho phải có dạng KH + số.");
                }
                maKho = value;
            }
        }

        [Column("TinhTrang")]
        public TinhTrang TinhTrang
        {
            get
            {
                return tinhTrang;
            }
            set
            {
                if(!Enum.IsDefined(typeof(TinhTrang), value))
                {
                    throw new ArgumentException("Giá trị tình trạng của thiết bị không đúng định dạng");
                }
                tinhTrang = value;
            }
        }

        //Dùng để tạo thêm vào khi thêm thiết bị mới
        public ThietBi(string maThietBi, string maSanPham, string maKho)
        {
            MaThietBi = maThietBi;
            MaSanPham = maSanPham;
            MaKho = maKho;
            TinhTrang = TinhTrang.BinhThuong;
        }

        // Dùng để lấy thiết bị từ database ra
        public static ThietBi FromDataBase(string maThietBi, string maSanPham, string maKho, TinhTrang tinhTrang)
        {
            return new ThietBi(maThietBi, maSanPham, maKho)
            {
                TinhTrang = tinhTrang
            };
        }
    }
}
