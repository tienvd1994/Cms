using MyWeb.Core;
using MyWeb.Data;
using MyWeb.Presentation.Areas.Admin.Models;
using MyWeb.Services.Catalog;
using System;
using System.Web.Mvc;

namespace MyWeb.Presentation.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Admin/Product
        public ActionResult Index()
        {
            var products = _productService.SearchProducts("");

            return View(products);
        }

        public ActionResult Create()
        {
            ViewBag.ListCategory = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");

            return View(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            var product = new Product();
            product.Id = productViewModel.Id;
            product.ProductTypeId = productViewModel.ProductTypeId;
            product.ParentGroupedProductId = productViewModel.ParentGroupedProductId;
            product.VisibleIndividually = productViewModel.VisibleIndividually;
            product.Name = productViewModel.Name;
            product.ShortDescription = productViewModel.ShortDescription;
            product.FullDescription = productViewModel.FullDescription;
            product.AdminComment = productViewModel.AdminComment;
            product.ProductTemplateId = productViewModel.ProductTemplateId;
            product.VendorId = productViewModel.VendorId;
            product.ShowOnHomePage = productViewModel.ShowOnHomePage;
            product.MetaKeywords = productViewModel.MetaKeywords;
            product.MetaDescription = productViewModel.MetaDescription;
            product.MetaTitle = productViewModel.MetaTitle;
            product.AllowCustomerReviews = productViewModel.AllowCustomerReviews;
            product.ApprovedRatingSum = productViewModel.ApprovedRatingSum;
            product.NotApprovedRatingSum = productViewModel.NotApprovedRatingSum;
            product.ApprovedTotalReviews = productViewModel.ApprovedTotalReviews;
            product.NotApprovedTotalReviews = productViewModel.NotApprovedTotalReviews;
            product.SubjectToAcl = productViewModel.SubjectToAcl;
            product.LimitedToStores = productViewModel.LimitedToStores;
            product.Sku = productViewModel.Sku;
            product.ManufacturerPartNumber = productViewModel.ManufacturerPartNumber;
            product.Gtin = productViewModel.Gtin;
            product.IsGiftCard = productViewModel.IsGiftCard;
            product.GiftCardTypeId = productViewModel.GiftCardTypeId;
            product.OverriddenGiftCardAmount = productViewModel.OverriddenGiftCardAmount;
            product.RequireOtherProducts = productViewModel.RequireOtherProducts;
            product.RequiredProductIds = productViewModel.RequiredProductIds;
            product.AutomaticallyAddRequiredProducts = productViewModel.AutomaticallyAddRequiredProducts;
            product.IsDownload = productViewModel.IsDownload;
            product.DownloadId = productViewModel.DownloadId;
            product.UnlimitedDownloads = productViewModel.UnlimitedDownloads;
            product.MaxNumberOfDownloads = productViewModel.MaxNumberOfDownloads;
            product.DownloadExpirationDays = productViewModel.DownloadExpirationDays;
            product.DownloadActivationTypeId = productViewModel.DownloadActivationTypeId;
            product.HasSampleDownload = productViewModel.HasSampleDownload;
            product.SampleDownloadId = productViewModel.SampleDownloadId;
            product.HasUserAgreement = productViewModel.HasUserAgreement;
            product.UserAgreementText = productViewModel.UserAgreementText;
            product.IsRecurring = productViewModel.IsRecurring;
            product.RecurringCycleLength = productViewModel.RecurringCycleLength;
            product.RecurringTotalCycles = productViewModel.RecurringTotalCycles;
            product.IsRental = productViewModel.IsRental;
            product.RentalPriceLength = productViewModel.RentalPriceLength;
            product.RentalPricePeriodId = productViewModel.RentalPricePeriodId;
            product.IsShipEnabled = productViewModel.IsShipEnabled;
            product.IsFreeShipping = productViewModel.IsFreeShipping;
            product.ShipSeparately = productViewModel.ShipSeparately;
            product.AdditionalShippingCharge = productViewModel.AdditionalShippingCharge;
            product.DeliveryDateId = productViewModel.DeliveryDateId;
            product.IsTaxExempt = productViewModel.IsTaxExempt;
            product.TaxCategoryId = productViewModel.TaxCategoryId;
            product.IsTelecommunicationsOrBroadcastingOrElectronicServices = productViewModel.IsTelecommunicationsOrBroadcastingOrElectronicServices;
            product.ManageInventoryMethodId = productViewModel.ManageInventoryMethodId;
            product.ProductAvailabilityRangeId = productViewModel.ProductAvailabilityRangeId;
            product.UseMultipleWarehouses = productViewModel.UseMultipleWarehouses;
            product.WarehouseId = productViewModel.WarehouseId;
            product.StockQuantity = productViewModel.StockQuantity;
            product.DisplayStockAvailability = productViewModel.DisplayStockAvailability;
            product.DisplayStockQuantity = productViewModel.DisplayStockQuantity;
            product.MinStockQuantity = productViewModel.MinStockQuantity;
            product.LowStockActivityId = productViewModel.LowStockActivityId;
            product.NotifyAdminForQuantityBelow = productViewModel.NotifyAdminForQuantityBelow;
            product.BackorderModeId = productViewModel.BackorderModeId;
            product.AllowBackInStockSubscriptions = productViewModel.AllowBackInStockSubscriptions;
            product.OrderMinimumQuantity = productViewModel.OrderMinimumQuantity;
            product.OrderMaximumQuantity = productViewModel.OrderMaximumQuantity;
            product.AllowedQuantities = productViewModel.AllowedQuantities;
            product.AllowAddingOnlyExistingAttributeCombinations = productViewModel.AllowAddingOnlyExistingAttributeCombinations;
            product.NotReturnable = productViewModel.NotReturnable;
            product.DisableBuyButton = productViewModel.DisableBuyButton;
            product.DisableWishlistButton = productViewModel.DisableWishlistButton;
            product.AvailableForPreOrder = productViewModel.AvailableForPreOrder;
            product.PreOrderAvailabilityStartDateTimeUtc = productViewModel.PreOrderAvailabilityStartDateTimeUtc;
            product.CallForPrice = productViewModel.CallForPrice;
            product.Price = productViewModel.Price;
            product.OldPrice = productViewModel.OldPrice;
            product.ProductCost = productViewModel.ProductCost;
            product.CustomerEntersPrice = productViewModel.CustomerEntersPrice;
            product.MinimumCustomerEnteredPrice = productViewModel.MinimumCustomerEnteredPrice;
            product.MaximumCustomerEnteredPrice = productViewModel.MaximumCustomerEnteredPrice;
            product.BasepriceEnabled = productViewModel.BasepriceEnabled;
            product.BasepriceAmount = productViewModel.BasepriceAmount;
            product.BasepriceUnitId = productViewModel.BasepriceUnitId;
            product.BasepriceBaseAmount = productViewModel.BasepriceBaseAmount;
            product.BasepriceBaseUnitId = productViewModel.BasepriceBaseUnitId;
            product.MarkAsNew = productViewModel.MarkAsNew;
            product.MarkAsNewStartDateTimeUtc = productViewModel.MarkAsNewStartDateTimeUtc;
            product.MarkAsNewEndDateTimeUtc = productViewModel.MarkAsNewEndDateTimeUtc;
            product.HasTierPrices = productViewModel.HasTierPrices;
            product.HasDiscountsApplied = productViewModel.HasDiscountsApplied;
            product.Weight = productViewModel.Weight;
            product.Length = productViewModel.Length;
            product.Width = productViewModel.Width;
            product.Height = productViewModel.Height;
            product.AvailableStartDateTimeUtc = productViewModel.AvailableStartDateTimeUtc;
            product.AvailableEndDateTimeUtc = productViewModel.AvailableEndDateTimeUtc;
            product.DisplayOrder = productViewModel.DisplayOrder;
            product.Published = productViewModel.Published;
            product.Deleted = false;
            product.CreatedOnUtc = DateTime.Now;
            product.UpdatedOnUtc = DateTime.Now;
            product.Slug = CommonHelper.NameToSlug(productViewModel.Name);

            _productService.InsertProduct(product);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            _productService.DeleteProduct(product);

            return RedirectToAction("Index");
        }
    }
}