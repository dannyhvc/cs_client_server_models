using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExercisesDAL
{
    public class StudentDAO
    {
        readonly private IRepository<Student> repository;
        public StudentDAO() => repository = new SomeSchoolRepository<Student>();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                $"Problem in {GetType().Name} {MethodBase.GetCurrentMethod().Name} {ex.Message}"
            );

        public async Task<Student> GetByLastname(string name)
        {
            Student student = null;
            try
            {
                student = await repository.GetOne(stu => stu.LastName == name);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return student;
        }

        public async Task<Student> GetById(int id)
        {
            Student student = null;
            try
            {
                student = await repository.GetOne(s => s.Id == id);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return student;
        }

        public async Task<List<Student>> GetAll()
        {
            List<Student> students = new();

            try
            {
                students = await repository.GetAll();
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }

            return students;
        }

        public async Task<int> Add(Student student)
        {
            try
            {
                await repository.Add(student);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return student.Id;
        }

        public async Task<UpdateStatus> Update(Student student)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await repository.Update(student);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return status;
        }

        public async Task<int> Delete(int id)
        {
            int deleted = -1;
            try
            {
                deleted = await repository.Delete(id);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return deleted;
        }
    }
}
