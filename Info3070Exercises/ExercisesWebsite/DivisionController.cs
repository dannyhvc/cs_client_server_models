using ExercisesViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExercisesWebsite
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : ControllerBase
    {

        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' + MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                DivisionViewModel vm = new();
                List<DivisionViewModel> all_divisions = await vm.GetAll();
                return Ok(all_divisions);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
