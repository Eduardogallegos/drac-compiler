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
 *      ParamList       ::= IDList*
 *      VarDefList      ::= VarDef*
 *      StmtList        ::= Stmt*
 *      Stmt            ::= StmtAssign | StmtIncr | StmtDecr | StmtFunCall | StmtIf |
                        StmtWhile | StmtDoWhile | StmtBreak | StmtReturn | StmtEmpty
 *      StmtAssign      ::= ID "=" Expr
 *      StmtIncr        ::= "inc" ID
 *      StmtDecr        ::= "dec" ID
 *      StmtFunCall     ::= FunCall
 *      FunCall         ::= ID "(" ExprList ")"
 *      ExprList        ::= Expr*
 *      ExprListCont    ::= (, Expr)*
 *      StmtIf          ::= "if" "(" Expr ")" "{" StmtList "}" ElseIfList Else
 *      ElseIfList      ::= ("elif" "(" Expr ")" "{" StmtList "}")*
 *      Else            ::= ("else" "{" StmtList "}")*
 *      StmtWhile       ::= "while" "(" Expr ")" "{" StmtList "}" // TODO: Ask if semicolon or StmtEmpty is needed at the end
 *      StmtDoWhile     ::= "do" "{" StmtList "}" "while" "(" Expr ")" ";"
 *      StmtBreak       ::= "break" ";"
 *      StmtReturn      ::= "return" Expr ";"
 *      StmtEmpty       ::= ";"
 *      Expr            ::= ExprOr
 *      ExprOr          ::= ExprAnd ("or" ExprAnd)*
 *      ExprAnd         ::= ExprComp
 *      ExprComp        ::= ExprRel
 *      OpComp          ::= "==" | "<>"
 *      ExprRel         ::= ExprAdd (OpRel ExprAdd)*
 *      OpRel           ::= "<" | "<=" | ">" | ">="
 *      ExprAdd         ::= ExprMul (OpAdd ExprMul)*
 *      OpAdd           ::= "+" | "-"
 *      ExprMul         ::= ExprUnary (OpMul ExprUnary)*
 *      OpMul           ::= "*" | "/" | "%"
 *      ExprUnary       ::= OpUnary* ExprPrimary
 *      OpUnary         ::= "+" | "-" | "not"
 *      ExprPrimary     ::= ID | Expr | Array | Lit // TODO: Ask if Lit can be contained here
 *      Array           ::= "[" ExprList "]"
 *      Lit             ::= BoolLit | IntLit | CharLit | StrLit
 */
using System;
using System.Collections.Generic;

namespace Drac{
    class Parser{
        // LL1 ORs 
        static readonly ISet<TokenCategory> firstOfDef =
            new HashSet <TokenCategory>(){
                // token categories for Def
                TokenCategory.VAR,
                TokenCategory.IDENTIFIER
            };
        static readonly ISet<TokenCategory> fisrtOfStmt =
            new HashSet<TokenCategory>(){
                TokenCategory.IDENTIFIER,
                TokenCategory.INC,
                TokenCategory.DEC,
                TokenCategory.IF,
                TokenCategory.WHILE,
                TokenCategory.DO,
                TokenCategory.BREAK,
                TokenCategory.RETURN,
                TokenCategory.SEMICOLON
            };
        static readonly ISet<TokenCategory> fisrtOfOpComp =
            new HashSet<TokenCategory>(){
                TokenCategory.EQUALS,
                TokenCategory.DIFF
            };
        static readonly ISet<TokenCategory> fisrtOfOpRel =
            new HashSet<TokenCategory>(){
                TokenCategory.LESS,
                TokenCategory.GREATER,
                TokenCategory.MORE_EQUAL,
                TokenCategory.LESS_EQUAL
            };
        static readonly ISet<TokenCategory> fisrtOfOpAdd =
            new HashSet<TokenCategory>(){
                TokenCategory.PLUS,
                TokenCategory.NEG
            };
        static readonly ISet<TokenCategory> fisrtOfOpMul =
            new HashSet<TokenCategory>(){
                TokenCategory.MUL,
                TokenCategory.MOD,
                TokenCategory.DIV
            };
        static readonly ISet<TokenCategory> fisrtOfExprPrimary =
            new HashSet<TokenCategory>(){
                TokenCategory.IDENTIFIER,
                // TokenCategory.NEG,
                TokenCategory.TokenCategory.SQR_BRACKET_LEFT,
                TokenCategory.TRUE,
                TokenCategory.FALSE,
                TokenCategory.INT_LITERAL,
                TokenCategory.CHAR_LIT,
                TokenCategory.STRING_LIT
            };
        static readonly ISet<TokenCategory> fisrtOfLit =
            new HashSet<TokenCategory>(){
                TokenCategory.TRUE,
                TokenCategory.FALSE,
                TokenCategory.INT_LITERAL,
                TokenCategory.CHAR_LIT,
                TokenCategory.STRING_LIT
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