using Kreta.Dto;
using Kreta.Persisence;

namespace Kreta.Model
{
    public class UzenetModel
    {
        private readonly KretaDbContext _context;
        public UzenetModel(KretaDbContext context)
        {
            _context = context;
        }

        //Üzenet küldése Tanárként vagy Adminként egy diáknak

        public void SendUzenet(string tartalom, string cim, int fogado_id, int user_id)
        {
            var trx = _context.Database.BeginTransaction();
            {
                _context.Uzenetek.Add(new Uzenet
                {
                    tartalom = tartalom,
                    cim = cim,
                    fogado_id = fogado_id,
                    user_id = user_id
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }

        //Üzenet lekérdezése diák számára
        public IEnumerable<UzenetekDto> GetUzenetByDiakId(int diak_id)
        {
            return _context.Uzenetek.Where(x => x.fogado_id == diak_id).Select(x => new UzenetekDto
            {
                uzenet_id = x.uzenet_id,
                tartalom = x.tartalom,
                cim = x.cim,
                fogado_id = x.fogado_id,
                user_id = x.user_id
            }).OrderBy(x => x.uzenet_id);
        }

        //Üzenet törlése csak Adminként 
        public void DeleteUzenet(int uzenetid)
        {
            if(_context.Uzenetek.Any(x => x.uzenet_id != uzenetid))
            {
                throw new InvalidOperationException("Nincs ilyen uzenet");
            }
            var trx = _context.Database.BeginTransaction();
            {
                _context.Uzenetek.Remove(_context.Uzenetek.Where(x=>x.uzenet_id==uzenetid).FirstOrDefault());
                _context.SaveChanges();
                trx.Commit();
            }
        }

    }
}
