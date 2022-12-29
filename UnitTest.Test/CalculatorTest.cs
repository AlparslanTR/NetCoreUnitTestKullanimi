using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestApp;
using Xunit;

namespace UnitTest.Test
{
    public class CalculatorTest
    {
        [Fact]
        public void AddTest()
        {
            // Arrange -- Değişkenleri Verdiğimiz Yerdir.

            int a = 10;
            int b = 20;
            var calculator = new Calculator();

            // Act -- Test Edilecek Metotların Çalıştığı Yerdir

            var total=calculator.Add(a, b);

            // Assert -- Doğrulama Evresidir. Çıkan Sonucun Doğru olup olmadığını Test ettiğimiz yerdir.

            Assert.Equal(30,total); // İki değer aynı ise test başarılı geçer.
            /*Assert.NotEqual(30, total);*/ // İki değerden biri farklı olursa test başarılı geçer ama burada ikiside aynı olacağı için başarısız olacak.

        }

    }
}
