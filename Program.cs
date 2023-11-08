using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hiányzasok
{
    //Név;Osztály;Első nap; Utolsó nap; Mulasztott órák
    //Balogh Péter;6a;1;1;5
    class Tanulok
    {
        public string Nev {  get; set; }
        public string Osztaly { get; set; }
        public int ElsNap {  get; set; }
        public int UtolNap { get; set; }
        public int Mulaszt {  get; set; }
        public Tanulok (string sor)
        {
            var s = sor.Trim().Split(';');
            Nev = s[0];
            Osztaly = s[1];
            ElsNap = int.Parse(s[2]);
            UtolNap = int.Parse(s[3]);
            Mulaszt = int.Parse(s[4]);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var sr = new StreamReader("szeptember.csv", Encoding.Default);
            var elsosor = sr.ReadLine();
            var lista = new List<Tanulok>();

            while(!sr.EndOfStream)
            {
                lista.Add(new Tanulok(sr.ReadLine()));
            }

            //2. feladat:
            var mulasztott = (
                from sor in lista
                select sor.Mulaszt
            );
            Console.WriteLine($"2. feladat \n\t Összes mulasztott órák száma: {mulasztott.Sum()} ");

            //3.feladat:
            Console.WriteLine("3. feladat");
            Console.Write("\tKérem adjon meg egy napot (1,30): ");
            var BekertNap = int.Parse(Console.ReadLine());
            Console.Write("\tKérem adja meg a tanuló nevét: ");
            var BekertNev = Console.ReadLine();

            //4.feladat
            var tanulo = (
                from sor in lista
                where sor.Nev == BekertNev
                select sor
            );
            if (tanulo.Any())
            {
                Console.WriteLine("4. feladat \n\t A tanuló hiányzott szeptemberben");
            }
            else
            {
                Console.WriteLine("4. feladat \n\t A tanuló nem hiányzott szeptemberben");
            }

            //5. feladat
            Console.WriteLine($"5. feladat: Hiányzók 2017.09.{BekertNap:00}-n:");
            var VoltEhianyzo = false;
            for (int i = 0; i < lista.Count; i++)
            {
                if (BekertNap == lista[i].ElsNap)
                {
                    VoltEhianyzo = true;
                    Console.WriteLine($"\t{lista[i].Nev} ({lista[i].Osztaly})");
                }
            }
            if (VoltEhianyzo == false)
            {
                Console.WriteLine("\tNem volt hiányzó!");
            }

            //6. feladat
            //Lambda-t használó, működő
            var Mulasztasok = (
                from sor in lista
                group sor by sor.Osztaly
                into Ideiglenes
                orderby Ideiglenes.Key
                select new
                {
                    osz = Ideiglenes.Key,
                    m = Ideiglenes.Sum(x => x.Mulaszt)
                }
            );
            var ossz_lambda = new StreamWriter("osszesites_lambda.csv", false, Encoding.UTF8);
            foreach (var sor in Mulasztasok)
            {
                ossz_lambda.WriteLine($"{sor.osz};{sor.m}");
            }
            
            //LinQ-t használó)
            var stat1 = (
                from d in lista
                orderby d.Osztaly
                group d.Mulaszt by d.Osztaly
            );
            var ossz_linq = new StreamWriter("osszesites_linq.csv", false, Encoding.UTF8);
            foreach (var d in stat1)
            {
                ossz_linq.WriteLine($"{d.Key} - {d.Sum()}");
            }

            ossz_lambda.Close();
            ossz_linq.Close();
        }
    }
}
