using Tekton_Challenge.Entity;

namespace Tekton_Challenge.Utilities
{
    public class ProductResponse
    {
        public Int32 Status { get; set; }
        public Boolean Error { get; set; }
        public String Message { get; set; }
        public Product? Product { get; set; }

    }
}
