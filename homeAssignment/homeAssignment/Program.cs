using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace homeAssignment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var memoryStorage = new MemoryStorage<ExchangeRateList>(TimeSpan.FromHours(1), isReadOnly: false);
            var fileSystemStorage = new FileSystemStorage<ExchangeRateList>("exchangeRates.json", TimeSpan.FromHours(4), isReadOnly: false);
            var webServiceStorage = new WebServiceStorage<ExchangeRateList>("https://openexchangerates.org/api/latest.json?app_id=0f64665f6afe4813a8e6f5d3437dadfb&prettyprint=false&show_alternative=false");

            var storages = new List<IStorage<ExchangeRateList>>
        {
            memoryStorage,
            fileSystemStorage,
            webServiceStorage
        };

            var chainResource = new ChainResource<ExchangeRateList>(storages);
            try
            {
                var exchangeRateList = await chainResource.GetValueAsync();
                Console.WriteLine("Exchange rates retrieved successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
