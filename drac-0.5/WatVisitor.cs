/*
  Buttercup compiler - WebAssembly text file (Wat) code generator.
  Copyright (C) 2020-2021 Ariel Ortiz, ITESM CEM

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

using System;
using System.Text;
using System.Collections.Generic;

namespace Drac {

    class WatVisitor {

         IDictionary<string, Type> table;
        public WatVisitor(IDictionary<string, Type> table) {
            this.table = table;
        }


          
        public string Visit(True node) {
            return "    i32.const 1\n";
        }

       
        public string Visit(False node) {
            return "    i32.const 0\n";
        }

       
        public string Visit(Neg node) {
            return "    i32.const 0\n"
                + Visit((dynamic) node[0])
                + "    i32.sub\n";
        }

        
        public string Visit(And node) {
            return VisitBinaryOperator("i32.and", node);
        }

        
        public string Visit(Less node) {
            return VisitBinaryOperator("i32.lt_s", node);
        }

       
        public string Visit(Plus node) {
            return VisitBinaryOperator("i32.add", node);
        }

        //-----------------------------------------------------------
        public string Visit(Mul node) {
            return VisitBinaryOperator("i32.mul", node);
        }
    }
}
