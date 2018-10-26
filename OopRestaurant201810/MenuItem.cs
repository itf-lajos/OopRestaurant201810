namespace OopRestaurant201810
{
    /// <summary>
    /// Az étlapon szereplő tételek közül egy tétel adatait tartalmazza
    /// </summary>
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string  Description { get; set; }

        public int Price { get; set; }

        public Category Category { get; set; }
    }
}