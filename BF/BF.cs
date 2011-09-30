using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using BF.Parser;
using BF.Tokens;
using BF.Exceptions;

namespace BF
{
    public class BF
    {
        private static string ReadFile(string filename)
        {
            string result = null;

            using (var fs = new FileStream(filename, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    try
                    {
                        result = sr.ReadToEnd();
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("File does not exist.");
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Unable to read file.");
                    }
                }
            }

            return result;
        }

        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: bf.exe <input file> <output file> <tape size>");
            }
            else
            {
                var code = ReadFile(args[0]);

                int tapeSize = 1024;

                if (!int.TryParse(args[2], out tapeSize))
                {
                    Console.WriteLine("Tape size must be an integer. Using default value of 1024.");
                }

                if (code != null)
                {
                    var validChars = code.Where(c =>
                        c == '.' || c == ',' ||
                        c == '<' || c == '>' ||
                        c == '+' || c == '-' ||
                        c == '[' || c == ']');

                    try
                    {
                        IEnumerable<Token> tokens = BFParser.Parse(new string(validChars.ToArray()));

                        if (tokens == null)
                        {
                            Console.WriteLine("Unable to parse input.");
                        }
                        else
                        {
                            new ILBuilder(args[1], "bfoutput", tapeSize, tokens).Build();
                        }
                    }
                    catch (ParseException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
