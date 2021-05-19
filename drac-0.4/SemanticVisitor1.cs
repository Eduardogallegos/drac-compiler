/*
  Drac compiler - This class performs the lexical analysis,
  (a.k.a. scanning).
  Alejandro Chavez A01374974
  Pedro Cortes Soberanes A01374919
  Eduardo Gallegos Solis A01745776
  ITESM CEM
*/

using System;
using System.Collections.Generic;

namespace Drac
{
    class SemanticVisitor1
    {
        public struct Function
        {
            bool IsPrimitive { get; set; }
            public int Arity { get; set; }
            public IDictionary<string, bool> SymbolTable { get; set; }
            public Function(bool primitive, int arity, IDictionary<string, bool> symbolTable)
            {
                this.IsPrimitive = primitive;
                this.Arity = arity;
                this.SymbolTable = symbolTable;
            }
            public override string ToString() => $"\t| {IsPrimitive} \t | {Arity}  \t | {SymbolTable} \t |";
        }
        public IDictionary<string, Function> GlobalFunctionsTable
        {
            get;
            private set;
        }

        public ISet<string> GlobalVariablesTable
        {
            get;
            private set;
        }

        private void SetAPIFunction(string functionName, int functionArity, bool functionPrimitive = true, IDictionary<string, bool> functionSymbolTable = null){
            Function newFunction = new Function(primitive: functionPrimitive, arity: functionArity, symbolTable: functionSymbolTable);
            GlobalFunctionsTable.Add(functionName, newFunction);
        }
        private void SetAPIFunctions(){
            SetAPIFunction(functionName:"printi", functionArity:1);
            SetAPIFunction(functionName:"printc", functionArity:1);
            SetAPIFunction(functionName:"prints", functionArity:1);
            SetAPIFunction(functionName:"println", functionArity:0);
            SetAPIFunction(functionName:"readi", functionArity:0);
            SetAPIFunction(functionName:"reads", functionArity:0);
            SetAPIFunction(functionName:"new", functionArity:1);
            SetAPIFunction(functionName:"size", functionArity:1);
            SetAPIFunction(functionName:"add", functionArity:2);
            SetAPIFunction(functionName:"get", functionArity:2);
            SetAPIFunction(functionName:"set", functionArity:3);
        }

        public SemanticVisitor1()
        {
            GlobalFunctionsTable = new SortedDictionary<string, Function>();
            GlobalVariablesTable = new HashSet<string>();
            SetAPIFunctions();
        }

        public void Visit(Program node)
        {
            VisitChildren(node);
        }

        public void Visit(VarDef node)
        {
            VisitChildren(node);
        }

        public void Visit(Identifier node)
        {
            VisitChildren(node);
        }

        public void Visit(IdList node)
        {
            foreach (var childNode in node)
            {
                var variableName = childNode.AnchorToken.Lexeme;
                if (GlobalVariablesTable.Contains(variableName))
                {
                    throw new SemanticError("Duplicated global variable: " + variableName, childNode.AnchorToken);
                }
                else
                {
                    GlobalVariablesTable.Add(variableName);
                }
            }
        }

        public void Visit(Funcion node)
        {
            // VisitChildren(node); ASK: No se debe visitar los hijos en la primer iteracion, cierto?
            var functionName = node.AnchorToken.Lexeme;
            if (GlobalFunctionsTable.ContainsKey(functionName))
            {
                throw new SemanticError("Duplicated function: " + functionName, node.AnchorToken);
            }
            else
            {
                Function newFunction = new Function(primitive: false, arity: node[0].length, symbolTable: null);
                GlobalFunctionsTable.Add(functionName, newFunction);
            }
        }
// ASK: Las siguientes funciones son necesarias?
        public void Visit(VarDefList node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtList node)
        {
            VisitChildren(node);
        }

        public void Visit(Assignment node)
        {
            VisitChildren(node);
        }

        public void Visit(FunctionCall node)
        {
            VisitChildren(node);
        }

        public void Visit(Increase node)
        {
            VisitChildren(node);
        }

        public void Visit(Decrease node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtIf node)
        {
            VisitChildren(node);
        }

        public void Visit(ElseIfList node)
        {
            VisitChildren(node);
        }

        public void Visit(ElseIf node)
        {
            VisitChildren(node);
        }

        public void Visit(Else node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtWhile node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtDoWhile node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtBreak node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtReturn node)
        {
            VisitChildren(node);
        }

        public void Visit(StmtEmpty node)
        {
            VisitChildren(node);
        }

        public void Visit(Equals node)
        {
            VisitChildren(node);
        }

        public void Visit(Diff node)
        {
            VisitChildren(node);
        }
        public void Visit(Less node)
        {
            VisitChildren(node);
        }
        public void Visit(LessEqual node)
        {
            VisitChildren(node);
        }

        public void Visit(Greater node)
        {
            VisitChildren(node);
        }

        public void Visit(MoreEqual node)
        {
            VisitChildren(node);
        }

        public void Visit(Neg node)
        {
            VisitChildren(node);
        }

        public void Visit(Plus node)
        {
            VisitChildren(node);
        }

        public void Visit(Mul node)
        {
            VisitChildren(node);
        }

        public void Visit(Div node)
        {
            VisitChildren(node);
        }
        public void Visit(Mod node)
        {
            VisitChildren(node);
        }
        public void Visit(Not node)
        {
            VisitChildren(node);
        }
        public void Visit(Positive node)
        {
            VisitChildren(node);
        }
        public void Visit(Negative node)
        {
            VisitChildren(node);
        }
        public void Visit(True node)
        {
            VisitChildren(node);
        }
        public void Visit(False node)
        {
            VisitChildren(node);
        }
        public void Visit(Int_literal node)
        {
            VisitChildren(node);
        }
        public void Visit(Char_lit node)
        {
            VisitChildren(node);
        }
        public void Visit(String_lit node)
        {
            VisitChildren(node);
        }
        public void Visit(Or node)
        {
            VisitChildren(node);
        }
        public void Visit(And node)
        {
            VisitChildren(node);
        }
        public void Visit(Array node)
        {
            VisitChildren(node);
        }
        public void Visit(ExprList node)
        {
            VisitChildren(node);
        }

        void VisitChildren(Node node)
        {
            foreach (var n in node)
            {
                Visit((dynamic)n);
            }
        }
    }
}