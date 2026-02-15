using System.Security.Claims;
using Kreta.Dto;
using Kreta.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Kreta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserModel _model;
        public UserController(UserModel model)
        {
            _model = model;
        }
        // Diakok nevei,osztálya kiírása 
        [Authorize(Roles = "Admin,Tanar")]
        [HttpGet("/DiakAlapKiírása")]
        public ActionResult<IEnumerable<UserDto>> GetDiakController()
        {
            try
            {
                return Ok(_model.GetDiak());
            }
            catch
            {
                return BadRequest();
            }
        }
        //Tanarok nevei,tantargyai kiírása
        [Authorize(Roles = "Admin")]
        [HttpGet("/TanarAlapKiírása")]
        public ActionResult<IEnumerable<UserDto>> GetTanarController()
        {
            try
            {
                return Ok(_model.GetTanar());
            }
            catch
            {
                return BadRequest();
            }
        }
        //Regisztráció felhasználó létrehozásával alapbol diakkent tudsz csak regisztrálni de majd kesőbb lehet tanár is az admin segitségével
        [HttpPost("/Registration")]
        public ActionResult RegistrationController(string name, string password)
        {
            try
            {
                _model.Registration(name, password);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //Bejelentkezés felhasználó ellenőrzéssel

        [HttpPost("/Login")]
        public async Task<ActionResult<UserDto>> LoginController(string username, string password)
        {
            try
            {
                var user = _model.ValidateUser(username, password);
                if (null == user)
                {
                    return Unauthorized();
                }
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier,user._user_id.ToString()),
                    new Claim(ClaimTypes.Name,user._belepesnev),
                    new Claim(ClaimTypes.Role,user._Role)
                };
                var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(id);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        //Teljes kijelentkezés a fiókból
        [HttpPost("/logout")]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        //Diakok és Tanárok teljes törlése a felhasználó törlésével
        [Authorize(Roles = "Admin")]
        [HttpDelete("/deleteuser/{id}")]
        public async Task<ActionResult> DeleteUserController([FromQuery] int id)
        {
            try
            {
                _model.DeleteUser(id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound("nincs ilyen felhasznalo");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        // Felhasználó diákról tanárra váltása
        [Authorize(Roles ="Admin")]
        [HttpPut("/UpgradeRole")]
        public ActionResult UpdateRole(int id,string tantargy)
        {
            try
            {
                _model.PromoteToTanar(id, tantargy);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }
        //Diak és tanár jelszó váltás
        [Authorize]
        [HttpPut("/UpdatePassword")]
        public ActionResult UpdatePassword(int userid,string password)
        {
            try
            {
                _model.ChangePassword(userid, password);
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
        //Diak profil teljes listázása
        [Authorize(Roles = "Admin,Tanar")]
        [HttpGet("/DiakLista")]
        public ActionResult<IEnumerable<UserDto>> GetFullListDiakController()
        {
            try
            {
                return Ok(_model.GetFullListDiak());
            }

            catch
            {
                return BadRequest();
            }
        }
        //Tanar profil teljes listázása
        [Authorize(Roles = "Admin")]
        [HttpGet("/TanarLista")]
        public ActionResult<IEnumerable<UserDto>> GetFullListTanarController()
        {
            try
            {
                return Ok(_model.GetFullListTanar());
            }
            catch
            {
                return BadRequest();
            }
        }
        //Diak Adatok modositása
        [Authorize]
        [HttpPut("/DiakModositas")]


        public async Task<ActionResult> ModifyDiakController(int diak__id,string diak__nev,string email__cim,int jegyek__,string lakcim__,int osztaly__id,DateTime szuletesi__datum,string szulo__neve,int tanar__id)
        {
            try
            {
                _model.ModifyDiak(new ModifyDiakDto
                {
                    _diak_id = diak__id,
                    _diak_nev = diak__nev,
                    _emailcim = email__cim,
                    _jegyek = jegyek__,
                    _lakcim = lakcim__,
                    _osztaly_id = osztaly__id,
                    _szuletesi_datum = szuletesi__datum,
                    _szuloneve = szulo__neve,
                    _tanar_id = tanar__id
                });
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Tanar Adatok modositása   
        [Authorize(Roles = "Admin,Tanar")]
        [HttpPut("/TanarModositas")]
        public async Task<ActionResult> ModifyTanarController(int diak__id,string szak__,int tanar__id,string tanar__nev,int tantargy__id)
        {
            try
            {
                _model.ModifyTanar(new ModifyTanarDto
                {
                    _diak_id = diak__id,
                    _szak = szak__,
                    _tanar_id = tanar__id,
                    _tanar_nev = tanar__nev,
                    _tantargy_id = tantargy__id
                });
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



    }
}
