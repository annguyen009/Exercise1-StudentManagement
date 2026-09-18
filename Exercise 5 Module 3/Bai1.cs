using System;
using System.Collections.Generic;
using System.Linq;

namespace Bai1_ChuyenXe
{
    // Lớp cha (trừu tượng): các thông tin và hành vi chung của mọi chuyến xe
    abstract class ChuyenXe
    {
        public string MaSoChuyen { get; set; }
        public string HoTenTaiXe { get; set; }
        public string SoXe { get; set; }
        public double DoanhThu { get; set; }

        protected ChuyenXe(string maSoChuyen, string hoTenTaiXe, string soXe, double doanhThu)
        {
            MaSoChuyen = maSoChuyen;
            HoTenTaiXe = hoTenTaiXe;
            SoXe = soXe;
            DoanhThu = doanhThu;
        }

        // Mỗi loại chuyến xe hiển thị thông tin theo cách riêng => trừu tượng
        public abstract override string ToString();
    }

    // Chuyến xe nội thành: Mã số chuyến, Họ tên tài xế, số xe, số tuyến, số km đi được, doanh thu
    class ChuyenXeNoiThanh : ChuyenXe
    {
        public string SoTuyen { get; set; }
        public double SoKmDiDuoc { get; set; }

        public ChuyenXeNoiThanh(string maSoChuyen, string hoTenTaiXe, string soXe,
                                 string soTuyen, double soKmDiDuoc, double doanhThu)
            : base(maSoChuyen, hoTenTaiXe, soXe, doanhThu)
        {
            SoTuyen = soTuyen;
            SoKmDiDuoc = soKmDiDuoc;
        }

        public override string ToString()
        {
            return $"[Nội thành] Mã chuyến: {MaSoChuyen} - Tài xế: {HoTenTaiXe} - Số xe: {SoXe} " +
                   $"- Tuyến: {SoTuyen} - Số km: {SoKmDiDuoc} - Doanh thu: {DoanhThu:N0}";
        }
    }

    // Chuyến xe ngoại thành: Mã số chuyến, Họ tên tài xế, số xe, nơi đến, số ngày đi được, doanh thu
    class ChuyenXeNgoaiThanh : ChuyenXe
    {
        public string NoiDen { get; set; }
        public int SoNgayDiDuoc { get; set; }

        public ChuyenXeNgoaiThanh(string maSoChuyen, string hoTenTaiXe, string soXe,
                                   string noiDen, int soNgayDiDuoc, double doanhThu)
            : base(maSoChuyen, hoTenTaiXe, soXe, doanhThu)
        {
            NoiDen = noiDen;
            SoNgayDiDuoc = soNgayDiDuoc;
        }

        public override string ToString()
        {
            return $"[Ngoại thành] Mã chuyến: {MaSoChuyen} - Tài xế: {HoTenTaiXe} - Số xe: {SoXe} " +
                   $"- Nơi đến: {NoiDen} - Số ngày: {SoNgayDiDuoc} - Doanh thu: {DoanhThu:N0}";
        }
    }

    class Program
    {
        static void Main()
        {
            // Tạo sẵn mỗi loại 2 chuyến xe vào danh sách (không nhập từ bàn phím)
            List<ChuyenXe> danhSach = new List<ChuyenXe>
            {
                new ChuyenXeNoiThanh("NT01", "Nguyễn Văn A", "51A-12345", "Tuyến 01", 15.5, 350_000),
                new ChuyenXeNoiThanh("NT02", "Trần Thị B", "51A-67890", "Tuyến 05", 22.0, 480_000),
                new ChuyenXeNgoaiThanh("NG01", "Lê Văn C", "51B-11111", "Vũng Tàu", 2, 2_500_000),
                new ChuyenXeNgoaiThanh("NG02", "Phạm Thị D", "51B-22222", "Đà Lạt", 3, 4_200_000),
            };

            Console.WriteLine("===== DANH SÁCH TẤT CẢ CÁC CHUYẾN XE =====");
            foreach (var cx in danhSach)
                Console.WriteLine(cx);

            double tongDoanhThuNoiThanh = danhSach.OfType<ChuyenXeNoiThanh>().Sum(c => c.DoanhThu);
            double tongDoanhThuNgoaiThanh = danhSach.OfType<ChuyenXeNgoaiThanh>().Sum(c => c.DoanhThu);

            Console.WriteLine("\n===== TỔNG DOANH THU TỪNG LOẠI CHUYẾN XE =====");
            Console.WriteLine($"Tổng doanh thu chuyến xe nội thành:  {tongDoanhThuNoiThanh:N0}");
            Console.WriteLine($"Tổng doanh thu chuyến xe ngoại thành: {tongDoanhThuNgoaiThanh:N0}");
        }
    }
}
