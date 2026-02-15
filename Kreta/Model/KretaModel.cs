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
        #region 1. Jegyek Hozzadasa
        public async Task Addjegy(int _jegy_id, int _ertek)
        {
            var jegy = _context.Jegyek.Where(x=> x.jegy_id == _jegy_id).FirstOrDefault();
            var trx = _context.Database.BeginTransaction();
            {
                _context.Jegyek.Add(new Jegy
                {
                    jegy_id = _jegy_id,
                    ertek = _ertek,
                    datum = DateTimeOffset.UtcNow
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion

        #region 2. Jegyek Modositasa
        public async Task ModifyJegy(int _jegy_id, int _ertek) 
        {
            using (var trx = _context.Database.BeginTransaction()) 
            {
                _context.Jegyek.Where(x=> x.jegy_id==_jegy_id).First().ertek=_ertek;
                _context.Jegyek.Where(x=> x.jegy_id==_jegy_id).First().updatedatum=DateTimeOffset.UtcNow;
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion

        #region 3. Jegyek Torlese
        public async Task DeleteJegy(int _jegy_id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Jegyek.Remove(_context.Jegyek.Where(x=>x.jegy_id== _jegy_id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion

        #region 4. Hianyzas Hozzadasa
        public async Task AddHianyzas(int _hianyzas_id, int _hianyzottorakszama)
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
        #endregion

        #region 5. Hianyzas Modositasa
        public async Task ModifyHianyzas(int _hianyzas_id, int _hianyzottorakszama)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Hianyzasok.Where(x => x.hianyzas_id == _hianyzas_id).First().hianyzottorakszama = _hianyzottorakszama;
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion

        #region 6. Hianyzas Torlese
        public async Task DeleteHianyzas(int _hianyzas_id) 
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Hianyzasok.Remove(_context.Hianyzasok.Where(x => x.hianyzas_id == _hianyzas_id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion

        #region 7. Orarend hozzadasa
        public async Task AddOrarend(int _orarend_id, int _osztaly_id, Osztaly _osztaly, DayOfWeek _nap, string _ora, int _tantargy_id, List<Tanar> _Tanar)
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
        #endregion

        #region 8. Ora hozzadasa az orarendhez
        public async Task AddLesson(string _ora, DayOfWeek _nap, string _osztaly, string _tantargy, string _tanar)
        {

            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Orarendek.Add(new Orarend
                {
                    ora = _ora,
                    nap = _nap,
                    osztaly_id = _context.Osztalyok.Where(x => x.osztaly_nev == _osztaly).First().osztaly_id,
                    tantargy_id = _context.Tantargyok.Where(x=> x.tantargy_nev == _tantargy).First().tantargy_id,
                    tanar_id = _context.Tanarok.Where(x => x.tanar_nev == _tanar).First().tanar_id,
                   
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion

        #region Ora torlese
        public async Task DeleteLesson(string _ora, DayOfWeek _nap) 
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.Orarendek.Remove(_context.Orarendek.Where(x => x.nap == _nap && x.ora == _ora).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        #endregion


    }
}
