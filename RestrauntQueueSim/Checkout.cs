using System;
using System.Threading;

namespace RestrauntQueueSim
{
    class Checkout
    {
        private int _number;
        public Mutex cmu = new Mutex();
        private static Random r = new Random();
        public SafeQueue Queue = new SafeQueue();

        public void Enter()
        {
            Console.WriteLine("\tCustomer № " + Thread.CurrentThread.Name + " entered checkout № " + _number);
            var time = r.Next(1000, 5000);
            Thread.Sleep(time);
            Console.WriteLine("\tCustomer № " + Thread.CurrentThread.Name + " leaved checkout № " + _number + " after " + time + "ms");
        }
        public Checkout(int number)
        {
            _number = number;
            Queue.number = number;
        }
    }
}
