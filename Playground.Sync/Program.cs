using System.Threading;

namespace Playground.Sync
{
    internal static class Program
    {
        private static void Main()
        {
            var readWriteLock = new ReadWriteLock();

            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(Read);
                thread.Start();
            }
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(Write);
                thread.Start();
            }
            
            void Read()
            {
                readWriteLock.EnterReadLock();
                readWriteLock.ReleaseReadLock();
            }

            void Write()
            {
                readWriteLock.EnterWriteLock();
                readWriteLock.ReleaseWriteLock();
            }
        }
    }
}