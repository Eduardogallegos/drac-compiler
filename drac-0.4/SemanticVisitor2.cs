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
        public SemanticVisitor1 visitor1{
            get;
        }
        public SemanticVisitor2(SemanticVisitor1 visitor1)
        {
            this.visitor1 = visitor1;
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

        public void Visit(IdList node)
        {
            foreach (var childNode in node)
            {
                var variableName = childNode.AnchorToken.Lexeme;
                if (visitor1.GlobalVariablesTable.Contains(variableName) | LocalVariablesTable.Contains(variableName) | LocalParametersTable.Contains(variableName))
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

        // CHECK
        public void Visit(Assignment node)
        {
            var variableName = node.AnchorToken.Lexeme;

            if (LocalVariablesTable.Contains(variableName))
            {
                
            }
            else if (LocalParametersTable.Contains(variableName))
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

        public void Visit(FunctionCall node)
        {
            var functionName = node.AnchorToken.Lexeme;

            if (!visitor1.GlobalFunctionsTable.ContainsKey(functionName))
            {
                throw new SemanticError("The function doesn't exist" + functionName, node.AnchorToken);
            }
            if (node[0].length != visitor1.GlobalFunctionsTable[functionName].Arity)
            {
                throw new SemanticError("Incorrect number of parameters" + functionName, node.AnchorToken);
            }
            //VisitChildren(node); ASK: Es necesario visitar a los nodos hijos de nuevo?
        }

        public void Visit(Funcion node)
        {
            LocalVariablesTable = new HashSet<string>();
            LocalParametersTable = new HashSet<string>();
            var functionName = node.AnchorToken.Lexeme;
            if (visitor1.GlobalFunctionsTable.ContainsKey(functionName))
            {

                throw new SemanticError("Duplicate function" + functionName, node.AnchorToken);
            }
            foreach (var parameterNode in node[0])
            {
                var parameter = parameterNode.AnchorToken.Lexeme;
                if (!LocalParametersTable.Contains(parameter))
                {
                    LocalParametersTable.Add(parameter);
                }
                else
                {
                    throw new SemanticError("Duplicate parameter" + parameter, parameterNode.AnchorToken);
                }

            }

            foreach (var variableNode in node[1])
            {
                var variable = variableNode.AnchorToken.Lexeme;
                if (LocalVariablesTable.Contains(variable))
                {
                    throw new SemanticError("Duplicate variable" + variable, variableNode.AnchorToken);
                }
                LocalVariablesTable.Add(variable);
            }
            // TODO: Add references to the GlobalFunctionsTable

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