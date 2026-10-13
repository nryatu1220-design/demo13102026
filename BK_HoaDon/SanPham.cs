using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    [Table("SAN_PHAM")]
    public class SanPham
    {
        private string maSanPham;
        private string tenSanPham;
        //private string tenLoaiSanPham;
        private string tenLoaiSanPham = "Chưa phân loại";
        private string moTaSanPham;
        private decimal tienNhapVe;
        private decimal tienBan;

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

        [Column("TenSanPham")]
        public string TenSanPham
        {
            get
            {
                return tenSanPham;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên sản phẩm không được để trống.");
                }

                if(!Regex.IsMatch(value, @"^[\p{L}\p{N} \-_/().,+#]+$"))
        {
                    throw new ArgumentException("Tên sản phẩm không được chứa ký hiệu đặc biệt.");
                }

                tenSanPham = value;
            }
        }

        [Column("TenLoaiSanPham")]
        public string TenLoaiSanPham
        {
            get
            {
                return tenLoaiSanPham;
            }
            set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //{
                //    //throw new ArgumentException("Tên loại sản phẩm không được để trống.");
                //    tenLoaiSanPham = "Chưa phân loại";
                //}

                //if (!Regex.IsMatch(value, @"^[\p{L}\p{N} \-_/().,+#]+$"))
                //{
                //    throw new ArgumentException("Tên loại sản phẩm không được chứa ký hiệu đặc biệt.");
                //}

                tenSanPham = value;
            }
        }

        [Column("MoTaSanPham")]
        public string MoTaSanPham
        {
            get
            {
                return moTaSanPham;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mô tả sản phẩm không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L}\p{N} :]+$"))
                {
                    throw new ArgumentException("Mô tả không được chứa ký hiệu đặc biệt.");
                }

                moTaSanPham = value;
            }
        }

        [Column("TienNhapVe")]
        public decimal TienNhapVe
        {
            get
            {
                return tienNhapVe;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Tiền nhập về không được bé hơn hoặc bằng 0");

                tienNhapVe = value;
            }
        }

        [Column("TienBan")]
        public decimal TienBan
        {
            get
            {
                return tienBan;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Tiền bán không được bé hơn hoặc bằng 0");

                tienBan = value;
            }
        }

        public SanPham(string maSanPham, string tenSanPham, string tenLoaiSanPham, string moTaSanPham,
                         decimal tienNhapVe, decimal tienBan)
        {
            MaSanPham = maSanPham;
            TenSanPham = tenSanPham;
            TenLoaiSanPham = tenLoaiSanPham;
            MoTaSanPham = moTaSanPham;
            TienNhapVe = tienNhapVe;
            TienBan = tienBan;
        }
    }
}
