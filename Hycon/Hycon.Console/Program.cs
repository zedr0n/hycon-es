using System;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Hycon.CrossCuttingConcerns;
using Hycon.Infrastructure.Logging;
using SimpleInjector;

using static System.Console;

namespace Hycon.Console
{
    class Instance
    {
        private readonly Container _container = new Container();

        public Instance()
        {
            Initialise();
            WriteLine("Initialization complete!");
        }
        
        private void Initialise()
        {
            var root = new CompositionRoot();
            root.ComposeApplication(_container);
            _container.Collection.Register<IHyconListener>(
                typeof(CreateBlockListener),
                typeof(PutBlockListener),
                typeof(ListBlockListener));
            _container.Verify();
        }
        
        public void RunListeners(string text)
        {
            var inputStream = new AntlrInputStream(text);
            var lexer = new HyconLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new HyconParser(commonTokenStream)
            {
                ErrorHandler = new BailErrorStrategy()
            };

            var context = parser.command();
            
            foreach(var listener in _container.GetAllInstances<IHyconListener>())
                ParseTreeWalker.Default.Walk(listener, context);    
        }
    }
    
    
    class Program
    {
        static void Run()
        {
            string input;
            var instance = new Instance();
                
            // to type the EOF character and end the input: use CTRL+D, then press <enter>
            while ((input = ReadLine()) != "EOF")
            {
                if(input == "")
                    continue;
                var text = new StringBuilder();
                text.AppendLine(input);
                    
                instance.RunListeners(text.ToString());
            }    
        }
        
        static void Main(string[] args)
        {
            WriteLine("Hycon.Console");
            {
                try
                {
                    Run();
                }
                catch (Exception ex)
                {
                    if(!(ex is ParseCanceledException))
                        WriteLine("Error: " + ex);                
                } 
            }
        }
    }
}