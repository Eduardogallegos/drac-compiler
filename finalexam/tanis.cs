/*
Final Exam
- Eduardo Gallegos A01745776
- Alejandro Chavez A01374974
- Pedro Cortes A01374919
*/

/*
    Tanis LL(1) Grammar:

    prog => expr EOF
    expr => max | min | float
    max => "[" list "]"
    min => "{" list "}"
    list => expr ("," expr)*

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tanis
{
    public enum TokenCategory
    {
        OPEN_BRACKET,
        CLOSE_BRACKET,
        OPEN_SQUARE_BRACKET,
        CLOSE_SQUARE_BRACKET,
        COMMA,
        FLOAT,
        ILLEGAL,
        EOF
    }

    public class Token
    {
        public TokenCategory Category { get; }
        public String Lexeme { get; }

        public Token(TokenCategory category, String lexeme)
        {
            Category = category;
            Lexeme = lexeme;
        }

        public override String ToString()
        {
            return $"[{Category}, \"{Lexeme}\"]";
        }
    }

    public class Scanner
    {

        readonly string input;
        static readonly Regex regex = new Regex(
            @"
                    (?<SqrBracketLeft>  [\[]    )
                |   (?<SqrBracketRight> [\]]    )
                |   (?<BracketLeft>     [{]     )
                |   (?<BracketRight>    [}]     )
                |   (?<Float>           -?\d+([.]\d+)? )
                |   (?<Comma>           [,]     )
                |   (?<Spaces>          \s      )
                |   (?<Other>           .       )
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
        );
        public Scanner(string input)
        {
            this.input = input;
        }

        public IEnumerable<Token> Scan()
        {
            var result = new LinkedList<Token>();
            foreach (Match m in regex.Matches(input))
            {
                if (m.Groups["Spaces"].Success)
                {
                    // Ignora los espacios
                }
                else if (m.Groups["Other"].Success)
                {
                    result.AddLast(new Token(TokenCategory.ILLEGAL, m.Value));
                }
                else if (m.Groups["Float"].Success)
                {
                    result.AddLast(new Token(TokenCategory.FLOAT, m.Value));
                }
                else if (m.Groups["SqrBracketLeft"].Success)
                {
                    result.AddLast(new Token(TokenCategory.OPEN_SQUARE_BRACKET, m.Value));
                }
                else if (m.Groups["SqrBracketRight"].Success)
                {
                    result.AddLast(new Token(TokenCategory.CLOSE_SQUARE_BRACKET, m.Value));
                }
                else if (m.Groups["BracketLeft"].Success)
                {
                    result.AddLast(new Token(TokenCategory.OPEN_BRACKET, m.Value));
                }
                else if (m.Groups["BracketRight"].Success)
                {
                    result.AddLast(new Token(TokenCategory.CLOSE_BRACKET, m.Value));
                }
                else if (m.Groups["Comma"].Success)
                {
                    result.AddLast(new Token(TokenCategory.COMMA, m.Value));
                }
            }
            result.AddLast(new Token(TokenCategory.EOF, null));

            return result;
        }
    }

    public class SyntaxError : Exception { }

    public class SemanticError : Exception { }

    public class Parser
    {
        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream)
        {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken
        {
            get
            {
                return tokenStream.Current.Category;
            }
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
                throw new SyntaxError();
            }
        }

        public Node Prog()
        {
            var result = Expr();
            Expect(TokenCategory.EOF);
            var newNode = new Program();
            newNode.Add(result);
            return newNode;
        }

        public Node Expr()
        {
            switch (CurrentToken)
            {
                case TokenCategory.OPEN_BRACKET:
                    return Min();
                case TokenCategory.OPEN_SQUARE_BRACKET:
                    return Max();
                case TokenCategory.FLOAT:
                    return new Float()
                    {
                        AnchorToken = Expect(TokenCategory.FLOAT)
                    };
                default:
                    throw new SyntaxError();
            }
        }

        public Node Min()
        {
            var result = new Min();
            Expect(TokenCategory.OPEN_BRACKET);
            result.Add(Expr());
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                result.Add(Expr());
            }
            Expect(TokenCategory.CLOSE_BRACKET);
            return result;
        }

        public Node Max()
        {
            var result = new Max();
            Expect(TokenCategory.OPEN_SQUARE_BRACKET);
            result.Add(Expr());
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                result.Add(Expr());
            }
            Expect(TokenCategory.CLOSE_SQUARE_BRACKET);
            return result;
        }


    }

    public class Node : IEnumerable<Node>
    {
        IList<Node> children = new List<Node>();

        public int length
        {
            get
            {
                return children.Count;
            }
        }

        public bool hasChildren
        {
            get
            {
                return children.Count > 0;
            }
        }

        public Node this[int index]
        {
            get
            {
                return children[index];
            }
        }

        public Token AnchorToken { get; set; }

        public void Add(Node node)
        {
            children.Add(node);
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{GetType().Name} {AnchorToken}";
        }

        public string ToStringTree()
        {
            var sb = new StringBuilder();
            TreeTraversal(this, "", sb);
            return sb.ToString();
        }

        static void TreeTraversal(Node node, string indent, StringBuilder sb)
        {
            sb.Append(indent);
            sb.Append(node);
            sb.Append('\n');
            foreach (var child in node.children)
            {
                TreeTraversal(child, indent + "  ", sb);
            }
        }
    }

    // node classes
    public class Program : Node { }

    public class Max : Node { }

    public class Min : Node { }

    public class Float : Node { }

    public class WatVisitor
    {
        public String Visit(Program node)
        {
            return
                "(module\n"
                + "\t(func\n"
                + "\t\t(export \"start\")\n"
                + "\t\t(result i32)\n"
                + Visit((dynamic)node[0])
                + "\t\treturn\n"
                + "\t\t)\n"
                + ")\n";
        }

        public String Visit(Max node){
            var sb = new StringBuilder ();
            sb.Append(Visit ((dynamic) node[0]));
            sb.Append(Visit ((dynamic) node[1]));
            sb.Append("\t\tf64.max\n");
            return sb.ToString();
        }

        public String Visit(Min node){
            var sb = new StringBuilder ();
            if (node.)
            sb.Append(Visit ((dynamic) node[0]));
            foreach (var n in node){
                sb.Append(Visit((dynamic)n));
                sb.Append("\t\tf64.min\n");
            }
            // sb.Append(Visit ((dynamic) node[1]));
            // sb.Append("\t\tf64.min\n");
            return sb.ToString();
        }

        public String Visit(Float node){
            return $"\t\tf64.const {node.AnchorToken.Lexeme}\n";
        }

        string VisitChildren(Node node)
        {
            var sb = new StringBuilder();
            foreach (var n in node)
            {
                sb.Append(Visit((dynamic)n));
            }
            return sb.ToString();
        }
    }

    public class Driver
    {
        public static void Main(string[] args)
        {
            // Console.Write("> ");
            // var line = Console.ReadLine();
            var parser = new Parser(new Scanner(args[0]).Scan().GetEnumerator());
            try
            {
                var result = parser.Prog();
                Console.WriteLine(result.ToStringTree());
                Console.WriteLine();

                // File.WriteAllText(
                // "output.wat",
                // new WATVisitor().Visit((dynamic) result));

            }
            catch (SyntaxError)
            {
                Console.WriteLine("Parse error");
            }
            catch (SemanticError)
            {
                Console.WriteLine("Bad semantics!");
            }
        }
    }
}