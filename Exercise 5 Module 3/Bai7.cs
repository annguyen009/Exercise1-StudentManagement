using System;
using System.Collections.Generic;
using System.Linq;

namespace Bai7_QuanLyNguoi
{
    // ===== Lớp Person: lớp cha =====
    class Person
    {
        public string HoTen { get; set; }
        public string DiaChi { get; set; }

        public Person(string hoTen, string diaChi)
        {
            HoTen = hoTen;
            DiaChi = diaChi;
        }

        public override string ToString()
        {
            return $"Họ tên: {HoTen,-20} Địa chỉ: {DiaChi}";
        }
    }

    // ===== Lớp Student: kế thừa Person, có điểm môn học 1 & 2 =====
    class Student : Person
    {
        public double DiemMonHoc1 { get; set; }
        public double DiemMonHoc2 { get; set; }

        public Student(string hoTen, string diaChi, double diemMonHoc1, double diemMonHoc2)
            : base(hoTen, diaChi)
        {
            DiemMonHoc1 = diemMonHoc1;
            DiemMonHoc2 = diemMonHoc2;
        }

        // Tính điểm trung bình
        public double TinhDiemTrungBinh() => (DiemMonHoc1 + DiemMonHoc2) / 2;

        // Đánh giá học lực dựa trên điểm trung bình
        public string DanhGia()
        {
            double dtb = TinhDiemTrungBinh();
            if (dtb >= 8.5) return "Giỏi";
            if (dtb >= 7.0) return "Khá";
            if (dtb >= 5.0) return "Trung bình";
            return "Yếu";
        }

        // Trả về bảng điểm sinh viên (gồm thông tin thuộc tính và điểm trung bình)
        public override string ToString()
        {
            return "[Sinh viên] " + base.ToString() +
                   $" Điểm MH1: {DiemMonHoc1,-5} Điểm MH2: {DiemMonHoc2,-5} " +
                   $"ĐTB: {TinhDiemTrungBinh(),-5:0.00} Đánh giá: {DanhGia()}";
        }
    }

    // ===== Lớp Employee: kế thừa Person, có hệ số lương =====
    class Employee : Person
    {
        public const double MucLuongCoSo = 1_800_000; // mức lương cơ sở dùng để tính lương
        public double HeSoLuong { get; set; }

        public Employee(string hoTen, string diaChi, double heSoLuong)
            : base(hoTen, diaChi)
        {
            HeSoLuong = heSoLuong;
        }

        // Tính lương = hệ số lương * mức lương cơ sở
        public double TinhLuong() => HeSoLuong * MucLuongCoSo;

        public string DanhGia()
        {
            double luong = TinhLuong();
            if (luong >= 15_000_000) return "Lương cao";
            if (luong >= 8_000_000) return "Lương khá";
            return "Lương trung bình";
        }

        // Trả về bảng lương cho nhân viên (gồm thông tin thuộc tính và tiền lương)
        public override string ToString()
        {
            return "[Nhân viên] " + base.ToString() +
                   $" Hệ số lương: {HeSoLuong,-5} Lương: {TinhLuong(),12:N0} Đánh giá: {DanhGia()}";
        }
    }

    // ===== Lớp Customer: kế thừa Person, có tên công ty & trị giá hóa đơn =====
    class Customer : Person
    {
        public string TenCongTy { get; set; }
        public double TriGiaHoaDon { get; set; }

        public Customer(string hoTen, string diaChi, string tenCongTy, double triGiaHoaDon)
            : base(hoTen, diaChi)
        {
            TenCongTy = tenCongTy;
            TriGiaHoaDon = triGiaHoaDon;
        }

        public string DanhGia()
        {
            return TriGiaHoaDon >= 50_000_000 ? "Khách hàng VIP" : "Khách hàng thường";
        }

        // Trả về thông tin hóa đơn của khách hàng (gồm thông tin thuộc tính của đối tượng)
        public override string ToString()
        {
            return "[Khách hàng] " + base.ToString() +
                   $" Công ty: {TenCongTy,-15} Trị giá hóa đơn: {TriGiaHoaDon,12:N0} Đánh giá: {DanhGia()}";
        }
    }

    // ===== Lớp Management: lưu trữ toàn bộ Person (đa hình) bằng mảng =====
    class Management
    {
        private Person?[] danhSach;
        private int soLuong;

        public Management(int n)
        {
            danhSach = new Person?[n];
            soLuong = 0;
        }

        // Thêm một người vào danh sách
        public bool ThemNguoi(Person p)
        {
            if (soLuong >= danhSach.Length) return false;
            danhSach[soLuong] = p;
            soLuong++;
            return true;
        }

        // Xóa một người khỏi danh sách theo họ tên
        public bool XoaNguoi(string hoTen)
        {
            int idx = -1;
            for (int i = 0; i < soLuong; i++)
                if (danhSach[i]!.HoTen.Equals(hoTen, StringComparison.OrdinalIgnoreCase)) { idx = i; break; }
            if (idx == -1) return false;

            for (int i = idx; i < soLuong - 1; i++) danhSach[i] = danhSach[i + 1];
            danhSach[soLuong - 1] = null;
            soLuong--;
            return true;
        }

        // Sắp xếp danh sách theo thứ tự họ tên (Array.Sort + Comparison<T>)
        public void SapXepTheoHoTen()
        {
            Person[] tam = danhSach.Take(soLuong).Select(x => x!).ToArray();
            Array.Sort(tam, (a, b) => string.Compare(a.HoTen, b.HoTen, StringComparison.OrdinalIgnoreCase));
            for (int i = 0; i < soLuong; i++) danhSach[i] = tam[i];
        }

        // Xuất danh sách theo dạng bảng - nhờ đa hình, mỗi loại tự in đúng ToString() của nó
        public void XuatDanhSachDangBang()
        {
            if (soLuong == 0) { Console.WriteLine("Danh sách rỗng."); return; }
            Console.WriteLine(new string('-', 100));
            for (int i = 0; i < soLuong; i++)
                Console.WriteLine($"{i + 1,3}. {danhSach[i]}");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine($"Tổng số người hiện có: {soLuong}");
        }
    }

    // ===== Lớp Test: hàm main để kiểm nghiệm (menu, thể hiện đa hình) =====
    class Program
    {
        static Management ql = new Management(50);

        static void Main()
        {
            NapDuLieuMau();

            bool tiepTuc = true;
            while (tiepTuc)
            {
                Console.WriteLine("\n============ MENU QUẢN LÝ NGƯỜI ============");
                Console.WriteLine("1. Thêm sinh viên");
                Console.WriteLine("2. Thêm nhân viên");
                Console.WriteLine("3. Thêm khách hàng");
                Console.WriteLine("4. Xóa người (theo họ tên)");
                Console.WriteLine("5. Sắp xếp theo họ tên");
                Console.WriteLine("6. Xuất danh sách theo dạng bảng");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");
                string? luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1": ThemSinhVien(); break;
                    case "2": ThemNhanVien(); break;
                    case "3": ThemKhachHang(); break;
                    case "4":
                        Console.Write("Nhập họ tên cần xóa: ");
                        bool xoaOk = ql.XoaNguoi(Console.ReadLine() ?? "");
                        Console.WriteLine(xoaOk ? "Xóa thành công!" : "Không tìm thấy người này!");
                        break;
                    case "5":
                        ql.SapXepTheoHoTen();
                        Console.WriteLine("Đã sắp xếp theo họ tên.");
                        break;
                    case "6":
                        ql.XuatDanhSachDangBang();
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

        static void ThemSinhVien()
        {
            Console.Write("Họ tên: "); string hoTen = Console.ReadLine() ?? "";
            Console.Write("Địa chỉ: "); string diaChi = Console.ReadLine() ?? "";
            Console.Write("Điểm môn học 1: "); double.TryParse(Console.ReadLine(), out double d1);
            Console.Write("Điểm môn học 2: "); double.TryParse(Console.ReadLine(), out double d2);
            bool ok = ql.ThemNguoi(new Student(hoTen, diaChi, d1, d2));
            Console.WriteLine(ok ? "Thêm sinh viên thành công!" : "Danh sách đã đầy!");
        }

        static void ThemNhanVien()
        {
            Console.Write("Họ tên: "); string hoTen = Console.ReadLine() ?? "";
            Console.Write("Địa chỉ: "); string diaChi = Console.ReadLine() ?? "";
            Console.Write("Hệ số lương: "); double.TryParse(Console.ReadLine(), out double heSo);
            bool ok = ql.ThemNguoi(new Employee(hoTen, diaChi, heSo));
            Console.WriteLine(ok ? "Thêm nhân viên thành công!" : "Danh sách đã đầy!");
        }

        static void ThemKhachHang()
        {
            Console.Write("Họ tên: "); string hoTen = Console.ReadLine() ?? "";
            Console.Write("Địa chỉ: "); string diaChi = Console.ReadLine() ?? "";
            Console.Write("Tên công ty: "); string congTy = Console.ReadLine() ?? "";
            Console.Write("Trị giá hóa đơn: "); double.TryParse(Console.ReadLine(), out double triGia);
            bool ok = ql.ThemNguoi(new Customer(hoTen, diaChi, congTy, triGia));
            Console.WriteLine(ok ? "Thêm khách hàng thành công!" : "Danh sách đã đầy!");
        }

        static void NapDuLieuMau()
        {
            ql.ThemNguoi(new Student("Nguyen Van An", "Q.1, TP.HCM", 8.5, 9.0));
            ql.ThemNguoi(new Employee("Tran Thi Binh", "Q.3, TP.HCM", 4.5));
            ql.ThemNguoi(new Customer("Le Van Cuong", "Q.7, TP.HCM", "Cong ty ABC", 65_000_000));
        }
    }
}
