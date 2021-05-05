/*
  Drac compiler - This class performs the lexical analysis,
  (a.k.a. scanning).
  Alejandro Chavez A01374974
  Pedro Cortes Soberanes A01374919
  Eduardo Gallegos Solis A01745776
  ITESM CEM
*/

namespace Drac
{

    class Program : Node { }
    class VarDef : Node { }
    class Identifier : Node { }
    class Funcion : Node { }
    class VarDefList : Node { }
    class StmtList : Node { }
    class Assignment : Node { }
    class FunCallCont : Node { }
    class Increase : Node { }
    class Decrease : Node { }
    class StmtIf : Node { }
    class ElseIfList : Node { }
    class ElseIf : Node { }
    class Else : Node { }
    class StmtWhile : Node { }
    class StmtDoWhile : Node { }
    class StmtBreak : Node { }
    class StmtReturn : Node { }
    class StmtEmpty : Node { }
    class Equals : Node { }
    class Diff : Node { }
    class Less : Node { }
    class LessEqual : Node { }
    class Greater : Node { }
    class MoreEqual : Node { }
    class Neg : Node { }
    class Plus : Node { }
    class Mul : Node { }
    class Div : Node { }
    class Mod : Node { }
    class Not : Node { }
    class exprPrimary : Node { }
}