using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dhcpSzimulacio
{
    class Program
    {
        static List<string> excluded = new List<string>();
        static Dictionary<string, string> dhcp = new Dictionary<string, string>();
        static Dictionary<string, string> reserved = new Dictionary<string, string>();



        static void BeolvExcluded()
        {
            try
            {
                StreamReader sr = new StreamReader("excluded.csv");
                try
                {
                    while (!sr.EndOfStream)
                    {
                        excluded.Add(sr.ReadLine());
                    }
                }
                catch (Exception es)
                {

                    Console.WriteLine(es.Message);
                }
                finally
                {
                    sr.Close();
                }



                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            
        }

        static string CimEgyelno(string cim)
        {
            // cim = 192.168.10.100 akkor returnben 192.168.10.101
            // Szétvágni '.'
            // az utolsó int-é konvertálni és egyet hozzáadni(255-öt ne lépje túl)
            //Összefűzni stringé

            
                string[] adat = cim.Split('.');
                int okt4 = Convert.ToInt32(adat[3]);
                if (okt4 < 255)
                {
                    okt4++;
                }

                return adat[0] + "." + adat[1] + "." + adat[2] + "." + okt4.ToString();
         }

        static void BeolvDictionary(Dictionary<string,string> d, string filenev)
        {
            try
            {
                StreamReader file = new StreamReader(filenev);
                while (!file.EndOfStream)
                {
                    string[] adatok = file.ReadLine().Split(';');
                    d.Add(adatok[0], adatok[1]);
                }




                file.Close();
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            BeolvExcluded();

            BeolvDictionary(dhcp, "dhcp.csv");
            BeolvDictionary(reserved, "reserved.csv");
            foreach (var t in reserved)
            {
                Console.WriteLine(t);
            }





            Console.ReadKey();
        }
    }
}
