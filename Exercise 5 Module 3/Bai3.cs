using System;
using System.Collections.Generic;
using System.Linq;

namespace Bai3_GiaoDich
{
    // Lớp cha trừu tượng
    abstract class GiaoDich
    {
        public string MaGiaoDich { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public double DonGia { get; set; }
        public double SoLuong { get; set; }

        protected GiaoDich(string maGiaoDich, DateTime ngayGiaoDich, double donGia, double soLuong)
        {
            MaGiaoDich = maGiaoDich;
            NgayGiaoDich = ngayGiaoDich;
            DonGia = donGia;
            SoLuong = soLuong;
        }

        public abstract double ThanhTien();

        public override string ToString()
        {
            return $"Mã GD: {MaGiaoDich} - Ngày: {NgayGiaoDich:dd/MM/yyyy} - Đơn giá: {DonGia:N0} " +
                   $"- Số lượng: {SoLuong} - Thành tiền: {ThanhTien():N0}";
        }
    }

    // Giao dịch vàng: thành tiền = số lượng * đơn giá
    class GiaoDichVang : GiaoDich
    {
        public string LoaiVang { get; set; }

        public GiaoDichVang(string maGiaoDich, DateTime ngayGiaoDich, double donGia, double soLuong, string loaiVang)
            : base(maGiaoDich, ngayGiaoDich, donGia, soLuong)
        {
            LoaiVang = loaiVang;
        }

        public override double ThanhTien() => SoLuong * DonGia;

        public override string ToString() => "[Vàng] " + base.ToString() + $" - Loại vàng: {LoaiVang}";
    }

    enum LoaiTienTe { VND, USD, EUR }

    // Giao dịch tiền tệ:
    // - USD hoặc EUR: thành tiền = số lượng * đơn giá * tỉ giá
    // - VND: thành tiền = số lượng * đơn giá
    class GiaoDichTienTe : GiaoDich
    {
        public double TiGia { get; set; }
        public LoaiTienTe LoaiTien { get; set; }

        public GiaoDichTienTe(string maGiaoDich, DateTime ngayGiaoDich, double donGia, double soLuong,
                               double tiGia, LoaiTienTe loaiTien)
            : base(maGiaoDich, ngayGiaoDich, donGia, soLuong)
        {
            TiGia = tiGia;
            LoaiTien = loaiTien;
        }

        public override double ThanhTien()
        {
            if (LoaiTien == LoaiTienTe.USD || LoaiTien == LoaiTienTe.EUR)
                return SoLuong * DonGia * TiGia;
            return SoLuong * DonGia; // VND
        }

        public override string ToString() =>
            "[Tiền tệ] " + base.ToString() + $" - Loại tiền: {LoaiTien} - Tỉ giá: {TiGia:N2}";
    }

    class Program
    {
        static void Main()
        {
            // Tạo sẵn mỗi loại 3 giao dịch vào danh sách (không nhập từ bàn phím)
            List<GiaoDich> danhSach = new List<GiaoDich>
            {
                new GiaoDichVang("GDV01", new DateTime(2026, 5, 1), 76_500_000, 2, "SJC"),
                new GiaoDichVang("GDV02", new DateTime(2026, 5, 3), 76_200_000, 1.5, "9999"),
                new GiaoDichVang("GDV03", new DateTime(2026, 5, 5), 76_800_000, 3, "SJC"),

                new GiaoDichTienTe("GDT01", new DateTime(2026, 5, 2), 25_000, 500, 1.0, LoaiTienTe.VND),
                new GiaoDichTienTe("GDT02", new DateTime(2026, 5, 4), 1.0, 2000, 25_450, LoaiTienTe.USD),
                new GiaoDichTienTe("GDT03", new DateTime(2026, 5, 6), 1.0, 1500, 27_300, LoaiTienTe.EUR),
            };

            Console.WriteLine("===== DANH SÁCH TẤT CẢ GIAO DỊCH =====");
            foreach (var gd in danhSach)
                Console.WriteLine(gd);

            // a) Tổng số lượng cho từng loại
            double tongSLVang = danhSach.OfType<GiaoDichVang>().Sum(g => g.SoLuong);
            double tongSLTienTe = danhSach.OfType<GiaoDichTienTe>().Sum(g => g.SoLuong);
            Console.WriteLine("\n===== TỔNG SỐ LƯỢNG TỪNG LOẠI =====");
            Console.WriteLine($"Tổng số lượng giao dịch vàng:   {tongSLVang:N2}");
            Console.WriteLine($"Tổng số lượng giao dịch tiền tệ: {tongSLTienTe:N2}");

            // b) Trung bình thành tiền của giao dịch tiền tệ
            double tbThanhTienTienTe = danhSach.OfType<GiaoDichTienTe>().Average(g => g.ThanhTien());
            Console.WriteLine($"\nTrung bình thành tiền của giao dịch tiền tệ: {tbThanhTienTienTe:N0}");

            // c) Các giao dịch có đơn giá > 1 tỷ
            Console.WriteLine("\n===== GIAO DỊCH CÓ ĐƠN GIÁ > 1 TỶ =====");
            var giaoDichLon = danhSach.Where(g => g.DonGia > 1_000_000_000).ToList();
            if (giaoDichLon.Count == 0)
                Console.WriteLine("Không có giao dịch nào có đơn giá vượt 1 tỷ.");
            else
                foreach (var gd in giaoDichLon)
                    Console.WriteLine(gd);
        }
    }
}
