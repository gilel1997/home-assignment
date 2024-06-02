using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace homeAssignment
{
    public class ChainResource<T>
    {
        private readonly List<IStorage<T>> storages;

        public ChainResource(List<IStorage<T>> storages)
        {
            try
            {
                if (storages == null)
                {
                    throw new ArgumentNullException(nameof(storages));
                }
                else
                {
                    this.storages = storages;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("class: ChainResource | this problam is: " + e);
            }
        }

        public async Task<List<List<T>>> GetValueAsync()
        {
            List<T> valuesRead = new List<T>();
            List<T> valuesWrite = new List<T>();
            foreach (var storage in storages)
            {
                var (isSuccessful, value) = await storage.TryReadAsync();
                if (isSuccessful)
                {
                    valuesRead.Add(value);
                    // Propagate the value upwards
                    Console.WriteLine("you can now write in to the data(best in a JSON form: ");
                    string temp = Console.ReadLine();
                    if (temp != null)
                    {
                        JsonDocument test = JsonDocument.Parse(temp);
                        if (test != null)
                        {
                            value = (T)Convert.ChangeType(temp, typeof(T));
                        }
                        else
                        {
                            Console.WriteLine("the value is null and in the wirte list in will apear as null");
                        }
                    }
                    await PropagateValueUpwardsAsync(value);
                    if (storage.GetType() != typeof(WebServiceStorage<T>))
                    {
                        valuesWrite.Add(value);
                    }
                }
            }
            if (valuesRead.Count == 0 || valuesWrite.Count == 0)
            {
                throw new InvalidOperationException("No valid storage found.");
            }
            List<List<T>> result = new List<List<T>>();
            result.Add(valuesRead);
            result.Add(valuesWrite);
            return result;

        }

        private async Task PropagateValueUpwardsAsync(T value)
        {
            foreach (var storage in storages)
            {
                if (!storage.IsReadOnly)
                {
                    await storage.TryWriteAsync(value);
                }
            }
        }

    }
}
