using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CssIronyParser
{
    // Used grammar:
    // http://www.w3.org/TR/CSS21/grammar.html

    [Language("Css 2.1")]
    class CssGrammarBnf : Grammar
    {
        public CssGrammarBnf()
        {
            #region 1. Terminals

            var string1 = new RegexBasedTerminal("string1", "");
            var string2 = new RegexBasedTerminal("string2", "");
            var S = new RegexBasedTerminal("S", @"[ \t\r\n\f]+");
            var CDO = new RegexBasedTerminal("CDO", @"(<!--)");
            var CDC = new RegexBasedTerminal("CDC", @"(-->)");

            #endregion

            #region 2. Non-terminals

            var stylesheet = new NonTerminal("stylesheet");
            var import = new NonTerminal("import");
            var media = new NonTerminal("media");
            var media_list = new NonTerminal("media_list");
            var medium = new NonTerminal("medium");
            var page = new NonTerminal("page");
            var pseudo_page = new NonTerminal("pseudo_page");
            var Operator = new NonTerminal("operator");
            var combinator = new NonTerminal("combinator");
            var unary_operator = new NonTerminal("unary_operator");
            var property = new NonTerminal("property");
            var ruleset = new NonTerminal("ruleset");
            var selector = new NonTerminal("selector");
            var simple_selector = new NonTerminal("simple_selector");
            var Class = new NonTerminal("class");
            var element_name = new NonTerminal("element_name");
            var attrib = new NonTerminal("attrib");
            var pseudo = new NonTerminal("pseudo");
            var declaration = new NonTerminal("declaration");
            var prio = new NonTerminal("prio");
            var expr = new NonTerminal("expr");
            var term = new NonTerminal("term");
            var function = new NonTerminal("function");
            var hexcolor = new NonTerminal("hexcolor");
            var resString = new NonTerminal("string");

            var encoding = new NonTerminal("encoding");
            var stylesheetGr2_1 = new NonTerminal("stylesheetGr2_1"); // [S|CDO|CDC]
            var stylesheetGr2_1_ = new NonTerminal("stylesheetGr2_1_"); // [S|CDO|CDC]*
            var stylesheetGr2_2 = new NonTerminal("stylesheetGr2_2"); // [ import [ CDO S* | CDC S* ]* ]
            var stylesheetGr2_2_ = new NonTerminal("stylesheetGr2_2_"); // [ import [ CDO S* | CDC S* ]* ]*
            var stylesheetGr2_2_1 = new NonTerminal("stylesheetGr2_2_1"); // [ CDO S* | CDC S* ]
            var SStar = new NonTerminal("SStar"); // S*
            var stylesheetGr2_2_1_ = new NonTerminal("stylesheetGr2_2_1_"); // [ CDO S* | CDC S* ]*
            var stylesheetGr2 = new NonTerminal("stylesheetGr2"); // [S|CDO|CDC]* [ import [ CDO S* | CDC S* ]* ]
            var stylesheetGr2_ = new NonTerminal("stylesheetGr2_"); // [S|CDO|CDC]* [ import [ CDO S* | CDC S* ]* ]*
            var stylesheetGr3 = new NonTerminal("stylesheetGr3"); // [ [ ruleset | media | page ] [ CDO S* | CDC S* ]* ]         [ CDO S* | CDC S* ]* = 2_2_1_
            var stylesheetGr3_ = new NonTerminal("stylesheetGr3_"); // [ [ ruleset | media | page ] [ CDO S* | CDC S* ]* ]* 
            var stylesheetGr3_1 = new NonTerminal("stylesheetGr3_1"); // [ ruleset | media | page ]
            #endregion

            #region 3. BNF rules

            resString.Rule = string1
                             | string2;

            encoding.Rule = ToTerm("@charset ") + resString + ";";

            stylesheetGr2_1.Rule = S
                                 | CDO
                                 | CDC;

            stylesheetGr2_1_.Rule = MakeStarRule(stylesheetGr2_1_, stylesheetGr2_1);

            SStar.Rule = MakeStarRule(SStar, S);

            stylesheetGr2_2_1.Rule = CDO + SStar
                                     | CDC + SStar;

            stylesheetGr2_2_1_.Rule = MakeStarRule(stylesheetGr2_2_1_, stylesheetGr2_2_1);

            stylesheetGr2_2.Rule = import + stylesheetGr2_2_1_;

            stylesheetGr2_2_.Rule = MakeStarRule(stylesheetGr2_2_, stylesheetGr2_2);

            stylesheetGr2.Rule = stylesheetGr2_1_ + stylesheetGr2_2_;

            stylesheetGr3_1.Rule = ruleset
                                   | media
                                   | page;

            stylesheetGr3.Rule = stylesheetGr3_1 + stylesheetGr2_2_1_;

            stylesheetGr3_.Rule = MakeStarRule(stylesheetGr3_, stylesheetGr3);

            stylesheet.Rule = encoding
                              | stylesheetGr2_
                              | stylesheetGr3_;

            #endregion
            
        }
    }
}
