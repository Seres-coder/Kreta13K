using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Kreta.Persisence;
using Microsoft.EntityFrameworkCore;

namespace Kreta.Model
{
    public class KretaModel
    {
        private readonly KretaDbContext _context;
        public KretaModel(KretaDbContext context)
        {
            _context = context;
        }
        //Jegy beirasa
        public void Addjegy(int _jegy_id, int _ertek, DateTime _datum)
        {
            var jegy = _context.Jegyek.Where(x=> x.jegy_id == _jegy_id).FirstOrDefault();
            var trx = _context.Database.BeginTransaction();
            {
                _context.Jegyek.Add(new Jegy
                {
                    jegy_id = _jegy_id,
                    ertek = _ertek,
                    datum = _datum
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Jegy modositas
        public void ModifyJegy(int _jegy_id, int _ertek) 
        {
            using (var trx = _context.Database.BeginTransaction()) 
            {
                _context.Jegyek.Where(x=> x.jegy_id==_jegy_id).First().ertek=_ertek;
                _context.Jegyek.Where(x=> x.jegy_id==_jegy_id).First().updatedatum=DateTimeOffset.UtcNow;
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Jegy torlese
        public void DeleteJegy(int _jegy_id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Jegyek.Remove(_context.Jegyek.Where(x=>x.jegy_id== _jegy_id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        /*/ public class Uzenet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int uzenet_id { get; set; }
        public string tartalom { get; set; }
        public string cim { get; set; }
        public int user_id { get; set; }
        public User User { get; set; }

    }
        /*/
        
        
        //Hianyzas hozzaadasa
        public void AddHianyzas(int _hianyzas_id, int _hianyzottorakszama)
        {
            var hianyzas = _context.Hianyzasok.Where(x => x.hianyzas_id == _hianyzas_id).FirstOrDefault();
            var trx = _context.Database.BeginTransaction();
            {
                _context.Hianyzasok.Add(new Hianyzas
                {
                    hianyzas_id = _hianyzas_id,
                    hianyzottorakszama = _hianyzottorakszama
                });
                _context.SaveChanges();
                trx.Commit();
            }

        }
        //Hianyzasok modositasa
        public void ModifyHianyzas(int _hianyzas_id, int _hianyzottorakszama)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Hianyzasok.Where(x => x.hianyzas_id == _hianyzas_id).First().hianyzottorakszama = _hianyzottorakszama;
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Hianyzas torlese
        public void DeleteHianyzas(int _hianyzas_id) 
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Hianyzasok.Remove(_context.Hianyzasok.Where(x => x.hianyzas_id == _hianyzas_id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        /*
        public int orarend_id { get; set; }
        public int osztaly_id { get; set; }
        public Osztaly osztaly { get; set; }

        public DateTime nap { get; set; }
        public string ora { get; set; }
        public int tantargy_id { get; set; }
        public Tantargy tantargy { get; set; }

        public int tanar_id { get; set; }
        public List<Tanar> Tanar { get; set; }
        */
        //Orarend hozzaadasa
        public void AddOrarend(int _orarend_id, int _osztaly_id, Osztaly _osztaly, DayOfWeek _nap, string _ora, int _tantargy_id, List<Tanar> _Tanar)
        {
            var orarend = _context.Orarendek.Where(x => x.orarend_id == _orarend_id).FirstOrDefault();
            var trx = _context.Database.BeginTransaction();
            {

                _context.Orarendek.Add(new Orarend
                {
                    orarend_id = _orarend_id,
                    osztaly_id = _osztaly_id,
                    nap = _nap,
                    ora = _ora,
                    tantargy_id = _tantargy_id,
                    Tanar = _Tanar
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        //Ora hozzaadasa az orarendhez
        public void AddLesson(string _ora, DayOfWeek _nap, Osztaly _osztaly, Tantargy _tantargy, Tanar _tanar,int _orarend_id)
        {
            var lesson = _context.Orarendek.Where(x => x.orarend_id == _orarend_id).FirstOrDefault();
            var trx = _context.Database.BeginTransaction();
            {
                _context.Orarendek.Add(new Orarend
                {
                    ora = _ora,
                    nap = _nap,
                    osztaly_id = _osztaly,
                    tantargy_id = _tantargy,
                    tanar_id = _tanar
                });
            }
        }
        //Ora torlese az orarendbol
        public void DeleteLesson(string _ora, DayOfWeek _nap) 
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Orarendek.Remove(_context.Orarendek.Where(x => x.nap == _nap && x.ora == _ora).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }


    }
}
