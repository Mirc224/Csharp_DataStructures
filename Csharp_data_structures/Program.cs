using Csharp_data_structures.DataStructures.PairingHeap;
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
            var tmpHeap = new PairingHeap<int, int>();
            var realPriority = new List<int>();
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
            Random gen = new Random();
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
            Console.WriteLine($"Inserts: {inserts}  Geters: {geters}");
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
