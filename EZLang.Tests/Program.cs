using EZLang.Compiler;
namespace EZLang.Tests
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CompilerManager compiler = new CompilerManager();
            compiler.Compile(program);
        }

        static string program = @"
program{
    package sys
    sys.Console.Print(""Hello World!"")
    4 + 6 * 2 / 12 - 2
}
if(hello){
    sys.Console.Print(""Hi!"")
}
globals{
    function Test(){
    }
}
";
    }
}

