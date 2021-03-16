/*
  Buttercup compiler - Token categories for the scanner.
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace Drac {

enum TokenCategory {
        AND,
        
        ASSIGN,
        BOOL,
        BREAK,
        DEC,
        DO,
        END,
        ELIF,
        ELSE,
        EOF,
        FALSE,
        IDENTIFIER,
        IF,
        INC,
        INT,
        INT_LITERAL,
        LESS,
        MUL,
        NEG,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        COMMENT,
        ML_COMMENT,
        OR,
        GREATER,
        MORE_EQUAL,
        LESS_EQUAL,
        MOD,
        DIV,
        COMA,
        SEMICOLON,
        NOT,
        CHAR_LIT,
        STRING_LIT,
        BACK_SLASH,
        BRACKET_LEFT,
        BRACKET_RIGHT,
        RETURN,
        SQR_BRACKET_LEFT,
        SQR_BRACKET_RIGHT,
        PLUS,
        THEN,
        TRUE,
        ILLEGAL_CHAR,
        WHILE,
        VAR
    }
}
