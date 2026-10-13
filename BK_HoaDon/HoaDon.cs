using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    public enum TrangThai
    {
        Nhap = 0,
        XacNhan = 1,
        Huy = 2
    }
    [Table("HOA_DON")]
    public class HoaDon
    {
        private string maHoaDon;
        private string maNhanVien;
        private DateTime ngayTao;
        private decimal tongTien = 0;
        private TrangThai trangThai;
        private List<ChiTietHoaDon> chiTietHoaDons = new List<ChiTietHoaDon>();

        [Column("MaHoaDon")]
        public string MaHoaDon
        {
            get { return maHoaDon; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã hóa đơn không được để trống");

                if (!Regex.IsMatch(value, @"^HD\d+$"))
                    throw new ArgumentException("Mã hóa đơn phải có định dạng HD + số");

                maHoaDon = value;
            }
        }

        [Column("MaNhanVien")]
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

        [Column("NgayTao")]
        public DateTime NgayGioTao
        {
            get
            {
                return ngayTao;
            }
            private set
            {
                ngayTao = value;
            }
        }

        [Column("TongTien")]
        public decimal TongTien
        {
            get
            {
                // Nếu có danh sách chi tiết thì tính theo chi tiết, ngược lại lấy giá trị từ DB
                if (chiTietHoaDons != null && chiTietHoaDons.Any())
                {
                    return chiTietHoaDons.Sum(x => x.SoLuong * x.GiaBan);
                }
                return tongTien;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Tổng tiền hóa đơn không được âm.");
                tongTien = value;
            }
        }

        [Column("TrangThai")]
        public TrangThai TrangThai
        {
            get
            {
                return trangThai;
            }
            set
            {
                if (!Enum.IsDefined(typeof(TrangThai), value))
                {
                    throw new ArgumentException("Trạng thái của hóa đơn không hợp lệ");
                }
                trangThai = value;
            }
        }

        [ForeignKey("MaHoaDon")]
        public List<ChiTietHoaDon>ChiTietHoaDons
        {
            get
            {
                return chiTietHoaDons;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Danh sách chi tiết hóa đơn không hợp lệ");
                chiTietHoaDons = value;
            }
        } 

        public HoaDon(string maHoaDon, string maNhanVien)
        {
            MaHoaDon = maHoaDon;
            MaNhanVien = maNhanVien;
            NgayGioTao = DateTime.Now;
            TrangThai = TrangThai.Nhap;
            ChiTietHoaDons = new List<ChiTietHoaDon>();
        }

        public static HoaDon FromDatabase(string maHoaDon, string maNhanVien, DateTime ngayGioTao,
                                            decimal tongTien, TrangThai trangThai)
        {
            return new HoaDon(maHoaDon, maNhanVien)
            {
                NgayGioTao = ngayGioTao,
                TongTien = tongTien,
                TrangThai = trangThai
            };
        }
    }
}
