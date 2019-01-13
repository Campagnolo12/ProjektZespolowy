using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;

namespace Projekt_zespolowy
{
    class sqlClass
    {
        SqlConnection polaczenie = new SqlConnection();
        string nazwa_gry = "Gra_01";

        public sqlClass(SqlConnection pol)
        {
            polaczenie = pol;
        }

        //public sqlClass(SqlConnection polaczenie)
        //{
        //    this.polaczenie = polaczenie;
        //}

        public void zmiana_kasy(int kwota)
        {
            int hajs;
            // # pobranie ile gotówki ma gracz
            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText = "SELECT hajs FROM " + nazwa_gry + " WHERE tura_gracza=1";
            SqlDataReader czytnik = komenda.ExecuteReader();
            hajs = czytnik.GetInt16(0);
            hajs = hajs + kwota;
            czytnik.Close();

            // # zmian ilości kasy 

            komenda.CommandText = "UPDATE "+nazwa_gry+"SET hajs="+hajs+" WHERE tura_gracza=1" ;
            komenda.ExecuteNonQuery();

        }
        public void transfer_kasy(int kwota)
        {
            int hajs;
            // # zabranie kasy graczowi którego jest tura
            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText = "SELECT hajs FROM " + nazwa_gry + " WHERE tura_gracza=1";
            SqlDataReader czytnik = komenda.ExecuteReader();
            hajs = czytnik.GetInt16(0);
            hajs = hajs - kwota;
            czytnik.Close();
            komenda.CommandText = "UPDATE " + nazwa_gry + "SET hajs=" + hajs + " WHERE tura_gracza=1";
            komenda.ExecuteNonQuery();

            // # Przekazanie kasy graczowi 
            komenda = polaczenie.CreateCommand();
            komenda.CommandText = "SELECT hajs FROM " + nazwa_gry + " WHERE tura_gracza=0";
            czytnik = komenda.ExecuteReader();
            hajs = czytnik.GetInt16(0);
            hajs = hajs + kwota;
            czytnik.Close();
            komenda.CommandText = "UPDATE " + nazwa_gry + "SET hajs=" + hajs + " WHERE tura_gracza=0";
            komenda.ExecuteNonQuery();
        }

        static void wypisz(ref SqlConnection pol)
        {
            SqlCommand komendaSQL = pol.CreateCommand();
            komendaSQL.CommandText = "SELECT * FROM Gra_01";        // tekst zapytania
            SqlDataReader czytnik = komendaSQL.ExecuteReader();
            Console.WriteLine("Wiersze tabeli:");

            while (czytnik.Read())
            {
                Console.WriteLine(czytnik["id_gracza"] + " " + czytnik["nazwa"] + " " + czytnik["hajs"]);
            }
            czytnik.Close();
            komendaSQL.Cancel();
        }

        static void pobierz_plansze(List<Pola> plansza, SqlConnection pol)
        {
            SqlCommand komendaSQL = pol.CreateCommand();
            komendaSQL.CommandText = "SELECT * FROM Pola";
            SqlDataReader czytnik = komendaSQL.ExecuteReader();
            Pola temp = new Pola();
            while (czytnik.Read())
            {
                //Console.WriteLine(czytnik.GetDataTypeName(0));
                //  temp.edit(czytnik["nazwa"].ToString(),czytnik["ilosc_domkow"],czytnik["koszt_uslugi"],czytnik["koszt_zakupu"],czytnik["koszt_domkow"],czytnik["mnoznik"],czytnik["wlasciciel"]);
                //  temp.edit(czytnik["nazwa"].ToString(),czytnik.GetInt16(0), czytnik.GetInt16(0), czytnik.GetInt16(0), czytnik.GetInt16(0), czytnik.GetInt16(0), czytnik.GetInt16(0));
                temp.wypisz();
            }
        }

        static bool oczekanko(SqlConnection pol)
        {
            int a = 123;
            SqlCommand komenda = pol.CreateCommand();
            komenda.CommandText = "SELECT tura_gracza FROM Gra_01 WHERE id_gracza=1;";
            while (a != 1)
            {
                SqlDataReader czytnik = komenda.ExecuteReader();
                czytnik.Read();
                a = czytnik.GetInt16(0);
                Console.WriteLine(a);
                czytnik.Close();
                Thread.Sleep(5000);
            }

            if (a == 1)
            {
                Console.WriteLine(a);
                return true;
            }
            else
                Console.WriteLine("no patrz nie udalo sie");
            return false;
        }

        public void wrzucRzutDoBazy(int rzut)
        {
            string nazwa_gry="Gra_01";
            // # kod do wrzucania rzutu
            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText = "UPDATE rzut_kostka SET wartosc=" + rzut + " Where id_rzutu=1;";
            komenda.ExecuteNonQuery();

            // # kod do zmiany położenia pionka gracza na planszy
            komenda.CommandText = "SELECT aktualne_pole FROM "+nazwa_gry+" WHERE tura_gracza=1;";
            SqlDataReader czytnik = komenda.ExecuteReader();
            int a = czytnik.GetInt16(0);
            a = (a + rzut)%41;
            if (a == 0)
                a++;
            czytnik.Close();

            komenda.CommandText = "UPDATE " + nazwa_gry + " SET aktualne_pole=" + a + " WHERE tura_gracza=1;";
            komenda.ExecuteNonQuery();
        }

        public int pobierzRzutZBazy()
        {
            // # kod do pobierania rzutu

            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText = "SELECT wartosc From rzut-kostka;";
            SqlDataReader czytnik = komenda.ExecuteReader();
            int a = czytnik.GetInt16(0);
            czytnik.Close();
            return a;
        }

        public void rozpoznajPole(int id_pola)
        {
            // # kod pobrania rodzaju pola
            string temp = "";
            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText="SELCET typ_pola FROM Pola WHERE id_pola="+id_pola+";";
            SqlDataReader czytnik = komenda.ExecuteReader();
            temp = czytnik.GetString(0);
            
            switch (temp)
            {
                case "dzialka":
                    break;
                case "kolej":
                    break;
                case "ryzyko":
                    break;
                case "wiezienie":
                    break;
                case "idziesz_do_wiezienia":
                    break;
                case "dla_odwiedzajacych":
                    break;
                case "start":
                    break;
                case "elektrownia":
                    break;
                case "wodociagi":
                    break;
                case "podatek":
                    break;
                case "platny_parking":
                    break;
                case "bezplatny_parking":
                    break;
                default:
                    break;
            }
        }

        public void ruchDzialka(int id_pola)
        {
            bool t = false;
            int id_wlasciciela_pola;
            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText = "SELECT wlasciciel From Pola WHERE id_pola="+id_pola+";";
            SqlDataReader czytnik = komenda.ExecuteReader();
            id_wlasciciela_pola = czytnik.GetInt16(0);
            czytnik.Close();
            // pobierz Z BAZY do kogo nalezy pole
            if (/*jest niczyje*/id_wlasciciela_pola==0)
            {
                // wyswietl okienko decyzji zakupu pola
                if(/* kupi */ t)
                {
                    komenda.CommandText = "SELECT id_gracza FROM " + nazwa_gry + " WHERE tura_gracza=1";
                    czytnik = komenda.ExecuteReader();
                    id_wlasciciela_pola = czytnik.GetInt16(0);
                    czytnik.Close();

                    komenda.CommandText = "UPDATE Pola SET wlasciciel="+id_wlasciciela_pola+" WHERE id_pola=" + id_pola + ";";
                    komenda.ExecuteNonQuery();
                }
            }
            else
            {
                // pobierz id gracza który ma ruch
                komenda.CommandText = "SELECT id_gracza FROM " + nazwa_gry + " WHERE tura_gracza=1";
                czytnik = komenda.ExecuteReader();
                int id_aktualnego_gracza = czytnik.GetInt16(0);
                czytnik.Close();
               
                //posesja gracza który ma ruch
                if (id_aktualnego_gracza == id_wlasciciela_pola) //kupienie domku
                {
                    //  pobranie ile aktualnie jest domkow na polu
                    komenda.CommandText = "SELECT ilosc_domkow FROM Pola WHERE id_pola=" + id_pola + ";";
                    czytnik = komenda.ExecuteReader();
                    czytnik.Close();
                    int ilosc_domkow = czytnik.GetInt16(0);
                    if (ilosc_domkow < 4) //kupno domaka jest mozliwe
                    {
                       //pobranie ile kosztuje domek 
                        komenda.CommandText = "SELECT koszt_domka FROM pola WHERE id_pola=" + id_pola + ";";
                        czytnik = komenda.ExecuteReader();
                        int koszt = czytnik.GetInt16(0);
                        zmiana_kasy(koszt);
                        ilosc_domkow++;
                        czytnik.Close();

                        // aktualizacja ilosci domkow
                        komenda.CommandText = "UPDATE Pola SET ilosc_domkow="+ilosc_domkow+" WHERE id_pola=" + id_pola + ";";
                    }
                }
                else
                {
                    //  pobranie ile aktualnie jest domkow na polu
                    komenda.CommandText = "SELECT ilosc_domkow FROM Pola WHERE id_pola=" + id_pola + ";";
                    czytnik = komenda.ExecuteReader();
                    czytnik.Close();
                    int ilosc_domkow = czytnik.GetInt16(0);

                    // pobierz Z BAZY koszt naruszenia nieruchomosci
                    komenda.CommandText = "SELECT koszt_uslugi"+ilosc_domkow+" FROM Pola WHERE id_pola=" + id_pola + ";";
                    czytnik = komenda.ExecuteReader();
                    int koszt = czytnik.GetInt16(0);
                    // aktualizuj BAZE: transferuj pieniadze
                    transfer_kasy(koszt);
                }
            }
        }

        public void ruchKolej(int id_pola)
        {
            bool t = false;
            // pobierz Z BAZY do kogo nalezy pole
            int id_wlasciciela_pola;
            SqlCommand komenda = polaczenie.CreateCommand();
            komenda.CommandText = "SELECT wlasciciel From Pola WHERE id_pola=" + id_pola + ";";
            SqlDataReader czytnik = komenda.ExecuteReader();
            id_wlasciciela_pola = czytnik.GetInt16(0);
            czytnik.Close();
            if (/*jest niczyje*/id_wlasciciela_pola==0)
            {
                // wyswietl okienko decyzji zakupu pola
                if (/* kupi */ t)
                {
                    // aktualizuj BAZE 
                    komenda.CommandText = "SELECT id_gracza FROM " + nazwa_gry + " WHERE tura_gracza=1";
                    czytnik = komenda.ExecuteReader();
                    id_wlasciciela_pola = czytnik.GetInt16(0);
                    czytnik.Close();

                    // aktualizuj BAZE 
                    komenda.CommandText = "UPDATE Pola SET wlasciciel=" + id_wlasciciela_pola + " WHERE id_pola=" + id_pola + ";";
                    komenda.ExecuteNonQuery();
                }
            }
            else
            {
                // pobierz id gracza który ma ruch
                komenda.CommandText = "SELECT id_gracza FROM " + nazwa_gry + " WHERE tura_gracza=1";
                czytnik = komenda.ExecuteReader();
                int id_aktualnego_gracza = czytnik.GetInt16(0);
                czytnik.Close();
                if (id_wlasciciela_pola == id_aktualnego_gracza)
                {
                    //przesuniecie pionka
                    ;
                }
                else
                {
                    // pobranie ile koleij ma ich wlasciciel
                    komenda.CommandText = "SELECT COUNT(*) FROM Pola WHERE typ_pola='kolej' AND wlasciciel="+id_wlasciciela_pola+";";
                    czytnik = komenda.ExecuteReader();
                    int ilosc_koleji = czytnik.GetInt32(0);
                    czytnik.Close();

                    // pobierz Z BAZY koszt naruszenia nieruchomosci
                    komenda.CommandText = "SELECT koszt_uslugi" + ilosc_koleji + " FROM Pola WHERE id_pola=" + id_pola + ";";
                    czytnik = komenda.ExecuteReader();
                    int koszt = czytnik.GetInt16(0);
                    // aktualizuj BAZE: transferuj pieniadze
                    transfer_kasy(koszt);
                }
            }
        }

        public void ruchRyzyko()
        {
            string t = "";
            // pobierz Z BAZY tresc ryzyka (trzeba zrobic baze ryzyka)
            switch (t /* typ ryzka */)
            {
                case "b":
                    break;
                case "a":
                    break;
            }
            // (tu będzie dużo różnych zmian w bazie)
        }

        public void ruchIdzieszDoWiezienia()
        {

        }

        public void ruchDlaOdwiedzajacych()
        {

        }

        public void ruchStart()
        {
            
        }

        public void ruchElektrownia()
        {

        }

        public void ruchWodociagi()
        {

        }

        public void ruchPodatek()
        {
            zmiana_kasy(200);
        }

        public void ruchParkingPlatny()
        {
            zmiana_kasy(100);
        }

        public void ruchParkingBezplatny()
        {

        }







        public void main()
        {
            List<Pola> plansza = new List<Pola>();

            try
            {
                // dane do połaczenia z baza danych
                //SqlConnection polaczenie = new SqlConnection("Server=projektasd.database.windows.net;DATABASE=monopol;User ID=master;Password=Krowa123!;");
                //polaczenie.Open();

                // ta funkcja jeszcze nie działa 
               // pobierz_plansze(plansza, polaczenie);

                //wypisz(ref polaczenie);

                //funkcja sprawdzajaca ture 
                //oczekanko(polaczenie);


                //polaczenie.Close();

                //Console.ReadKey();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Wystąpił nieoczekiwany błąd!");
                Console.WriteLine(e.Message);
               // Console.ReadKey();
            }



        }



    }
}
