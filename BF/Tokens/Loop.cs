using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace BF.Tokens
{
    class Loop : Token
    {
        public IEnumerable<Token> Tokens { get; set; }

        public Loop(IEnumerable<Token> tokens)
        {
            Tokens = tokens;
        }

        public void EmitIL(ILGenerator body, FieldInfo tape, FieldInfo ptr)
        {
            var l1 = body.DefineLabel();
            var l2 = body.DefineLabel();

            body.MarkLabel(l1);

            body.Emit(OpCodes.Ldsfld, tape);
            body.Emit(OpCodes.Ldsfld, ptr);
            body.Emit(OpCodes.Ldelem_I4);
            body.Emit(OpCodes.Brfalse, l2);

            Tokens.ToList().ForEach(t => t.EmitIL(body, tape, ptr));

            body.Emit(OpCodes.Br, l1);
            body.MarkLabel(l2);

            body.Emit(OpCodes.Nop);
        }
    }
}
