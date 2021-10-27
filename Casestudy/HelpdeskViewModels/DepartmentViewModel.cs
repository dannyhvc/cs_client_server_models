using HelpdeskDAL;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private readonly DepartmentDAO _dao;
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Timer { get; set; }

        public DepartmentViewModel() => _dao = new DepartmentDAO();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' + MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> all_vm = new();
            try
            {
                var derpartment = await _dao.GetAll();
                foreach (Department depo in derpartment)
                {
                    DepartmentViewModel svm = new()
                    {
                        Id = depo.Id,
                        Name = depo.DepartmentName,
                        Timer = Convert.ToBase64String(depo.Timer),
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

    }
}
