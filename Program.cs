using System;

namespace Lexico_1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lexico L = new Lexico())
                {
                    while (!L.FinArchivo())
                    {
                        L.nextToken();
                    }
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error: "+e.Message);
            }

        }
    }
}
