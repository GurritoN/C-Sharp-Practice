using System;
using System.Collections.Generic;
using System.Threading;

namespace RestrauntQueueSim
{
    class Program
    {
        static void CustomerLogic()
        {
            SingletonRestraunt restraunt = SingletonRestraunt.Instance;
            Checkout currCheckout = new Checkout(0);
            int min = int.MaxValue;
            foreach (var checkout in restraunt.Checkouts)
            {
                if (checkout.Queue.Size < min)
                {
                    min = checkout.Queue.Size;
                    currCheckout = checkout;
                }
            }
            currCheckout.Queue.Add(Thread.CurrentThread);
            while (true)
            {
                var position = currCheckout.Queue.Pos(Thread.CurrentThread);
                if (position == 0)
                {
                    currCheckout.cmu.WaitOne();
                    currCheckout.Enter();
                    currCheckout.cmu.ReleaseMutex();
                    currCheckout.Queue.Remove(Thread.CurrentThread);
                    break;
                }
                min = int.MaxValue;
                Checkout nextCheckout = new Checkout(0);
                foreach (var checkout in restraunt.Checkouts)
                {
                    if (checkout.Queue.Size < min)
                    {
                        min = checkout.Queue.Size;
                        nextCheckout = checkout;
                    }
                }
                if (currCheckout.Queue.Pos(Thread.CurrentThread) > min)
                {
                    Console.WriteLine("Customer № " + Thread.CurrentThread.Name + " is moving to the queue №" + nextCheckout.Queue.number);
                    currCheckout.Queue.Remove(Thread.CurrentThread);
                    nextCheckout.Queue.Add(Thread.CurrentThread);
                    currCheckout = nextCheckout;
                }
            }
        }

        static void Main(string[] args)
        {
            Random r = new Random();
            SingletonRestraunt rest = SingletonRestraunt.Instance;
            List<Thread> threadlist = new List<Thread>();
            Console.WriteLine("Enter the number of customers:");
            int customersCount = int.Parse(Console.ReadLine());
            Console.Clear();
            for(int i = 0; i < customersCount; i++)
            {
                Thread.Sleep(r.Next(200, 700));
                Thread thread = new Thread(CustomerLogic);
                threadlist.Add(thread);
                thread.Name = i.ToString();
                thread.Start();
            }
            while (threadlist.Count != 0)
            {
                for(int i = 0; i < customersCount; i++)
                {
                    if (!threadlist[i].IsAlive)
                    {
                        customersCount--;
                        threadlist.Remove(threadlist[i]);
                    }
                }
            }
            Console.WriteLine("End");
            Console.ReadKey();
        }
    }
}
