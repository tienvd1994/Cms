using MyWeb.Data;
using System.Collections.Generic;

namespace MyWeb.Services.Catalog
{
    public interface IProductService
    {
        #region Products

        void DeleteProduct(Product product);

        IList<Product> GetAllProductsDisplayedOnHomePage();

        Product GetProductById(int productId);

        IList<Product> GetProductsByIds(int[] productIds);

        void InsertProduct(Product product);

        void UpdateProduct(Product product);

        void UpdateProductReviewTotals(Product product);

        Product GetProductBySku(string sku);

        void UpdateHasTierPricesProperty(Product product);

        IList<Product> SearchProducts(string keywords = null, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion
    }
}
