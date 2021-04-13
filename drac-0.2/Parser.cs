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
 *     (0) Program         ::= DefList
 *     (1) DefList         ::= Def*
 *     (2) Def             ::= VarDef | FunDef
 *     (3) VarDef          ::= "var" VarList ";"
 *     (4) VarList         ::= IDList
 *     (5) IDList          ::= ID IDListCont
 *     (6) IDListCont      ::= ("," ID)*
 *     (7) FunDef          ::= ID "(" ParamList ")" "{" VarDefList StmtList "}"
 *     (8) ParamList       ::= IDList*
 *     (9) VarDefList      ::= VarDef*
 *     (10) StmtList        ::= Stmt*
 *     (11) Stmt            ::= StmtAssign | StmtIncr | StmtDecr | StmtFunCall | StmtIf |
                        StmtWhile | StmtDoWhile | StmtBreak | StmtReturn | StmtEmpty
 *     (12) StmtAssign      ::= ID "=" Expr
 *     (13) StmtIncr        ::= "inc" ID
 *     (14) StmtDecr        ::= "dec" ID
 *     (15) StmtFunCall     ::= FunCall
 *     (16) FunCall         ::= ID "(" ExprList ")"
 *     (17) ExprList        ::= Expr*
 *     (18) ExprListCont    ::= (, Expr)*
 *     (19) StmtIf          ::= "if" "(" Expr ")" "{" StmtList "}" ElseIfList Else
 *     (20) ElseIfList      ::= ("elif" "(" Expr ")" "{" StmtList "}")*
 *     (21) Else            ::= ("else" "{" StmtList "}")*
 *     (22) StmtWhile       ::= "while" "(" Expr ")" "{" StmtList "}" // TODO: Ask if semicolon or StmtEmpty is needed at the end
 *     (23) StmtDoWhile     ::= "do" "{" StmtList "}" "while" "(" Expr ")" ";"
 *     (24) StmtBreak       ::= "break" ";"
 *     (25) StmtReturn      ::= "return" Expr ";"
 *     (26) StmtEmpty       ::= ";"
 *     (27) Expr            ::= ExprOr
 *     (28) ExprOr          ::= ExprAnd ("or" ExprAnd)*
 *     (29) ExprAnd         ::= ExprComp
 *     (30) ExprComp        ::= ExprRel
 *     (31) OpComp          ::= "==" | "<>"
 *     (32) ExprRel         ::= ExprAdd (OpRel ExprAdd)*
 *     (33) OpRel           ::= "<" | "<=" | ">" | ">="
 *     (34) ExprAdd         ::= ExprMul (OpAdd ExprMul)*
 *     (35) OpAdd           ::= "+" | "-"
 *     (36) ExprMul         ::= ExprUnary (OpMul ExprUnary)*
 *     (37) OpMul           ::= "*" | "/" | "%"
 *     (38) ExprUnary       ::= OpUnary* ExprPrimary
 *     (39) OpUnary         ::= "+" | "-" | "not"
 *     (40) ExprPrimary     ::= ID | Expr | Array | Lit // TODO: Ask if Lit can be contained here
 *     (41) Array           ::= "[" ExprList "]"
 *     (42) Lit             ::= BoolLit | IntLit | CharLit | StrLit
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

        //0
        public int Program(){
            var result = DefList();
            return result; 

        }

        //1
        public int DefList(){
            var result;
            while(Current == Def()){
                result=+Def();
            }
            return result;
        }

        //2
        public int Def(){
            switch (Current){
                case VarDef():
                    var result = VarDef();
                    return result;
                case FunDef():
                var result = FunDef();
                    return result;
                default:
                    throw new SyntaxError();
            }
        }

        //3
        public int VarDef(){
            Expect(TokenCategory.VAR);
            var result = VarList();
            Expect(TokenCategory.SEMICOLON);
            return result;
        }

        //4
        public int VarList(){
            var result= IDList();
            return result;
        }
        

        //5


        //6
        public int IDListCont(){
            var result;
            while(Current == TokenCategory.COMA){
                
                Expect(TokenCategory.COMA);
                result+= ID();
                

            }
            return result;
        }

        //7


        //8
        public int ParamList(){
            var result;
            while(Current == IDList()){
                    result +=IDList();
            }
            return result;
        }

        //9
        public int VarDefList(){
            var result;
            while(Current == VarDef()){
                    result +=VarDef();
            }
            return result;
        }


        //10
         public int StmtList(){
            var result;
            while(Current == Stmt()){
                    result +=Stmt();
            }
            return result;
        }


        //11


        //12
        

        //13
        public int StmtIncr(){
            Expect(TokenCategory.INC);
                var result = ID();
                return result;
        }


        //14
        public int StmtDecr(){
            Expect(TokenCategory.DEC);
                var result = ID();
                return result;
        }

        //15
        public int StmtFunCall(){
            var result = FunCall();
            return result;
        }


        //16


        //17
        public int Expr(){
            var result;
            while(Current == Expr()){
                    result+=Expr();

            }
            return result;
        }

        //18 //check
        public int ExprListCont(){
            var result;
            while(CurrentToken == TokenCategory.COMA ){
               Expect(TokenCategory.COMA);
                result+=Expr();
            }
        }


        //19 //check
        public int StmtIf(){
            if(Expr()){
                StmtIf()
            }ElseIfList() Else()
        }


        //20 //check
        public int ElseIfList(){
            ElseIfList(){
                Expr()
            }StmtList()

        }

        //21 //check
        public int Else(){
            Else(){
                StmtList()
            }
        }

        //22 //check
        public int StmtWhile(){
            while(Expr()){
                StmtList()
            }
        }

        //23

        //24
        public int StmBreak(){
            Expect(TokenCategory.BREAK);
            Expect(TokenCategory.SEMICOLON);
        }
        //25
        public int StmtReturn(){
            Expect(TokenCategory.RETURN);
            var result= Expr();
            Expect(TokenCategory.SEMICOLON);
            return result;
            }
        //26
         public int StmtEmpty(){
            Expect(TokenCategory.SEMICOLON);
            }

        //27
         public int StmtReturn(){
            
            var result= ExprOr();
            return result;
            }


        //28
        public int ExprOr(){
            var result = ExprAnd();
            while(CurrentToken ==TokenCategory.OR){
               Expect(TokenCategory.OR);
               result+=ExprAnd(); 
            }
            return result;

        }
        //29
        public int ExprAnd(){
            var result = ExprComp();
            return result;
        }

        //30
        public int ExprComp(){
            var result= ExprRel();
            return result;
        }

        //31
        public int OpComp(){
            switch (Current) {

        case TokenCategory.EQUALS:
            Expect(TokenCategory.EQUALS);
            

        case TokenCategory.DIFF:
            Expect(TokenCategory.DIFF);
            

        default:
            throw new SyntaxError();
        }
        }

        //32
         public int ExprAdd(){
            var result = ExprAdd();
            while(Current ==OpRel()){
               result+=OpRel();
               result+=ExprAnd(); 
            }
            return result;

        }

        //33
        public int OpComp(){
            switch (Current) {

        case TokenCategory.LESS:
            Expect(TokenCategory.LESS);
            

        case TokenCategory.LESS_EQUAL:
            Expect(TokenCategory.LESS_EQUAL);

        case TokenCategory.GREATER:
            Expect(TokenCategory.GREATER);

        case TokenCategory.MORE_EQUAL:
            Expect(TokenCategory.MORE_EQUAL);
            

        default:
            throw new SyntaxError();
        }
        }

        //34
        public int ExprAdd(){
            var result= ExprAdd();
            while(Current == OpAdd()){
                result+=OpAdd();
                result+=ExprMul();
            }
            return result;
        }

        //35
        public int OpAdd(){
           switch (Current) {

        case TokenCategory.NEG:
            Expect(TokenCategory.NEG);
            

        case TokenCategory.PLUS:
            Expect(TokenCategory.PLUS);

       

        default:
            throw new SyntaxError();
        } 
        }

        //36
        public int ExprMul(){
            var result= ExprUnuary();
            while(Current == OpMul()){
                result+=OpMul();
                result+=ExprUnuary();
            }
            return result;
        }
        //37
        public int OpMul(){
           switch (Current) {

        case TokenCategory.MUL:
            Expect(TokenCategory.MUL);
            

        case TokenCategory.DIV:
            Expect(TokenCategory.DIV);

        case TokenCategory.MOD:
            Expect(TokenCategory.MOD);


       

        default:
            throw new SyntaxError();
        } 
        }

        //38

        //39


        //40


        //42

        //42


    }
}