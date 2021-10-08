using ExercisesDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExercisesViewModels
{
    public class StudentViewModel
    {
        private readonly StudentDAO _dao;
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DivisionId { get; set; }
        public string DivisonName { get; set; }
        public int Id { get; set; }
        public string Picture64 { get; set; }

        public StudentViewModel() => _dao = new StudentDAO();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' + MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task GetByLastname()
        {
            try
            {
                Student s = await _dao.GetByLastname(Lastname);
                Title = s.Title;
                Firstname = s.FirstName;
                Lastname = s.LastName;
                Phoneno = s.PhoneNo;
                Email = s.Email;
                Id = s.Id;
                DivisionId = s.DivisionId;
                if (s.Picture != null)
                    Picture64 = Convert.ToBase64String(s.Picture);
                Timer = Convert.ToBase64String(s.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
        }

        public async Task GetById()
        {
            try
            {
                Student s = await _dao.GetById(Id);
                Title = s.Title;
                Firstname = s.FirstName;
                Lastname = s.LastName;
                Phoneno = s.PhoneNo;
                Email = s.Email;
                Id = s.Id;
                DivisionId = s.DivisionId;
                if (s.Picture != null)
                    Picture64 = Convert.ToBase64String(s.Picture);
                Timer = Convert.ToBase64String(s.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                CLS_DBG(ex);
                throw;
            }
        }

        public async Task<List<StudentViewModel>> GetAll()
        {
            List<StudentViewModel> all_vm = new();
            try
            {
                var students = await _dao.GetAll();
                foreach (Student student in students)
                {
                    StudentViewModel svm = new()
                    {
                        Title = student.Title,
                        Firstname = student.FirstName,
                        Lastname = student.LastName,
                        Phoneno = student.PhoneNo,
                        Email = student.Email,
                        Id = student.Id,
                        DivisionId = student.DivisionId,
                        Timer = Convert.ToBase64String(student.Timer),
                    };
                    all_vm.Add(svm);
                }
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return all_vm;
        }

        public async Task Add()
        {
            Id = -1;
            try
            {
                Student stu = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DivisionId = DivisionId
                };
                Id = await _dao.Add(stu);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
        }

        public async Task<int> Update()
        {
            UpdateStatus status;
            try
            {
                Student stu = new()
                {
                    Id = Id,
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DivisionId = DivisionId,
                };
                if (Picture64 != null)
                    stu.Picture = Convert.FromBase64String(Picture64);
                stu.Timer = Convert.FromBase64String(Timer);
                status = await _dao.Update(stu);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return Convert.ToInt16(status);
        }

        public async Task<int> Delete()
        {
            int deleted = -1;
            try
            {
                deleted = await _dao.Delete(Id);
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
