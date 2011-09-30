using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BF.Tokens;
using System.Collections;
using BF.Exceptions;

namespace BF.Parser
{
    public static class BFParser
    {
        public static IEnumerable<Token> Parse(string code)
        {
            IList<Token> top = new List<Token>();
            Stack<IList<Token>> stack = new Stack<IList<Token>>();

            int pos = 0;

            foreach (char character in code)
            { 
                switch (character)
                {
                    case '[':
                        stack.Push(top);
                        top = new List<Token>();
                        break;
                    case ']':
                        if (!stack.Any())
                        {
                            throw new ParseException("Unexpected ] at position " + pos);
                        }
                        IEnumerable<Token> innerTokens = top;
                        top = stack.Pop();
                        top.Add(new Loop(innerTokens));
                        break;
                    case '+':
                        top.Add(new Incr());
                        break;
                    case '-':
                        top.Add(new Decr());
                        break;
                    case '.':
                        top.Add(new Out());
                        break;
                    case ',':
                        top.Add(new In());
                        break;
                    case '<':
                        top.Add(new DecrPtr());
                        break;
                    case '>':
                        top.Add(new IncrPtr());
                        break;
                }

                pos++;
            }

            if (stack.Any())
            {
                throw new ParseException("Expected ] at EOF");
            }

            return top;
        }
    }
}
