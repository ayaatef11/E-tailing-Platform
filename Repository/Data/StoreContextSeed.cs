using Core.Entities.OrderEntities;
using Core.Entities.ProductEntities;


namespace Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedProductDataAsync(StoreContext _storeContext)
        {

            if (!_storeContext.ProductBrands.Any())
            {
                var brandsJSONData = File.ReadAllText("../Repository/Data/DataSeeding/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsJSONData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        _storeContext.ProductBrands.Add(brand);
                    }
                }
            }

            if (!_storeContext.ProductCategories.Any())
            {
                var catrgoriesJSONData = File.ReadAllText("../Repository/Data/DataSeeding/categories.json");

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(catrgoriesJSONData);

                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        _storeContext.ProductCategories.Add(category);
                    }
                }
            }

            if (!_storeContext.Products.Any())
            {
                var ProductsJSONData = File.ReadAllText("../Repository/Data/DataSeeding/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsJSONData);

                if (products?.Count> 0)
                {
                    foreach (var product in products)
                    {
                        _storeContext.Products.Add(product);
                    }
                }
            }

            if (!_storeContext.OrderDeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Repository/Data/DataSeeding/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<OrderDeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        _storeContext.OrderDeliveryMethods.Add(deliveryMethod);
                    }
                }
            }

            await _storeContext.SaveChangesAsync();
        }
    }
}