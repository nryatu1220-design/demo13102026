using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    [Table("KHO")]
    public class Kho
    {
        private string maKho;
        private string tenDuong;
        private string tenQuan;
        private string tenTinh;
        private string? maKhoChiuTrachNhiem;

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

        [Column("TenDuong")]

        public string TenDuong
        {
            get
            {
                return tenDuong;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên đường không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L}\p{N} ]+$"))
                {
                    throw new ArgumentException("Tên đường không được chứa ký hiệu đặc biệt.");
                }

                tenDuong = value;
            }
        }

        [Column("TenQuan")]

        public string TenQuan
        {
            get
            {
                return tenQuan;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên quận không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L}\p{N} ]+$"))
                {
                    throw new ArgumentException("Tên quận không được chứa ký hiệu đặc biệt.");
                }

                tenQuan = value;
            }
        }

        [Column("TenTinh")]
        public string TenTinh
        {
            get
            {
                return tenTinh;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên tỉnh không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L}\p{N} ]+$"))
                {
                    throw new ArgumentException("Tên tỉnh không được chứa ký hiệu đặc biệt.");
                }

                tenTinh = value;
            }
        
        }

        public string? MaKhoChiuTrachNhiem
        {
            get
            {
                return maKhoChiuTrachNhiem;
            }
            set
            {
                if (value is null) // Nếu đó là kho trung tâm
                {
                    maKhoChiuTrachNhiem = null;
                    return;
                }

                if (!Regex.IsMatch(value, @"^KH\d+$")) // Nếu là kho vệ tinh 
                {
                    throw new ArgumentException("Mã kho phải có dạng KH + số.");
                }

                maKhoChiuTrachNhiem = value;
            }
        }

        public Kho (string maKho, string tenDuong, string tenQuan, string tenTinh, string? maKhoChiuTrachNhiem)
        {
            MaKho = maKho;
            TenDuong = tenDuong;
            TenQuan = tenQuan;
            TenTinh = tenTinh;
            MaKhoChiuTrachNhiem = maKhoChiuTrachNhiem;
        }

    }
}
