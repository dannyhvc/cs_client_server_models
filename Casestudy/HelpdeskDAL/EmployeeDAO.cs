using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        readonly private IRepository<Employee> repo;
        public EmployeeDAO() => repo = new HelpdeskRepository<Employee>();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' +
                MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task<Employee> GetByEmail(string email)
        {
            Employee employee = null;
            try
            {
                employee = await repo.GetOne(emp => emp.Email == email);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return employee;
        }

        public async Task<Employee> GetById(int id)
        {
            Employee employee = null;
            try
            {
                employee = await repo.GetOne(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return employee;
        }

        public async Task<List<Employee>> GetAll()
        {
            List<Employee> employees = new();

            try
            {
                employees = await repo.GetAll();
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }

            return employees;

        }

        public async Task<int> Add(Employee employee)
        {
            try
            {
                await repo.Add(employee);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return employee.Id;
        }

        public async Task<UpdateStatus> Update(Employee employee)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await repo.Update(employee);
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
                deleted = await repo.Delete(id);
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
