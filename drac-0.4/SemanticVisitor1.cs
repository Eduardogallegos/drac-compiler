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
        public IDictionary<string, bool, int, WeakReference> GlobalFunctionsTable {
            get;
            private set;
        }

        public ISet<string> GlobalVariablesTable {
            get;
            private set;
        }
        public SemanticVisitor1(){
            GlobalFunctionsTable = new IDictionary<string, bool, int, WeakReference>();
            GlobalVariablesTable = new ISet<string>();
        }

        public void Visit(Program node){

        }

        public void Visit(VarDef node){
            
        }

        public void Visit(Identifier node){
            
        }

        public void Visit(IdList node){
            
        }

        public void Visit(Funcion node){
            
        }

        public void Visit(VarDefList node){
            
        }

        public void Visit(StmtList node){
            
        }

        public void Visit(Assignment node){
            
        }

        public void Visit(FunctionCall node){
            
        }

        public void Visit(Increase node){
            
        }

        public void Visit(Decrease node){
            
        }

        public void Visit(StmtIf node){
            
        }

        public void Visit(ElseIfList node){
            
        }

        public void Visit(ElseIf node){
            
        }

        public void Visit(Else node){
            
        }

        public void Visit(StmtWhile node){
            
        }

        public void Visit(StmtDoWhile node){
            
        }

        public void Visit(StmtBreak node){
            
        }

        public void Visit(StmtReturn node){
            
        }

        public void Visit(StmtEmpty node){
            
        }

        public void Visit(Equals node){
            
        }

        public void Visit(Diff node){
            
        }
        public void Visit(Less node){
            
        }
        public void Visit(LessEqual node){
            
        }

        public void Visit(Greater node){
            
        }

        public void Visit(MoreEqual node){
            
        }

        public void Visit(Neg node){
            
        }

        public void Visit(Plus node){
            
        }

        public void Visit(Mul node){
            
        }

        public void Visit(Div node){
            
        }
        public void Visit(Mod node){
            
        }
        public void Visit(Not node){
            
        }
        public void Visit(Positive node){
            
        }
        public void Visit(Negative node){
            
        }
        public void Visit(exprPrimary node){
            
        }
        public void Visit(True node){
            
        }
        public void Visit(False node){
            
        }
        public void Visit(Int_literal node){
            
        }
        public void Visit(Char_lit node){
            
        }
        public void Visit(String_lit node){
            
        }
        public void Visit(Or node){
            
        }
        public void Visit(And node){
            
        }
        public void Visit(Array node){
            
        }
        public void Visit(ExprList node){
            
        }

        void VisitChildren(Node node) {
            foreach (var n in node) {
                Visit((dynamic) n);
            }
        }
    }
}