using System;
using System.Collections.Generic;
using System.Linq;

namespace Bai2_ThuVien
{
    // Lớp cha trừu tượng: các thuộc tính chung của mọi loại sách
    abstract class Sach
    {
        public string MaSach { get; set; }
        public DateTime NgayNhap { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public string NhaXuatBan { get; set; }

        protected Sach(string maSach, DateTime ngayNhap, double donGia, int soLuong, string nhaXuatBan)
        {
            MaSach = maSach;
            NgayNhap = ngayNhap;
            DonGia = donGia;
            SoLuong = soLuong;
            NhaXuatBan = nhaXuatBan;
        }

        // Cách tính thành tiền khác nhau tùy loại sách => trừu tượng
        public abstract double ThanhTien();

        public override string ToString()
        {
            return $"Mã sách: {MaSach} - Ngày nhập: {NgayNhap:dd/MM/yyyy} - Đơn giá: {DonGia:N0} " +
                   $"- Số lượng: {SoLuong} - NXB: {NhaXuatBan} - Thành tiền: {ThanhTien():N0}";
        }
    }

    // Sách giáo khoa: có thêm tình trạng "mới" / "cũ"
    class SachGiaoKhoa : Sach
    {
        public string TinhTrang { get; set; } // "mới" hoặc "cũ"

        public SachGiaoKhoa(string maSach, DateTime ngayNhap, double donGia, int soLuong,
                             string nhaXuatBan, string tinhTrang)
            : base(maSach, ngayNhap, donGia, soLuong, nhaXuatBan)
        {
            TinhTrang = tinhTrang;
        }

        public override double ThanhTien()
        {
            // Nếu "mới": thành tiền = số lượng * đơn giá
            // Nếu "cũ" : thành tiền = số lượng * đơn giá * 50%
            if (TinhTrang.Equals("mới", StringComparison.OrdinalIgnoreCase))
                return SoLuong * DonGia;
            return SoLuong * DonGia * 0.5;
        }

        public override string ToString()
        {
            return "[Giáo khoa] " + base.ToString() + $" - Tình trạng: {TinhTrang}";
        }
    }

    // Sách tham khảo: có thêm thuế
    class SachThamKhao : Sach
    {
        public double Thue { get; set; }

        public SachThamKhao(string maSach, DateTime ngayNhap, double donGia, int soLuong,
                             string nhaXuatBan, double thue)
            : base(maSach, ngayNhap, donGia, soLuong, nhaXuatBan)
        {
            Thue = thue;
        }

        // Thành tiền = số lượng * đơn giá + thuế
        public override double ThanhTien() => SoLuong * DonGia + Thue;

        public override string ToString()
        {
            return "[Tham khảo] " + base.ToString() + $" - Thuế: {Thue:N0}";
        }
    }

    class Program
    {
        static void Main()
        {
            // Tạo sẵn mỗi loại 3 cuốn sách vào danh sách (không nhập từ bàn phím)
            List<Sach> danhSach = new List<Sach>
            {
                new SachGiaoKhoa("SGK01", new DateTime(2026, 1, 10), 25_000, 100, "NXB Giáo Dục", "mới"),
                new SachGiaoKhoa("SGK02", new DateTime(2025, 8, 20), 30_000, 50, "NXB Giáo Dục", "cũ"),
                new SachGiaoKhoa("SGK03", new DateTime(2026, 2, 15), 22_000, 80, "NXB Đại Học Quốc Gia", "mới"),

                new SachThamKhao("STK01", new DateTime(2026, 3, 1), 45_000, 40, "NXB Trẻ", 50_000),
                new SachThamKhao("STK02", new DateTime(2025, 11, 5), 60_000, 20, "NXB Kim Đồng", 30_000),
                new SachThamKhao("STK03", new DateTime(2026, 4, 12), 55_000, 35, "NXB Trẻ", 45_000),
            };

            // a) Tính tổng thành tiền cho từng loại
            double tongSGK = danhSach.OfType<SachGiaoKhoa>().Sum(s => s.ThanhTien());
            double tongSTK = danhSach.OfType<SachThamKhao>().Sum(s => s.ThanhTien());
            Console.WriteLine("===== TỔNG THÀNH TIỀN TỪNG LOẠI =====");
            Console.WriteLine($"Tổng thành tiền sách giáo khoa: {tongSGK:N0}");
            Console.WriteLine($"Tổng thành tiền sách tham khảo: {tongSTK:N0}");

            // b) Xuất ra các sách giáo khoa của nhà xuất bản K (yêu cầu nhập K)
            Console.Write("\nNhập tên nhà xuất bản K cần tra cứu sách giáo khoa: ");
            string? nxbK = Console.ReadLine();
            var sachTheoNXB = danhSach.OfType<SachGiaoKhoa>()
                                       .Where(s => string.Equals(s.NhaXuatBan, nxbK, StringComparison.OrdinalIgnoreCase))
                                       .ToList();
            Console.WriteLine($"===== SÁCH GIÁO KHOA CỦA NXB \"{nxbK}\" =====");
            if (sachTheoNXB.Count == 0)
                Console.WriteLine("Không tìm thấy sách giáo khoa nào của nhà xuất bản này.");
            else
                foreach (var s in sachTheoNXB)
                    Console.WriteLine(s);

            // c) Tìm thành tiền cao nhất
            Sach sachCaoNhat = danhSach.OrderByDescending(s => s.ThanhTien()).First();
            Console.WriteLine("\n===== SÁCH CÓ THÀNH TIỀN CAO NHẤT =====");
            Console.WriteLine(sachCaoNhat);
        }
    }
}
