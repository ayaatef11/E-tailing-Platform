using OrdersAndItemsService.Core.Entities.BasketEntites;
using OrdersAndItemsService.Core.interfaces.Repositories;
using StackExchange.Redis;

namespace OrdersAndItemsService.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<Basket?> CreateOrUpdateBasketAsync(Basket basket)
        {
            
            var createdOrUpdated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));//specify the expiration time for the cached data 
            if (createdOrUpdated is false) return null;
            return await GetBasketAsync(basket.Id);
        }

      

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);// get a string value for the given key.
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(basket);
        }

        public async Task AddItemToBasketAsync(string basketId, BasketItem item)
        {
            var basket = await CreateOrGetBasketAsync(basketId);
            basket.Items.Add(item);
            await SaveBasketAsync(basket);
        }

        public async Task<Basket> CreateOrGetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            if (basket.HasValue)
            {
                return JsonSerializer.Deserialize<Basket>(basket);
            }
            else
            {
                // Initialize a new basket if not found
                var newBasket = new Basket { Id = basketId, Items = new List<BasketItem>() };
                await SaveBasketAsync(newBasket);
                return newBasket;
            }
        }
        private async Task<bool> SaveBasketAsync(Basket basket)
        {
            var serializedBasket = JsonSerializer.Serialize(basket);
           return  await _database.StringSetAsync(basket.Id, serializedBasket,TimeSpan.FromDays(30));
        }

        public async Task RemoveItemFromBasketAsync(string basketId, int itemId)
        {
            var basket = await CreateOrGetBasketAsync(basketId);
            var item = basket.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                basket.Items.Remove(item);
                await SaveBasketAsync(basket);
            }
        }
        public async Task UpdateItemQuantityAsync(string basketId, int itemId, int quantity)
        {
            var basket = await CreateOrGetBasketAsync(basketId);
            var item = basket.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                await SaveBasketAsync(basket);
            }
        }
    }
}