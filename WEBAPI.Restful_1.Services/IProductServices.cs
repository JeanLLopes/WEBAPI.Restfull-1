using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBAPI.Restful_1.Entities;

namespace WEBAPI.Restful_1.Services
{
    public interface IProductServcices
    {
        ProductEntity GetProductById(int productId);
        IEnumerable<ProductEntity> GetAllProducts();
        int CreateProduct(ProductEntity productEntity);
        bool UpdateProduct(int productId, ProductEntity productEntity);
        bool DeleteProduct(int productId);
    }
}
