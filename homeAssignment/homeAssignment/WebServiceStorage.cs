using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace homeAssignment
{
    public class WebServiceStorage<T> : IStorage<T>
    {
        private readonly string apiUrl;
        private static readonly HttpClient httpClient = new HttpClient();

        public WebServiceStorage(string apiUrl)
        {
            this.apiUrl = apiUrl;
            IsReadOnly = true;
        }

        public bool IsReadOnly { get; set; }

        public bool IsExpired()
        {
            return false; // Expiration is irrelevant for web service
        }

        public async Task<(bool IsSuccessful, T Value)> TryReadAsync()
        {
            try
            {
                string response = await httpClient.GetStringAsync(apiUrl);
                T value = JsonSerializer.Deserialize<T>(response);
                return (true, value);
            }
            catch
            {
                return (false, default);
            }
        }

        public Task<bool> TryWriteAsync(T value)
        {
            throw new NotSupportedException("Web service storage is read-only.");
        }
    }

}
