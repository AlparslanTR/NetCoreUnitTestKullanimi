using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.ContentModel;
using ProductMvc.Controllers;
using ProductMvc.Models;
using ProductMvc.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestForMvc
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mock;
        private readonly ProductsController _controller;
        private List<Product> _products;
        public ProductControllerTest()
        {
            _mock = new Mock<IRepository<Product>>();
            _controller = new ProductsController(_mock.Object);
            _products = new List<Product>() { new Product {Id=1, Name="Lamba",Price=50,Stock=100 } };
        }

        /// <summary>
        /// Index Metodu İçin Test İşlemleri
        /// </summary>

        [Fact]
        public async void Index_ActionExecutes_ReturnView() // Sıralama: Metodun ismi - Action çalışması - Geriye Viewin dönmesi.
        {
            var result=await _controller.Index();
            Assert.IsType<ViewResult>(result); // Index View dönüyor mu kontrolü.
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mock.Setup(repo => repo.GetAll()).ReturnsAsync(_products); // GetAll methdu geriye list dönüyormu kontrolu
            var result = await _controller.Index(); 
            var viewResult= Assert.IsType<ViewResult>(result); // Index View dönüyor mu kontrolü.
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model); // List İstediğim tiptemi kontrolu
        }

        /// <summary>
        /// Index Metodu İçin Test İşlemleri
        /// </summary>
  
        /// ***************************************************************************///

        /// <summary>
        /// Details Metodu İçin Test İşlemleri
        /// </summary>

        [Fact]
        public async void Details_IdIsNull_ReturnRedirecToIndexAction() // Null olma durumu test etme.
        {
            var result= await _controller.Details(null);
            var redirect=Assert.IsType<RedirectToActionResult>(result); // Tipinin RedirecToActionResult Olmasını kontrol eder.
            Assert.Equal("Index", redirect.ActionName); // Index sayfasına dönmesi lazım ve aciton name alması lazım. Fakat benim controllerım not found döndüğü için test başarısız olacak.
        }

        [Fact]
        public async void Details_IdInValid_ReturnNotFound() // Id geçersiz olduğunda bana not found dönmesi gerekir.
        {
            Product product = null;
           _mock.Setup(x => x.GetById(0)).ReturnsAsync(product);
            var result = await _controller.Details(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void Details_ValidId_ReturnProduct(int productId) // Id geçerli olduğunda product dönmeli.
        {
            Product product = _products.First(x=>x.Id== productId);
            _mock.Setup(x=>x.GetById(productId)).ReturnsAsync(product);
            var result = await _controller.Details(productId);
            var viewResult= Assert.IsType<ViewResult>(result);
            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(product.Id, resultProduct.Id);
            Asset.Equals(product.Name, resultProduct.Name);
        }
        /// <summary>
        /// Details Metodu İçin Test İşlemleri
        /// </summary>

        /// ***************************************************************************///

        /// <summary>
        /// Create Metodu İçin Test İşlemleri
        /// </summary>
        
        [Fact]
        public void Create_ActionExecute_ReturnView() // Post işlemi yapmayan get dönen Creat methotunun testi sadece sayfayı döndürecek.
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Create_InValidModelState_ReturnView() // Post işlemi Yapan crete methodunun model state hata testi.
        {
            _controller.ModelState.AddModelError("Name", "Ad Alanı Boş Bırakılamaz.");
            var result=await _controller.Create(_products.First());
            var viewResult=Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(viewResult.Model);
        }

        [Fact]
        public async void Create_ValidModelState_ReturnRedirectToIndexAction() // Post işlemi Yapan Create methodunun çalışması.
        {
            var result= await _controller.Create(_products.First());
            var redirect=Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void Create_ValidModelState_CreateMethodExecute() // Model state geçerli olduğunda create çalışıyor mu testi.
        {
            Product product = null;
            _mock.Setup(x => x.Create(It.IsAny<Product>())).Callback<Product>(x=> product =x);
            var result = await _controller.Create(_products.First());
            _mock.Verify(x => x.Create(It.IsAny<Product>()),Times.Once);
            Assert.Equal(_products.First().Id, product.Id);
        }

        [Fact]
        public async void Create_InValidModelState_NeverCreateExecute() // Model state geçerli olmadığında create çalışmama testi.
        {
            _controller.ModelState.AddModelError("Ad Gereklidir", "");
            var result = await _controller.Create(_products.First());
            _mock.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
        }

        /// <summary>
        /// Create Metodu İçin Test İşlemleri
        /// </summary>

        /// ***************************************************************************///

        /// <summary>
        /// Edit Metodu İçin Test İşlemleri
        /// </summary>

        [Fact]
        public async void Edit_IdIsNull_ReturnViewNotFound() // Edit Id eğer null dönerse sayfa not found olacak testi.
        {
            var result = await _controller.Edit(null);
            var redirect = Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void Edit_ActionExecute_ReturnProduct(int productId) // Seçilen Id nin edit kısma taşınması testi.
        {
            var product = _products.First(x => x.Id == productId);
            _mock.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _controller.Edit(productId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(product.Id, resultProduct.Id);
            Assert.Equal(product.Name, resultProduct.Name);
        }

        [Theory]
        [InlineData(1)] // Burada manuel 1 id verdik
        public async void Edit_IdIsNotEqualProduct_ReturnNotFound(int productId) // Seçilen Id eğer farklı ise Not found olacak testi
        {
            var result = await _controller.Edit(2, _products.First(x => x.Id == productId)); // Id 2 olan ürün olmadığı için not found dönecek.
            var redirect = Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void Edit_ValidModelState_ReturnRedirectToIndexAction(int productId) // Güncellemeden sonra sayfayı döndürme testi.
        {
            var result = await _controller.Edit(productId, _products.First(x => x.Id == productId));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        public async void Edit_ValidModelState_UpdateMethodExecute(int productId) // Güncelleme Yapma Testi.
        {
            var product = _products.First(x=>x.Id==productId);
            _mock.Setup(x => x.Update(product));
            await _controller.Edit(productId,product);
            _mock.Verify(x=>x.Update(It.IsAny<Product>()),Times.Once);
        }


        /// <summary>
        /// Edit Metodu İçin Test İşlemleri
        /// </summary>

        /// ***************************************************************************///

        /// <summary>
        /// Delete Metodu İçin Test İşlemleri
        /// </summary>


        [Fact]
        public async void Delete_IdIsNull_ReturnViewNotFound() // Id Null olduğunda Not Found Sayfasına gönderme Testi
        {
            var result = await _controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(0)]
        public async void Delete_IdIsNotEqualProduct_ReturnViewNotFound(int productId) // Geçersiz Null ürün olduğunda not found sayfasına gönderme Testi.
        {
            Product product = null;
            _mock.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _controller.Delete(productId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void Delete_ActionExecutes_ReturnProduct(int productId) // Geçerli Id olduğunda Delete methodu çalışsın ve sayfayı döndürsün.
        {
            var product= _products.First(x=>x.Id==productId);
            _mock.Setup(x=>x.GetById(productId)).ReturnsAsync(product);
            var result= await _controller.Delete(productId);
            var viewResult= Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Product>(viewResult.Model);    
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_ReturnRedirectToIndexAction(int productId) // Silme işlemi tamamlandığında sayfaya dönme testi.
        {
            var result = await _controller.DeleteConfirmed(productId);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_DeleteMethodExecutes(int productId) // Delete Confirmed methotu çalışıyor mu testi.
        {
             var product=_products.First(x=>x.Id== productId);
            _mock.Setup(x => x.Delete(product));
            await _controller.DeleteConfirmed(productId);
            _mock.Verify(x=>x.Delete(It.IsAny<Product>()),Times.Once); 
        }
    }
}
