using HelpdeskDAL;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phoneno { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsTech { get; set; }
        public string StaffPicture64 { get; set; }
        public string Timer { get; set; }

        public EmployeeViewModel() => _dao = new EmployeeDAO();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' + MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task GetByEmail()
        {
            try
            {
                Employee s = await _dao.GetByEmail(Email);
                Title = s.Title;
                Firstname = s.FirstName;
                Lastname = s.LastName;
                Phoneno = s.PhoneNo;
                Email = s.Email;
                Id = s.Id;
                DepartmentId = s.DepartmentId;
                if (s.StaffPicture != null)
                    StaffPicture64 = Convert.ToBase64String(s.StaffPicture);
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
                Employee s = await _dao.GetById(Id);
                Title = s.Title;
                Firstname = s.FirstName;
                Lastname = s.LastName;
                Phoneno = s.PhoneNo;
                Email = s.Email;
                Id = s.Id;
                DepartmentId = s.DepartmentId;
                if (s.StaffPicture != null)
                    StaffPicture64 = Convert.ToBase64String(s.StaffPicture);
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

        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> all_vm = new();
            try
            {
                var Employees = await _dao.GetAll();
                foreach (Employee Employee in Employees)
                {
                    EmployeeViewModel evm = new()
                    {
                        Title = Employee.Title,
                        Firstname = Employee.FirstName,
                        Lastname = Employee.LastName,
                        Phoneno = Employee.PhoneNo,
                        Email = Employee.Email,
                        Id = Employee.Id,
                        DepartmentId = Employee.DepartmentId,
                        Timer = Convert.ToBase64String(Employee.Timer),
                    };
                    all_vm.Add(evm);
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
                Employee stu = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId
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
                Employee stu = new()
                {
                    Id = Id,
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId,
                };
                if (StaffPicture64 != null)
                    stu.StaffPicture = Convert.FromBase64String(StaffPicture64);
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
