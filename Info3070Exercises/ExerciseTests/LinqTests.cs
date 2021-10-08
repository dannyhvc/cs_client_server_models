using ExercisesDAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExerciseTests
{
    public class LinqTests
    {
        [Fact]
        public void Test1()
        {
            SomeSchoolContext _db = new();
            var selectedStudents = from stu in _db.Students
                                   where stu.Id == 2
                                   select stu;
            Assert.True(selectedStudents.Any());
        }

        [Fact]
        public void Test2()
        {
            SomeSchoolContext _db = new();
            var selectedStudents = from stu in _db.Students
                                   where stu.Title == "Ms." || stu.Title == "Mrs."
                                   select stu;
            Assert.True(selectedStudents.Any());
        }

        [Fact]
        public void Test3()
        {
            SomeSchoolContext _db = new();
            var selectedStudents = from stu in _db.Students
                                   where stu.Division.Name == "Design"
                                   select stu;
            Assert.True(selectedStudents.Any());
        }

        [Fact]
        public void Test4()
        {
            SomeSchoolContext _db = new();
            Student selectedStudent = _db.Students.FirstOrDefault(stu => stu.Id == 2);
            Assert.True(selectedStudent.FirstName == "Teachers");
        }

        [Fact]
        public void Test5()
        {
            SomeSchoolContext _db = new();
            var selectedStudent = _db.Students.Where(stu => stu.Title == "Ms." || stu.Title == "Mrs.");
            Assert.True(selectedStudent.Any());
        }

        [Fact]
        public void Test6()
        {
            SomeSchoolContext _db = new();
            var selectedStudent = _db.Students.Where(stu => stu.Division.Name == "Design");
            Assert.True(selectedStudent.Any());
        }

        [Fact]
        public async Task Test7()
        {
            SomeSchoolContext _db = new();

            Student selectedStudenFromName = await _db.Students.FirstOrDefaultAsync(
                stu => stu.FirstName == "Daniel" && stu.LastName == "Herrera"
            );
            var selectedStudent = await _db.Students.FirstOrDefaultAsync(stu => stu.Id == selectedStudenFromName.Id);

            if (selectedStudent != null)
            {
                string oldEmail = selectedStudent.Email;
                string newEmail = oldEmail == "dh@someschool.com" ? "jb@someschool.com" : "dh@someschool.com";
                selectedStudent.Email = newEmail;
                _db.Entry(selectedStudent).CurrentValues.SetValues(selectedStudent);
            }
            Assert.Equal(1, await _db.SaveChangesAsync());  // rows updated
        }

        [Fact]
        public async Task Test8()
        {
            SomeSchoolContext _db = new();
            Student student = new()
            {
                FirstName = "Daniel",
                LastName = "Herrera",
                PhoneNo = "(555) 555-1234",
                Title = "Mr.",
                DivisionId = 10,
                Email = "dh@someschool.com"
            };
            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();
            Assert.True(student.Id > 0);
        }

        [Fact]
        public async Task Test9()
        {
            SomeSchoolContext _db = new();
            Student selectedStudent = await _db.Students.FirstOrDefaultAsync(stu => stu.FirstName == "Daniel");
            if (selectedStudent != null)
            {
                _db.Students.Remove(selectedStudent);
                Assert.Equal(1, await _db.SaveChangesAsync());
            }
            else Assert.True(false);
        }

    }
}
