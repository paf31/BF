using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace BF.Tokens
{
    public interface Token
    {
        void EmitIL(ILGenerator body, FieldInfo tape, FieldInfo pointer);
    }
}
