using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using HelpdeskDAL;
using HelpdeskViewModels;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Student_GetByEmailTest()
        {
            EmployeeViewModel vm;
            try
            {
                vm = new EmployeeViewModel { Email = "bs@abc.com" };
                await vm.GetByEmail();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.NotNull(vm.Firstname);

        }

        [Fact]
        public async Task Student_GetByIdTest()
        {
            EmployeeViewModel vm;
            try
            {
                vm = new EmployeeViewModel { Id = 1 };
                await vm.GetById();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Student_GetAllTest()
        {
            List<EmployeeViewModel> vms;
            EmployeeViewModel viewModel = new();
            try
            {
                vms = await viewModel.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.NotEmpty(vms);
        }

        [Fact]
        public async Task Student_AddTest()
        {
            EmployeeViewModel vm;
            try
            {
                vm = new EmployeeViewModel
                {
                    Title = "Mr.",
                    Firstname = "Daniel",
                    Lastname = "Herrera",
                    DepartmentId = 100,
                    Email = "dh@abc.com",
                    Phoneno = "(519) 555-5555"
                };
                await vm.Add();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public async Task Student_UpdateTest()
        {
            int status;
            try
            {
                EmployeeViewModel vm = new() { Email = "dh@abc.com" };
                await vm.GetByEmail();
                vm.Phoneno = vm.Phoneno == "(519) 555-5555" ? "(555) 555-1234" : "(519) 555-5555";
                status = await vm.Update();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.NotEqual(-1, status);
        }

        [Fact]
        public async Task Student_DeleteTest()
        {
            int deleted = -1;
            try
            {
                EmployeeViewModel vm = new() { Email = "dh@abc.com" };
                await vm.GetByEmail();
                deleted = await vm.Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.NotEqual(-1, deleted);
        }

    }
}


