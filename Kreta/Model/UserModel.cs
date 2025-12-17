using Kreta.Dto;
using System.Text;
using Kreta.Persisence;
using Microsoft.EntityFrameworkCore;

namespace Kreta.Model
{
    public class UserModel
    {

        private readonly KretaDbContext _context;
        public UserModel(KretaDbContext context)
        {
            _context = context;
        }

        //jelszo titkositasa 
        private string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        //Regisztráció felhasználó létrehozásával alapbol diakkent tudsz csak regisztrálni de majd kesőbb lehet tanár is az admin segitségével
        public void Registration(string name, string password, string role ="Diak")
        {
            if (_context.Users.Any(x => x.belepesnev == name))
            {
                throw new InvalidOperationException("mar letezik");
            }
            var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Add(new User { belepesnev = name, jelszo = HashPassword(password), Role = role });
                int id = _context.Users.Last().user_id;/*reménykedjünk, hogy működik--> todo*/
                if (role == "Diak")
                {
                    _context.Diakok.Add(new Diak { 
                    diak_id = id,
                    });
                } 
                    _context.SaveChanges();
                trx.Commit();
            }
        }

        //Bejelentkezés felhasználó ellenőrzéssel

        public UserDto? ValidateUser(string name, string password,string role="Diak")
        {
            string hashpass = HashPassword(password);
            var user = _context.Users.Where(x => x.belepesnev == name);
            return user.Where(x => x.jelszo == hashpass).Select(x => new UserDto
            {
                _belepesnev = x.belepesnev,
                _jelszo = x.jelszo,
                _Role = x.Role,

            }).FirstOrDefault();
        }

        // Diakok nevei,osztálya kiírása 
        public IEnumerable<UserDto> GetDiak()
        {
            return _context.Diakok.Include(x=> x.User).Include(x=>x.Osztaly)
                .Select(x => 
                new UserDto { _belepesnev = x.User.belepesnev,_osztaly=x.Osztaly.osztaly_nev })
                .OrderBy(x => x._belepesnev);
        }
        // Tanarok nevei,tantargyanaj  kiírása
        public IEnumerable<UserDto> GetTanar()
        {
            return _context.Tanarok.Include(x => x.User).Include(x=>x.Tantargy)
                .Select(x => new UserDto { _belepesnev = x.User.belepesnev,_tantargy=x.Tantargy.tantargy_nev })
                .OrderBy(x => x._belepesnev);
        }

        //Diakok és Tanárok teljes törlése a felhasználó törlésével
        public void DeleteUser(int userId)
        {
            var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Remove(
                    _context.Users.Where(x => x.user_id == userId).First()
                );
                if(_context.Diakok.Any(x=> x.diak_id == userId))
                {
                    _context.Diakok.Where(x => x.diak_id == userId).First();
                }
                else if (_context.Tanarok.Any(x => x.tanar_id == userId))
                {
                    _context.Tanarok.Where(x => x.tanar_id == userId).First();
                }
                _context.SaveChanges();
                trx.Commit();
            }
        }
        // Felhasználó diákról tanárra váltása
        public void PromoteToTanar(int userId,string tantargy)
        {
            var trx = _context.Database.BeginTransaction();
            {
               var user= _context.Diakok.Where(x => x.diak_id == userId).First();
               var tantargyid=_context.Tantargyok.Where(x=>x.tantargy_nev == tantargy).First().tantargy_id;
                _context.Diakok.Remove(user);
                _context.Tanarok.Add(new Tanar
                {
                    user_id = userId,
                    tanar_id = userId,
                    Role = "Tanar",
                    tantargy_id = tantargyid,

                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Diak és tanár jelszó váltás
        public void ChangePassword(int userId, string ujjelszo)
        {
            var trx = _context.Database.BeginTransaction();
            {
                var user = _context.Users.Where(x => x.user_id == userId).First().jelszo = HashPassword(ujjelszo); 
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Diak profil teljes listázása
        public IEnumerable<DiakListDto> GetFullListDiak()
        {
            return _context.Diakok.Select(x=> new DiakListDto
            {
                _diak_id = x.diak_id,
                _osztaly_id = x.osztaly_id,
                _tanar_id = x.tanar_id,
                _szuletesi_datum = x.szuletesi_datum,
                _lakcim = x.lakcim,
                _szuloneve = x.szuloneve,
                _emailcim = x.emailcim,
                _jegyek = x.jegyek,
            }).OrderBy(x => x._diak_nev);
        }
        //Tanar profil teljes listázása
        public IEnumerable<TanarListDto> GetFullListTanar()
        {
            return _context.Tanarok.Select(x => new TanarListDto
            {
                _tanar_id = x.tanar_id,
                _tanar_nev = x.tanar_nev,
                _szak = x.szak,
                _diak_id = x.diak_id,
                _tantargy_id = x.tantargy_id,
            }).OrderBy(x => x._tanar_nev);
        }

        //Diak Adatok modositása

        public void ModifyDiak(ModifyDiakDto dto)
        {
            var trx = _context.Database.BeginTransaction();
            {
                _context.Diakok.Where(x=>x.diak_id==dto._diak_id).First().diak_nev = dto._diak_nev;
                _context.Diakok.Where(x => x.diak_id == dto._diak_id).First().osztaly_id = dto._osztaly_id;
                _context.Diakok.Where(x => x.diak_id == dto._diak_id).First().szuletesi_datum = dto._szuletesi_datum;
                _context.Diakok.Where(x => x.diak_id == dto._diak_id).First().lakcim = dto._lakcim;
                _context.Diakok.Where(x => x.diak_id == dto._diak_id).First().szuloneve = dto._szuloneve;
                _context.Diakok.Where(x => x.diak_id == dto._diak_id).First().emailcim = dto._emailcim;
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Tanar Adatok modositása   
        public void ModifyTanar(ModifyTanarDto dto)
        {
            var trx = _context.Database.BeginTransaction();
            {
                _context.Tanarok.Where(x => x.tanar_id == dto._tanar_id).First().tanar_nev = dto._tanar_nev;
                _context.Tanarok.Where(x => x.tanar_id == dto._tanar_id).First().szak = dto._szak;
                _context.Tanarok.Where(x => x.tanar_id == dto._tanar_id).First().tantargy_id = dto._tantargy_id;
                _context.SaveChanges();
                trx.Commit();
            }
        }
    }

}
