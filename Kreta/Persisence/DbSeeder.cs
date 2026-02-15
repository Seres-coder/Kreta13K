namespace Kreta.Persisence
{
    public class DbSeeder
    {
      
            public static void Seed(KretaDbContext db)
            {
                // Ha már van adat, ne seedeljünk újra
                if (db.Users.Any()) return;

                // =========================
                // USERS
                // =========================
                var users = new List<User>
            {
                new User { belepesnev = "admin", jelszo = "admin123", Role = "Admin" },
                new User { belepesnev = "tanar1", jelszo = "tanar123", Role = "Tanar" },
                new User { belepesnev = "diak1", jelszo = "diak123", Role = "Diak" },
                new User { belepesnev = "diak2", jelszo = "diak123", Role = "Diak" }
            };

                db.Users.AddRange(users);
                db.SaveChanges();

                // =========================
                // OSZTALYOK
                // =========================
                var osztalyok = new List<Osztaly>
            {
                new Osztaly { osztaly_nev = "9.A" },
                new Osztaly { osztaly_nev = "10.B" }
            };

                db.Osztalyok.AddRange(osztalyok);
                db.SaveChanges();

                // =========================
                // TANTÁRGYAK
                // =========================
                var tantargyak = new List<Tantargy>
            {
                new Tantargy { tantargy_nev = "Matematika" },
                new Tantargy { tantargy_nev = "Magyar" },
                new Tantargy { tantargy_nev = "Informatika" }
            };

                db.Tantargyok.AddRange(tantargyak);
                db.SaveChanges();

                // =========================
                // TANÁR
                // =========================
                var tanar = new Tanar
                {
                    tanar_nev = "Kovács Péter",
                    szak = "Matematika",
                    user_id = users[1].user_id,
                    tantargy_id = tantargyak[0].tantargy_id
                };

                db.Tanarok.Add(tanar);
                db.SaveChanges();

                // =========================
                // DIÁKOK
                // =========================
                var diakok = new List<Diak>
            {
                new Diak
                {
                    diak_nev = "Nagy Anna",
                    user_id = users[2].user_id,
                    osztaly_id = osztalyok[0].osztaly_id,
                    szuletesi_datum = new DateTime(2008, 5, 12),
                    lakcim = "Budapest",
                    szuloneve = "Nagy Éva",
                    emailcim = "anna@email.com"
                },
                new Diak
                {
                    diak_nev = "Kiss Bence",
                    user_id = users[3].user_id,
                    osztaly_id = osztalyok[0].osztaly_id,
                    szuletesi_datum = new DateTime(2008, 8, 22),
                    lakcim = "Debrecen",
                    szuloneve = "Kiss Mária",
                    emailcim = "bence@email.com"
                }
            };

                db.Diakok.AddRange(diakok);
                db.SaveChanges();

                // =========================
                // JEGYEK
                // =========================
                var jegyek = new List<Jegy>
            {
                new Jegy
                {
                    datum = DateTimeOffset.Now,
                    updatedatum = DateTimeOffset.Now,
                    ertek = 5,
                    tantargy_id = tantargyak[0].tantargy_id,
                    tanar_id = tanar.tanar_id
                },
                new Jegy
                {
                    datum = DateTimeOffset.Now,
                    updatedatum = DateTimeOffset.Now,
                    ertek = 4,
                    tantargy_id = tantargyak[0].tantargy_id,
                    tanar_id = tanar.tanar_id
                }
            };

                db.Jegyek.AddRange(jegyek);
                db.SaveChanges();

                // =========================
                // ÓRAREND
                // =========================
                var orarend = new Orarend
                {
                    osztaly_id = osztalyok[0].osztaly_id,
                    nap = DayOfWeek.Monday,
                    ora = "08:00-08:45",
                    tantargy_id = tantargyak[0].tantargy_id,
                    tanar_id = tanar.tanar_id
                };

                db.Orarendek.Add(orarend);
                db.SaveChanges();

                // =========================
                // HIÁNYZÁS
                // =========================
                var hianyzas = new Hianyzas
                {
                    hianyzottorakszama = 3
                };

                db.Hianyzasok.Add(hianyzas);
                db.SaveChanges();
            }
        }
    }
