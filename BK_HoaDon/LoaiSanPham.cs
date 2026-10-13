using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    [Table("LOAI_SAN_PHAM")]
    public class LoaiSanPham
    {
        private string tenLoaiSanPham;

        [Column("TenLoaiSanPham")]
        public string TenLoaiSanPham
        {
            get
            {
                return tenLoaiSanPham;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên loại sản phẩm không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L}\p{N} ]+$"))
                {
                    throw new ArgumentException("Tên loại sản phẩm không được chứa ký hiệu đặc biệt.");
                }
                tenLoaiSanPham = value;
            }
        }

        public LoaiSanPham(string tenLoaiSanPham)
        {
            TenLoaiSanPham = tenLoaiSanPham;
        }
    }
}
