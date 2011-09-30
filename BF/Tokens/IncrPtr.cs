using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace BF.Tokens
{
    public class IncrPtr : Token
    {
        public void EmitIL(ILGenerator body, FieldInfo tape, FieldInfo ptr)
        {
            body.Emit(OpCodes.Ldsfld, ptr);
            body.Emit(OpCodes.Ldc_I4_1);
            body.Emit(OpCodes.Add);
            body.Emit(OpCodes.Stsfld, ptr);
        }
    }
}
