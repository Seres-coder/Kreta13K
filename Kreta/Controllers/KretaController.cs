using Kreta.Model;
using Kreta.Persisence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kreta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KretaController : ControllerBase
    {
        private readonly KretaModel _model;
        public KretaController(KretaModel model)
        {
            _model = model;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/addjegy")]
        public ActionResult JegyBeiras(int _jegy_id, int _ertek)
        {
            try
            {
                _model.Addjegy(_jegy_id,_ertek);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/deletejegy/{id}")]
        
        public ActionResult JegyDelete([FromQuery] int id)
        {
            try
            {
                _model.DeleteJegy(id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("nincs ez a jegy");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/modifyjegy")]
        public ActionResult ModifyJegy(int _jegy_id, int _ertek)
        {
            try
            {
                _model.ModifyJegy(_jegy_id, _ertek);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/AddHianyzas")]
        public ActionResult HianyzasBeiras(int _hianyzas_id, int _hianyzottorakszama)
        {
            try
            {
                _model.AddHianyzas(_hianyzas_id, _hianyzottorakszama);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/deletehianyzas/{id}")]

        public ActionResult HianyzasDelete([FromQuery] int id)
        {
            try
            {
                _model.DeleteHianyzas(id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("nincs ez a jegy");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/modifyhianyzas")]
        public ActionResult ModifyHianyzas(int _hianyzas_id, int _hianyzottorakszama)
        {
            try
            {
                _model.ModifyHianyzas(_hianyzas_id, _hianyzottorakszama);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/addorarend")]
        public ActionResult AddOrarend(int _orarend_id, int _osztaly_id, Osztaly _osztaly, DayOfWeek _nap, string _ora, int _tantargy_id, List<Tanar> _Tanar)
        {
            try
            {
                _model.AddOrarend(_orarend_id, _osztaly_id, _osztaly,_nap,_ora,_tantargy_id,_Tanar);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("/addora")]
        public ActionResult AddLesson(string _ora, DayOfWeek _nap, Osztaly _osztaly, Tantargy _tantargy, Tanar _tanar, int _orarend_id)
        {
            try
            {
                _model.AddLesson(_ora, _nap, _osztaly, _tantargy, _tanar, _orarend_id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/deleteora/{id}")]

        public ActionResult DeleteLesson(string _ora, DayOfWeek _nap)
        {
            try
            {
                _model.DeleteLesson(_ora, _nap);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("nincs ilyen ora");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
