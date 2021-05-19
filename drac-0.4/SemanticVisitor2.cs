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

namespace Drac{
    class SemanticVisitor2{

      public ISet<string> LocalVariablesTable
        {
            get;
            private set;
        }

      public void Visit(IdList node)
        {
            foreach (var childNode in node)
            {
                var variableName = childNode.AnchorToken.Lexeme;
                if (SemanticVisitor1.GlobalVariablesTable.Contains(variableName)|LocalVariablesTable.Contains(variableName))
                {
                    throw new SemanticError("Duplicated variable: " + variableName, childNode);
                }
                else
                {
                    LocalVariablesTable.Add(variableName);
                }
            }
        }
        
     public SemanticVisitor2()
        {
            
            LocalVariablesTable = new ISet<string>();
            
        }



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

          var variableName = node.AnchorToken.Lexeme;

            if (Table.ContainsKey(variableName)) {

                var expectedType = Table[variableName];

                if (expectedType != Visit((dynamic) node[0])) {
                    throw new SemanticError(
                        "Expecting type " + expectedType
                        + " in assignment statement",
                        node.AnchorToken);
                }

            } else {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    node.AnchorToken);
            }

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