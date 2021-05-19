/*
  Drac compiler - This class performs the lexical analysis,
  (a.k.a. scanning).
  Alejandro Chavez A01374974
  Pedro Cortes Soberanes A01374919
  Eduardo Gallegos Solis A01745776
  ITESM CEM
*/
using System;

namespace Drac {

    class SemanticError: Exception {

        public SemanticError(string message, Token token):
            base($"Semantic Error: {message} \n"
                 + $"at row {token.Row}, column {token.Column}.") {
        }

        public SemanticError(string message):
            base($"Semantic Error: {message}."){

            }
    }
}