﻿/*
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

        int labelCounter = 0;
        bool is_param = false;
        public SemanticVisitor1 visitor1{ get; }
        public WatVisitor(SemanticVisitor1 visitor1) {
            this.visitor1 = visitor1;
        }

        public string Visit(Program node)
        {
            // stringCode.Append(";; Generated by the drac compiler "
            //     + "(module\n");
            // stringCode.Append($"\t\t(import \"drac\" \"printi\" (func $printi (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"printc\" (func $printc (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"prints\" (func $prints (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"println\" (func $println (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"readi\" (func $readi (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"reads\" (func $reads (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"new\" (func $new (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"size\" (func $size (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"add\" (func $add (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"get\" (func $get (param i32) (result i32)))\n");
            //  stringCode.Append($"\t\t(import \"drac\" \"set\" (func $set (param i32) (result i32)))\n");
            Console.WriteLine("program");
            return ";; WebAssembly text format code generated by "
                + "the drac compiler.\n\n"
                + "(module\n"
                + "\t(import \"drac\" \"printi\" (func $printi (param i32) (result i32)))\n"
                + "\t(import \"drac\" \"printc\" (func $printc (param i32) (result i32)))\n"
                + "\t(import \"drac\" \"prints\" (func $prints (param i32) (result i32)))\n"
                + "\t(import \"drac\" \"println\" (func $println (result i32)))\n"
                + "\t(import \"drac\" \"readi\" (func $readi (result i32)))\n"
                + "\t(import \"drac\" \"reads\" (func $reads (result i32)))\n"
                + "\t(import \"drac\" \"new\" (func $new (param i32) (result i32)))\n"
                + "\t(import \"drac\" \"size\" (func $size (param i32) (result i32)))\n"
                + "\t(import \"drac\" \"add\" (func $add (param i32 i32) (result i32)))\n"
                + "\t(import \"drac\" \"get\" (func $get (param i32 i32) (result i32)))\n"
                + "\t(import \"drac\" \"set\" (func $set (param i32 i32 i32) (result i32)))\n\n"
                + VisitChildren(node)
                + ")\n";
        }

        public string Visit(VarDef node){
            Console.WriteLine("vardef");
            return VisitChildren(node);
        }

        public string Visit(VarDefList node){
            Console.WriteLine("vardeflist");
            return VisitChildren(node);
        }

        public string Visit(Identifier node)
        {
            Console.WriteLine("id");
            var sb = new StringBuilder();
            var varName = node.AnchorToken.Lexeme;
            if (!visitor1.GlobalFunctionsTable.ContainsKey(varName)){
                var varScope = getVarScope(varName);
                sb.Append($"\t\t{varScope}.get ${varName}\n");
            }
            return sb.ToString();
        }

        public string Visit(IdList node)
        {
            Console.WriteLine("idlist");
            var sb = new StringBuilder();
            if(node.hasChildren){
                foreach (var childNode in node){
                    var localVarName = childNode.AnchorToken.Lexeme;
                    if(is_param){
                        sb.Append($"\t\t(param ${localVarName} i32)\n");
                    }
                    else{
                        sb.Append($"\t\t(local ${localVarName} i32)\n");
                    }
                }
            }
            return sb.ToString();
        }
        public string Visit(Funcion node){
            Console.WriteLine("funcion");
            var stringCode = new StringBuilder();
            var functionName = node.AnchorToken.Lexeme;

            if (functionName.Contains("main"))
            {
                stringCode.Append($"\t(func\n" + $"\t\t(export \"{functionName}\")\n");
                // stringCode.Append(Visit((dynamic) node[2]));
            }else{
                stringCode.Append($"\t(func ${functionName}\n");
            }
            is_param = true;
            stringCode.Append(Visit((dynamic) node[0])); // params
            // stringCode.Append("\t\t\t(result i32)\n");
            is_param = false;
            stringCode.Append("\t\t(result i32)\n");
            stringCode.Append("\t\t(local $_temp i32)\n");
            stringCode.Append("\t\t(local $s i32)\n");
            stringCode.Append(Visit((dynamic) node[1])); // locals vars

            stringCode.Append(Visit((dynamic) node[2])); // stmt list

            stringCode.Append("\t\ti32.const 0\n");
            stringCode.Append("\t)\n");
            return stringCode.ToString();

        }

        public string Visit(ExprList node)
        {
            Console.WriteLine("exprlist");
            // var sb = new StringBuilder();
            // foreach(var childNode in node){
            //     var varName = childNode.AnchorToken.Lexeme;
            //     var varScope = getVarScope(varName);
            //     sb.Append($"\t\t{varScope}.get ${varName}");
            // }
            
            // return sb.ToString();
            return VisitChildren(node);
        }
        public string Visit(StmtList node)
        {
            Console.WriteLine("stmtlist");
            return VisitChildren(node);
        }

        public string Visit(Assignment node){
            Console.WriteLine("assig");
            var varName = node.AnchorToken.Lexeme;
            var varScope = getVarScope(varName);
            return Visit((dynamic)node[0]) + $"\t\t{varScope}.set ${varName}\n";
        }

        public string Visit(FunctionCall node) {
            Console.WriteLine("fncall");
            if(node.hasChildren){
                return Visit((dynamic) node[0])
                + $"\t\tcall ${node.AnchorToken.Lexeme}\n\t\tdrop\n";
            }else{
                return $"\t\tcall ${node.AnchorToken.Lexeme}\n";
            }
            
        }

        public string Visit(Increase node){
            Console.WriteLine("inc");
            var varName = node[0].AnchorToken.Lexeme;
            var varScope = getVarScope(varName);
            return $"\t\t{varScope}.get ${varName}\n"
            + "\t\ti32.const 1\n"
            + "\t\ti32.add\n"
            + $"\t\t{varScope}.set ${varName}\n";
        }

        public string Visit(Decrease node){
            Console.WriteLine("dec");
            var varName = node[0].AnchorToken.Lexeme;
            var varScope = getVarScope(varName);
            return $"\t\t{varScope}.get ${varName}\n"
            + "\t\ti32.const 1\n"
            + "\t\ti32.sub\n"
            + $"\t\t{varScope}.set ${varName}\n";
        }

        public string Visit(StmtIf node){
            Console.WriteLine("if");
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("\t\tif\n"
                      + Visit((dynamic) node[1])
                      + Visit((dynamic) node[2])
                      + Visit((dynamic) node[3]));

            return sb.ToString();
        }

        public string Visit(ElseIfList node){
            Console.WriteLine("eliflist");
            return VisitChildren(node);
        }

        public string Visit(ElseIf node){
            Console.WriteLine("elif");
            var sb = new StringBuilder();
            if (node.hasChildren)
            {
                sb.Append(Visit((dynamic)node[0]));
                sb.Append("\t\tif\n"
                          + Visit((dynamic)node[1])
                          + "\t\tend\n");
            }

            return sb.ToString();
        }

        public string Visit(Else node){
            Console.WriteLine("else");
            var sb = new StringBuilder();
            if (node.hasChildren)
            {
                sb.Append("\t\telse\n"
                          + VisitChildren(node)
                          + "\t\tend\n");
            }
            else
            {
                sb.Append("\t\tend\n");
            }

            return sb.ToString();
        }

        public string Visit(StmtWhile node){
            Console.WriteLine("while");
            var label1 = GenerateLabel();
            var label2 = GenerateLabel();

            return "\t\tblock " + label1 + "\n"
            + "\t\tloop " + label2 + "\n"
            + Visit((dynamic) node[0])
            + "\t\ti32.eqz\n"
            + "\t\tbr_if " + label1 + "\n"
            + Visit ((dynamic) node[1])
            + "\t\tbr " + label2 + "\n"
            + "\t\tend\n"
            + "\t\tend\n";
        }

        public string Visit(StmtDoWhile node){
            Console.WriteLine("do");
            var label1 = GenerateLabel();
            var label2 = GenerateLabel();

            return "\t\tblock " + label1 + "\n"
            + "\t\tloop " + label2 + "\n"
            + Visit ((dynamic) node[0])
            + "\t\tbr " + label2 + "\n"
            + Visit((dynamic) node[1])
            + "\t\ti32.eqz\n"
            + "\t\tbr_if " + label1 + "\n"
            + "\t\tend\n"
            + "\t\tend\n";
        }

        public string Visit(StmtBreak node){
            Console.WriteLine("br");
            return VisitChildren(node);
        }

        public string Visit(StmtReturn node)
        {
            Console.WriteLine("return");
            return  VisitChildren(node) + "\t\treturn\n";
        }

        public string Visit(StmtEmpty node){
            Console.WriteLine("empty");
            return VisitChildren(node);
        }

        public string Visit(Equals node){
            Console.WriteLine("equals");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.eq\n";
        }

        public string Visit(Diff node){
            Console.WriteLine("diff");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.ne\n";
        }

        public string Visit(Less node) {
            Console.WriteLine("less");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.lt_s\n";
        }

        public string Visit(LessEqual node){
            Console.WriteLine("lessequals");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.le_s\n";
        }

        public string Visit(Greater node){
            Console.WriteLine("greater");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.gt_s\n";
        }

        public string Visit(MoreEqual node){
            Console.WriteLine("moreequals");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.ge_s\n";
        }

        public string Visit(Neg node) {
            Console.WriteLine("neg");
            if(node.length == 1){
                return $"\t\ti32.const 0\n"
                + Visit((dynamic) node[1])
                + "\t\ti32.sub\n";
            }else{
                return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\ti32.sub\n";
            }
            
        }

        public string Visit(Plus node) {
            Console.WriteLine("plus");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.add\n";
        }

        public string Visit(Mul node) {
            Console.WriteLine("mul");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.mul\n";
        }

        public string Visit(Div node){
            Console.WriteLine("div");
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\ti32.div_s\n";
        }

        public string Visit(Mod node){
            Console.WriteLine("mod");
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\ti32.rem_s\n";
        }

        public string Visit(Not node){
            Console.WriteLine("not");
            return Visit((dynamic)node[0])
                   + "\t\ti32.eqz\n";
        }

        public string Visit(Positive node){
            Console.WriteLine("pos");
            return VisitChildren(node);
        }

        public string Visit(Negative node){
            Console.WriteLine("neg");
            return VisitChildren(node);
        }

        public string Visit(True node) {
            Console.WriteLine("true");
            return "    i32.const 1\n";
        }

        public string Visit(False node) {
            Console.WriteLine("false");
            return "    i32.const 0\n";
        }

        public string Visit(Int_literal node)
        {
            Console.WriteLine("int_lit");
            return $"\t\ti32.const {node.AnchorToken.Lexeme}\n";
        }

        public string Visit(Char_lit node){
            Console.WriteLine("char");
            var asciiChar = convertCharToASCII(node.AnchorToken.Lexeme);
            var sb = new StringBuilder();
            sb.Append("\t\ti32.const 0\n");
            sb.Append("\t\tcall $new\n");
            sb.Append($"\t\tlocal.set $_temp\n");
            sb.Append($"\t\tlocal.get $_temp\n");
            sb.Append($"\t\tlocal.get $_temp\n");
            sb.Append($"\t\ti32.const {asciiChar[0]}\n"
                + "\t\tcall $add\n"
                + "\t\tdrop\n");
            return sb.ToString();
        }

        public string Visit(String_lit node){
            Console.WriteLine("string");
            var asciiChars = convertCharToASCII(node.AnchorToken.Lexeme);
            var sb = new StringBuilder();
            sb.Append("\t\ti32.const 0\n");
            sb.Append("\t\tcall $new\n");
            sb.Append($"\t\tlocal.set $_temp\n");
            foreach (var entry in asciiChars)
            {
                sb.Append($"\t\tlocal.get $_temp\n");
            }
            sb.Append($"\t\tlocal.get $_temp\n");
            foreach (var entry in asciiChars){
                sb.Append($"\t\ti32.const {entry}\n"
                    + "\t\tcall $add\n"
                    + "\t\tdrop\n");
            }
            return sb.ToString();
        }

        public string Visit(Or node){
            Console.WriteLine("or");
            return Visit((dynamic)node[0])
                   + "\t\tif(result i32)\n"
                   + "\t\ti32.const 1\n"
                   + "\t\telse\n"
                   + Visit((dynamic)node[1])
                   + "\t\tif(result i32)\n"
                   + "\t\ti32.const 1\n"
                   + "\t\telse\n"
                   + "\t\ti32.const 0\n"
                   + "\t\tend\n"
                   + "\t\tend\n";
        }

        public string Visit(And node) {
            Console.WriteLine("and");
            return Visit((dynamic)node[0])
                   + Visit((dynamic)node[1])
                   + "\t\ti32.and\n";
        }

        public string Visit(Array node){
            Console.WriteLine("array");
            return VisitChildren(node);
        }

        string VisitChildren(Node node) {
            var sb = new StringBuilder();
            foreach (var n in node) {
                sb.Append(Visit((dynamic) n));
            }
            return sb.ToString();
        }

        public String GenerateLabel(){
            return $"${labelCounter++:00000}";
        }

        public String getVarScope(String varName){
            var varScope = "local";
            if (visitor1.GlobalVariablesTable.Contains(varName)){
                varScope = "global";
            }
            return varScope;
        }

        private IList<int> convertCharToASCII(String Lexeme){
            var result = new List<int>();
            for (int i = 0; i < Lexeme.Length; i++)
            {
                int asciiChar = (int)Lexeme[i];
                if(asciiChar != 34 && asciiChar != 39){
                    if(asciiChar == 92){
                        i++;
                        switch((int)Lexeme[i]){
                            case 34:
                                result.Add(34);
                                break;
                            case 39:
                                result.Add(39);
                                break;
                            case 92:
                                result.Add(92);
                                break;
                            case 110:
                                result.Add(10);
                                break;
                            case 114:
                                result.Add(13);
                                break;
                            case 116:
                                result.Add(9);
                                break;
                        }
                    }else{
                        result.Add(asciiChar);
                    }
                }
            }
            return result;
        }
    }
}
