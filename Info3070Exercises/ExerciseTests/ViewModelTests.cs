using ExercisesDAL;
using ExercisesViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ExerciseTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Student_GetByLastnameTest()
        {
            StudentViewModel vm;
            try
            {
                vm = new StudentViewModel { Lastname = "Pet" };
                await vm.GetByLastname();
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
            StudentViewModel vm;
            try
            {
                vm = new StudentViewModel { Id = 2 };
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
            List<StudentViewModel> vms;
            StudentViewModel viewModel = new();
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
            StudentViewModel vm;
            try
            {
                vm = new StudentViewModel
                {
                    Title = "Mr.",
                    Firstname = "Daniel",
                    Lastname = "Herrera",
                    DivisionId = 10,
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
            int status = -1;
            try
            {
                StudentViewModel svm = new() { Lastname = "Herrera" };
                await svm.GetByLastname();
                svm.Phoneno = svm.Phoneno == "(519) 555-5555" ? "(555) 555-1234" : "(519) 555-5555";
                status = await svm.Update();
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
                StudentViewModel svm = new() { Lastname = "Herrera" };
                await svm.GetByLastname();
                deleted = await svm.Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
                throw;
            }
            Assert.NotEqual(-1, deleted);
        }

        [Fact]
        public async Task Student_ConcurrencyTest()
        {
            int status = -1;
            try
            {
                StudentViewModel vm1 = new() { Lastname = "Herrera" };
                StudentViewModel vm2 = new() { Lastname = "Herrera" };
                await vm1.GetByLastname();
                await vm2.GetByLastname();
                vm1.Phoneno = vm1.Phoneno == "(519) 555-5551" ? "(519) 555-5552" : "(519) 555-5551";

                if (await vm1.Update() == 1)
                {
                    vm2.Phoneno = "(666) 666-6666";
                    status = await vm2.Update();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
            }
            Assert.Equal(-2, status);
        }

    }
}
