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
using System.Text.RegularExpressions;

namespace Drac {

    class Scanner {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"
                (?<MultComment>     ([(][*](\s|.)*?[*][)])  )
              | (?<Comment>         [-][-].*)
              | (?<Newline>    \n        )
              | (?<WhiteSpace> \s        )     # Must go after Newline.
              | (?<Tab>         \t       )
              | (?<And>        and       )
              | (?<Or>         or        )
              | (?<Dec>         dec        )
              | (?<Inc>         inc       )
              | (?<Break>         break       )
              | (?<Do>         do       )
              | (?<Elif>         elif       )
              | (?<Else>         else     )
              | (?<Return>         return     )
              | (?<While>         while     )
              | (?<Not>         not    )
              | (?<Var>         var    )
              | (?<IntLiteral>  -?\d+       )
              | (?<MoreEqual>   [>][=]  )
              | (?<LessEqual>   [<][=]  )
              | (?<Diff>        [<][>] )
              | (?<Equals>      [=][=])
              | (?<Less>       [<]       )
              | (?<Greater>     [>]      )
              | (?<Plus>       [+]       )
              | (?<Mul>        [*]       )
              | (?<Neg>        [-]       )
              | (?<Mod>         [%]     )
              | (?<Div>         [/]     )
              | (?<Coma>        [,]     )
              | (?<Semicolon>   [;]     )
              | (?<CharLit>         (['].?['])  )
              | (?<StringLit>       ([""].*?[""])   )
              | (?<SingleQuote>     [\']    )
              | (?<DoubleQuote>     [\""])
              | (?<Unicode>     [\\][u][0-9a-fA-F]{6})
              | (?<BackSlash>       [\\]    )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<BracketLeft>     [{]     )
              | (?<BracketRight>    [}]     )
              | (?<SqrBracketLeft>  [\[]    )
              | (?<SqrBracketRight> [\]]    )
              | (?<Assign>      [=]       )
              | (?<True>        true      )
              | (?<False>       false      )
              | (?<Bool>        bool      )
              | (?<End>         end       )
              | (?<If>          if        )
              | (?<Int>         int       )
              | (?<Then>        then      )
              | (?<Identifier>  [a-zA-Z][a-zA-Z0-9_]* )     # Must go after all keywords
              | (?<Other>       .         )     # Must be last: match any other character.
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        static readonly IDictionary<string, TokenCategory> tokenMap =
            new Dictionary<string, TokenCategory>() {
                {"And", TokenCategory.AND},
                {"Dec", TokenCategory.DEC},
                {"Inc", TokenCategory.INC},
                {"Break", TokenCategory.BREAK},
                {"Do", TokenCategory.DO},
                {"Elif", TokenCategory.ELIF},
                {"Else", TokenCategory.ELSE},
                {"Return", TokenCategory.RETURN},
                {"While", TokenCategory.WHILE},
                {"Unicode", TokenCategory.UNICODE},
                {"Equals", TokenCategory.EQUALS},
                {"SingleQuote", TokenCategory.SINGLE_QUOTE},
                {"DoubleQuote", TokenCategory.DOUBLE_QUOTE},
                {"Not", TokenCategory.NOT},
                {"Var", TokenCategory.VAR},
                {"Diff", TokenCategory.DIFF},
                {"Tab", TokenCategory.TAB},
                {"Less", TokenCategory.LESS},
                {"Plus", TokenCategory.PLUS},
                {"Mul", TokenCategory.MUL},
                {"Neg", TokenCategory.NEG},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"Comment", TokenCategory.COMMENT},
                {"MultComment", TokenCategory.ML_COMMENT},
                {"Assign", TokenCategory.ASSIGN},
                {"True", TokenCategory.TRUE},
                {"False", TokenCategory.FALSE},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"Bool", TokenCategory.BOOL},
                {"End", TokenCategory.END},
                {"If", TokenCategory.IF},
                {"Int", TokenCategory.INT},
                {"Then", TokenCategory.THEN},
                {"Identifier", TokenCategory.IDENTIFIER},
                {"Or", TokenCategory.OR},
                {"Greater", TokenCategory.GREATER},
                {"MoreEqual", TokenCategory.MORE_EQUAL},
                {"LessEqual", TokenCategory.LESS_EQUAL},
                {"Mod", TokenCategory.MOD},
                {"Div", TokenCategory.DIV},
                {"Coma", TokenCategory.COMA},
                {"Semicolon", TokenCategory.SEMICOLON},
                {"CharLit", TokenCategory.CHAR_LIT},
                {"StringLit", TokenCategory.STRING_LIT},
                {"BackSlash", TokenCategory.BACK_SLASH},
                {"BracketLeft", TokenCategory.BRACKET_LEFT},
                {"BracketRight", TokenCategory.BRACKET_RIGHT},
                {"SqrBracketLeft", TokenCategory.SQR_BRACKET_LEFT},
                {"SqrBracketRight", TokenCategory.SQR_BRACKET_RIGHT},
            };

        public Scanner(string input) {
            this.input = input;
        }

        public IEnumerable<Token> Scan() {

            var result = new LinkedList<Token>();
            var row = 1;
            var columnStart = 0;

            foreach (Match m in regex.Matches(input)) {

                if (m.Groups["Newline"].Success) {

                    row++;
                    columnStart = m.Index + m.Length;

                } else if (m.Groups["WhiteSpace"].Success
                    || m.Groups["Comment"].Success) {

                    // Skip white space and comments.

                } else if (m.Groups["Other"].Success) {

                    // Found an illegal character.
                    result.AddLast(
                        new Token(m.Value,
                            TokenCategory.ILLEGAL_CHAR,
                            row,
                            m.Index - columnStart + 1));

                } else {

                    // Must be any of the other tokens.
                    result.AddLast(FindToken(m, row, columnStart));
                }
            }

            result.AddLast(
                new Token(null,
                    TokenCategory.EOF,
                    row,
                    input.Length - columnStart + 1));

            return result;
        }

        Token FindToken(Match m, int row, int columnStart) {
            foreach (var name in tokenMap.Keys) {
                if (m.Groups[name].Success) {
                    return new Token(m.Value,
                        tokenMap[name],
                        row,
                        m.Index - columnStart + 1);
                }
            }
            throw new InvalidOperationException(
                "regex and tokenMap are inconsistent: " + m.Value);
        }
    }
}
