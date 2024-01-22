namespace WareHouseManagementSystem.Models
{
    public class CategoryDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
