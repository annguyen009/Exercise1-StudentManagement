using System;
using System.Collections.Generic;
using System.Linq;

namespace Bai6_HangHoa
{
    // =====================================================================
    // a) Thiết kế mô hình lớp:
    //    - HangHoa: lớp trừu tượng (abstract) chứa thuộc tính/phương thức chung
    //    - ThucPham, DienMay, SanhSu: các lớp cụ thể kế thừa từ HangHoa
    //    - TinhVAT() và DanhGiaBanBuon() là phương thức trừu tượng vì cách
    //      tính/đánh giá khác nhau hoàn toàn giữa từng loại hàng hóa
    // =====================================================================

    abstract class HangHoa
    {
        // Mã hàng: không được sửa, không được để trống -> chỉ có getter, gán ở constructor
        public string MaHang { get; }
        // Tên hàng: không được sửa -> chỉ có getter, mặc định "xxx" nếu để trống
        public string TenHang { get; }

        private double donGia;
        public double DonGia
        {
            get => donGia;
            set => donGia = value < 0 ? 0 : value;
        }

        private int soLuongTon;
        public int SoLuongTon
        {
            get => soLuongTon;
            set => soLuongTon = value < 0 ? 0 : value;
        }

        protected HangHoa(string maHang, string tenHang, double donGia, int soLuongTon)
        {
            if (string.IsNullOrWhiteSpace(maHang))
                throw new ArgumentException("Mã hàng không được để trống!");
            MaHang = maHang;
            TenHang = string.IsNullOrWhiteSpace(tenHang) ? "xxx" : tenHang;
            DonGia = donGia;
            SoLuongTon = soLuongTon;
        }

        // Số tiền VAT của loại hàng hóa này (5% thực phẩm, 10% điện máy & sành sứ)
        public abstract double TinhVAT();

        // Đánh giá mức độ bán buôn: mỗi loại hàng có tiêu chí riêng
        public abstract string DanhGiaBanBuon();

        public override string ToString()
        {
            return $"Mã hàng: {MaHang,-8} Tên hàng: {TenHang,-15} Đơn giá: {DonGia,12:N0} " +
                   $"Tồn: {SoLuongTon,5} VAT: {TinhVAT(),12:N0}  Đánh giá: {DanhGiaBanBuon()}";
        }
    }

    // ================= HÀNG THỰC PHẨM =================
    class ThucPham : HangHoa
    {
        public string NhaCungCap { get; set; }

        private DateTime ngaySanXuat;
        public DateTime NgaySanXuat
        {
            get => ngaySanXuat;
            set => ngaySanXuat = value < DateTime.Now ? value : DateTime.Now;
        }

        private DateTime hanSuDung;
        public DateTime HanSuDung
        {
            get => hanSuDung;
            set => hanSuDung = value > ngaySanXuat ? value : ngaySanXuat;
        }

        public ThucPham(string maHang, string tenHang, double donGia, int soLuongTon,
                         string nhaCungCap, DateTime ngaySanXuat, DateTime hanSuDung)
            : base(maHang, tenHang, donGia, soLuongTon)
        {
            NhaCungCap = nhaCungCap;
            NgaySanXuat = ngaySanXuat;
            HanSuDung = hanSuDung;
        }

        // VAT hàng thực phẩm = 5%
        public override double TinhVAT() => DonGia * SoLuongTon * 0.05;

        // Còn tồn kho và đã hết hạn sử dụng => khó bán
        public override string DanhGiaBanBuon()
        {
            if (SoLuongTon > 0 && HanSuDung < DateTime.Now)
                return "Khó bán";
            return "Không đánh giá";
        }

        public override string ToString()
        {
            return "[Thực phẩm] " + base.ToString() +
                   $"  NCC: {NhaCungCap}  NSX: {NgaySanXuat:dd/MM/yyyy}  HSD: {HanSuDung:dd/MM/yyyy}";
        }
    }

    // ================= HÀNG ĐIỆN MÁY =================
    class DienMay : HangHoa
    {
        private int thoiGianBaoHanh;
        public int ThoiGianBaoHanh
        {
            get => thoiGianBaoHanh;
            set => thoiGianBaoHanh = value < 0 ? 0 : value;
        }

        private double congSuat;
        public double CongSuat
        {
            get => congSuat;
            set => congSuat = value < 0 ? 0 : value;
        }

        public DienMay(string maHang, string tenHang, double donGia, int soLuongTon,
                        int thoiGianBaoHanh, double congSuat)
            : base(maHang, tenHang, donGia, soLuongTon)
        {
            ThoiGianBaoHanh = thoiGianBaoHanh;
            CongSuat = congSuat;
        }

        // VAT hàng điện máy = 10%
        public override double TinhVAT() => DonGia * SoLuongTon * 0.10;

        // Số lượng tồn < 3 => bán được
        public override string DanhGiaBanBuon()
        {
            if (SoLuongTon < 3)
                return "Bán được";
            return "Không đánh giá";
        }

        public override string ToString()
        {
            return "[Điện máy] " + base.ToString() +
                   $"  Bảo hành: {ThoiGianBaoHanh} tháng  Công suất: {CongSuat} KW";
        }
    }

    // ================= HÀNG SÀNH SỨ =================
    class SanhSu : HangHoa
    {
        public string NhaSanXuat { get; set; }

        private DateTime ngayNhapKho;
        public DateTime NgayNhapKho
        {
            get => ngayNhapKho;
            set => ngayNhapKho = value < DateTime.Now ? value : DateTime.Now;
        }

        public SanhSu(string maHang, string tenHang, double donGia, int soLuongTon,
                       string nhaSanXuat, DateTime ngayNhapKho)
            : base(maHang, tenHang, donGia, soLuongTon)
        {
            NhaSanXuat = nhaSanXuat;
            NgayNhapKho = ngayNhapKho;
        }

        // VAT hàng sành sứ = 10%
        public override double TinhVAT() => DonGia * SoLuongTon * 0.10;

        // Tồn kho > 50 hoặc thời gian lưu kho > 10 ngày => bán chậm
        public override string DanhGiaBanBuon()
        {
            int soNgayLuuKho = (DateTime.Now - NgayNhapKho).Days;
            if (SoLuongTon > 50 || soNgayLuuKho > 10)
                return "Bán chậm";
            return "Không đánh giá";
        }

        public override string ToString()
        {
            return "[Sành sứ] " + base.ToString() +
                   $"  NSX: {NhaSanXuat}  Ngày nhập kho: {NgayNhapKho:dd/MM/yyyy}";
        }
    }

    // =====================================================================
    // c) Lớp quản lý danh sách hàng hóa - dùng MẢNG để lưu trữ
    // =====================================================================
    class QuanLyHangHoa
    {
        private HangHoa?[] ds;
        private int count;

        public QuanLyHangHoa(int n)
        {
            ds = new HangHoa?[n];
            count = 0;
        }

        // Thêm hàng hóa - thành công nếu không trùng mã hàng và còn chỗ trống
        public bool ThemHangHoa(HangHoa h)
        {
            if (count >= ds.Length) return false;
            if (ds.Take(count).Any(x => x!.MaHang.Equals(h.MaHang, StringComparison.OrdinalIgnoreCase)))
                return false;
            ds[count] = h;
            count++;
            return true;
        }

        // Lấy thông tin toàn bộ danh sách
        public List<HangHoa> LayTatCa() => ds.Take(count).Select(x => x!).ToList();

        // Lấy thông tin từng loại hàng hóa
        public List<ThucPham> LayDanhSachThucPham() => LayTatCa().OfType<ThucPham>().ToList();
        public List<DienMay> LayDanhSachDienMay() => LayTatCa().OfType<DienMay>().ToList();
        public List<SanhSu> LayDanhSachSanhSu() => LayTatCa().OfType<SanhSu>().ToList();

        // Tìm kiếm hàng hóa theo mã hàng
        public HangHoa? TimKiem(string maHang)
        {
            return ds.Take(count).FirstOrDefault(x => x!.MaHang.Equals(maHang, StringComparison.OrdinalIgnoreCase));
        }

        // Sắp xếp theo tên hàng tăng dần (dùng Array.Sort + Comparison<T>)
        public void SapXepTheoTenTang()
        {
            HangHoa[] tam = ds.Take(count).Select(x => x!).ToArray();
            Array.Sort(tam, (a, b) => string.Compare(a.TenHang, b.TenHang, StringComparison.OrdinalIgnoreCase));
            for (int i = 0; i < count; i++) ds[i] = tam[i];
        }

        // Sắp xếp theo số lượng tồn giảm dần
        public void SapXepTheoSoLuongGiam()
        {
            HangHoa[] tam = ds.Take(count).Select(x => x!).ToArray();
            Array.Sort(tam, (a, b) => b.SoLuongTon.CompareTo(a.SoLuongTon));
            for (int i = 0; i < count; i++) ds[i] = tam[i];
        }

        // Lấy các hàng thực phẩm khó bán
        public List<ThucPham> LayThucPhamKhoBan()
        {
            return LayDanhSachThucPham().Where(t => t.DanhGiaBanBuon() == "Khó bán").ToList();
        }

        // Xóa hàng hóa theo mã hàng
        public bool XoaHangHoa(string maHang)
        {
            int idx = -1;
            for (int i = 0; i < count; i++)
                if (ds[i]!.MaHang.Equals(maHang, StringComparison.OrdinalIgnoreCase)) { idx = i; break; }
            if (idx == -1) return false;

            for (int i = idx; i < count - 1; i++) ds[i] = ds[i + 1];
            ds[count - 1] = null;
            count--;
            return true;
        }

        // Sửa đơn giá của hàng hóa theo mã hàng
        public bool SuaDonGia(string maHang, double donGiaMoi)
        {
            HangHoa? h = TimKiem(maHang);
            if (h == null) return false;
            h.DonGia = donGiaMoi;
            return true;
        }
    }

    // =====================================================================
    // d) Lớp thử nghiệm với menu lựa chọn
    // =====================================================================
    class Program
    {
        static QuanLyHangHoa qlHangHoa = new QuanLyHangHoa(50);

        static void Main()
        {
            NapDuLieuMau();

            bool tiepTuc = true;
            while (tiepTuc)
            {
                Console.WriteLine("\n============ MENU QUẢN LÝ HÀNG HÓA ============");
                Console.WriteLine("1. Thêm hàng hóa");
                Console.WriteLine("2. Hiển thị toàn bộ danh sách");
                Console.WriteLine("3. Hiển thị theo từng loại hàng hóa");
                Console.WriteLine("4. Tìm kiếm hàng hóa theo mã hàng");
                Console.WriteLine("5. Sắp xếp theo tên hàng tăng dần");
                Console.WriteLine("6. Sắp xếp theo số lượng tồn giảm dần");
                Console.WriteLine("7. Danh sách hàng thực phẩm khó bán");
                Console.WriteLine("8. Xóa hàng hóa theo mã hàng");
                Console.WriteLine("9. Sửa đơn giá hàng hóa theo mã hàng");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");
                string? luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1": ThemHangHoaMoi(); break;
                    case "2": HienThiDanhSach(qlHangHoa.LayTatCa()); break;
                    case "3": HienThiTheoLoai(); break;
                    case "4":
                        Console.Write("Nhập mã hàng cần tìm: ");
                        var kq = qlHangHoa.TimKiem(Console.ReadLine() ?? "");
                        Console.WriteLine(kq != null ? kq.ToString() : "Không tìm thấy hàng hóa.");
                        break;
                    case "5":
                        qlHangHoa.SapXepTheoTenTang();
                        Console.WriteLine("Đã sắp xếp theo tên hàng tăng dần.");
                        HienThiDanhSach(qlHangHoa.LayTatCa());
                        break;
                    case "6":
                        qlHangHoa.SapXepTheoSoLuongGiam();
                        Console.WriteLine("Đã sắp xếp theo số lượng tồn giảm dần.");
                        HienThiDanhSach(qlHangHoa.LayTatCa());
                        break;
                    case "7":
                        HienThiDanhSach(qlHangHoa.LayThucPhamKhoBan().Cast<HangHoa>().ToList());
                        break;
                    case "8":
                        Console.Write("Nhập mã hàng cần xóa: ");
                        bool xoaOk = qlHangHoa.XoaHangHoa(Console.ReadLine() ?? "");
                        Console.WriteLine(xoaOk ? "Xóa thành công!" : "Không tìm thấy mã hàng!");
                        break;
                    case "9":
                        Console.Write("Nhập mã hàng cần sửa đơn giá: ");
                        string ma = Console.ReadLine() ?? "";
                        Console.Write("Nhập đơn giá mới: ");
                        double.TryParse(Console.ReadLine(), out double giaMoi);
                        bool suaOk = qlHangHoa.SuaDonGia(ma, giaMoi);
                        Console.WriteLine(suaOk ? "Cập nhật thành công!" : "Không tìm thấy mã hàng!");
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

        static void HienThiDanhSach(List<HangHoa> list)
        {
            if (list.Count == 0) { Console.WriteLine("Danh sách rỗng."); return; }
            foreach (var h in list) Console.WriteLine(h);
        }

        static void HienThiTheoLoai()
        {
            Console.WriteLine("--- Hàng thực phẩm ---");
            HienThiDanhSach(qlHangHoa.LayDanhSachThucPham().Cast<HangHoa>().ToList());
            Console.WriteLine("--- Hàng điện máy ---");
            HienThiDanhSach(qlHangHoa.LayDanhSachDienMay().Cast<HangHoa>().ToList());
            Console.WriteLine("--- Hàng sành sứ ---");
            HienThiDanhSach(qlHangHoa.LayDanhSachSanhSu().Cast<HangHoa>().ToList());
        }

        static void ThemHangHoaMoi()
        {
            Console.Write("Loại hàng (1: Thực phẩm, 2: Điện máy, 3: Sành sứ): ");
            string? loai = Console.ReadLine();
            Console.Write("Mã hàng: "); string maHang = Console.ReadLine() ?? "";
            Console.Write("Tên hàng: "); string tenHang = Console.ReadLine() ?? "";
            Console.Write("Đơn giá: "); double.TryParse(Console.ReadLine(), out double donGia);
            Console.Write("Số lượng tồn: "); int.TryParse(Console.ReadLine(), out int soLuong);

            HangHoa? h = null;
            try
            {
                switch (loai)
                {
                    case "1":
                        Console.Write("Nhà cung cấp: "); string ncc = Console.ReadLine() ?? "";
                        h = new ThucPham(maHang, tenHang, donGia, soLuong, ncc,
                                          DateTime.Now.AddDays(-5), DateTime.Now.AddDays(10));
                        break;
                    case "2":
                        Console.Write("Thời gian bảo hành (tháng): "); int.TryParse(Console.ReadLine(), out int bh);
                        Console.Write("Công suất (KW): "); double.TryParse(Console.ReadLine(), out double cs);
                        h = new DienMay(maHang, tenHang, donGia, soLuong, bh, cs);
                        break;
                    case "3":
                        Console.Write("Nhà sản xuất: "); string nsx = Console.ReadLine() ?? "";
                        h = new SanhSu(maHang, tenHang, donGia, soLuong, nsx, DateTime.Now.AddDays(-3));
                        break;
                    default:
                        Console.WriteLine("Loại hàng không hợp lệ.");
                        return;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return;
            }

            bool ok = qlHangHoa.ThemHangHoa(h!);
            Console.WriteLine(ok ? "Thêm hàng hóa thành công!" : "Thêm thất bại: mã hàng đã tồn tại hoặc kho đầy!");
        }

        static void NapDuLieuMau()
        {
            qlHangHoa.ThemHangHoa(new ThucPham("TP01", "Sữa tươi", 25_000, 40, "Vinamilk",
                DateTime.Now.AddDays(-20), DateTime.Now.AddDays(-2))); // đã hết hạn -> khó bán
            qlHangHoa.ThemHangHoa(new ThucPham("TP02", "Bánh mì", 15_000, 0, "ABC Bakery",
                DateTime.Now.AddDays(-2), DateTime.Now.AddDays(3)));  // hết hàng -> không đánh giá

            qlHangHoa.ThemHangHoa(new DienMay("DM01", "Tủ lạnh Samsung", 12_000_000, 2, 24, 0.15)); // <3 -> bán được
            qlHangHoa.ThemHangHoa(new DienMay("DM02", "Máy giặt LG", 8_500_000, 10, 12, 0.5));

            qlHangHoa.ThemHangHoa(new SanhSu("SS01", "Bình hoa Bát Tràng", 350_000, 60, "Bát Tràng",
                DateTime.Now.AddDays(-5))); // tồn > 50 -> bán chậm
            qlHangHoa.ThemHangHoa(new SanhSu("SS02", "Chén sứ cao cấp", 90_000, 15, "Minh Long",
                DateTime.Now.AddDays(-20))); // lưu kho > 10 ngày -> bán chậm
        }
    }
}
