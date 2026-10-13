using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    [Table("SAN_PHAM_KHO")]
    public class SanPhamKho
    {
        private string maSanPham;
        private string maKho;
        private string maViTri;
        private int soLuongLyThuyet;
        private int soLuongThucTe;
        private List<ThietBi> thietBis = new List<ThietBi>();

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

        [Column("MaViTri")]
        public string MaViTri
        {
            get
            {
                return maViTri;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Mã vị trí không được để trống.");
                }
                if (!Regex.IsMatch(value, @"^VT\d+$"))
                {
                    throw new ArgumentException("Mã vị trí phải có dạng VT + số.");
                }
                maViTri = value;
            }
        }

        [Column("SoLuongLyThuyet")]
        public int SoLuongLyThuyet
        {
            get { return soLuongLyThuyet; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Số lượng lý thuyết phải lớn hơn hoặc bằng 0");
                }
                soLuongLyThuyet = value;
            }
        }

        [Column("SoLuongThucTe")]
        public int SoLuongThucTe
        {
            get { return soLuongThucTe; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Số lượng thực tế phải lớn hơn hoặc bằng 0");
                }
                soLuongThucTe = value;
            }
        }

        public List<ThietBi> ThietBis
        {
            get { return thietBis; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Giá trị danh sách thiết bị của kho không hợp lệ");
                thietBis = value;
            }
        }

        // Contructor dành cho việc tạo sản phẩm mới
        public SanPhamKho(string maSanPham, string maKho, string maViTri)
        {
            MaSanPham = maSanPham;
            MaKho = maKho;
            MaViTri = maViTri;
            //  SoLuongLyThuyet = 0;
            // SoLuongThucTe = 0;
            ThietBis = new List<ThietBi>();
        }

        // Contructor dành cho việc đổ cho database
        public static SanPhamKho FromDatabase(string maSanPham, string maKho, string maViTri,
            int soLuongLyThuyet, int soLuongThucTe)
        {
            return new SanPhamKho(maSanPham, maKho, maViTri)
            {
                SoLuongLyThuyet = soLuongLyThuyet,
                SoLuongThucTe = soLuongThucTe
            };
        }
    }
}
