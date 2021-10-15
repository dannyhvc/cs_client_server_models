using ExercisesViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExercisesWebsite
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private void CLS_DBG(Exception ex) =>
            Debug.WriteLine(
                "Problem in " + GetType().Name + ' ' + MethodBase.GetCurrentMethod().Name + ' ' + ex.Message
            );

        [HttpGet("{lastname}")]
        public async Task<IActionResult> GetByLastname(string lastname)
        {
            try
            {
                StudentViewModel vm = new() { Lastname = lastname };
                await vm.GetByLastname();
                return Ok(vm);
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(StudentViewModel viewmodel)
        {
            try
            {
                int retval = await viewmodel.Update();
                return retval switch
                {
                    1 => Ok(new { msg = "Student " + viewmodel.Lastname + " updated!"}),
                    -1 => Ok(new { msg = "Student " + viewmodel.Lastname + " not updated!"}),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Lastname + ", Student not updated!"}),
                    _ => Ok(new { msg = "Student " + viewmodel.Lastname + " not updated!"}),
                };
            }
            catch (Exception ex)
            {
                CLS_DBG(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);  // something went wrong
            }
        }
    }
}
