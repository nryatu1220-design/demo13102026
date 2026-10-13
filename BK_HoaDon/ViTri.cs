using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TheLastTerm_Draft.Models
{
    [Table("VI_TRI")]
    public class ViTri
    {
        private string maViTri;
        private string tenViTri;

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

        [Column("TenViTri")]
        public string TenViTri
        {
            get { return tenViTri; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên vị trí không được để trống.");
                }

                if (!Regex.IsMatch(value, @"^[\p{L}\p{N} ]+$"))
                {
                    throw new ArgumentException("Tên vị trí không được chứa ký hiệu đặc biệt.");
                }

                tenViTri = value;
            }
        }


        public ViTri(string maViTri, string tenViTri)
        {
            MaViTri = maViTri;
            TenViTri = tenViTri;
        }
    }
}
