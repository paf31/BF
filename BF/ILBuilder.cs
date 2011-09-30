using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using BF.Tokens;

namespace BF
{
    public class ILBuilder
    {
        private readonly int tapeSize;
        private readonly string filename;
        private readonly string assemblyName;
        private readonly IEnumerable<Token> tokens;

        public const string ProgramClassName = "Program";
        public const string MainMethodName = "Main";
        public const string TapeFieldName = "Tape";
        public const string PointerFieldName = "Ptr";

        public ILBuilder(string filename, string assemblyName, int tapeSize, IEnumerable<Token> tokens)
        {
            this.filename = filename;
            this.assemblyName = assemblyName;
            this.tapeSize = tapeSize;
            this.tokens = tokens;
        }

        public void Build() 
        {
            var name = new AssemblyName(assemblyName);
            var domain = AppDomain.CurrentDomain;
            var assembly = domain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);

            var module = assembly.DefineDynamicModule(assemblyName, filename);

            var programClass = module.DefineType(ProgramClassName,
                TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.Class);

            var mainMethod = programClass.DefineMethod(MainMethodName,
                MethodAttributes.Static | MethodAttributes.Public,
                null, new[] { typeof(string[]) });

            var tape = programClass.DefineField(TapeFieldName, typeof(int[]),
                FieldAttributes.Private | FieldAttributes.Static);

            var ptr = programClass.DefineField(PointerFieldName, typeof(int),
                FieldAttributes.Private | FieldAttributes.Static);

            var body = mainMethod.GetILGenerator();
            
            body.Emit(OpCodes.Ldc_I4, tapeSize);
            body.Emit(OpCodes.Newarr, typeof(int));
            body.Emit(OpCodes.Stsfld, tape);

            tokens.ToList().ForEach(t => t.EmitIL(body, tape, ptr));

            programClass.CreateType();

            assembly.SetEntryPoint(mainMethod);

            assembly.Save(filename);
        }
    }
}
