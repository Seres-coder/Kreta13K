using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Kreta.Persisence
{
    public class KretaDbContext : DbContext
    {

    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        public string belepesnev { get; set; }
        public string jelszo { get; set; }
        public string Role { get; set; } = "User";
    }
    public class Tanar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tanar_id { get; set; }
        public string tanar_nev { get; set; }
        public string szak { get; set; }
        public int diak_id { get; set; }
        public List<Diak> diak { get; set; }

        public int jegy_id { get; set; }
        public List<Jegy> jegy { get; set; }
        public int tantargy_id { get; set; }
        public Tantargy Tantargy { get; set; }

        public string Role { get; set; } = "Tanar";

    }

    public class Diak
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int diak_id { get; set; }

        public int user_id { get; set; }
        public User User { get; set; }

        public int tanar_id { get; set; }
        public List<Tanar> Tanar { get; set; }
        public DateTime szuletesi_datum { get; set; }
        public string lakcim { get; set; }
        public string szuloneve { get; set; }
        public string emailcim { get; set; }
        public int jegyek { get; set; }

        public string Role { get; set; } = "Diak";
    }
    public class Tantargy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tantargy_id { get; set; }
    }

    public class Uzenetek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int uzenet_id { get; set; }
        public string tartalom { get; set; }
        public string cim { get; set; }
        public int user_id { get; set; }
        public User User { get; set; }

    }

    public class Jegy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int jegy_id { get; set; }
        public DateTime datum { get; set; }
        public int ertek { get; set; }

        public int tantargy_id { get; set; }
        public Tantargy tantargy { get; set; }

        public int tanar_id { get; set; }
        public List<Tanar> Tanar { get; set; }
    }

    public class Orarend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orarend_id { get; set; }
        public int osztaly_id { get; set; }
        public Osztaly osztaly { get; set; }

        public DateTime nap { get; set; }
        public string ora { get; set; }
        public int tantargy_id { get; set; }
        public Tantargy tantargy { get; set; }

        public int tanar_id { get; set; }
        public List<Tanar> Tanar { get; set; }
    }

    public class Osztaly
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int osztaly_id { get; set; }
    }





}
