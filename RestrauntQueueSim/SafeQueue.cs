using System;
using System.Threading;

namespace RestrauntQueueSim
{
    class SafeQueue
    {
        Mutex qmu = new Mutex();
        Node tail = null;
        Node head = null;
        public int Size = 0;
        public int number;

        public void Add(Thread thread)
        {
            qmu.WaitOne();
            Size++;
            if (head==null)
            {
                head = tail = new Node(thread);
            }
            else
            {
                tail.Next = new Node(thread) { Prev = tail };
                tail = tail.Next;
            }
            Console.WriteLine("Customer № " + Thread.CurrentThread.Name + " entered the queue №" + number.ToString());
            qmu.ReleaseMutex();
        }

        public void Remove(Thread thread)
        {
            qmu.WaitOne();
            if (thread == head.Thread)
            {
                Size--;
                head = head.Next;
                if (head != null) head.Prev = null;
                Console.WriteLine("Customer № " + Thread.CurrentThread.Name + " leaved the queue №" + number.ToString());
                qmu.ReleaseMutex();
                return;
            }
            if (thread == tail.Thread)
            {
                Size--;
                tail = tail.Prev;
                if (tail != null) tail.Next = null;
                Console.WriteLine("Customer № " + Thread.CurrentThread.Name + " leaved the queue №" + number.ToString());
                qmu.ReleaseMutex();
                return;
            }
            var curr = head;
            while (curr != null)
            {
                if (curr.Thread == thread)
                {
                    Size--;
                    curr.Prev.Next = curr.Next;
                    curr.Next.Prev = curr.Prev;
                    Console.WriteLine("Customer № " + Thread.CurrentThread.Name + " leaved the queue №" + number.ToString());
                    qmu.ReleaseMutex();
                    return;
                }
                curr = curr.Next;
            }
        }

        public int Pos(Thread thread)
        {
            qmu.WaitOne();
            var pos = -1;
            var curr = head;
            while(curr != null)
            {
                pos++;
                if (curr.Thread == thread) break;
                curr = curr.Next;
            }
            qmu.ReleaseMutex();
            return pos;
        }

        class Node
        {
            public Thread Thread;
            public Node Next;
            public Node Prev;
            public Node(Thread thread)
            {
                Thread = thread;
            }
        }
    }
}
