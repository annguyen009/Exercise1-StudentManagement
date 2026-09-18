using System;
using System.Collections.Generic;

namespace Bai8_Shape
{
    // ===== Shape: lớp trừu tượng, khai báo 3 phương thức trừu tượng =====
    abstract class Shape
    {
        public abstract void Draw();
        public abstract void Erase();
        public abstract void Move(int x, int y);
    }

    // ===== Circle: kế thừa trực tiếp từ Shape =====
    class Circle : Shape
    {
        public override void Draw() => Console.WriteLine("Draw a Circle.");
        public override void Erase() => Console.WriteLine("Erase a Circle.");
        public override void Move(int x, int y) => Console.WriteLine($"Move a Circle to ({x}, {y}).");
    }

    // ===== Quad: kế thừa từ Shape, là lớp cha của Rectangle =====
    class Quad : Shape
    {
        public override void Draw() => Console.WriteLine("Draw a Quad.");
        public override void Erase() => Console.WriteLine("Erase a Quad.");
        public override void Move(int x, int y) => Console.WriteLine($"Move a Quad to ({x}, {y}).");
    }

    // ===== Rectangle: kế thừa từ Quad =====
    class Rectangle : Quad
    {
        public override void Draw() => Console.WriteLine("Draw a Rectangle.");
        public override void Erase() => Console.WriteLine("Erase a Rectangle.");
        public override void Move(int x, int y) => Console.WriteLine($"Move a Rectangle to ({x}, {y}).");
    }

    // ===== Triangle: kế thừa trực tiếp từ Shape =====
    class Triangle : Shape
    {
        public override void Draw() => Console.WriteLine("Draw a Triangle.");
        public override void Erase() => Console.WriteLine("Erase a Triangle.");
        public override void Move(int x, int y) => Console.WriteLine($"Move a Triangle to ({x}, {y}).");
    }

    // ===== Polygon: kế thừa trực tiếp từ Shape =====
    class Polygon : Shape
    {
        public override void Draw() => Console.WriteLine("Draw a Polygon.");
        public override void Erase() => Console.WriteLine("Erase a Polygon.");
        public override void Move(int x, int y) => Console.WriteLine($"Move a Polygon to ({x}, {y}).");
    }

    // ===== Drawing: gọi draw() của từng đối tượng Shame một cách đa hình =====
    class Drawing
    {
        public void DrawShape(Shape theShape)
        {
            theShape.Draw();
        }
    }

    class Program
    {
        static void Main()
        {
            // Mảng các Shape khác nhau (đa hình - polymorphism)
            List<Shape> danhSachShape = new List<Shape>
            {
                new Circle(),
                new Rectangle(),
                new Triangle(),
                new Polygon(),
            };

            Drawing drawing = new Drawing();

            Console.WriteLine("===== GỌI drawShape() CHO TỪNG ĐỐI TƯỢNG (đa hình) =====");
            foreach (Shape s in danhSachShape)
            {
                drawing.DrawShape(s); // luôn gọi Drawing.DrawShape, nhưng in ra đúng loại Shape thực sự nhờ đa hình
            }

            Console.WriteLine("\n===== MINH HỌA THÊM erase() và move() =====");
            foreach (Shape s in danhSachShape)
            {
                s.Erase();
                s.Move(10, 20);
            }
        }
    }
}
