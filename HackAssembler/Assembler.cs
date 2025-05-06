namespace HackAssembler;

public class Assembler
{
    private Dictionary<string, int> _symbolTable = new Dictionary<string, int>();
    private Dictionary<string, string> _jumps = new Dictionary<string, string>();
    private Dictionary<string, string> _comps = new Dictionary<string, string>();

    private int _addressForVariable = 16;
    public void Initialize()
    {
        for (int i = 0; i < 16; i++) {
            _symbolTable.Add('R' + i.ToString(), i);
        }
        
        _symbolTable.Add("SCREEN", 16384);
        _symbolTable.Add("KBD", 24576);
        _symbolTable.Add("SP", 0);
        _symbolTable.Add("LCL", 1);
        _symbolTable.Add("ARG", 2);
        _symbolTable.Add("THIS", 3);
        _symbolTable.Add("THAT", 4);
        
        _jumps.Add("JMP", "111");
        _jumps.Add("JLE", "110");
        _jumps.Add("JNE", "101");
        _jumps.Add("JLT", "100");
        _jumps.Add("JGE", "011");
        _jumps.Add("JEQ", "010");
        _jumps.Add("JGT", "001");
        
        _comps.Add("0", "101010");
        _comps.Add("1", "111111");
        _comps.Add("-1", "111010");
        _comps.Add("D", "001100");
        _comps.Add("A", "110000");
        _comps.Add("!D", "001101");
        _comps.Add("!A", "110001");
        _comps.Add("-D", "001111");
        _comps.Add("-A", "110011");
        _comps.Add("D+1", "011111");
        _comps.Add("A+1", "110111");
        _comps.Add("D-1", "001110");
        _comps.Add("A-1", "110010");
        _comps.Add("D+A", "000010");
        _comps.Add("A+D", "000010");
        _comps.Add("D-A", "010011");
        _comps.Add("A-D", "000111");
        _comps.Add("D&A", "000000");
        _comps.Add("D|A", "010101");
        _comps.Add("A&D", "000000");
        _comps.Add("A|D", "010101");
    }
    
    public void TranslateToBinary(string file, string output)
    {
        FileStream fileStream = new FileStream(file, FileMode.Open);
        FileStream outputStream = new FileStream(output, FileMode.Create, FileAccess.Write);
        
        StreamReader reader = new StreamReader(fileStream);
        StreamWriter writer = new StreamWriter(outputStream);
        
        string[] lines = reader.ReadToEnd().Replace("\r", "").Replace(" ", "").Split('\n');
        int lineCounter = 0;
        
        // First loop - find labels and add to symbol table
        foreach (var line in lines)
        {
            string instruction = ClearFromComments(line);
            if (instruction == "") continue;
            
            if (instruction.StartsWith('(') && instruction.EndsWith(')'))
            {
                lineCounter--;
                string label = instruction.Substring(1, instruction.Length - 2);
                _symbolTable[label] = lineCounter+1;
            }
            lineCounter++;
        }
        
        // Second loop - translate instructions to binary
        foreach (var line in lines)
        {
            string instruction = ClearFromComments(line);
            if (instruction == "") continue;

            if (instruction.StartsWith("@"))
            {
                string label = instruction.Substring(1, instruction.Length - 1);
                
                if (!Int32.TryParse(label, out int value))
                {
                    if (!_symbolTable.ContainsKey(label))
                    {
                        _symbolTable.Add(label, _addressForVariable);
                        _addressForVariable++;
                    }
                }
                writer.WriteLine(TranslateAInstruction(instruction));
            }
            else if (instruction.StartsWith('('))
                continue;
            else
                writer.WriteLine(TranslateCInstruction(instruction));
        }
        writer.Flush();
        fileStream.Close();
        outputStream.Close();
    }
    
    private string TranslateAInstruction(string instruction)
    {
        string valueForA = instruction.Substring(1, instruction.Length - 1);
        
        if (_symbolTable.ContainsKey(valueForA))
            return ConvertIntToBinaryString(Convert.ToInt32(_symbolTable[valueForA]), 16);
        
        return ConvertIntToBinaryString(Convert.ToInt32(valueForA), 16);
    }
    
    private string TranslateCInstruction(string instruction)
        => "111" + GetCompAndABits(instruction) + GetDestBits(instruction) + GetJumpBits(instruction);
    
    
    // Utils
    private string ClearFromComments(string instruction)
        => instruction.Substring(0, instruction.Contains("//") ? instruction.IndexOf('/') : instruction.Length);

    private string ConvertIntToBinaryString(int value, int numberOfBits)
        => Convert.ToString(value & 0xFFFF, 2).PadLeft(numberOfBits, '0');
    
    
    // C Instruction bits
    private string GetJumpBits(string instruction)
    {
        if (instruction.Contains(';'))
        {
            string jump = instruction.Split(';')[1];
            return _jumps[jump];
        }
        
        return "000";
    }

    private string GetDestBits(string instruction)
    {
        string res = "";
        
        if (instruction.Contains('='))
        {
            string dest = instruction.Split('=')[0];
                    
            if (dest.Contains('A')) res += '1';
            else res += '0';
                    
            if (dest.Contains('D')) res += '1';
            else res += '0';
                    
            if (dest.Contains('M')) res += '1';
            else res += '0';

            return res;
        }
        return "000";
    }

    private string GetCompAndABits(string instruction)
    {
        string compBits, aBit = "0";
        
        if (instruction.Contains('='))
        {
            string comp = instruction.Substring(instruction.IndexOf('=') + 1, instruction.Contains(';') 
                ? instruction.IndexOf(';') : instruction.Length - 2);
            
            if (comp.Contains('M'))
            {
                aBit = "1";
                comp = comp.Replace('M', 'A');
            }
            compBits = _comps[comp];
        }
        else
        {
            compBits = "101010";
            
            if (!instruction.StartsWith("0"))
            {
                string comp = instruction.Substring(0, instruction.IndexOf(';'));
                if (comp.Contains('M'))
                {
                    aBit = "1";
                    comp = comp.Replace('M', 'A');
                }
                compBits = _comps[comp];
            }
        }
        return aBit + compBits;
    }
}