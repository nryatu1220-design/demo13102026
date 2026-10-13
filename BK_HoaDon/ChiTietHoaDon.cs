using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    [PrimaryKey(nameof(MaHoaDon), nameof(MaSanPham))]
    public class ChiTietHoaDon
    {
        private string maHoaDon;
        private string maSanPham;
        private int soLuong;
        private decimal giaBan;

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

        public int SoLuong
        {
            get
            {
                return soLuong;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số lượng bán không được bé hơn 1");
                soLuong = value;
            }
        }

        public decimal GiaBan
        {
            get
            {
                return giaBan;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Tiền bán không được bé hơn hoặc bằng 0");

                giaBan = value;
            }
        }

        public ChiTietHoaDon(string maHoaDon, string maSanPham, int soLuong, decimal giaBan)
        {
            MaHoaDon = maHoaDon;
            MaSanPham = maSanPham;
            SoLuong = soLuong;
            GiaBan = giaBan;
        }

    }
}
