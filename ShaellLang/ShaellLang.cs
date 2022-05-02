using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;

namespace ShaellLang
{
    public class ShaellLang
    {
        private ExecutionVisitor _executioner;
        private ShaellErrorReporter _errorReporter;

        public ShaellLang()
        {
            _executioner = new ExecutionVisitor();
        }
        
        public ShaellLang(IEnumerable<string> args)
        {
            _executioner = new ExecutionVisitor(args.ToArray());
        }

        private ShaellParser CreateParser(string code)
        {
            _errorReporter = new ShaellErrorReporter();
            AntlrInputStream inputStream = new AntlrInputStream(code);
            ShaellLexer shaellLexer = new ShaellLexer(inputStream);
            
            _errorReporter.SetErrorListener(shaellLexer);
            
            CommonTokenStream commonTokenStream = new CommonTokenStream(shaellLexer);
            var parser = new ShaellParser(commonTokenStream);
            
            _errorReporter.SetErrorListener(parser);
            if (_errorReporter.HasErrors)
            {
                throw new ShaellLangSyntaxException(_errorReporter);
            }
            return parser;
        }
        
        public void LoadStdLib()
        {
            _executioner.SetGlobal("print", new NativeFunc(StdLib.PrintFunc, 0));
            _executioner.SetGlobal("cd", new NativeFunc(StdLib.CdFunc, 0));
            _executioner.SetGlobal("exit", new NativeFunc(StdLib.ExitFunc, 0));
            _executioner.SetGlobal("debug_break", new NativeFunc(StdLib.DebugBreakFunc, 0));
            _executioner.SetGlobal("T", TableLib.CreateLib());
            _executioner.SetGlobal("A", TestLib.CreateLib());
        }
        
        public void SetGlobal(string name, IValue value)
        {
            _executioner.SetGlobal(name, value);
        }

        public IValue RunCode(string code)
        {
            var parser = CreateParser(code);
            //If we just run code, we cant have the args() syntax
            //Therefore we just parse stmts
            var stmts = parser.stmts();
            if (_errorReporter.HasErrors)
            {
                throw new ShaellLangSyntaxException(_errorReporter);
            }
            return _executioner.VisitStmts(stmts, false, true);
        }

        public IValue RunFile(string path)
        {
            var parser = CreateParser(File.ReadAllText(path));
            var progContext = parser.prog();
            if (_errorReporter.HasErrors)
            {
                throw new ShaellLangSyntaxException(_errorReporter);
            }
            return _executioner.Visit(progContext);
        }
    }

    class ShaellLangSyntaxException : Exception
    {
        private ShaellErrorReporter _errorListener;
        public ShaellLangSyntaxException(ShaellErrorReporter errorListener)
        {
            _errorListener = errorListener;
        }

        public override string ToString()
        {
            return _errorListener.ToString();
        }
    }
}