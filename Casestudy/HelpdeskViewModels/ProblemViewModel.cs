using HelpdeskDAL;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private readonly ProblemDAO _dao;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Timer { get; set; }

        public ProblemViewModel() => _dao = new ProblemDAO();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' + MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task<List<ProblemViewModel>> GetAll()
        {
            List<ProblemViewModel> all_vm = new();
            try
            {
                var derpartment = await _dao.GetAll();
                foreach (Problem depo in derpartment)
                {
                    ProblemViewModel svm = new()
                    {
                        Id = depo.Id,
                        Name = depo.Description,
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
