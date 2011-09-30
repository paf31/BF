using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace BF.Tokens
{
    public class In : Token
    {
        public void EmitIL(ILGenerator body, FieldInfo tape, FieldInfo ptr)
        {
            body.Emit(OpCodes.Ldsfld, tape);
            body.Emit(OpCodes.Ldsfld, ptr);
            body.EmitCall(OpCodes.Call,
                typeof(Console).GetMethod("Read", new Type[0]), null);
            body.Emit(OpCodes.Stelem_I4);
        }
    }
}
