using System.Text.Json;
using OrdersAndItemsService.Core.Models;
using OrdersAndItemsService.Repository.Data;
namespace OrdersAndItemsService.Repository.Seeding
{
    public class DbCotextSeed
    {
        //get them from the json file   
        public static async Task SeedAsync(AppDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Items.Any())//if the list is empty 
                {
                    var itemsData = File.ReadAllText("./Seeding/Item.json");//go to the json file

                    var items = JsonSerializer.Deserialize<List<Item>>(itemsData);//deserialize data into a list of items
                    if (items != null)
                        foreach (var item in items)
                        {
                            context.Items.Add(item);
                        }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                loggerFactory.Dispose();//////****
            }
        }
    }
}
