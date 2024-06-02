using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeAssignment
{
    public interface IStorage<T>
    {
        /// <summary>
        /// task that build from 2 arguments
        /// </summary>
        Task<(bool IsSuccessful, T Value)> TryReadAsync();
        Task<bool> TryWriteAsync(T value);
        bool IsExpired();
        bool IsReadOnly { get; set; }
    }

}
