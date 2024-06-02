using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeAssignment
{
    
    public class MemoryStorage<T> : IStorage<T>
    {
        private T value;
        private DateTime lastUpdated;
        private TimeSpan expirationInterval;

        public MemoryStorage(TimeSpan expirationInterval, bool isReadOnly)
        {
            this.expirationInterval = expirationInterval;
            IsReadOnly = isReadOnly;
            lastUpdated = DateTime.Now;
        }

        public bool IsReadOnly { get; set; }

        public bool IsExpired()
        {
            return DateTime.Now - lastUpdated > expirationInterval;
        }

        public Task<(bool IsSuccessful, T Value)> TryReadAsync()
        {
            if (IsExpired())
            {
                return Task.FromResult((false, default(T)));
            }
            return Task.FromResult((true, value));
        }

        public Task<bool> TryWriteAsync(T value)
        {
            if (IsReadOnly)
            {
                return Task.FromResult(false);
            }
            this.value = value;
            lastUpdated = DateTime.Now;
            return Task.FromResult(true);
        }
    }
}
