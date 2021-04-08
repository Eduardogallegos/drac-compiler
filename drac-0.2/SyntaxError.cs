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
using System.Text;

namespace Buttercup {

    class SyntaxError: Exception {

        public SyntaxError(TokenCategory expectedCategory,
                           Token token):
            base($"Syntax Error: Expecting {expectedCategory} \n"
                 + $"but found {token.Category} (\"{token.Lexeme}\") at "
                 + $"row {token.Row}, column {token.Column}.") {
        }

        public SyntaxError(ISet<TokenCategory> expectedCategories,
                           Token token):
            base($"Syntax Error: Expecting one of {Elements(expectedCategories)}\n"
                 + $"but found {token.Category} (\"{token.Lexeme}\") at "
                 + $"row {token.Row}, column {token.Column}.") {
        }

        static string Elements(ISet<TokenCategory> expectedCategories) {
            var sb = new StringBuilder("{");
            var first = true;
            foreach (var elem in expectedCategories) {
                if (first) {
                    first = false;
                } else {
                    sb.Append(", ");
                }
                sb.Append(elem);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}