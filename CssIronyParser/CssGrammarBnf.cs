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

            var string1 = new RegexBasedTerminal("string1", ""); // add regex
            var string2 = new RegexBasedTerminal("string2", ""); // add regex 
            var S = new RegexBasedTerminal("S", @"[ \t\r\n\f]+");
            var CDO = new RegexBasedTerminal("CDO", @"(<!--)");
            var CDC = new RegexBasedTerminal("CDC", @"(-->)");
            var IMPORT_SYM = new RegexBasedTerminal("IMPORT_SYM", ""); // add regex
            var MEDIA_SYM = new RegexBasedTerminal("MEDIA_SYM", ""); // add regex
            var IDENT = new RegexBasedTerminal("IDENT", ""); // add regex
            var PAGE_SYM = new RegexBasedTerminal("PAGE_SYM", ""); // add regex

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
            var STRING = new NonTerminal("string");

            var encoding = new NonTerminal("encoding");
            var stylesheetGr2_1 = new NonTerminal("stylesheetGr2_1"); // [S|CDO|CDC]
            var stylesheetGr2_1_star = new NonTerminal("stylesheetGr2_1_"); // [S|CDO|CDC]*
            var stylesheetGr2_2 = new NonTerminal("stylesheetGr2_2"); // [ import [ CDO S* | CDC S* ]* ]
            var stylesheetGr2_2_star = new NonTerminal("stylesheetGr2_2_"); // [ import [ CDO S* | CDC S* ]* ]*
            var stylesheetGr2_2_1 = new NonTerminal("stylesheetGr2_2_1"); // [ CDO S* | CDC S* ]
            var S_star = new NonTerminal("SStar"); // S*
            var stylesheetGr2_2_1_star = new NonTerminal("stylesheetGr2_2_1_"); // [ CDO S* | CDC S* ]*
            var stylesheetGr2 = new NonTerminal("stylesheetGr2"); // [S|CDO|CDC]* [ import [ CDO S* | CDC S* ]* ]
            var stylesheetGr3 = new NonTerminal("stylesheetGr3"); // [ [ ruleset | media | page ] [ CDO S* | CDC S* ]* ]         [ CDO S* | CDC S* ]* = 2_2_1_
            var stylesheetGr3_star = new NonTerminal("stylesheetGr3_"); // [ [ ruleset | media | page ] [ CDO S* | CDC S* ]* ]* 
            var stylesheetGr3_1 = new NonTerminal("stylesheetGr3_1"); // [ ruleset | media | page ]
            var URI = new NonTerminal("URI");
            var importGr2_1 = new NonTerminal("importGr1_1");
            var ruleset_star = new NonTerminal("ruleset_star");
            var media_listGr1_1 = new NonTerminal("media_listGr1_1"); // [ COMMA S* medium]
            var media_listGr1_1_star = new NonTerminal("media_listGr1_1_star"); // [ COMMA S* medium]*
            var pageGr2_1 = new NonTerminal("pageGr2_1"); // [ ';' S* declaration? ]
            var pageGr2_1_star = new NonTerminal("pageGr2_1_star"); // [ ';' S* declaration? ]*
            var rulesetGr1_1 = new NonTerminal("rulesetGr1_1"); // [ ',' S* selector ]
            var rulesetGr1_1_star = new NonTerminal("rulesetGr1_1_star"); // [ ',' S* selector ]*
            var rulesetGr2_1 = new NonTerminal("rulesetGr2_1"); // [ ';' S* declaration? ]
            var rulesetGr2_1_star = new NonTerminal("rulesetGr2_1_star"); // [ ';' S* declaration? ]*

            #endregion

            #region 3. BNF rules

            #region stylesheet rule
            
            //BNF form:
            //stylesheet
            //  : [ CHARSET_SYM STRING ';' ]?
            //    [S|CDO|CDC]* [ import [ CDO S* | CDC S* ]* ]*
            //    [ [ ruleset | media | page ] [ CDO S* | CDC S* ]* ]*
            //  ;

            STRING.Rule = string1
                             | string2;

            encoding.Rule = ToTerm("@charset ") + STRING + ";";

            stylesheetGr2_1.Rule = S
                                 | CDO
                                 | CDC;

            stylesheetGr2_1_star.Rule = MakeStarRule(stylesheetGr2_1_star, stylesheetGr2_1);

            S_star.Rule = MakeStarRule(S_star, S);

            stylesheetGr2_2_1.Rule = CDO + S_star
                                     | CDC + S_star;

            stylesheetGr2_2_1_star.Rule = MakeStarRule(stylesheetGr2_2_1_star, stylesheetGr2_2_1);

            stylesheetGr2_2.Rule = import + stylesheetGr2_2_1_star;

            stylesheetGr2_2_star.Rule = MakeStarRule(stylesheetGr2_2_star, stylesheetGr2_2);

            stylesheetGr2.Rule = stylesheetGr2_1_star + stylesheetGr2_2_star;

            stylesheetGr3_1.Rule = ruleset
                                   | media
                                   | page;

            stylesheetGr3.Rule = stylesheetGr3_1 + stylesheetGr2_2_1_star;

            stylesheetGr3_star.Rule = MakeStarRule(stylesheetGr3_star, stylesheetGr3);

            stylesheet.Rule = encoding
                              | stylesheetGr2
                              | stylesheetGr3_star;

            #endregion

            #region import rule

            // BNF form:
            //import
            //  : IMPORT_SYM S*
            //    [STRING|URI] S* media_list? ';' S*
            //  ;

            importGr2_1.Rule = STRING
                               | URI;

            import.Rule = IMPORT_SYM + S_star
                          | importGr2_1 + S_star + media_list.Q() + ";" + S_star;

            #endregion

            #region media rule

            // BNF form:
            //media
            //  : MEDIA_SYM S* media_list '{' S* ruleset* '}' S*
            //  ;

            ruleset_star.Rule = MakeStarRule(ruleset_star, ruleset);

            media.Rule = MEDIA_SYM + S_star + media_list + "{" + S_star + ruleset_star + "}" + S_star;

            #endregion

            #region media_list rule

            // BNF form:
            //media_list
            //  : medium [ COMMA S* medium]*
            //  ;

            media_listGr1_1.Rule = ToTerm(",") + S_star + medium;

            media_listGr1_1_star.Rule = MakeStarRule(media_listGr1_1_star, media_listGr1_1);

            media_list.Rule = medium + media_listGr1_1_star;

            #endregion

            #region medium rule

            // BNF form:
            //medium
            //  : IDENT S*
            //  ;

            medium.Rule = IDENT + S_star;

            #endregion

            #region page rule

            // BNF form:
            //page
            //  : PAGE_SYM S* pseudo_page?
            //    '{' S* declaration? [ ';' S* declaration? ]* '}' S*
            //  ;

            pageGr2_1.Rule = ToTerm(";") + S_star + declaration.Q();

            pageGr2_1_star.Rule = MakeStarRule(pageGr2_1_star, pageGr2_1);

            page.Rule = PAGE_SYM + S_star + pseudo_page.Q()
                        | ToTerm("{") + S_star + declaration.Q() + pageGr2_1_star + "}" + S_star;

            #endregion

            #region pseudo_page rule

            // BNF form:
            //pseudo_page
            //  : ':' IDENT S*
            //  ;

            pseudo_page.Rule = ToTerm(":") + IDENT + S_star;

            #endregion

            #region operator rule

            // BNF form: 
            //operator
            //  : '/' S* | ',' S*
            //  ;

            Operator.Rule = ToTerm("/") + S_star
                            | ToTerm(",") + S_star;

            #endregion

            #region combinator rule

            // BNF form:
            //combinator
            //  : '+' S*
            //  | '>' S*
            //  ;

            combinator.Rule = ToTerm("+") + S_star
                              | ToTerm(">") + S_star;

            #endregion

            #region unary_operator rule

            // BNF form:
            //unary_operator
            //    : '-' | '+'
            //    ;

            unary_operator.Rule = ToTerm("-")
                                  | "+";

            #endregion

            #region property rule

            // BNF form:
            //property
            //  : IDENT S*
            //  ;

            property.Rule = IDENT + S_star;

            #endregion

            #region ruleset rule

            // BNF form:
            //ruleset
            //  : selector [ ',' S* selector ]*
            //    '{' S* declaration? [ ';' S* declaration? ]* '}' S*
            //  ;

            rulesetGr1_1.Rule = ToTerm(",") + S_star + selector;

            rulesetGr1_1_star.Rule = MakeStarRule(rulesetGr1_1_star, rulesetGr1_1);

            rulesetGr2_1.Rule = ToTerm(";") + S_star + declaration.Q();

            rulesetGr2_1_star.Rule = MakeStarRule(rulesetGr2_1_star, rulesetGr2_1);

            ruleset.Rule = selector + rulesetGr1_1_star
                           | ToTerm("{") + S_star + declaration.Q() + rulesetGr2_1_star + "}" + S_star;

            #endregion

            #endregion

        }
    }
}
