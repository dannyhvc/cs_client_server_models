using ExercisesDAL;
using System.Threading.Tasks;
using Xunit;

namespace ExerciseTests
{
    public class DOATests
    {
        [Fact]
        public async Task Student_GetByLastnameTests()
        {
            StudentDAO dao = new StudentDAO();
            Student student = await dao.GetByLastname("Pet");
            Assert.NotNull(student);
        }

        [Fact]
        public async Task Student_GetByIdTests()
        {
            StudentDAO dao = new StudentDAO();
            Student student = await dao.GetById(2); // should be Teachers Pet
            Assert.NotNull(student);
        }

        [Fact]
        public async Task Student_GetAllTests()
        {
            StudentDAO dao = new StudentDAO();
            var students = await dao.GetAll();
            Assert.NotEmpty(students);
        }

        [Fact]
        public async Task Student_AddTests()
        {
            StudentDAO dao = new StudentDAO();
            Student student = new()
            {
                FirstName = "Billy",
                LastName = "Joel",
                DivisionId = 10,
                Title = "Mr.",
                PhoneNo = "(555) 555-1234",
                Email = "bj@someschool.com"
            };
            Assert.True(await dao.Add(student) > 0);
        }

        [Fact]
        public async Task Student_UpdateTests()
        {
            StudentDAO dao = new StudentDAO();
            Student student = await dao.GetByLastname("Joel");
            if (student != null)
                student.PhoneNo = "(555) 555-1234" == student.PhoneNo
                                    ? "(555) 555-5555"
                                    : "(555) 555-1234";
            Assert.Equal(UpdateStatus.Ok, await dao.Update(student));
        }

        [Fact]
        public async Task Student_DeleteTests()
        {
            StudentDAO dao = new StudentDAO();
            Student student = await dao.GetByLastname("Joel");
            Assert.True(await dao.Delete(student.Id) > 0);
        }

        [Fact]
        public async Task Student_ConcurrencyTest()
        {
            StudentDAO dao_one = new();
            StudentDAO dao_two = new();
            Student s_update_one = await dao_one.GetByLastname("Joel");
            Student s_update_two = await dao_two.GetByLastname("Joel");

            if (s_update_one != null)
            {
                string old_phone_num = s_update_one.PhoneNo;
                string new_phone_num = old_phone_num == "(519) 555-1234" ? "(555) 555-555" : "(519) 555-1234";
                s_update_one.PhoneNo = new_phone_num;
                if (await dao_one.Update(s_update_one) == UpdateStatus.Ok)
                {
                    s_update_two.PhoneNo = "(666) 666-6666";
                    Assert.Equal(UpdateStatus.Stale, await dao_two.Update(s_update_two));
                }
                else
                    Assert.True(false);
            }
        }
    }
}
