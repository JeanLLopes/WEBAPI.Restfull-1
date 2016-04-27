using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WEBAPI.Restful_1.DataModel;

namespace WEBAPI.Restful_1.Business
{
    public class ProductServices : IProductServices
    {
        private readonly UnitOfWork _unitOfWork;


        //CONTRUCTOR
        public ProductServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ProductEntity GetProductById(int productId)
        {
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product != null)
            {
                Mapper.CreateMap<PRODUCTS, ProductEntity>();
                var productModel = Mapper.Map<PRODUCTS, ProductEntity>(product);
                return productModel;
            }
            return null;
        }

        public IEnumerable<ProductEntity> GetAllProducts()
        {
            var products = _unitOfWork.ProductRepository.GetAll().ToList();
            if (products.Any())
            {
                Mapper.CreateMap<PRODUCTS, ProductEntity>();
                var productsModel = Mapper.Map<List<PRODUCTS>, List<ProductEntity>>(products);
                return productsModel;
            }
            return null;
        }

        public int CreateProduct(ProductEntity productEntity)
        {
            var product = new PRODUCTS
            {
                PRODUCT_NAME = productEntity.ProductName
            };
            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();
            return product.PRODUCT_ID;
        }

        public bool UpdateProduct(int productId, ProductEntity productEntity)
        {
            var success = false;
            if (productEntity != null)
            {
                var product = _unitOfWork.ProductRepository.GetById(productId);
                if (product != null)
                {
                    product.PRODUCT_NAME = productEntity.ProductName;
                    _unitOfWork.ProductRepository.Update(product);
                    _unitOfWork.Save();
                    success = true;
                }
            }
            return success;
        }

        public bool DeleteProduct(int productId)
        {
            var success = false;
            if (productId > 0)
            {
                    var product = _unitOfWork.ProductRepository.GetById(productId);
                    if (product != null)
                    {
                        _unitOfWork.ProductRepository.Delete(product);
                        _unitOfWork.Save();
                        success = true;
                    }
                }
            return success;
        }
    }
}
