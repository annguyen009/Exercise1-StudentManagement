using System;

public class Student
{
    private string name;
    private double score;
    private static int totalStudents = 0;

    // Constructor
    public Student(string name, double score)
    {
        this.name = name;
        this.score = score;
        totalStudents++;
    }

    // Instance Methods
    public string GetName()
    {
        return name;
    }

    public double GetScore()
    {
        return score;
    }

    public bool IsPassed()
    {
        return score >= 5.0;
    }

    public string GetClassification()
    {
        if (score >= 8.0) return "Excellent";
        else if (score >= 6.5) return "Good";
        else if (score >= 5.0) return "Average";
        else return "Weak";
    }

    // Static Methods
    public static int GetTotalStudents()
    {
        return totalStudents;
    }

    public static Student FindTopStudent(Student[] students)
    {
        Student top = students[0];
        foreach (Student s in students)
        {
            if (s.GetScore() > top.GetScore())
            {
                top = s;
            }
        }
        return top;
    }

    public static double CalculateAverageScore(Student[] students)
    {
        double sum = 0;
        foreach (Student s in students)
        {
            sum += s.GetScore();
        }
        return sum / students.Length;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Student[] students = new Student[]
        {
            new Student("An", 9.0),
            new Student("Binh", 7.5),
            new Student("Chi", 6.0),
            new Student("Dung", 4.5),
            new Student("Hoa", 8.2)
        };

        Console.WriteLine("Total students: " + Student.GetTotalStudents());

        foreach (Student s in students)
        {
            Console.WriteLine($"{s.GetName()} - Score: {s.GetScore()} - " +
                              $"Classification: {s.GetClassification()} - " +
                              $"Passed: {s.IsPassed()}");
        }

        Student top = Student.FindTopStudent(students);
        Console.WriteLine("Top student: " + top.GetName() + " with score " + top.GetScore());

        double avg = Student.CalculateAverageScore(students);
        Console.WriteLine("Class average score: " + avg);
    }
}
