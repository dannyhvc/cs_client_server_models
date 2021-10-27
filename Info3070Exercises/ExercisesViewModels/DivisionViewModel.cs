using ExercisesDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExercisesViewModels
{
    public class DivisionViewModel
    {
        private readonly DivisionDAO _dao;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Timer { get; set; }


        public DivisionViewModel() => _dao = new DivisionDAO();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                $"Problem in {GetType().Name} {MethodBase.GetCurrentMethod().Name} {ex.Message}"
            );

        public async Task<List<DivisionViewModel>> GetAll()
        {
            List<DivisionViewModel> all_vm = new();
            try
            {
                var divisions = await _dao.GetAll();
                foreach (Division division in divisions)
                {
                    DivisionViewModel dvm = new()
                    {
                        Id = division.Id,
                        Name = division.Name,
                        Timer = Convert.ToBase64String(division.Timer),
                    };
                    all_vm.Add(dvm);
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
