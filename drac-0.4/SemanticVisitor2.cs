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
    }
}