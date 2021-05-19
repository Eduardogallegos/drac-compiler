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
    class SemanticVisitor2
    {
        int loop_level = 0;
        public SemanticVisitor1 visitor1
        {
            get;
            private set;
        }

        public IDictionary<string, bool> SymbolTable
        {
            // Bool represents Is_Param?
            get;
            private set;
        }

        public SemanticVisitor2(SemanticVisitor1 visitor1)
        {
            this.visitor1 = visitor1;
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
            if (node != null)
            {
                foreach (var childNode in node)
                {
                    var variableName = childNode.AnchorToken.Lexeme;
                    if (SymbolTable.ContainsKey(variableName))
                    {
                        throw new SemanticError("Duplicated parameter: " + variableName, childNode.AnchorToken);
                    }
                    else
                    {
                        SymbolTable.Add(variableName, true);
                    }
                
            }
            VisitChildren(node);
            }
        }

        public void Visit(Funcion node)
        {
            SymbolTable = new SortedDictionary<string, bool>();
            VisitChildren(node);
            visitor1.GlobalFunctionsTable[node.AnchorToken.Lexeme].SymbolTable = SymbolTable;

        }
        public void Visit(VarDefList node)
        {
            
            if (node != null)
            {
                foreach (var variableNode in node)
                {
                    var variable = variableNode.AnchorToken.Lexeme;
                    if (SymbolTable.ContainsKey(variable))
                    {
                        throw new SemanticError("Duplicated local variable" + variable, variableNode.AnchorToken);
                    }
                    SymbolTable.Add(variable, false);
                }
            
            VisitChildren(node);}
        }

        public void Visit(StmtList node)
        {
            
            VisitChildren(node);
        }

        // CHECK
        public void Visit(Assignment node)
        {
            if(node!=null){
            var variableName = node.AnchorToken.Lexeme;

            if (SymbolTable.ContainsKey(variableName))
            {

            }
            else if (visitor1.GlobalVariablesTable.Contains(variableName))
            {

            }
            else
            {
                throw new SemanticError(
                    "Undeclared variable: " + variableName, node.AnchorToken);
            }

            VisitChildren(node);
            }
        }

        public void Visit(FunctionCall node)
        {
            if(node!=null){
            var functionName = node.AnchorToken.Lexeme;

            if (!visitor1.GlobalFunctionsTable.ContainsKey(functionName))
            {
                throw new SemanticError("The function doesn't exist" + functionName, node.AnchorToken);
            }
            if (node[0].length != visitor1.GlobalFunctionsTable[functionName].Arity)
            {
                throw new SemanticError("Incorrect number of parameters" + functionName, node.AnchorToken);
            }
            
            VisitChildren(node);}
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
            loop_level++;
            VisitChildren(node);
        }

        public void Visit(StmtDoWhile node)
        {
            loop_level++;
            VisitChildren(node);
        }

        public void Visit(StmtBreak node)
        {
            loop_level--;
            if(loop_level<0){
                throw new SemanticError("The break statement isn't inside the while and the do-while"+ node.AnchorToken.Lexeme, node.AnchorToken);
            }
            
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
            var intStr = node.AnchorToken.Lexeme;
            int value;

            if (!Int32.TryParse(intStr, out value))
            {
                throw new SemanticError(
                    $"Integer literal too large: {intStr}",
                    node.AnchorToken);
            }
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