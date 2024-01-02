
namespace Tekton_Challenge.Entity
{
    public class Product
    {
        public Int32 ProductId { get; set; }
        public String Name { get; set; } = null!;
        public Int32 Status { get; set; }
        public String StatusName { get; set; } = null!;
        public Int32 Stock { get; set; }
        public String Description { get; set; } = null!;
        public Decimal Price { get; set; }
        public String Discount { get; set; } = null!;
        public Decimal FinalPrice { get; set; }
        public String Category { get; set; } = null!;
        public String RegistrationDate { get; set; } = null!;
        public String Customer { get; set; } = null!;

    }
 
}
