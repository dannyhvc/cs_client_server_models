using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        readonly private IRepository<Department> repo;
        public DepartmentDAO() => repo = new HelpdeskRepository<Department>();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' +
                MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task<List<Department>> GetAll()
        {
            List<Department> departments = new();
            try
            {
                departments = await repo.GetAll();
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return departments;
        }
    }
}
