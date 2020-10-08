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
        static List<string> commands = new List<string>();

        static void BeolvasList(List<string> l, string fileneve)
        {
            try
            {
                StreamReader sr = new StreamReader(fileneve);
                try
                {
                    while (!sr.EndOfStream)
                    {
                        l.Add(sr.ReadLine());
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

        static void Feladat(string parancs)
        {
            //Parancs = "request;D193135570AB2"
            /// először csak "request" foglalkozunk
            /// Megnézzük hogy request-e
            /// Ki kell szedni a MAC címet a parancsból
            
            if (parancs.Contains("request"))
            {
                string[] a = parancs.Split(';');
                string mac = a[1];
                if (dhcp.ContainsKey(mac))
                {
                    Console.WriteLine($"DHCP {mac} --> {dhcp[mac]}");
                }
                else
                {
                    if (reserved.ContainsKey(mac))
                    {
                        Console.WriteLine($"Reserved {mac} --> {reserved[mac]}");
                        dhcp.Add(mac, reserved[mac]);
                    }
                    else
                    {
                        string indulo = "192.168.10.100";
                        int okt4 = 100;
                        while (okt4 < 200 && dhcp.ContainsValue(indulo) ||
                            reserved.ContainsValue(indulo) || 
                            excluded.Contains(indulo))
                        {
                            okt4++;
                            indulo = CimEgyelno(indulo);
                        }

                        if (okt4 < 200)
                        {
                            Console.WriteLine($"kiosztott { mac} --> {indulo}");
                            dhcp.Add(mac, indulo);
                        }
                        else
                        {
                            Console.WriteLine($"{mac} Nincs IP");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("nem oké");
            }
        }
        static void Feladatok()
        {
            foreach (var command in commands)
            {
                Feladat(command);
            }
        }
        
        static void Main(string[] args)
        {
            BeolvasList(excluded, "excluded.csv");
            BeolvasList(commands, "test.csv");
            BeolvDictionary(dhcp, "dhcp.csv");
            BeolvDictionary(reserved, "reserved.csv");

            Feladatok();
            //foreach (var t in commands)
            //{
            //    Console.WriteLine(t);
            //}

            //Feladat("request");
            


            Console.ReadKey();
        }
    }
}
