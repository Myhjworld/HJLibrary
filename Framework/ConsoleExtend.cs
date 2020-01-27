using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class ConsoleExtend
    {
        public static void Show<T>(this T t)
        {
            Type type = t.GetType();
            Console.WriteLine("*********************Type.Name--Show-Start***********************");
            foreach (var prop in type.GetProperties())
            {
                Console.WriteLine($"{type.Name}.{prop.Name}={prop.GetValue(t)}");
            }
            Console.WriteLine("*********************Type.Name--Show-End***********************");

        }
    }

}
