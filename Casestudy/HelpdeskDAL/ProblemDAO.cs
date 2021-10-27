using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        readonly private IRepository<Problem> repo;
        public ProblemDAO() => repo = new HelpdeskRepository<Problem>();

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' +
                MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        public async Task<List<Problem>> GetAll()
        {
            List<Problem> problems = new();

            try
            {
                problems = await repo.GetAll();
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                throw;
            }
            return problems;
        }
    }
}
