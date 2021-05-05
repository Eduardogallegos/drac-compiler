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
 *     (7)  IdReduced       ::= ID ("=" ExprOr | FunCallCont ) ";"
 *     (8)  FunCallCont     ::= "(" ExprList ")"
 *     (9)  StmtIncr         ::= "inc" ID ";"
 *     (10) StmtDecr        ::= "dec" ID ";"
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
 *     (32) ExprPrimary     ::= ID FunCallCont? | "[" ExprList "]" | BoolLit | IntLit | CharLit | StrLit | "(" ExprOr ")"
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
        public Node Program()
        {
            var program = new Program();

            while (firstOfDef.Contains(CurrentToken))
            {
                program.Add(Def());
            }
            return program;
        }

        //1
        public Node Def()
        {
            switch (CurrentToken)
            {
                case TokenCategory.VAR:
                    return VarDef();
                case TokenCategory.IDENTIFIER:
                    return FunDef();
                default:
                    throw new SyntaxError(firstOfDef, tokenStream.Current);
            }
        }

        //2
        public Node VarDef()
        {
            var varToken = Expect(TokenCategory.VAR);
            var idlist = IDList();
            Expect(TokenCategory.SEMICOLON);
            var result = new VarDef() { idlist };
            result.AnchorToken = varToken;
            return result;
        }

        //3
        public Node IDList()
        {
            var expr1 = new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            };
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                var expr2 = new Identifier()
                {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)
                };
                expr2.Add(expr1);
                expr1 = expr2;
            }
            return expr1;
        }

        //4 TOCHECK
        public Node FunDef()
        {

            var idToken = Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if (CurrentToken == TokenCategory.IDENTIFIER) var idList = IDList();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            var varDefLst = new VarDefList();
            while (CurrentToken == TokenCategory.VAR)
            {
                varDefLst.Add(VarDef());
            }
            var stmtList = StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            var result = new Funcion() { varDefLst, stmtList };
            result.AnchorToken = idToken;
            return result;
        }

        //5
        public Node StmtList()
        {
            var stmtList = new StmtList();
            while (fisrtOfStmt.Contains(CurrentToken))
            {
                stmtList.add(Stmt());
            }
            return stmtList;
        }

        //6
        public Node Stmt()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    return IdReduced();
                case TokenCategory.INC:
                    return StmtIncr();
                case TokenCategory.DEC:
                    return StmtDecr();
                case TokenCategory.IF:
                    return StmtIf();
                case TokenCategory.WHILE:
                    return StmtWhile();
                case TokenCategory.DO:
                    return StmtDoWhile();
                case TokenCategory.BREAK:
                    return StmtBreak();
                case TokenCategory.RETURN:
                    return StmtReturn();
                case TokenCategory.SEMICOLON:
                    return StmtEmpty();
                default:
                    throw new SyntaxError(fisrtOfStmt, tokenStream.Current);
            }
        }

        //7
        public Node IdReduced()
        {
            var idToken = Expect(TokenCategory.IDENTIFIER);
            if (CurrentToken == TokenCategory.ASSIGN)
            {
                Expect(TokenCategory.ASSIGN);
                var exprOr = ExprOr();
                Expect(TokenCategory.SEMICOLON);
                var result = new Assignment() { exprOr };
                result.AnchorToken = idToken;
                return result;
            }
            else if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
            {
                var funCallCont = FunCallCont();
                Expect(TokenCategory.SEMICOLON);
                funCallCont.AnchorToken = idToken;
                return funCallCont;
            }
            else
            {
                throw new SyntaxError(fisrtOfIdentifier, tokenStream.Current);
            }
        }

        //8
        public Node FunCallCont()
        {
            Expect(TokenCategory.PARENTHESIS_OPEN);
            var result = new FunCallCont();
            if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
            {
                result.Add(ExprList());
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            return result;
        }

        //9
        public Node StmtIncr()
        {
            var result = new Increase()
            {
                AnchorToken = Expect(TokenCategory.INC)
            };
            result.Add(new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });
            Expect(TokenCategory.SEMICOLON);
            return result;
        }

        //10
        public void StmtDecr()
        {
            var result = new Decrease()
            {
                AnchorToken = Expect(TokenCategory.DEC)
            };
            result.Add(new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });
            Expect(TokenCategory.SEMICOLON);
            return result;
        }

        //11
        public Node ExprList()
        {
            var expr1 = ExprOr();
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                var expr2 = ExprOr();
                expr2.Add(expr1);
                expr1 = expr2;
            }
            return expr1;
        }

        //12
        public Node StmtIf()
        {
            var iftoken = Expect(TokenCategory.IF);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            var expr1 = ExprOr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            var expr2 = StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            var expr3 = ElseIfList();
            var expr4 = Else();

            var result = new StmtIf() { expr1, expr2, expr3, expr4 };
            result.AnchorToken = iftoken;
            return result;


        }

        //13
        public Node ElseIfList()
        {
            var result = new ElseIfList();
            while (CurrentToken == TokenCategory.ELIF)
            {
                var elifToken = Expect(TokenCategory.ELIF);
                Expect(TokenCategory.PARENTHESIS_OPEN);
                var expr3 = ExprOr();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                Expect(TokenCategory.BRACKET_LEFT);
                var expr4 = StmtList();
                Expect(TokenCategory.BRACKET_RIGHT);

                var elif = new ElseIf() { expr3, expr4 };

                elif.AnchorToken = elifToken;

                result.Add(elif);
            }

            return result;
        }

        //14
        public Node Else()
        {
            var result = new Else();
            if (CurrentToken == TokenCategory.ELSE)
            {
                var elseToken = Expect(TokenCategory.ELSE);
                Expect(TokenCategory.BRACKET_LEFT);
                var expr = StmtList();
                Expect(TokenCategory.BRACKET_RIGHT);
                result.Add(expr);
                result.AnchorToken = elseToken;
            }
            return result;

        }

        //15
        public Node StmtWhile()
        {

            var whileToken = Expect(TokenCategory.WHILE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            var expr1 = ExprOr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            var expr2 = StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            var result = new StmtWhile() { expr1, expr2 };
            result.AnchorToken = whileToken;
            return result;
        }

        //16
        public Node StmtDoWhile()
        {
            var doToken = Expect(TokenCategory.DO);
            Expect(TokenCategory.BRACKET_LEFT);
            var expr1 = StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            Expect(TokenCategory.WHILE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            var expr2 = ExprOr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.SEMICOLON);
            var result = new StmtDoWhile() { expr1, expr2 };
            result.AnchorToken = doToken;
            return result;
        }
        //17
        public Node StmtBreak()
        {
            var result = new StmtBreak() { AnchorToken = Expect(TokenCategory.BREAK) };
            Expect(TokenCategory.SEMICOLON);
            return result;
        }
        //18
        public Node StmtReturn()
        {
            var returnToken = Expect(TokenCategory.RETURN);
            var expr1 = ExprOr();
            Expect(TokenCategory.SEMICOLON);
            var result = new StmtReturn() { expr1 };
            result.AnchorToken = returnToken;
            return result;

        }
        //19 TODO: Check if this needs a token
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
                    if (CurrentToken != TokenCategory.SQR_BRACKET_RIGHT)
                    {
                        ExprList();
                    }
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
                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    ExprOr();
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    break;
                default:
                    throw new SyntaxError(fisrtOfExprPrimary, tokenStream.Current);
            }
        }
    }
}