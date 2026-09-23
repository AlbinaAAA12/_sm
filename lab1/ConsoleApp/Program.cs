using System;
using BusinessLogic;
using Model;

class Program
{
    static void Main()
    {
        Logic logic = new Logic();
        bool run = true;

        while (run)
        {
            Console.WriteLine();
            Console.WriteLine("DecanatPRO");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Удалить студента");
            Console.WriteLine("3. Показать список студентов");
            Console.WriteLine("4. Показать распределение по направлениям");
            Console.WriteLine("5. Выйти из приложения");
            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                AddStudent(logic);
            }
            else if (choice == "2")
            {
                RemoveStudent(logic);
            }
            else if (choice == "3")
            {
                ShowStudents(logic);
            }
            else if (choice == "4")
            {
                ShowCounts(logic);
            }
            else if (choice == "5")
            {
                run = false;
            }
            else
            {
                Console.WriteLine("Введите число от 1 до 5.");
            }
        }
    }

    static void AddStudent(Logic logic)
    {
        Console.Write("ФИО: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Направление подготовки: ");
        string speciality = Console.ReadLine() ?? "";
        Console.Write("Группа: ");
        string group = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(speciality) || string.IsNullOrWhiteSpace(group))
        {
            Console.WriteLine("Все поля должны быть заполнены.");
            return;
        }

        Student student = new Student();
        student.Name = name;
        student.Speciality = speciality;
        student.Group = group;

        if (logic.Add(student))
        {
            Console.WriteLine("Студент добавлен.");
        }
        else
        {
            Console.WriteLine("Не удалось добавить студента.");
        }
    }

    static void RemoveStudent(Logic logic)
    {
        var students = logic.GetAll();
        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст.");
            return;
        }

        ShowStudents(logic);
        Console.Write("Введите номер студента для удаления: ");
        string value = Console.ReadLine() ?? "";
        int number;

        if (!int.TryParse(value, out number) || number < 1 || number > students.Count)
        {
            Console.WriteLine("Введите номер из списка.");
            return;
        }

        if (logic.Remove(number - 1))
        {
            Console.WriteLine("Студент удален.");
        }
        else
        {
            Console.WriteLine("Не удалось удалить студента.");
        }
    }

    static void ShowStudents(Logic logic)
    {
        var students = logic.GetAll();
        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст.");
            return;
        }

        Console.WriteLine("№ | ФИО | Направление | Группа");
        Console.WriteLine("-----------------------------------------------");
        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine((i + 1) + " | " + students[i].Name + " | " + students[i].Speciality + " | " + students[i].Group);
        }
    }

    static void ShowCounts(Logic logic)
    {
        var counts = logic.GetCounts();
        if (counts.Count == 0)
        {
            Console.WriteLine("Нет данных для распределения.");
            return;
        }

        foreach (var item in counts)
        {
            string bar = "";
            for (int i = 0; i < item.Value; i++)
            {
                bar += "#";
            }
            Console.WriteLine(item.Key + ": " + bar + " (" + item.Value + ")");
        }
    }
}
