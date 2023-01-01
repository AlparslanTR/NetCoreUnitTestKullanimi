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
        [Fact] // Methodumuz eğer hiç parametre almıyorsa Fact Attribütü kullanmak zorunda.
        public void AddTest()
        {
            // Arrange -- Değişkenleri Verdiğimiz Yerdir.

            //int a = 10;
            //int b = 20;
            //var calculator = new Calculator();

            // Act -- Test Edilecek Metotların Çalıştığı Yerdir

            //var total=calculator.Add(a, b);

            // Assert -- Doğrulama Evresidir. Çıkan Sonucun Doğru olup olmadığını Test ettiğimiz yerdir.

            /* Assert.Equal(30,total);*/ // İki değer aynı ise test başarılı geçer.
            /*Assert.NotEqual(30, total);*/ // İki değerden biri farklı olursa test başarılı geçer ama burada ikiside aynı olacağı için başarısız olacak.

            /*  ////////////////////////////  */

            /*Assert.Contains("Kayhan", "Alparslan Akbaş");*/ // İçinde Kayhan kelimesini bulamadığı için hata verecek.
            /* Assert.DoesNotContain("Kayhan", "Alparslan Akbaş");*/ // İçinde kayhan kelimesi olmayacak dediğimiz için bu sefer doğru çalışıcak.

            /*  ////////////////////////////  */

            //var names = new List<string>() { "Alparslan", "Kayhan", "Ali", };
            //Assert.Contains(names, x => x == "Gazi");

            /*  ////////////////////////////  */

            //var regex = "^dog"; 
            //Assert.Matches(regex, "dog alparslan"); // Test sürecinde gelen değerin bildirilmiş olan Regex ifadesine uyup uymadığını kontrol eden metotlardır.

            /*  ////////////////////////////  */

            /* Assert.StartsWith("Alparslan", "Alparslan Akbas");*/  // Başta girilen string değerin istenilen değer en başta olup olmamasını kontrol eder.
            /* Assert.EndsWith("Alparslan", "Akbaş Alparslan");*/   // Başta girilen string değerin istenilen değer en sonda olup olmamasını kontrol eder.

            /*  ////////////////////////////  */

            /* Assert.IsType<string>("Alparslan");*/ // Belirlenen tipi kontrol eder.
            /*Assert.IsNotType<string>(41);*/ // Belirlenen Tip doğru değilse kontrol eder.

            /*  ////////////////////////////  */


        }

        [Theory] // Methotta istenilen değerleri inline olarak giriyoruz bu attribute sayesinde.
        [InlineData(2,10,12)]
        public void AddTest2(int a,int b,int total)
        {
            var calculator = new Calculator();
            var actualTotal = calculator.Add(a, b);
            Assert.Equal(total, actualTotal);
        }
    }
}
