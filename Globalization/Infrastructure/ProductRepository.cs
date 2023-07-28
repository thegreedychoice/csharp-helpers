using System.Reflection;
using Newtonsoft.Json;
using Domain.Models;
using System.Globalization;

namespace Infrastructure;

public class ProductRepository
{
    public IEnumerable<Product> Get()
    {
        Assembly assembly;

        try
        {
            assembly = Assembly.GetExecutingAssembly()
                .GetSatelliteAssembly(CultureInfo.CurrentCulture);
        }
        catch(Exception)
        {
            assembly = Assembly.GetExecutingAssembly();
        }

        using var stream = 
            assembly.GetManifestResourceStream("Infrastructure.Data.products.json");
        
        using var reader = new StreamReader(stream);

        var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(reader.ReadToEnd());

        return products;
    }
}
