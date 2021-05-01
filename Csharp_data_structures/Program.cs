using Csharp_data_structures.DataStructures.PairingHeap;
using Csharp_data_structures.DataStructures.SortedTable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csharp_data_structures
{
    class Program
    {
        class TMPClass : IComparable
        {
            public int CompareTo(object obj)
            {
                return 0;
            }
        }
        static void Main(string[] args)
        {
            //var tmpHeap = new PriorityHeap<int, int, string>();
            //var tmpHeap = new PairingHeap<int, int>();
            //var realPriority = new List<int>();
            KD_Tree.KDTree<double, double, double> kDTree = new KD_Tree.KDTree<double, double, double>(2);

            List<KeyValuePair<KeyValuePair<double, double>, double>> zoznam = new List<KeyValuePair<KeyValuePair<double, double>, double>>();
            Random gen = new Random();

            double result;
            double cordX;
            double cordY;

            int generatedIndex;
            KeyValuePair<KeyValuePair<double, double>, double> prvok;
            bool naslo;

            for (int i = 0; i < 10000; ++i)
            {
                if(gen.NextDouble() < 0.55)
                {
                    result = i;
                    cordX = i;
                    cordY = i;
                    result = gen.Next(10);
                    cordX = gen.NextDouble() * 10;
                    cordY = gen.NextDouble() * 10;
                    zoznam.Add(new KeyValuePair<KeyValuePair<double, double>, double>(key: new KeyValuePair<double, double>(key: cordX, value: cordY), value: result));

                    kDTree.Insert(new double[] { cordX, cordY }, result, result);
                }
                else
                {
                    if(zoznam.Count != 0)
                    {
                        generatedIndex = gen.Next(zoznam.Count);
                        prvok = zoznam[generatedIndex];
                        zoznam.RemoveAt(generatedIndex);
                        var najdene = kDTree.FindData(new double[] { prvok.Key.Key, prvok.Key.Value });
                        naslo = false;
                        foreach (var vysledok in najdene)
                        {
                            if (vysledok == prvok.Value)
                            {
                                naslo = true;
                                break;
                            }
                        }
                        if (naslo != true)
                        {
                            Console.WriteLine("Chyba!");
                        }
                        else
                        {
                            if (kDTree.Delete(new double[] { prvok.Key.Key, prvok.Key.Value }, prvok.Value) != prvok.Value)
                            {
                                Console.WriteLine("Nezmazalo!!");
                            }
                        }
                    }
                }
            }

/*            foreach(var nieco in kDTree)
            {
                Console.WriteLine(nieco.ToString());
            }*/

            while (zoznam.Count != 0)
            {
                generatedIndex = gen.Next(zoznam.Count);
                prvok = zoznam[generatedIndex];
                zoznam.RemoveAt(generatedIndex);
                var najdene = kDTree.FindData(new double[] { prvok.Key.Key, prvok.Key.Value });
                naslo = false;
                foreach(var vysledok in najdene)
                {
                   if(vysledok == prvok.Value)
                    {
                        naslo = true;
                        break;
                    }
                }
                if(naslo != true)
                {
                    Console.WriteLine("Chyba!");
                }
                else
                {
                    if(kDTree.Delete(new double[] { prvok.Key.Key, prvok.Key.Value }, prvok.Value) != prvok.Value)
                    {
                        Console.WriteLine("Nezmazalo!!");
                    }
                }
                
            }

            if(kDTree.Count != zoznam.Count)
                Console.WriteLine("Zly pocet!!!");
            /*int cislo1 = -1;
            int cislo2 = -1;
            var gen = new Random();
            for(int i = 0; i < 50; ++i)
            {
                cislo1 = gen.Next(10);
                cislo2 = gen.Next(3);
                tmpHeap.Insert(cislo1, cislo2, $"Cislo: {cislo1} Priorita: {cislo2}");
            }*/

            /*while (tmpHeap.Count != 0)
            {
                Console.WriteLine(tmpHeap.GetMin());
            }
*/
            /*if(tmpHeap.Peek == null)
            {
                Console.WriteLine("Je to null");
            }
            Console.WriteLine(tmpHeap.Peek);*/
            /*            Random gen = new Random();
                        int numberOfTests = 100000;

                        double probabilityOfInsert = 0.5;

                        int cislo = -1;
                        int inserts = 0;
                        int geters = 0;
                        for (int i = 0; i < numberOfTests; ++i)
                        {
                            if (gen.NextDouble() < probabilityOfInsert)
                            {
                                cislo = gen.Next(100);
                                tmpHeap.Insert(cislo, cislo);
                                realPriority.Add(cislo);
                                ++inserts;
                            }
                            else
                            {
                                if (realPriority.Count != 0)
                                {
                                    ++geters;
                                    int minimal = realPriority.Min();
                                    if (minimal != tmpHeap.GetMin())
                                    {
                                        Console.WriteLine("Chyba!");
                                        return;
                                    }
                                    realPriority.Remove(minimal);
                                }
                            }
                        }
                        Console.WriteLine($"Inserts: {inserts}  Geters: {geters}");*/



            /*var sortedTable = new SortedTable<int, int>();

            sortedTable.Insert(5, 5);
            sortedTable.Insert(3, 3);
            sortedTable.Insert(2, 2);
            sortedTable.Insert(6, 6);
            sortedTable.Insert(8, 8);
            
            sortedTable.CutOff(1,3);
            sortedTable.Remove(6);
            sortedTable.Insert(3, 3);
            foreach (var item in sortedTable)
            {
                Console.WriteLine(item);
            }
*/



            /*realPriority.Sort();
            foreach (var item in realPriority)
            {
                Console.WriteLine(item);
            }
            *//*
            if(realPriority.Count != tmpHeap.Count)
            {
                Console.WriteLine("Chyba!");
            }*/
            /*Console.WriteLine("Moj heap");
            while(tmpHeap.Count != 0)
            {
                Console.WriteLine(tmpHeap.GetMin());
            }*/
        }
    }
}
