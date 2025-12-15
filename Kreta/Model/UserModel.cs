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
                    _context.Diakok.Add(new Diak { });
                } else if (role == "Tanar")
                {
                    _context.Tanarok.Add(new Tanar { });
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

        // Diakok neveinek kiírása 
        public IEnumerable<UserDto> GetDiak()
        {
            return _context.Diakok.Include(x=> x.User).Select(x => new UserDto { _belepesnev = x.User.belepesnev }).OrderBy(x => x._belepesnev);
        }
        // Tanarok neveinek kiírása
        public IEnumerable<UserDto> GetTanar()
        {
            return _context.Tanarok.Include(x => x.User).Select(x => new UserDto { _belepesnev = x.User.belepesnev }).OrderBy(x => x._belepesnev);
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
    }

}
