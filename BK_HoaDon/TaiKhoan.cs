using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    public enum TinhTrangLaoDong
    {
        [Display (Name = "Nghỉ làm")]
        NghiLam = 0,
        [Display(Name = "Còn làm")]
        ConLam = 1
    }

    public enum TrangThaiHoatDong
    {
        [Display(Name = "Đăng nhập")] 
        DangXuat = 0,
        [Display(Name = "Đăng xuất")]
        DangNhap = 1
    }
    public class TaiKhoan
    {
        private string maNhanVien;
        private string tenDangNhap;
        private string matKhau;
        private TinhTrangLaoDong tinhTrangLaoDong;  
        private TrangThaiHoatDong trangThaiHoatDong;

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

        public string TenDangNhap
        {
            get
            {
                return tenDangNhap;
            }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên đăng nhập không được để trống.");
                }

                tenDangNhap = value;
            }
        }

        public string MatKhau
        {
            get
            {
                return matKhau;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mật khẩu không được để trống.");
                }

                // Nếu là mật khẩu đã hash từ database
                if (value.StartsWith("$2a$") ||
                    value.StartsWith("$2b$") ||
                    value.StartsWith("$2y$"))
                {
                    matKhau = value;
                    return;
                }

                // Nếu là mật khẩu người dùng nhập vào
                if (value.Length < 8)
                {
                    throw new ArgumentException("Mật khẩu phải có ít nhất 8 ký tự.");
                }

                if (value.Contains(' '))
                {
                    throw new ArgumentException("Mật khẩu không được chứa khoảng trắng.");
                }

                if (!Regex.IsMatch(value, @"[A-Z]"))
                {
                    throw new ArgumentException(
                        "Mật khẩu phải có ít nhất một chữ cái viết hoa.");
                }

                if (!Regex.IsMatch(value, @"[a-z]"))
                {
                    throw new ArgumentException(
                        "Mật khẩu phải có ít nhất một chữ cái viết thường.");
                }

                if (!Regex.IsMatch(value, @"\d"))
                {
                    throw new ArgumentException(
                        "Mật khẩu phải có ít nhất một chữ số.");
                }

                if (!Regex.IsMatch(value, @"[^a-zA-Z0-9\s]"))
                {
                    throw new ArgumentException(
                        "Mật khẩu phải có ít nhất một ký tự đặc biệt.");
                }

                matKhau = value;
            }
        }


        public TinhTrangLaoDong TinhTrangLaoDong
        {
            get
            {
                return tinhTrangLaoDong;
            }
            set
            {
                if(!Enum.IsDefined(typeof(TinhTrangLaoDong), value))
                {
                    throw new ArgumentException("Giá trị tình trạng lao động không hợp lệ");
                }
                tinhTrangLaoDong = value;
            }
        }

        public TrangThaiHoatDong TrangThaiHoatDong
        {
            get
            {
                return trangThaiHoatDong;
            }
            set
            {
                if (!Enum.IsDefined(typeof(TrangThaiHoatDong), value))
                {
                    throw new ArgumentException("Giá trị trạng thái hoạt động không hợp lệ");
                }
                trangThaiHoatDong = value;
            }
        }

        public TaiKhoan(string maNhanVien, string tenDangNhap, string matKhau)
        {
            MaNhanVien = maNhanVien;
            TenDangNhap = tenDangNhap;
            MatKhau = matKhau;
            TinhTrangLaoDong = TinhTrangLaoDong.ConLam;
            TrangThaiHoatDong = TrangThaiHoatDong.DangXuat;
        }

        public static TaiKhoan FromDatabase( // contructor dành cho đổ dữ liệu từ database
            string maNhanVien,
            string tenDangNhap,
            string matKhau,
             TinhTrangLaoDong tinhTrangLaoDong,
                 TrangThaiHoatDong trangThaiHoatDong)
        {
            return new TaiKhoan(maNhanVien, tenDangNhap, matKhau)
            {
                TinhTrangLaoDong = tinhTrangLaoDong,
                TrangThaiHoatDong = trangThaiHoatDong
            };
        }
    }
}
