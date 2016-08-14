using System;

namespace WebApiProxy.Tools.TTGenerator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var app = new ParserGeneratorApplication();

                return app.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return 1;
            }
            
        }
    }
}
