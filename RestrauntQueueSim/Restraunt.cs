using System;
using System.Collections.Generic;

namespace RestrauntQueueSim
{
    class SingletonRestraunt
    {
        private static SingletonRestraunt instance = new SingletonRestraunt();
        public List<Checkout> Checkouts = new List<Checkout>();
        private int size;

        private SingletonRestraunt()
        {
            Console.WriteLine("Enter the number of checkouts:");
            size = int.Parse(Console.ReadLine());
            for(int i = 0; i < size; i++)
            {
                Checkouts.Add(new Checkout(i));
            }
        }

        public static SingletonRestraunt Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
