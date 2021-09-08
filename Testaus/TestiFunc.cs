using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KoodinenV1.Testaus
{
    public class TestiFunc
    {

        const string template = @"
        public class ScriptedClass
            {
                public string DoPrint()
                {
                    @code
                }
            }";
        public static string TestaaKoodi(string syöte)
        {
            string data = "Hei maailma!";
            Console.WriteLine($"Kirjoita metodin koodi joka palauttaa tekstin {data} paluuarvona");

            string code = template.Replace("@code", syöte);
            CSharpScriptEngine.Execute(code);
            var ret = CSharpScriptEngine.Execute("new ScriptedClass().DoPrint()");
            if (ret.ToString() == data)
            {
                return "Oikein";
            }
            else
            {
                return "Pieleen meni";
            }
        }


    }


}
