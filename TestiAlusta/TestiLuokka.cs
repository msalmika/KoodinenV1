using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestiAlusta
{
    public class TestiLuokka
    {
        public void MuodostaTesti(string syöte)
        {
            string path = Path.GetFullPath(@"C:\work\v11\TestiAlusta\");
            string teksti = "using System; \nusing System.Collections.Generic;\nusing System.Linq; \n" +
                "using System.Text; \nusing System.Threading.Tasks; \n\nnamespace TestiAlusta\n\t{\n\tclass Dummy\n\t\t{\n\t\t";
            teksti += syöte;
            teksti += "}\n\t}";
            File.WriteAllText(path + "Dummy.cs", teksti);
        }
        public string TestaaSyöte()
        {
            #region Testit

            var asm = Assembly.LoadFrom(@"C:\work\v11\UnitTestit\bin\Debug\net5.0\UnitTestit.dll");


            // Get all the test classes in the assembly
            var testClassTypes = asm.GetTypes();
            var met = testClassTypes[1].GetMethods().Where(m => m.Name.Contains("Testaa"));
            var tc = Activator.CreateInstance(testClassTypes[1], null);
            bool kaikkiOikein = true;
            foreach (var m in met)
            {
                try
                {
                    var testResult = m.Invoke(tc, new object[] { 2 });
                    //Console.WriteLine("Success");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                    kaikkiOikein = false;
                }
            }
            if (kaikkiOikein == true)
            {
                return "Kaikki testit läpäisty onnistuneesti!";
            }
            return "Fail";
            #endregion Testit

        }
        public void KirjoitaPäälle()
        {
            string path = Path.GetFullPath(@"C:\work\v11\TestiAlusta\");
            string teksti = "using System; \nusing System.Collections.Generic;\nusing System.Linq; \n" +
                "using System.Text; \nusing System.Threading.Tasks; \n\nnamespace TestiAlusta\n\t{\n\tclass Dummy\n\t\t{\n\t\t}\n\t}";
            File.WriteAllText(path + "Dummy.cs", teksti);
        }
    }
}
