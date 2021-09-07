using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace UnitTestit
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestaaMethod1(int res)
        {
            var asm = Assembly.LoadFrom(@"C:\work\v11\TestiAlusta\bin\Debug\net5.0\TestiAlusta.dll");
            var luokat = asm.GetTypes().Where(a => a.FullName.Contains("Dummy")).FirstOrDefault();
            var metodit = luokat.GetMethods().First();
            if (metodit == null)
            {
                throw new Exception("Metodi virheellinen tai puuttuu");
            }

            //ParameterInfo[] parametrit = metodi.GetParameters();
            //if (parametrit.Length != 2 || parametrit[0].ParameterType != typeof(int) || parametrit[1].ParameterType != typeof(int))
            //{
            //    throw new Exception("Metodin parametrit virheelliset");
            //}
            var instanssi = asm.CreateInstance(luokat.FullName);
            //var tulos = metodit.Invoke(instanssi, new object[] { 12, 2 });
            var tulos = metodit.Invoke(instanssi, null);
            try
            {
                Assert.AreEqual(res, tulos);

            }
            catch (Exception)
            {

                throw new Exception("Tulos ei vastannut oletettua");
            }
        }
    }
}
