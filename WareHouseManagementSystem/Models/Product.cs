using System;

namespace WareHouseManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public Double Quantity { get; set; }
        public string Measure { get; set; }
        public Double Price { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditOn { get; set; }
    }
}
