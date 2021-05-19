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
        private static visitor1 = SemanticVisitor1;
        public SemanticVisitor2()
        {




        }


        public ISet<string> LocalVariablesTable
        {
            get;
            private set;
        }

        public ISet<string> LocalParametersTable
        {
            get;
            private set;
        }

        static readonly IDictionary<TokenCategory, Type> typeMapper =
            new Dictionary<TokenCategory, Type>() {
                { TokenCategory.BOOL, Type.BOOL },
                { TokenCategory.INT, Type.INT }
            };

        public void Visit(IdList node)
        {


            foreach (var childNode in node)
            {
                var variableName = childNode.AnchorToken.Lexeme;
                if (SemanticVisitor1.GlobalVariablesTable.ContainsKey(variableName) | LocalVariablesTable.ContainsKey(variableName) | LocalParametersTable.ContainsKey(variableName))
                {
                    throw new SemanticError("Duplicated variable: " + variableName, childNode.AnchorToken);
                }
                else
                {
                    LocalVariablesTable.Add(variableName);
                }
            }
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

            if (Table.ContainsKey(variableName))
            {

                var expectedType = Table[variableName];

                if (expectedType != Visit((dynamic)node[0]))
                {
                    throw new SemanticError(
                        "Expecting type " + expectedType
                        + " in assignment statement",
                        node.AnchorToken);
                }

            }
            else
            {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    node.AnchorToken);
            }

            VisitChildren(node);
        }

        public void Visit(FunctionCall node)
        {

            var functionName = node.AnchorToken.Lexeme;

            if (!SemanticVisitor1.GlobalFunctionsTable.ContainsKey(functionName))
            {
                throw new SemanticError("The function doesn't exist" + functionName, node.AnchorToken);
            }
            if (node[0].length != SemanticVisitor1.GlobalFunctionsTable[functionName].Arity)
            {
                throw new SemanticError("Incorrect number of parameters" + functionName, node.AnchorToken);
            }

            //VisitChildren(node);


        }

        public void Visit(Funcion node)
        {
            LocalVariablesTable = new ISet<string>();
            LocalParametersTable = new ISet<string>();
            var functionName = node.AnchorToken.lexeme;
            if (SemanticVisitor1.GlobalFunctionsTable.ContainsKey(functionName))
            {

                throw new SemanticError("Duplicate function" + functionName, node.AnchorToken);
            }
            foreach (var parameter in node[0])
            {
                if (!LocalParametersTable.ContainsKey(parameter))
                {
                    LocalParametersTable.add(parameter);
                }
                else
                {
                    throw new SemanticError("Duplicate parameter" + parameter, parameter.AnchorToken);
                }

            }

            foreach (var variable in node[1])
            {
                if (LocalVariablesTable.ContainsKey(variable))
                {
                    throw new SemanticError("Duplicate variable" + variable, variable.AnchorToken);
                }
                LocalVariablesTable.add(variable);
            }

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
            var returnStr = node.AnchorToken.Lexeme;
            int value;

            if (!Int32.TryParse(returnStr, out value))
            {
                throw new SemanticError(
                    $"Return literal too large: {returnStr}",
                    node.AnchorToken);
            }
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

            if (!Int32.TryParse(lit_Str, out value))
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

            var lit_Str = node.AnchorToken.Lexeme;
            int value;

            if (!Int32.TryParse(lit_Str, out value))
            {
                throw new SemanticError(
                    $"String literal too large: {lit_Str}",
                    node.AnchorToken);
            }
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
            var lit_Str = node.AnchorToken.Lexeme;
            int value;

            if (!Int32.TryParse(lit_Str, out value))
            {
                throw new SemanticError(
                    $"String literal too large: {lit_Str}",
                    node.AnchorToken);
            }
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