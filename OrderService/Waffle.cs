namespace OrderService
{
    public class Waffle
    {
        public int Id { get; set; }
        public List<Topping> Toppings { get; set; } = new ();

        public WaffleShape WaffleShape { get; set; }

        public decimal Price { get { return Total(); } }

        private decimal BasePrice = 7m;

        private decimal Total() => Toppings.Sum(x => x.Price) + BasePrice;
    }

    public class Topping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public enum WaffleShape
    {
        Circle = 0,
        Parallelogram = 1,
        Turtle = 2,
        Septagon = 3
    }
}
