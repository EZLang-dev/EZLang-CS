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
    package sys;
    sys.Print(""Hello World!"");
}
globals{
    function Test(){
        if(a == ""a"")
            return;
    }
}
";
    }
}

