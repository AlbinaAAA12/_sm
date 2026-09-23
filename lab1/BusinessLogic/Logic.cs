using Model;

namespace BusinessLogic;

public class Logic
{
    private readonly List<Student> _students = new();

    public bool Add(Student student)
    {
        if (student is null || string.IsNullOrWhiteSpace(student.Name) ||
            string.IsNullOrWhiteSpace(student.Speciality) || string.IsNullOrWhiteSpace(student.Group))
        {
            return false;
        }

        _students.Add(new Student
        {
            Name = student.Name.Trim(),
            Speciality = student.Speciality.Trim(),
            Group = student.Group.Trim()
        });

        return true;
    }

    public bool Remove(int index)
    {
        if (index < 0 || index >= _students.Count)
        {
            return false;
        }

        _students.RemoveAt(index);
        return true;
    }

    public List<Student> GetAll()
    {
        return new List<Student>(_students);
    }

    public Dictionary<string, int> GetCounts()
    {
        Dictionary<string, int> counts = new();

        foreach (Student student in _students)
        {
            if (counts.ContainsKey(student.Speciality))
            {
                counts[student.Speciality]++;
            }
            else
            {
                counts.Add(student.Speciality, 1);
            }
        }

        return counts;
    }
}
