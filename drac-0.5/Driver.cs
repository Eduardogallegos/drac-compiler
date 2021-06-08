/*
  Buttercup compiler - Program driver.
  Copyright (C) 2013-2021 Ariel Ortiz, ITESM CEM

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
using System.IO;
using System.Text;

namespace Drac
{

    public class Driver
    {

        const string VERSION = "0.4";
        const string AUTHORS = "Eduardo Gallegos, Alejandro Chavez, Pedro Cortes";

        //-----------------------------------------------------------
        static readonly string[] ReleaseIncludes = {
            "Lexical analysis",
            "Syntactic analysis",
            "AST construction",
            "Sematic Analysis"
        };

        //-----------------------------------------------------------
        void PrintAppHeader()
        {
            Console.WriteLine("Drac compiler, version " + VERSION);
            Console.WriteLine(
                "Copyright \u00A9 2021 by:" + AUTHORS + ", ITESM CEM."
                + "\n Compiler Design course. Professor Ariel Ortiz.");
            Console.WriteLine("This program is free software; you may "
                + "redistribute it under the terms of");
            Console.WriteLine("the GNU General Public License version 3 or "
                + "later.");
            Console.WriteLine("This program has absolutely no warranty.");
        }

        //-----------------------------------------------------------
        void PrintReleaseIncludes()
        {
            Console.WriteLine("Included in this release:");
            foreach (var phase in ReleaseIncludes)
            {
                Console.WriteLine("   * " + phase);
            }
        }

        //-----------------------------------------------------------
        void Run(string[] args)
        {

            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            if (args.Length != 1)
            {
                Console.Error.WriteLine(
                    "Please specify the name of the input file.");
                Environment.Exit(1);
            }

            try
            {
                var inputPath = args[0];
                var input = File.ReadAllText(inputPath);
                var outputPath=Path.ChangeExtension(inputPath, ".wat");

                var parser = new Parser(new Scanner(input).Scan().GetEnumerator());
                var program = parser.Program();

                var semantic1 = new SemanticVisitor1();
                semantic1.Visit((dynamic) program);

                if (!semantic1.GlobalFunctionsTable.ContainsKey("main") || semantic1.GlobalFunctionsTable["main"].Arity != 0)
                {
                    throw new SemanticError("No main() function declared");
                }

                var semantic2 = new SemanticVisitor2(semantic1);
                semantic2.Visit((dynamic) program);

                Console.WriteLine("Semantics OK.");
                Console.WriteLine();
                Console.WriteLine("Functions Table");
                Console.WriteLine("============");
                var functions = semantic1.GlobalFunctionsTable.Keys;
                Console.WriteLine("| NAME\t| IS PRIMITIVE?\t| Arity\t| Symbol Table\t|");
                foreach (var function in functions) {
                    Console.WriteLine($"| {function}" + semantic1.GlobalFunctionsTable[function]);
                }

                Console.WriteLine();
                Console.WriteLine("Global Variables Table");
                Console.WriteLine("============");
                foreach (var entry in semantic1.GlobalVariablesTable) {
                    Console.WriteLine("|\t"+entry+"\t|");
                }

                var codeGenerator = new WatVisitor(semantic1.GlobalFunctionsTable, semantic1.GlobalVariablesTable);
                File.WriteAllText(
                    outputPath,
                    codeGenerator.Visit((dynamic) ast));
                Console.WriteLine(
                    "Created Wat (WebAssembly text format) file "
                    + $"'{outputPath}'.");

            }
            catch (Exception e)
            {
                if (e is FileNotFoundException
                    || e is SyntaxError
                    || e is SemanticError)
                {
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                throw;
            }
        }

        //-----------------------------------------------------------
        public static void Main(string[] args)
        {
            new Driver().Run(args);
        }
    }
}
