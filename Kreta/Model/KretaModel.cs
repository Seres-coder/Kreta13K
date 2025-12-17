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
        
        public void AddUzenet(int _uzenet_id, string _tartalom, string _cim, int _user_id)
        {
            var uzenet = _context.Uzenetek.Where(x => x.uzenet_id == _uzenet_id).FirstOrDefault();
            var trx = _context.Database.BeginTransaction();
            {
                _context.Uzenetek.Add(new Uzenet
                {
                    uzenet_id = _uzenet_id,
                    tartalom = _tartalom,
                    cim = _cim,
                    user_id = _user_id
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public void AddHianyzas()
        {
            
            
        }
        public void ModifyHianyzas()
        {

        }
        public void DeleteHianyzas() 
        {

        }

    }
}
