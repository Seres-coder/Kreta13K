using Kreta.Dto;
using Kreta.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kreta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzenetController : ControllerBase
    {
        private readonly UzenetModel _model;
        public UzenetController(UzenetModel model)
        {
            _model = model;
        }
        //Üzenet küldése Tanárként vagy Adminként egy diáknak
        [HttpPost("/UzenetKuldese")]
        public ActionResult UzenetekKuldese(string tartalom, string cim, int fogado_id, int user_id)
        {
            try
            {
                _model.SendUzenet( tartalom,  cim,  fogado_id,  user_id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }



        //Üzenet lekérdezése diák számára

        [Authorize(Roles = "Diak")]
        [HttpGet("/UzenetLekerdezese")]
        public ActionResult<IEnumerable<UzenetekDto>> UzenetekByDiak(int diak_id)
        {
            try
            {
                return Ok(_model.GetUzenetByDiakId(diak_id));
            }
            catch
            {
                return BadRequest();
            }
        }


        //Üzenet törlése csak Adminként 
        [Authorize(Roles = "Admin")]
        [HttpDelete("/deletemessage/{id}")]
        public ActionResult DeleteUzenetById([FromQuery] int id)
        {
            try
            {
                _model.DeleteUzenet(id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("nincs ilyen uzenet");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


    }
}
