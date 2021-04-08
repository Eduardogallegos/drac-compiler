/*
  Drac compiler - This class performs the lexical analysis,
  (a.k.a. scanning).
  Alejandro Chavez A01374974
  Pedro Cortes Soberanes A01374919
  Eduardo Gallegos Solis A01745776
  ITESM CEM
*/

/*
 * Drac LL(1) Grammar:
 *      Program         ::= DefList
 *      DefList         ::= Def*
 *      Def             ::= VarDef | FunDef
 *      VarDef          ::= "var" VarList ";"
 *      VarList         ::= IDList
 *      IDList          ::= ID IDListCont
 *      IDListCont      ::= ("," ID)*
 *      FunDef          ::= ID "(" ParamList ")" "{" VarDefList StmtList "}"
 *      ParamList       ::= 
 *      VarDefList      ::= 
 *      StmtList        ::= 
 *      Stmt            ::= 
 *      StmtAssign      ::= 
 *      StmtIncr        ::= 
 *      StmtDecr        ::= 
 *      StmtFunCall     ::= 
 *      FunCall         ::= 
 *      ExprList        ::= 
 *      ExprListCont    ::= 
 *      StmtIf          ::= 
 *      ElseIfList      ::= 
 *      Else            ::= 
 *      StmtWhile       ::= 
 *      StmtDoWhile     ::= 
 *      StmtBreak       ::= 
 *      StmtReturn      ::= 
 *      StmtEmpty       ::= 
 *      Expr            ::= 
 *      ExprOr          ::= 
 *      ExprAnd         ::= 
 *      ExprComp        ::= 
 *      OpComp          ::= 
 *      ExprRel         ::= 
 *      OpRel           ::= 
 *      ExprAdd         ::= 
 *      OpAdd           ::= 
 *      ExprMul         ::= 
 *      OpMul           ::= 
 *      ExprUnary       ::= 
 *      OpUnary         ::= 
 *      ExprPrimary     ::= 
 *      Array           ::= 
 *      Lit             ::= 
 */
using System;
using System.Collections.Generic;

namespace Drac{
    class Parser{
        // LL1 ORs 
        static readonly ISet<TokenCategory> firstOfDef =
            new HashSet <TokenCategory>(){
                // token categories for Def
            };
        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream){
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        
    }
}