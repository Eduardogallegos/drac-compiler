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
 *     (0) Program          ::= Def*
 *     (1) Def              ::= VarDef | FunDef
 *     (2) VarDef           ::= "var" IDList ";"
 *     (3) IDList           ::= ID ("," ID)*
 *     (4) FunDef           ::= ID "(" IDList? ")" "{" VarDef* StmtList "}"
 *     (5) StmtList         ::= Stmt*
 *     (6) Stmt             ::= IdReduced | StmtIncr | StmtDecr | StmtIf |
                        StmtWhile | StmtDoWhile | StmtBreak | StmtReturn | StmtEmpty
 *     (7)  IdReduced       ::= ID ("=" ExprOr | FunCallCont )
 *     (8)  FunCallCont     ::= "(" ExprList ")"
 *     (9) StmtIncr         ::= "inc" ID
 *     (10) StmtDecr        ::= "dec" ID
 *     (11) ExprList        ::= (ExprOr ("," ExprOr )*)?
 *     (12) StmtIf          ::= "if" "(" ExprOr ")" "{" StmtList "}" ElseIfList Else
 *     (13) ElseIfList      ::= ("elif" "(" ExprOr ")" "{" StmtList "}")*
 *     (14) Else            ::= ("else" "{" StmtList "}")?
 *     (15) StmtWhile       ::= "while" "(" ExprOr ")" "{" StmtList "}"
 *     (16) StmtDoWhile     ::= "do" "{" StmtList "}" "while" "(" ExprOr ")" ";"
 *     (17) StmtBreak       ::= "break" ";"
 *     (18) StmtReturn      ::= "return" ExprOr ";"
 *     (19) StmtEmpty       ::= ";"
 *     (20) ExprOr          ::= ExprAnd ("or" ExprAnd)*
 *     (21) ExprAnd         ::= ExprComp ("and" ExprComp)*
 *     (22) ExprComp        ::= ExprRel (OpComp ExprRel)*
 *     (23) OpComp          ::= "==" | "<>"
 *     (24) ExprRel         ::= ExprAdd (OpRel ExprAdd)*
 *     (25) OpRel           ::= "<" | "<=" | ">" | ">="
 *     (26) ExprAdd         ::= ExprMul (OpAdd ExprMul)*
 *     (27) OpAdd           ::= "+" | "-"
 *     (28) ExprMul         ::= ExprUnary (OpMul ExprUnary)*
 *     (29) OpMul           ::= "*" | "/" | "%"
 *     (30) ExprUnary       ::= OpUnary* ExprPrimary
 *     (31) OpUnary         ::= "+" | "-" | "not"
 *     (32) ExprPrimary     ::= ID ( FunCallCont | Empty ) | "[" ExprList "]" | BoolLit | IntLit | CharLit | StrLit
 */
using System;
using System.Collections.Generic;

namespace Drac
{
    class Parser
    {
        static readonly ISet<TokenCategory> firstOfDef =
            new HashSet<TokenCategory>(){
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
        static readonly ISet<TokenCategory> fisrtOfIdentifier =
            new HashSet<TokenCategory>(){
                TokenCategory.ASSIGN,
                TokenCategory.PARENTHESIS_OPEN
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
                TokenCategory.SQR_BRACKET_LEFT,
                TokenCategory.TRUE,
                TokenCategory.FALSE,
                TokenCategory.INT_LITERAL,
                TokenCategory.CHAR_LIT,
                TokenCategory.STRING_LIT
            };
        static readonly ISet<TokenCategory> firstOfOpUnary =
            new HashSet<TokenCategory>(){
                TokenCategory.PLUS,
                TokenCategory.NEG,
                TokenCategory.NOT
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
            while (firstOfDef.Contains(CurrentToken))
            {
                Def();
            }
        }

        //1
        public void Def()
        {
            switch (CurrentToken)
            {
                case TokenCategory.VAR:
                    VarDef();
                    break;
                case TokenCategory.IDENTIFIER:
                    FunDef();
                    break;
                default:
                    throw new SyntaxError(firstOfDef, tokenStream.Current);
            }
        }

        //2
        public void VarDef()
        {
            Expect(TokenCategory.VAR);
            IDList();
            Expect(TokenCategory.SEMICOLON);
        }

        //3
        public void IDList()
        {
            Expect(TokenCategory.IDENTIFIER);
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                Expect(TokenCategory.IDENTIFIER);
            }
        }

        //4
        public void FunDef()
        {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if (CurrentToken == TokenCategory.IDENTIFIER) IDList();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            while (CurrentToken == TokenCategory.VAR)
            {
                VarDef();
            }
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
        }

        //5
        public void StmtList()
        {
            while (fisrtOfStmt.Contains(CurrentToken))
            {
                Stmt();
            }
        }

        //6
        public void Stmt()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    IdReduced();
                    break;
                case TokenCategory.INC:
                    StmtIncr();
                    break;
                case TokenCategory.DEC:
                    StmtDecr();
                    break;
                case TokenCategory.IF:
                    StmtIf();
                    break;
                case TokenCategory.WHILE:
                    StmtWhile();
                    break;
                case TokenCategory.DO:
                    StmtDoWhile();
                    break;
                case TokenCategory.BREAK:
                    StmBreak();
                    break;
                case TokenCategory.RETURN:
                    StmtReturn();
                    break;
                case TokenCategory.SEMICOLON:
                    StmtEmpty();
                    break;
                default:
                    throw new SyntaxError(fisrtOfStmt, tokenStream.Current);
            }
        }

        //7
        public void IdReduced()
        {
            Expect(TokenCategory.IDENTIFIER);
            if (CurrentToken == TokenCategory.ASSIGN)
            {
                Expect(TokenCategory.ASSIGN);
                ExprOr();
            }
            else if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
            {
                FunCallCont();
            }
            else
            {
                throw new SyntaxError(fisrtOfIdentifier, tokenStream.Current);
            }
        }

        //8
        public void FunCallCont()
        {
            Expect(TokenCategory.PARENTHESIS_OPEN);
            ExprList();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
        }

        //9
        public void StmtIncr()
        {
            Expect(TokenCategory.INC);
            Expect(TokenCategory.IDENTIFIER);
        }

        //10
        public void StmtDecr()
        {
            Expect(TokenCategory.DEC);
            Expect(TokenCategory.IDENTIFIER);
        }

        //11
        public void ExprList()
        {
            if (ExprOr())
            {
                while (CurrentToken == TokenCategory.COMA)
                {
                    Expect(TokenCategory.COMA);
                    ExprOr();
                }
            }
        }

        //12
        public void StmtIf()
        {
            Expect(TokenCategory.IF);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            ExprOr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            ElseIfList();
            Else();
        }

        //13
        public void ElseIfList()
        {
            while (CurrentToken == TokenCategory.ELIF)
            {
                Expect(TokenCategory.ELIF);
                Expect(TokenCategory.PARENTHESIS_OPEN);
                ExprOr();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                Expect(TokenCategory.BRACKET_LEFT);
                StmtList();
                Expect(TokenCategory.BRACKET_RIGHT);
            }
        }

        //14
        public void Else()
        {
            if (CurrentToken == TokenCategory.ELSE)
            {
                Expect(TokenCategory.ELSE);
                Expect(TokenCategory.BRACKET_LEFT);
                StmtList();
                Expect(TokenCategory.BRACKET_RIGHT);
            }
        }

        //15
        public void StmtWhile()
        {
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            ExprOr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
        }

        //16
        public void StmtDoWhile()
        {
            Expect(TokenCategory.DO);
            Expect(TokenCategory.BRACKET_LEFT);
            StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            ExprOr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.SEMICOLON);
        }
        //17
        public void StmBreak()
        {
            Expect(TokenCategory.BREAK);
            Expect(TokenCategory.SEMICOLON);
        }
        //18
        public void StmtReturn()
        {
            Expect(TokenCategory.RETURN);
            ExprOr();
            Expect(TokenCategory.SEMICOLON);
        }
        //19
        public void StmtEmpty()
        {
            Expect(TokenCategory.SEMICOLON);
        }

        //20
        public void ExprOr()
        {
            ExprAnd();
            while (CurrentToken == TokenCategory.OR)
            {
                Expect(TokenCategory.OR);
                ExprAnd();
            }
        }
        //21
        public void ExprAnd()
        {
            ExprComp();
            while (CurrentToken == TokenCategory.AND)
            {
                Expect(TokenCategory.AND);
                ExprComp();
            }
        }

        //22
        public void ExprComp()
        {
            ExprRel();
            while (fisrtOfOpComp.Contains(CurrentToken))
            {
                OpComp();
                ExprRel();
            }
        }

        //23
        public void OpComp()
        {
            switch (CurrentToken)
            {
                case TokenCategory.EQUALS:
                    Expect(TokenCategory.EQUALS);
                    break;
                case TokenCategory.DIFF:
                    Expect(TokenCategory.DIFF);
                    break;
                default:
                    throw new SyntaxError(fisrtOfOpComp, tokenStream.Current);
            }
        }

        //24
        public void ExprRel()
        {
            ExprAdd();
            while (fisrtOfOpRel.Contains(CurrentToken))
            {
                OpRel();
                ExprAnd();
            }
        }

        //25
        public void OpRel()
        {
            switch (CurrentToken)
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
            }
        }

        //26
        public void ExprAdd()
        {
            ExprMul();
            while (fisrtOfOpAdd.Contains(CurrentToken))
            {
                OpAdd();
                ExprMul();
            }
        }

        //27
        public void OpAdd()
        {
            switch (CurrentToken)
            {
                case TokenCategory.NEG:
                    Expect(TokenCategory.NEG);
                    break;
                case TokenCategory.PLUS:
                    Expect(TokenCategory.PLUS);
                    break;
                default:
                    throw new SyntaxError(fisrtOfOpAdd, tokenStream.Current);
            }
        }

        //28
        public void ExprMul()
        {
            ExprUnary();
            while (fisrtOfOpMul.Contains(CurrentToken))
            {
                OpMul();
                ExprUnary();
            }
        }
        //29
        public void OpMul()
        {
            switch (CurrentToken)
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
            }
        }
        //30
        public void ExprUnary()
        {
            while (firstOfOpUnary.Contains(CurrentToken))
            {
                OpUnary();
            }
            ExprPrimary();
        }

        //31
        public void OpUnary()
        {
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    Expect(TokenCategory.PLUS);
                    break;
                case TokenCategory.NEG:
                    Expect(TokenCategory.NEG);
                    break;
                case TokenCategory.NOT:
                    Expect(TokenCategory.NOT);
                    break;
                default:
                    throw new SyntaxError(firstOfOpUnary, tokenStream.Current);
            }
        }

        //32
        public void ExprPrimary()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);
                    if (CurrentToken == TokenCategory.PARENTHESIS_OPEN) FunCallCont();
                    break;
                case TokenCategory.SQR_BRACKET_LEFT:
                    Expect(TokenCategory.SQR_BRACKET_LEFT);
                    ExprList();
                    Expect(TokenCategory.SQR_BRACKET_RIGHT);
                    break;
                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;
                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;
                case TokenCategory.INT_LITERAL:
                    Expect(TokenCategory.INT_LITERAL);
                    break;
                case TokenCategory.CHAR_LIT:
                    Expect(TokenCategory.CHAR_LIT);
                    break;
                case TokenCategory.STRING_LIT:
                    Expect(TokenCategory.STRING_LIT);
                    break;
                default:
                    throw new SyntaxError(fisrtOfExprPrimary, tokenStream.Current);
            }
        }
    }
}