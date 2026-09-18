using System;
using System.Collections.Generic;

namespace Bai5_NhanVien
{
    // ===== Employee: lớp cha trừu tượng =====
    abstract class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }

        protected Employee(string firstName, string lastName, string ssn)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = ssn;
        }

        // Cách tính lương khác nhau tùy loại nhân viên => phương thức trừu tượng
        public abstract double Earnings();

        public override string ToString()
        {
            return $"{FirstName} {LastName}\nsocial security number: {SocialSecurityNumber}";
        }
    }

    // ===== SalariedEmployee: trả lương theo tuần =====
    class SalariedEmployee : Employee
    {
        public double WeeklySalary { get; set; }

        public SalariedEmployee(string firstName, string lastName, string ssn, double weeklySalary)
            : base(firstName, lastName, ssn)
        {
            WeeklySalary = weeklySalary;
        }

        public override double Earnings() => WeeklySalary;

        public override string ToString()
        {
            return "salaried employee: " + base.ToString() + $"\nweekly salary: {WeeklySalary:N0}";
        }
    }

    // ===== HourlyEmployee: trả lương theo giờ =====
    class HourlyEmployee : Employee
    {
        public double Wage { get; set; }   // số tiền / giờ
        public double Hours { get; set; }  // số giờ làm

        public HourlyEmployee(string firstName, string lastName, string ssn, double wage, double hours)
            : base(firstName, lastName, ssn)
        {
            Wage = wage;
            Hours = hours;
        }

        public override double Earnings()
        {
            if (Hours <= 40)
                return Wage * Hours;
            // Giờ tăng ca (>40) được trả gấp 1.5 lần
            return 40 * Wage + (Hours - 40) * Wage * 1.5;
        }

        public override string ToString()
        {
            return "hourly employee: " + base.ToString() +
                   $"\nhourly wage: {Wage:N0}; hours worked: {Hours}";
        }
    }

    // ===== CommissionEmployee: bán hàng part-time, lương theo doanh số và % hoa hồng =====
    class CommissionEmployee : Employee
    {
        public double GrossSales { get; set; }
        public double CommissionRate { get; set; }

        public CommissionEmployee(string firstName, string lastName, string ssn,
                                   double grossSales, double commissionRate)
            : base(firstName, lastName, ssn)
        {
            GrossSales = grossSales;
            CommissionRate = commissionRate;
        }

        public override double Earnings() => CommissionRate * GrossSales;

        public override string ToString()
        {
            return "commission employee: " + base.ToString() +
                   $"\ngross sales: {GrossSales:N0}\ncommission rate: {CommissionRate:P2}";
        }
    }

    // ===== BasePlusCommissionEmployee: NV chính thức = hoa hồng như CommissionEmployee + lương căn bản =====
    class BasePlusCommissionEmployee : CommissionEmployee
    {
        public double BaseSalary { get; set; }

        public BasePlusCommissionEmployee(string firstName, string lastName, string ssn,
                                           double grossSales, double commissionRate, double baseSalary)
            : base(firstName, lastName, ssn, grossSales, commissionRate)
        {
            BaseSalary = baseSalary;
        }

        public override double Earnings() => base.Earnings() + BaseSalary;

        public override string ToString()
        {
            return "base salaried commission employee: " + Employee_ToString() +
                   $"\ngross sales: {GrossSales:N0}\ncommission rate: {CommissionRate:P2}" +
                   $"\nbase salary: {BaseSalary:N0}";
        }

        // Helper để lấy phần "Họ tên + SSN" gốc từ Employee (tránh lặp phần gross sales/commission rate của lớp cha)
        private string Employee_ToString() => $"{FirstName} {LastName}\nsocial security number: {SocialSecurityNumber}";
    }

    class Program
    {
        static void Main()
        {
            // Tạo mảng các Employee (đa hình - polymorphism)
            List<Employee> nhanViens = new List<Employee>
            {
                new SalariedEmployee("Nguyen", "An", "111-11-1111", 8_000_000),
                new HourlyEmployee("Tran", "Binh", "222-22-2222", 50_000, 45),
                new CommissionEmployee("Le", "Chi", "333-33-3333", 20_000_000, 0.08),
                new BasePlusCommissionEmployee("Pham", "Dung", "444-44-4444", 15_000_000, 0.05, 3_000_000),
            };

            double tongLuong = 0;
            foreach (var nv in nhanViens)
            {
                Console.WriteLine(nv);
                Console.WriteLine($"earnings: {nv.Earnings():N0}\n");
                tongLuong += nv.Earnings();
            }

            Console.WriteLine($"===== TỔNG LƯƠNG TOÀN CÔNG TY: {tongLuong:N0} =====");
        }
    }
}
