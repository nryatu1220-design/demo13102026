using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    public enum RoleCode
    {
        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "Quản lý kho")]
        QuanLyKho = 2,

        [Display(Name = "Nhân viên kho")]
        NhanVienKho = 3,

        [Display(Name = "Nhân viên bán hàng")]
        NhanVienBanHang = 4
    }
    public class NhanVien
    {
        private string maNhanVien;
        private string hoTen;
        private string gioiTinh;
        private DateTime ngaySinh;
        private string soDienThoai;
        private string email;
        private decimal luong;
        private RoleCode roleCode;
        private string? maKho;

        public string MaNhanVien
        {
            get
            {
                return maNhanVien;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mã nhân viên không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^NV\d+$"))
                {
                    throw new ArgumentException("Mã nhân viên phải có dạng NV + số.");
                }

                maNhanVien = value;
            }
        }

        public string HoTen
        {
            get
            {
                return hoTen;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Họ tên không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L} ]+$"))
                {
                    throw new ArgumentException("Họ tên chỉ được chứa chữ cái và khoảng trắng.");
                }

                hoTen = value;
            }
        }

        public string GioiTinh
        {
            get
            {
                return gioiTinh;
            }
            set
            {
                if (value != "Nam" && value != "Nữ")
                {
                    throw new ArgumentException("Giới tính chỉ được là Nam hoặc Nữ.");
                }

                gioiTinh = value;
            }
        }

        public DateTime NgaySinh
        {
            get
            {
                return ngaySinh;
            }
            set
            {
                DateTime ngayHienTai = DateTime.Now;

                int tuoi = ngayHienTai.Year - value.Year;

                // Kiểm tra ngày sinh lớn hơn ngày hiện tại thì trừ bớt tuổi 1 đơn vị
                if (value.Date > ngayHienTai.AddYears(-tuoi).Date)
                {
                    tuoi--;
                }

                if (tuoi < 18)
                {
                    throw new ArgumentException("Nhân viên phải đủ 18 tuổi.");
                }

                ngaySinh = value;
            }
        }

        public string SoDienThoai
        {
            get
            {
                return soDienThoai;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Số điện thoại không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^\d{10}$"))
                {
                    throw new ArgumentException("Số điện thoại phải có đúng 10 chữ số.");
                }

                soDienThoai = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Email không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new ArgumentException("Email không đúng định dạng.");
                }

                email = value;
            }
        }

        public decimal Luong
        {
            get
            {
                return luong;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Lương không được âm.");
                }

                luong = value;
            }
        }

        public RoleCode RoleCode
        {
            get
            {
                return roleCode;
            }
            set
            {
                if (!Enum.IsDefined(typeof(RoleCode),value))
                {
                    throw new ArgumentException("Giá trị role code không hợp lệ");
                }

                roleCode = value;
            }
        }

        public string? MaKho
        {
            get
            {
                return maKho;
            }
            set
            {
                if (value is null) // Nếu đó là admin thì không thuộc về một kho nhất định
                {
                    maKho = null;
                    return;
                }

                if (!Regex.IsMatch(value, @"^KH\d+$")) // Nếu là nhân viên
                {
                    throw new ArgumentException("Mã kho phải có dạng KH + số.");
                }

                maKho = value;
            }
        }

        public NhanVien(string maNhanVien, string hoTen, string gioiTinh,
            DateTime ngaySinh, string soDienThoai, string email, 
            decimal luong, RoleCode roleCode, string? maKho)
        {
            MaNhanVien = maNhanVien;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            SoDienThoai = soDienThoai;
            Email = email;
            Luong = luong;
            RoleCode = roleCode;
            MaKho = maKho;

        }
    }
}
