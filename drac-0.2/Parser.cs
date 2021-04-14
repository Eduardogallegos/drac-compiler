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
 *     (0) Program          ::= DefList
 *     (1) DefList          ::= Def*
 *     (2) Def              ::= VarDef | FunDef
 *     (3) VarDef           ::= "var" IDList ";"
 *     (5) IDList           ::= ID ("," ID)*
 *     (7) FunDef           ::= ID "(" IDList? ")" "{" VarDefList StmtList "}"
 *     (9) VarDefList       ::= VarDef*
 *     (10) StmtList        ::= Stmt*
 *     (11) Stmt            ::= ID ("=" Expr | "(" ExprList ")") | StmtIncr | StmtDecr | StmtIf |
                        StmtWhile | StmtDoWhile | StmtBreak | StmtReturn | StmtEmpty
 *     (13) StmtIncr        ::= "inc" ID
 *     (14) StmtDecr        ::= "dec" ID
 *     (17) ExprList        ::= (Expr ("," Expr )*)?
 *     (19) StmtIf          ::= "if" "(" Expr ")" "{" StmtList "}" ElseIfList Else
 *     (20) ElseIfList      ::= ("elif" "(" Expr ")" "{" StmtList "}")*
 *     (21) Else            ::= ("else" "{" StmtList "}")?
 *     (22) StmtWhile       ::= "while" "(" Expr ")" "{" StmtList "}"
 *     (23) StmtDoWhile     ::= "do" "{" StmtList "}" "while" "(" Expr ")" ";"
 *     (24) StmtBreak       ::= "break" ";"
 *     (25) StmtReturn      ::= "return" Expr ";"
 *     (26) StmtEmpty       ::= ";"
 *     (27) Expr            ::= ExprOr
 *     (28) ExprOr          ::= ExprAnd ("or" ExprAnd)*
 *     (29) ExprAnd         ::= ExprComp ("and" ExprComp)*
 *     (30) ExprComp        ::= ExprRel (OpComp ExprRel)* // TODO: use OpComp
 *     (31) OpComp          ::= "==" | "<>"
 *     (32) ExprRel         ::= ExprAdd (OpRel ExprAdd)*
 *     (33) OpRel           ::= "<" | "<=" | ">" | ">="
 *     (34) ExprAdd         ::= ExprMul (OpAdd ExprMul)*
 *     (35) OpAdd           ::= "+" | "-"
 *     (36) ExprMul         ::= ExprUnary (OpMul ExprUnary)*
 *     (37) OpMul           ::= "*" | "/" | "%"
 *     (38) ExprUnary       ::= OpUnary* ExprPrimary
 *     (39) OpUnary         ::= "+" | "-" | "not"
 *     (40) ExprPrimary     ::= ID ("(" ExprList ")"|Empty) | "[" ExprList "]" | BoolLit | IntLit | CharLit | StrLit
 // TODO: break en switch case
 */
using System;
using System.Collections.Generic;

namespace Drac
{
    class Parser
    {
        static readonly ISet<TokenCategory> firstOfDef =
            new HashSet<TokenCategory>(){
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

        public Parser(IEnumerator<Token> tokenStream)
        {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken
        {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category)
        {
            if (CurrentToken == category)
            {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            }
            else
            {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        //0
        public void Program()
        {
            DefList();
        }

        //1
        public void DefList()
        {
            while (Current == Def())
            {
                Def();
            }
        }

        //2
        public void Def()
        {
            switch (Current)
            {
                case VarDef():
                    VarDef();
                    break;
                case FunDef():
                    FunDef();
                    break;
                default:
                    throw new SyntaxError(firstOfDef, tokenStream.Current);
                    break;
            }
        }

        //3
        public void VarDef()
        {
            Expect(TokenCategory.VAR);
            VarList();
            Expect(TokenCategory.SEMICOLON);
        }

        //4
        public void VarList()
        {
            IDList();
        }

        //5


        //6
        public void IDListCont()
        {
            while (Current == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                Expect(TokenCategory.IDENTIFIER);
            }
        }

        //7
        public void FunDef()
        {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            ParamList();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            VarDefList();
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
        }

        //8
        public void ParamList()
        {
            while (Current == IDList())
            {
                IDList();
            }
        }

        //9
        public void VarDefList()
        {
            while (Current == VarDef())
            {
                VarDef();
            }
        }

        //10
        public void StmtList()
        {
            while (Current == Stmt())
            {
                Stmt();
            }
        }


        //11


        //12
        public void StmtAssign()
        {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.EQUALS);
        }

        //13
        public void StmtIncr()
        {
            Expect(TokenCategory.INC);
            Expect(TokenCategory.IDENTIFIER);
        }


        //14
        public void StmtDecr()
        {
            Expect(TokenCategory.DEC);
            Expect(TokenCategory.IDENTIFIER);
        }

        //15
        public void StmtFunCall()
        {
            return FunCall();
        }

        //16
        public void FunCall(){
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategoty.PARENTHESIS_OPEN);
            ExprList();
            Expect(TokenCategoty.PARENTHESIS_CLOSE);
        }

        //17
        public void ExprList()
        {
            while (Current == Expr())
            {
                Expr();

            }
        }

        //18
        public void ExprListCont()
        {
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                Expr();
            }
        }


        //19
        public void StmtIf()
        {
            Expect(TokenCategory.IF);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            ElseIfList();
            Else();
        }


        //20
        public void ElseIfList()
        {
            while(CurrentToken == TokenCategory.ELIF){
                Expect(TokenCategory.ELIF);
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expr();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                Expect(TokenCategory.BRACKET_LEFT);
                StmtList();
                Expect(TokenCategory.BRACKET_RIGHT);
            }
        }

        //21
        public void Else()
        {
            if(CurrentToken == TokenCategory.ELSE){
                Expect(TokenCategory.ELSE);
                Expect(TokenCategory.BRACKET_LEFT);
                StmtList();
                Expect(TokenCategory.BRACKET_RIGHT);
            }
        }

        //22
        public void StmtWhile()
        {
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
        }

        //23
        public void StmtDoWhile()
        {
            Expect(TokenCategory.DO);
            Expect(TokenCategory.BRACKET_LEFT);
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.SEMICOLON);
        }
        //24
        public void StmBreak()
        {
            Expect(TokenCategory.BREAK);
            Expect(TokenCategory.SEMICOLON);
        }
        //25
        public void StmtReturn()
        {
            Expect(TokenCategory.RETURN);
            Expr();
            Expect(TokenCategory.SEMICOLON);
        }
        //26
        public void StmtEmpty()
        {
            Expect(TokenCategory.SEMICOLON);
        }

        //27
        public void Expr()
        {
            return ExprOr();
        }


        //28
        public void ExprOr()
        {
            ExprAnd();
            while (CurrentToken == TokenCategory.OR)
            {
                Expect(TokenCategory.OR);
                ExprAnd();
            }
        }
        //29
        public void ExprAnd()
        {
            ExprComp();
            while (CurrentToken == TokenCategory.AND)
            {
                Expect(TokenCategory.AND);
                ExprComp();
            }
        }

        //30
        public void ExprComp()
        {
            ExprRel();
            while (fisrtOfOpComp.Contains(CurrentToken))
            {
                OpComp();
                ExprRel();
            }
        }

        //31
        public void OpComp()
        {
            switch (Current)
            {
                case TokenCategory.EQUALS:
                    Expect(TokenCategory.EQUALS);
                    break;
                case TokenCategory.DIFF:
                    Expect(TokenCategory.DIFF);
                    break;
                default:
                    throw new SyntaxError(fisrtOfOpComp, tokenStream.Current);
                    break;
            }
        }

        //32
        public void ExprAdd()
        {
            ExprAdd();
            while (Current == OpRel())
            {
                OpRel();
                ExprAnd();
            }
        }

        //33
        public void OpRel()
        {
            switch (Current)
            {
                case TokenCategory.LESS:
                    Expect(TokenCategory.LESS);
                    break;
                case TokenCategory.LESS_EQUAL:
                    Expect(TokenCategory.LESS_EQUAL);
                    break;
                case TokenCategory.GREATER:
                    Expect(TokenCategory.GREATER);
                    break;
                case TokenCategory.MORE_EQUAL:
                    Expect(TokenCategory.MORE_EQUAL);
                    break;
                default:
                    throw new SyntaxError(fisrtOfOpRel, tokenStream.Current);
                    break;
            }
        }

        //34
        public void ExprAdd()
        {
            ExprAdd();
            while (Current == OpAdd())
            {
                OpAdd();
                ExprMul();
            }
        }

        //35
        public void OpAdd()
        {
            switch (Current)
            {
                case TokenCategory.NEG:
                    Expect(TokenCategory.NEG);
                    break;
                case TokenCategory.PLUS:
                    Expect(TokenCategory.PLUS);
                    break;
                default:
                    throw new SyntaxError(fisrtOfOpAdd, tokenStream.Current);
                    break;
            }
        }

        //36
        public void ExprMul()
        {
            ExprUnuary();
            while (Current == OpMul())
            {
                OpMul();
                ExprUnuary();
            }
        }
        //37
        public void OpMul()
        {
            switch (Current)
            {
                case TokenCategory.MUL:
                    Expect(TokenCategory.MUL);
                    break;
                case TokenCategory.DIV:
                    Expect(TokenCategory.DIV);
                    break;
                case TokenCategory.MOD:
                    Expect(TokenCategory.MOD);
                    break;
                default:
                    throw new SyntaxError(fisrtOfOpMul, tokenStream.Current);
                    break;
            }
        }

        //38

        //39


        //40


        //41

        //42


    }
}