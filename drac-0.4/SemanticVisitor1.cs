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
    class SemanticVisitor1{
        public IDictionary<string, bool, int, WeakReference> GlobalFunctions {
            get;
            private set;
        }

        public ISet<string> GlobalVariables {
            get;
            private set;
        }
        public SemanticVisitor1(){
            GlobalFunctions = new IDictionary<string, bool, int, WeakReference>();
            GlobalVariables = new ISet<string>();
        }
    }
}