using System;
using System.Threading;

namespace Playground.Sync
{
    public class ReadWriteLock
    {
        private int _readersActive = 0;
        private int _writersWaiting = 0;
        private bool _isWriterActive = false;
        
        private readonly object _g = new object();
        
        public void EnterReadLock()
        {
            lock (_g)
            {
                while (_writersWaiting > 0 || _isWriterActive)
                {
                    Monitor.Wait(_g);
                }

                _readersActive++;
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": acquired read lock");
            }
            
        }
        
        public void EnterWriteLock()
        {
            lock (_g)
            {
                _writersWaiting++;
                while (_readersActive > 0 || _isWriterActive)
                {
                    Monitor.Wait(_g);
                }

                _writersWaiting--;
                _isWriterActive = true;
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": acquired write lock"); 
            }
            
        }

        public void ReleaseReadLock()
        {
            lock (_g)
            {
                _readersActive--;
                if (_readersActive == 0)
                {
                    Monitor.PulseAll(_g);
                }
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": released read lock");
            }
            
        }

        public void ReleaseWriteLock()
        {
            lock (_g)
            {
                _isWriterActive = false;
                Monitor.PulseAll(_g);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": released write lock");
            }
        }
    }
}