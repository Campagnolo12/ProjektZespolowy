using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql_test
{
    class Pola {
        public
        string nazwa;
        int ilosc_domkow;
        int koszt_uslugi;
        int koszt_zakupu;
        int koszt_domkow;
        float mnoznik;
        int wlasciciel;
        public Pola() {
            ilosc_domkow = 0;
        }
        public void edit( string nnazwa, int nilosc_domkow, int nkoszt_uslugi, int nkoszt_zakupu, int nkoszt_domkow, float nmnoznik, int nwlasciciel)
        {
            nazwa = nnazwa;
            ilosc_domkow = nilosc_domkow;
            koszt_uslugi = nkoszt_uslugi;
            koszt_domkow = nkoszt_domkow;
            koszt_zakupu = nkoszt_zakupu;
            mnoznik = nmnoznik;
            wlasciciel = nwlasciciel;
        }
        public void wypisz() {
            Console.WriteLine(nazwa + " " + ilosc_domkow + " " + koszt_uslugi + " " + " " + koszt_zakupu + " " + koszt_domkow + " " + mnoznik + " " + wlasciciel);
        }
    }

     
}
