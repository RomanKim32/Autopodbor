namespace Autopodbor_312.Models
{
    public class CarsBrandsModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Price { get; set; }
        public int CarsBrandsId { get; set; }
        public CarsBrands CarsBrands { get; set; }
    }
}
