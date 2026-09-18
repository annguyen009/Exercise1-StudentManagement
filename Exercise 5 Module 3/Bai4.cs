using System;
using System.Collections.Generic;
using System.Linq;

namespace Bai4_QuanLySach
{
    // ===== Lớp Sach (trừu tượng) =====
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

        public abstract double GetThanhTien();

        public override string ToString()
        {
            return $"Mã sách: {MaSach} - Ngày nhập: {NgayNhap:dd/MM/yyyy} - Đơn giá: {DonGia:N0} " +
                   $"- Số lượng: {SoLuong} - NXB: {NhaXuatBan} - Thành tiền: {GetThanhTien():N0}";
        }
    }

    // ===== SachGiaoKhoa: tinhTrang (true = "mới", false = "cũ") =====
    class SachGiaoKhoa : Sach
    {
        public bool TinhTrang { get; set; } // true: mới, false: cũ

        public SachGiaoKhoa(string maSach, DateTime ngayNhap, double donGia, int soLuong,
                             string nhaXuatBan, bool tinhTrang)
            : base(maSach, ngayNhap, donGia, soLuong, nhaXuatBan)
        {
            TinhTrang = tinhTrang;
        }

        public override double GetThanhTien()
        {
            return TinhTrang ? SoLuong * DonGia : SoLuong * DonGia * 0.5;
        }

        public override string ToString()
        {
            string tt = TinhTrang ? "mới" : "cũ";
            return "[Giáo khoa] " + base.ToString() + $" - Tình trạng: {tt}";
        }
    }

    // ===== SachThamKhao: thue =====
    class SachThamKhao : Sach
    {
        public double Thue { get; set; }

        public SachThamKhao(string maSach, DateTime ngayNhap, double donGia, int soLuong,
                             string nhaXuatBan, double thue)
            : base(maSach, ngayNhap, donGia, soLuong, nhaXuatBan)
        {
            Thue = thue;
        }

        public override double GetThanhTien() => SoLuong * DonGia + Thue;

        public override string ToString() => "[Tham khảo] " + base.ToString() + $" - Thuế: {Thue:N0}";
    }

    // ===== DanhSachSach: lớp quản lý danh sách sách =====
    class DanhSachSach
    {
        private List<Sach> list;
        public int Count => list.Count;

        public DanhSachSach(int soLuongBanDau = 0)
        {
            list = new List<Sach>(soLuongBanDau);
        }

        // Thêm 1 sách vào danh sách, trả về true nếu thêm thành công (mã sách không trùng)
        public bool Them(Sach s)
        {
            if (list.Any(x => x.MaSach.Equals(s.MaSach, StringComparison.OrdinalIgnoreCase)))
                return false;
            list.Add(s);
            return true;
        }

        public override string ToString()
        {
            if (list.Count == 0) return "Danh sách rỗng.";
            return string.Join("\n", list.Select(s => s.ToString()));
        }

        public double TimTongThanhTienSGK() => list.OfType<SachGiaoKhoa>().Sum(s => s.GetThanhTien());

        public double TimTongThanhTienSTK() => list.OfType<SachThamKhao>().Sum(s => s.GetThanhTien());

        public List<Sach> TimSachGiaoKhoaTheoNXB(string nxb)
        {
            return list.OfType<SachGiaoKhoa>()
                        .Where(s => s.NhaXuatBan.Equals(nxb, StringComparison.OrdinalIgnoreCase))
                        .Cast<Sach>()
                        .ToList();
        }

        public double TimThanhTienCaoNhat()
        {
            if (list.Count == 0) return 0;
            return list.Max(s => s.GetThanhTien());
        }
    }

    class Program
    {
        static DanhSachSach ds = new DanhSachSach();

        static void Main()
        {
            NapDuLieuMau();

            bool tiepTuc = true;
            while (tiepTuc)
            {
                Console.WriteLine("\n===== MENU QUẢN LÝ SÁCH =====");
                Console.WriteLine("1. Thêm sách mới");
                Console.WriteLine("2. Hiển thị toàn bộ danh sách");
                Console.WriteLine("3. Tổng thành tiền sách giáo khoa");
                Console.WriteLine("4. Tổng thành tiền sách tham khảo");
                Console.WriteLine("5. Tìm sách giáo khoa theo nhà xuất bản");
                Console.WriteLine("6. Tìm thành tiền cao nhất");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");
                string? luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemSachMoi();
                        break;
                    case "2":
                        Console.WriteLine("\n" + ds.ToString());
                        break;
                    case "3":
                        Console.WriteLine($"Tổng thành tiền sách giáo khoa: {ds.TimTongThanhTienSGK():N0}");
                        break;
                    case "4":
                        Console.WriteLine($"Tổng thành tiền sách tham khảo: {ds.TimTongThanhTienSTK():N0}");
                        break;
                    case "5":
                        Console.Write("Nhập tên nhà xuất bản: ");
                        string? nxb = Console.ReadLine() ?? "";
                        var ketQua = ds.TimSachGiaoKhoaTheoNXB(nxb);
                        if (ketQua.Count == 0) Console.WriteLine("Không tìm thấy.");
                        else foreach (var s in ketQua) Console.WriteLine(s);
                        break;
                    case "6":
                        Console.WriteLine($"Thành tiền cao nhất: {ds.TimThanhTienCaoNhat():N0}");
                        break;
                    case "0":
                        tiepTuc = false;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }
            }
        }

        static void NapDuLieuMau()
        {
            ds.Them(new SachGiaoKhoa("SGK01", new DateTime(2026, 1, 10), 25_000, 100, "NXB Giáo Dục", true));
            ds.Them(new SachGiaoKhoa("SGK02", new DateTime(2025, 8, 20), 30_000, 50, "NXB Giáo Dục", false));
            ds.Them(new SachThamKhao("STK01", new DateTime(2026, 3, 1), 45_000, 40, "NXB Trẻ", 50_000));
            ds.Them(new SachThamKhao("STK02", new DateTime(2025, 11, 5), 60_000, 20, "NXB Kim Đồng", 30_000));
        }

        static void ThemSachMoi()
        {
            Console.Write("Loại sách (1: Giáo khoa, 2: Tham khảo): ");
            string? loai = Console.ReadLine();
            Console.Write("Mã sách: "); string maSach = Console.ReadLine() ?? "";
            Console.Write("Đơn giá: "); double.TryParse(Console.ReadLine(), out double donGia);
            Console.Write("Số lượng: "); int.TryParse(Console.ReadLine(), out int soLuong);
            Console.Write("Nhà xuất bản: "); string nxb = Console.ReadLine() ?? "";

            Sach? sachMoi = null;
            if (loai == "1")
            {
                Console.Write("Tình trạng (1: mới, 0: cũ): ");
                bool tinhTrang = Console.ReadLine() == "1";
                sachMoi = new SachGiaoKhoa(maSach, DateTime.Now, donGia, soLuong, nxb, tinhTrang);
            }
            else if (loai == "2")
            {
                Console.Write("Thuế: "); double.TryParse(Console.ReadLine(), out double thue);
                sachMoi = new SachThamKhao(maSach, DateTime.Now, donGia, soLuong, nxb, thue);
            }
            else
            {
                Console.WriteLine("Loại sách không hợp lệ.");
                return;
            }

            bool ok = ds.Them(sachMoi);
            Console.WriteLine(ok ? "Thêm sách thành công!" : "Thêm thất bại: mã sách đã tồn tại!");
        }
    }
}
