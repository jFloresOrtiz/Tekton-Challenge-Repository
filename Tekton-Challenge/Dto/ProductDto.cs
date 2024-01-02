namespace Tekton_Challenge.Dto
{
    public class ProductDto
    {
        public Int32 ProductId { get; set; }
        public String Name { get; set; } = null!;
        public Int32 Status { get; set; }
        public Int32 Stock { get; set; }
        public String Description { get; set; } = null!;
        public Decimal Price { get; set; }

    }
}
