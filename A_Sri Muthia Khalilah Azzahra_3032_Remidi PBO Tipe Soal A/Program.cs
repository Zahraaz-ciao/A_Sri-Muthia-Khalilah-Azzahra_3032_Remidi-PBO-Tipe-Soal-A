namespace MBG
{
    public class Sekolah
    {
        public string IdSekolah { get; set; }
        public string NamaSekolah { get; set; }
        public int JumlahPaketDiterima { get; set; }

        public Sekolah(string idSekolah, string namaSekolah, int jumlahPaketDiterima)
        {
            IdSekolah = idSekolah;
            NamaSekolah = namaSekolah;
            JumlahPaketDiterima = jumlahPaketDiterima;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"Sekolah dengan ID: {IdSekolah} Nama: {NamaSekolah} Paket Diterima: {JumlahPaketDiterima}");
        }
    }
    // ENCAPSULATION & ABSTRACT CLASS 
    public abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;
        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }
        public int StokDapur
        {
            get { return stokDapur; }
            private set
            {
                if (value >= 0)
                    stokDapur = value;
                else
                    stokDapur = 0;
            }
        }

        public PaketMBG(string idPaket, string namaPaket, int stokDapur)
        {
            IdPaket = idPaket;
            NamaPaket = namaPaket;
            StokDapur = stokDapur;
        }
        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Gagal: Jumlah pengurangan harus lebih dari 0.");
                return false;
            }
            if (jumlah > StokDapur)
            {
                Console.WriteLine($"Gagal: Stok {NamaPaket} tidak mencukupi (Tersedia: {StokDapur}, Diminta: {jumlah}).");
                return false;
            }

            StokDapur -= jumlah;
            return true;
        }
        public abstract int HitungBiaya(int jumlah);
    }

    // INHERITANCE & POLYMORPHISM 
    public class PaketReguler : PaketMBG
    {
        public PaketReguler(string idPaket, string namaPaket, int stokDapur)
            : base(idPaket, namaPaket, stokDapur)
        {
        }
        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 15000;
        }
    }

    public class PaketKhusus : PaketMBG
    {
        public string JenisDiet { get; set; }

        public PaketKhusus(string idPaket, string namaPaket, int stokDapur, string jenisDiet)
            : base(idPaket, namaPaket, stokDapur)
        {
            JenisDiet = jenisDiet;
        }
        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    //INTERFACE & DISTRIBUSI MBG
    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiMBG : DapatDidistribusikan
    {
        public Sekolah SekolahTujuan { get; set; }
        public PaketMBG PaketMakanan { get; set; }
        public int Jumlah { get; set; }

        public DistribusiMBG(Sekolah sekolahTujuan, PaketMBG paketMakanan, int jumlah)
        {
            SekolahTujuan = sekolahTujuan;
            PaketMakanan = paketMakanan;
            Jumlah = jumlah;
        }

        public void KirimPaket()
        {
            if (Jumlah <= 0)
            {
                Console.WriteLine("Distribusi gagal! Jumlah paket yang dikirim harus lebih dari 0.");
                return;
            }
            if (Jumlah > PaketMakanan.StokDapur)
            {
                Console.WriteLine($"Distribusi gagal! Stok dapur ({PaketMakanan.StokDapur}) tidak mencukupi untuk mengirim {Jumlah} paket.");
                return;
            }
            bool berhasilKurangi = PaketMakanan.KurangiStok(Jumlah);
            if (berhasilKurangi)
            {
                SekolahTujuan.JumlahPaketDiterima += Jumlah;
                Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {Jumlah} paket makanan telah dikirim ke sekolah {SekolahTujuan.NamaSekolah}.");
            }
        }
    }

    // PENGELOLA MBG (Aggregation)
    public class PengelolaMBG
    {
        private List<Sekolah> daftarSekolah = new List<Sekolah>();
        private List<PaketMBG> daftarPaket = new List<PaketMBG>();

        public void TambahSekolah(Sekolah sekolah)
        {
            daftarSekolah.Add(sekolah);
        }

        public void TambahPaket(PaketMBG paket)
        {
            daftarPaket.Add(paket);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("\nDATA SEKOLAH PENERIMA MBG");
            foreach (var s in daftarSekolah)
            {
                s.TampilInfo();
            }

            Console.WriteLine("\nDATA PERSEDIAAN PAKET DAPUR");
            foreach (var p in daftarPaket)
            {
                Console.WriteLine($"ID: {p.IdPaket}  Nama: {p.NamaPaket}  Sisa Stok: {p.StokDapur}");
            }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Sekolah sekolah1 = new Sekolah("01", "SDN Petra", 0);
            Sekolah sekolah2 = new Sekolah("02", "SMP Nusa", 10);
            sekolah1.TampilInfo();
            sekolah2.TampilInfo();

            Console.WriteLine("\nHitungBiaya");
            PaketMBG reguler = new PaketReguler("PKT-01", "Paket Nasi Ayam & Buah", 100);
            PaketMBG khusus = new PaketKhusus("PKT-02", "Paket Diet Rendah Gula", 50, "Rendah Gula");

            Console.WriteLine($"Biaya 20 {reguler.NamaPaket}: Rp{reguler.HitungBiaya(20)}");
            Console.WriteLine($"Biaya 15 {khusus.NamaPaket}: Rp{khusus.HitungBiaya(15)}");

            Console.WriteLine("\nINTERFACE & DISTRIBUSI MBG");
            DistribusiMBG dist1 = new DistribusiMBG(sekolah1, reguler, 40);
            dist1.KirimPaket();
            DistribusiMBG dist2 = new DistribusiMBG(sekolah2, khusus, 70);
            dist2.KirimPaket();

            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(sekolah1);
            pengelola.TambahSekolah(sekolah2);
            pengelola.TambahPaket(reguler);
            pengelola.TambahPaket(khusus);
            pengelola.TampilkanDataMBG();
        }
    }
}