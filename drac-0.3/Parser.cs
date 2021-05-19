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
 *     (28) ExprMul         ::= ExprPrimary (OpMul ExprPrimary)*
 *     (29) OpMul           ::= "*" | "/" | "%"
 *     (30) ExprPrimary     ::= ID FunCallCont? | "[" ExprList "]" | BoolLit | IntLit | CharLit | StrLit | "(" ExprOr ")" | "-" ExprPrimary | "+" ExprPrimary | "not" ExprPrimary
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
                TokenCategory.STRING_LIT,
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
            var result = new VarDef();
            if(CurrentToken == TokenCategory.VAR){
                var varToken = Expect(TokenCategory.VAR);
                var idlist = IDList();
                Expect(TokenCategory.SEMICOLON);
                result.Add(idlist);
                result.AnchorToken = varToken;
                
            }
            return result;
        }

        //3
        public Node IDList()
        {
            var idList = new IdList();
            if (CurrentToken == TokenCategory.IDENTIFIER)
            {
                var expr1 = new Identifier()
                {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)
                };
                idList.Add(expr1);
                while (CurrentToken == TokenCategory.COMA)
                {
                    Expect(TokenCategory.COMA);
                    var expr2 = new Identifier()
                    {
                        AnchorToken = Expect(TokenCategory.IDENTIFIER)
                    };
                    idList.Add(expr2);
                }
            }
            return idList;
        }

        //4 
        public Node FunDef()
        {
            var idToken = Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            var idList = IDList(); // check
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.BRACKET_LEFT);
            var varDef = VarDef();
            var stmtList = StmtList();
            Expect(TokenCategory.BRACKET_RIGHT);
            var result = new Funcion() { idList, varDef, stmtList };
            result.AnchorToken = idToken;
            return result;
        }

        //5
        public Node StmtList()
        {
            var stmtList = new StmtList();
            while (fisrtOfStmt.Contains(CurrentToken))
            {
                stmtList.Add(Stmt());
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
            var result = new FunctionCall();
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
        public Node StmtDecr()
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
            var result = new ExprList() { expr1 };
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                var expr2 = ExprOr();
                result.Add(expr2);
            }
            return result;
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
        //19
        public Node StmtEmpty()
        {
            return new StmtEmpty()
            {
                AnchorToken = Expect(TokenCategory.SEMICOLON)
            };
        }

        //20 CHECK
        public Node ExprOr()
        {
            var result = ExprAnd();
            while (CurrentToken == TokenCategory.OR)
            {
                var orStmt = new Or()
                {
                    AnchorToken = Expect(TokenCategory.OR)
                };
                var newResult = ExprAnd();
                orStmt.Add(result);
                orStmt.Add(newResult);
                result = orStmt;
            }
            return result;
        }
        //21
        public Node ExprAnd()
        {
            var result = ExprComp();
            while (CurrentToken == TokenCategory.AND)
            {
                var andStmt = new And()
                {
                    AnchorToken = Expect(TokenCategory.AND)
                };
                var newResult = ExprComp();
                andStmt.Add(result);
                andStmt.Add(newResult);
                result = andStmt;
            }
            return result;
        }

        //22
        public Node ExprComp()
        {
            var result = ExprRel();
            while (fisrtOfOpComp.Contains(CurrentToken))
            {
                var newResult = OpComp();
                newResult.Add(result);
                newResult.Add(ExprRel());
                result = newResult;
            }
            return result;
        }

        //23
        public Node OpComp()
        {
            switch (CurrentToken)
            {
                case TokenCategory.EQUALS:
                    return new Equals()
                    {
                        AnchorToken = Expect(TokenCategory.EQUALS)
                    };
                case TokenCategory.DIFF:
                    return new Diff()
                    {
                        AnchorToken = Expect(TokenCategory.DIFF)
                    };
                default:
                    throw new SyntaxError(fisrtOfOpComp, tokenStream.Current);
            }
        }

        //24
        public Node ExprRel()
        {
            var result = ExprAdd();
            while (fisrtOfOpRel.Contains(CurrentToken))
            {
                var newResult = OpRel();
                newResult.Add(result);
                newResult.Add(ExprAdd());
                result = newResult;
            }
            return result;
        }

        //25
        public Node OpRel()
        {
            switch (CurrentToken)
            {
                case TokenCategory.LESS:
                    return new Less()
                    {
                        AnchorToken = Expect(TokenCategory.LESS)
                    };
                case TokenCategory.LESS_EQUAL:
                    return new LessEqual()
                    {
                        AnchorToken = Expect(TokenCategory.LESS_EQUAL)
                    };
                case TokenCategory.GREATER:
                    return new Greater()
                    {
                        AnchorToken = Expect(TokenCategory.GREATER)
                    };
                case TokenCategory.MORE_EQUAL:
                    return new MoreEqual()
                    {
                        AnchorToken = Expect(TokenCategory.MORE_EQUAL)
                    };
                default:
                    throw new SyntaxError(fisrtOfOpRel, tokenStream.Current);
            }
        }

        //26
        public Node ExprAdd()
        {
            var result = ExprMul();
            while (fisrtOfOpAdd.Contains(CurrentToken))
            {
                var newResult = OpAdd();
                newResult.Add(result);
                newResult.Add(ExprMul());
                result = newResult;
            }
            return result;
        }

        //27
        public Node OpAdd()
        {
            switch (CurrentToken)
            {
                case TokenCategory.NEG:
                    return new Neg()
                    {
                        AnchorToken = Expect(TokenCategory.NEG)
                    };
                case TokenCategory.PLUS:
                    return new Plus()
                    {
                        AnchorToken = Expect(TokenCategory.PLUS)
                    };
                default:
                    throw new SyntaxError(fisrtOfOpAdd, tokenStream.Current);
            }
        }

        //28
        public Node ExprMul()
        {
            var result = ExprPrimary();
            while (fisrtOfOpMul.Contains(CurrentToken))
            {
                var newResult = OpMul();
                newResult.Add(result);
                newResult.Add(ExprPrimary());
                result = newResult;
            }
            return result;
        }
        //29
        public Node OpMul()
        {
            switch (CurrentToken)
            {
                case TokenCategory.MUL:
                    return new Mul()
                    {
                        AnchorToken = Expect(TokenCategory.MUL)
                    };
                case TokenCategory.DIV:
                    return new Div()
                    {
                        AnchorToken = Expect(TokenCategory.DIV)
                    };
                case TokenCategory.MOD:
                    return new Mod()
                    {
                        AnchorToken = Expect(TokenCategory.MOD)
                    };
                default:
                    throw new SyntaxError(fisrtOfOpMul, tokenStream.Current);
            }
        }

        //30
        public Node ExprPrimary()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    var id = Expect(TokenCategory.IDENTIFIER);
                    if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
                    {
                        var functionCall = FunCallCont();
                        functionCall.AnchorToken = id;
                        return functionCall;
                    }
                    else
                    {
                        return new Identifier()
                        {
                            AnchorToken = id
                        };
                    }

                case TokenCategory.SQR_BRACKET_LEFT:
                    var result2 = new Array();
                    Expect(TokenCategory.SQR_BRACKET_LEFT);
                    if (CurrentToken != TokenCategory.SQR_BRACKET_RIGHT)
                    {
                        result2.Add(ExprList());
                    }
                    Expect(TokenCategory.SQR_BRACKET_RIGHT);
                    return result2;
                case TokenCategory.TRUE:
                    return new True() { AnchorToken = Expect(TokenCategory.TRUE) };

                case TokenCategory.FALSE:
                    return new False() { AnchorToken = Expect(TokenCategory.FALSE) };

                case TokenCategory.INT_LITERAL:
                    return new Int_literal() { AnchorToken = Expect(TokenCategory.INT_LITERAL) };

                case TokenCategory.CHAR_LIT:
                    return new Char_lit() { AnchorToken = Expect(TokenCategory.CHAR_LIT) };
                case TokenCategory.STRING_LIT:
                    return new String_lit() { AnchorToken = Expect(TokenCategory.STRING_LIT) };
                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    var result3 = ExprOr();
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    return result3;
                case TokenCategory.PLUS:
                    var plusToken = Expect(TokenCategory.PLUS);
                    var expr1 = ExprPrimary();
                    var result4 = new Positive() { expr1 };
                    result4.AnchorToken = plusToken;
                    return result4;
                case TokenCategory.NEG:
                    var negToken = Expect(TokenCategory.NEG);
                    var expr2 = ExprPrimary();
                    var result5 = new Negative() { expr2 };
                    result5.AnchorToken = negToken;
                    return result5;
                case TokenCategory.NOT:
                    var notToken = Expect(TokenCategory.NOT);
                    var expr3 = ExprPrimary();
                    var result6 = new Not() { expr3 };
                    result6.AnchorToken = notToken;
                    return result6;
                default:
                    throw new SyntaxError(fisrtOfExprPrimary, tokenStream.Current);
            }
        }
    }
}