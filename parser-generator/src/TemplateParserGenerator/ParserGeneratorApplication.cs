using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;

namespace TemplateParserGenerator
{
    internal class ParserGeneratorApplication: CommandLineApplication
    {
        internal ParserGeneratorApplication()
        {
            this.Name = "ttgen";
            this.Description = "This tool is used to transform text templates for creating code generators.";
            this.FullName = "WebApiProxy Template Transformation Generator";

            this.Command("create", c =>
            {
                c.Description = "Creates a code generator from the given template.";

                var sourceFileOption = c.Option("-s|--source", "Source template file names", CommandOptionType.MultipleValue);
                var classNameOption = c.Option("-c|--class", "Class name", CommandOptionType.SingleValue);
                var namespaceOption = c.Option("-ns|--namespace", "Namespace", CommandOptionType.SingleValue);
                
                c.HelpOption("-?|-h|--help");

                c.OnExecute(() =>
                {
                    var sources = sourceFileOption.Value();
                    if (!sourceFileOption.HasValue())
                    {
                        Console.WriteLine($"No input file(s) specified. Using *.template in {AppContext.BaseDirectory}");
                        sources = string.Join(" ", Directory.GetFiles(AppContext.BaseDirectory, "*.template"));
                    }
                    
                    var className = classNameOption.Value();
                    if (!classNameOption.HasValue())
                    {
                        className = "TextTemplateParser";
                    }
                    var ns = namespaceOption.Value();
                    if (!namespaceOption.HasValue())
                    {
                        ns = "WebApiProxy.Tools";
                    }

                    var inputFiles = sources.Split(' ');
                    
                    foreach (var file in inputFiles)
                    {
                        var content = File.ReadAllText(file);
                        var generator = new Generator(content, ns, className);
                        var result = generator.Generate();

                        var fileInfo = new FileInfo(file);
                        var fileName = file.Replace(fileInfo.Extension, ".generator.cs");
                        File.WriteAllText(fileName, result);
                    }
                    

                    return 0;
                });
            });
            this.OnExecute(() =>
            {
                this.ShowHelp();
                return 2;
            });
        }
    }
}
