using System.Threading.Tasks;
using Xunit;
using HelpdeskDAL;

namespace CasestudyTests
{
    public class DAOTests
    {
        [Fact]
        public async Task Employee_GetByEmailTests()
        {
            EmployeeDAO dao = new();
            Employee Employee = await dao.GetByEmail("bs@abc.com");
            Assert.NotNull(Employee);
        }

        [Fact]
        public async Task Employee_GetByIdTests()
        {
            EmployeeDAO dao = new();
            Employee Employee = await dao.GetById(1); // should be Teachers Pet
            Assert.NotNull(Employee);
        }

        [Fact]
        public async Task Employee_GetAllTests()
        {
            EmployeeDAO dao = new();
            var Employees = await dao.GetAll();
            Assert.NotEmpty(Employees);
        }

        [Fact]
        public async Task Employee_AddTests()
        {
            EmployeeDAO dao = new();
            Employee Employee = new()
            {
                FirstName = "Billy",
                LastName = "Joel",
                DepartmentId = 100,
                Title = "Mr.",
                PhoneNo = "(555) 555-5554",
                Email = "bj@abc.com"
            };
            Assert.True(await dao.Add(Employee) > 0);
        }

        [Fact]
        public async Task Employee_UpdateTests()
        {
            EmployeeDAO dao = new();
            Employee Employee = await dao.GetByEmail("bj@abc.com");
            if (Employee != null)
                Employee.PhoneNo = "(555) 555-5554" == Employee.PhoneNo
                                    ? "(555) 555-5553"
                                    : "(555) 555-5554";
            Assert.Equal(UpdateStatus.Ok, await dao.Update(Employee));
        }

        [Fact]
        public async Task Employee_DeleteTests()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employee Employee = await dao.GetByEmail("bj@abc.com");
            Assert.True(await dao.Delete(Employee.Id) > 0);
        }

        [Fact]
        public async Task Student_ConcurrencyTest()
        {
            EmployeeDAO dao_one = new();
            EmployeeDAO dao_two = new();
            Employee s_update_one = await dao_one.GetByEmail("bj@abc.com");
            Employee s_update_two = await dao_two.GetByEmail("bj@abc.com");

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


