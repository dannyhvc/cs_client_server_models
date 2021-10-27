using ExercisesDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExercisesDAL
{
    public class DivisionDAO
    {
        readonly private IRepository<Division> repository;
        public DivisionDAO() => repository = new SomeSchoolRepository<Division>();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                $"Problem in {GetType().Name} {MethodBase.GetCurrentMethod().Name} {ex.Message}"
            );

        public async Task<List<Division>> GetAll()
        {
            List<Division> divisions = new();

            try
            {
                divisions = await repository.GetAll();
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }

            return divisions;
        }
    }
}
