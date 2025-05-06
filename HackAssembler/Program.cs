namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify file with asm extension to assemble");
            }
            else
            {
                string fileName = args[0];
                string outFile = Path.ChangeExtension(fileName, ".hack");
                
                Assembler assembler = new Assembler();
                assembler.Initialize();
                assembler.TranslateToBinary(fileName, outFile);
                
                Console.WriteLine("Assembling completed successfully. The output file is " + outFile);
            }
        }
    }
}