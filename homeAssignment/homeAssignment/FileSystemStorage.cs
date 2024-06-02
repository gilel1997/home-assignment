using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace homeAssignment
{
    public class FileSystemStorage<T> : IStorage<T>
    {
        private readonly string filePath;
        private DateTime lastUpdated;
        private readonly TimeSpan expirationInterval;

        public FileSystemStorage(string filePath, TimeSpan expirationInterval, bool isReadOnly)
        {
            this.filePath = filePath;
            this.expirationInterval = expirationInterval;
            IsReadOnly = isReadOnly;
            lastUpdated = DateTime.Now;
        }

        public bool IsReadOnly { get; set; }


        public bool IsExpired()
        {
            return DateTime.Now - lastUpdated > expirationInterval;
        }

        public async Task<(bool IsSuccessful, T Value)> TryReadAsync()
        {
            if (IsExpired() || !File.Exists(filePath))
            {
                return (false, default);
            }

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var value = JsonSerializer.Deserialize<T>(json);
                return (true, value);
            }
            catch
            {
                return (false, default);
            }
        }

        public async Task<bool> TryWriteAsync(T value)
        {
            if (IsReadOnly)
            {
                return false;
            }

            try
            {
                var json = JsonSerializer.Serialize(value);
                await File.WriteAllTextAsync(filePath, json);
                lastUpdated = DateTime.Now;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
