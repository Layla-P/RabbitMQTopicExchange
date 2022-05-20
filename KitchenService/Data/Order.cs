using KitechenService;

namespace KitchenService
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public List<Waffle> Waffles { get; set; } = new();
        public decimal TotalPrice { get { return Total(); } }

        private decimal Total() => Waffles.Sum(x => x.Price);


        public Order Seed(int id)
        {
            var order = new Order
            {
                Id = id,
                CustomerName = "Layla",
                Waffles = new()
                {
                    new Waffle
                    {
                        Id = 1,
                        WaffleShape = WaffleShape.Turtle,
                        Toppings = new()
                        {
                            new Topping
                            {
                                Id = 1,
                                Name = "Cherries",
                                Price = 2.5m
                            },
                            new Topping
                            {
                                Id = 2,
                                Name = "Chocolate",
                                Price = 1.2m
                            }
                        }
                    }
                }
            };


            return order;
        }
    }
}
