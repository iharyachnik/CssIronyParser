using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CssIronyParser
{
    // Used grammar:
    // http://dev.w3.org/csswg/css2/grammar.html

    [Language("CSS","2.2", "BNF")]
    class CssGrammarBnf : Grammar
    {
        public CssGrammarBnf()
        {
            #region 1. Terminals

            var Comment = new CommentTerminal("Comment", "/*", "*/");
            NonGrammarTerminals.Add(Comment);

            var string1 = new RegexBasedTerminal("string1", @"""([^\n\r\f""]|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])*""");
            var string2 = new RegexBasedTerminal("string2", @"'([^\n\r\f']|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])*'");
            var S = new RegexBasedTerminal("S", @"[ \t\r\n\f]+");
            var CDO = new RegexBasedTerminal("CDO", @"<!--");
            var CDC = new RegexBasedTerminal("CDC", @"-->");
            var IMPORT_SYM = new RegexBasedTerminal("IMPORT_SYM", "@import"); // add regex
            var MEDIA_SYM = new RegexBasedTerminal("MEDIA_SYM", "@media"); // add regex
            var IDENT = new RegexBasedTerminal("IDENT", @"[-]?([_a-zA-Z]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))([_a-zA-Z0-9-]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*"); // add regex
            var PAGE_SYM = new RegexBasedTerminal("PAGE_SYM", "@page"); // add regex
            var HASH = new RegexBasedTerminal("HASH", @"#([_a-zA-Z0-9-]|([^\0-\177])|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))+"); // add regex
            var INCLUDES = new RegexBasedTerminal("INCLUDES", @"~=");
            var DASHMATCH = new RegexBasedTerminal("DASHMATCH", @"|=");
            var FUNCTION = new RegexBasedTerminal("FUNCTION", @"[-]?([_a-z]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))([_a-zA-Z0-9-]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*\("); // add regex
            var IMPORTANT_SYM = new RegexBasedTerminal("IMPORTANT_SYM", @""); // add regex
            var NUMBER = new RegexBasedTerminal("NUMBER", @"[+-]?([0-9]+|[0-9]*\.[0-9]+)(e[+-]?[0-9]+)?"); 
            var PERCENTAGE = new RegexBasedTerminal("PERCENTAGE", @"[+-]?([0-9]+|[0-9]*\.[0-9]+)(e[+-]?[0-9]+)?%");
            var LENGTH = new RegexBasedTerminal("LENGTH", @""); // add regex
            var EMS = new RegexBasedTerminal("EMS", @""); // add regex
            var EXS = new RegexBasedTerminal("EXS", @""); // add regex
            var ANGLE = new RegexBasedTerminal("ANGLE", @""); // add regex
            var TIME = new RegexBasedTerminal("TIME", @""); // add regex
            var FREQ = new RegexBasedTerminal("FREQ", @""); // add regex

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
            var S_star = new NonTerminal("S_star"); // S*
            var S_plus = new NonTerminal("S_plus"); // S+
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
            var selectorGr1_1 = new NonTerminal("selectorGr1_1"); // [ combinator? selector ]
            var selectorGr1_2 = new NonTerminal("selectorGr1_2"); // [ combinator selector | S+ [ combinator? selector ]
            var simple_selectorGr1_1 = new NonTerminal("simple_selectorGr1_1"); // [ HASH | class | attrib | pseudo ]
            var simple_selectorGr1_1_star = new NonTerminal("simple_selectorGr1_1_star"); // [ HASH | class | attrib | pseudo ]*
            var simple_selectorGr1_1_plus = new NonTerminal("simple_selectorGr1_1_plus"); // [ HASH | class | attrib | pseudo ]+
            var attribGr1_1 = new NonTerminal("attribGr1_1"); // [ '=' | INCLUDES | DASHMATCH ]
            var attribGr1_2 = new NonTerminal("attribGr1_2"); // [ IDENT | STRING ]
            var attribGr1_3 = new NonTerminal("attribGr1_3"); // [ [ '=' | INCLUDES | DASHMATCH ] S* [ IDENT | STRING ] S* ]
            var pseudoGr1_1 = new NonTerminal("pseudoGr1_1"); // [ IDENT | FUNCTION S* [IDENT S*]? ')' ]
            var pseudoGr1_2 = new NonTerminal("pseudoGr1_2"); // [IDENT S*]
            var exprGr1_1 = new NonTerminal("exprGr1_1"); // [ operator? term ]
            var exprGr1_1_star = new NonTerminal("exprGr1_1_star"); // [ operator? term ]*
            var termGr1_1 = new NonTerminal("termGr1_1"); // [ NUMBER S* | PERCENTAGE S* | LENGTH S* | EMS S* | EXS S* | ANGLE S* | TIME S* | FREQ S* ]

            #endregion

            URI.Rule = new RegexBasedTerminal(@"(u|\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\r)(l|\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)\(([ \t\r\n\f]*)((""([^\n\r\f\""]|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*"")|('([^\n\r\f\']|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*'([ \t\r\n\f]*)\)")
                        | new RegexBasedTerminal(@"(u|\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\r)(l|\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)\(([ \t\r\n\f]*)([!#$%&*-\[\]-~]|([^\0-\177])|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*([ \t\r\n\f]*)\)");

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

            #region selector rule

            // BNF form:
            //selector
            //  : simple_selector [ combinator selector | S+ [ combinator? selector ]? ]?
            //  ;

            S_plus.Rule = MakePlusRule(S_plus, S);

            selectorGr1_1.Rule = combinator.Q() + selector;

            selectorGr1_2.Rule = combinator + selector
                                 | S_plus + selectorGr1_1.Q();

            selector.Rule = simple_selector + selectorGr1_2.Q();

            #endregion

            #region simple_selector rule

            // BNF form:
            //simple_selector
            //  : element_name [ HASH | class | attrib | pseudo ]*
            //  | [ HASH | class | attrib | pseudo ]+
            //  ;

            simple_selectorGr1_1.Rule = HASH
                                        | Class
                                        | attrib
                                        | pseudo;

            simple_selectorGr1_1_star.Rule = MakeStarRule(simple_selectorGr1_1_star, simple_selectorGr1_1);

            simple_selectorGr1_1_plus.Rule = MakePlusRule(simple_selectorGr1_1_plus, simple_selectorGr1_1);

            simple_selector.Rule = element_name + simple_selectorGr1_1_star
                                   | simple_selectorGr1_1_plus;

            #endregion

            #region class rule

            // BNF form:
            //class
            //  : '.' IDENT
            //  ;

            Class.Rule = ToTerm(".") + IDENT;

            #endregion

            #region element_name rule

            // BNF form:
            //element_name
            //  : IDENT | '*'
            //  ;

            element_name.Rule = IDENT
                                | ToTerm("*");

            #endregion

            #region attrib rule

            // BNF form:
            //attrib
            //  : '[' S* IDENT S* [ [ '=' | INCLUDES | DASHMATCH ] S*
            //    [ IDENT | STRING ] S* ]? ']'
            //  ;

            attribGr1_1.Rule = ToTerm("=")
                               | INCLUDES
                               | DASHMATCH;

            attribGr1_2.Rule = IDENT
                               | STRING;

            attribGr1_3.Rule = attribGr1_1 + S_star + attribGr1_2 + S_star;

            attrib.Rule = ToTerm("[") + S_star + IDENT + S_star + attribGr1_3.Q() + "]";

            #endregion

            #region pseudo rule

            // BNF form:
            //pseudo
            //  : ':' [ IDENT | FUNCTION S* [IDENT S*]? ')' ]
            //  ;

            pseudoGr1_2.Rule = IDENT + S_star;

            pseudoGr1_1.Rule = IDENT
                               | FUNCTION + S_star + pseudoGr1_2.Q() + ")";

            pseudo.Rule = ToTerm(":") + pseudoGr1_1;

            #endregion

            #region declaration rule

            // BNF form:
            //declaration
            //  : property ':' S* expr prio?
            //  ;

            declaration.Rule = property + ":" + S_star + expr + prio.Q();

            #endregion

            #region prio rule

            // BNF form:
            //prio
            //  : IMPORTANT_SYM S*
            //  ;

            prio.Rule = IMPORTANT_SYM + S_star;

            #endregion

            #region expr rule

            // BNF form:
            //expr
            //  : term [ operator? term ]*
            //  ;

            exprGr1_1.Rule = Operator.Q() + term;

            exprGr1_1_star.Rule = MakeStarRule(exprGr1_1_star, exprGr1_1);

            expr.Rule = term + exprGr1_1_star;

            #endregion

            #region term rule

            // BNF form:
            //term
            //  : unary_operator?
            //    [ NUMBER S* | PERCENTAGE S* | LENGTH S* | EMS S* | EXS S* | ANGLE S* |
            //      TIME S* | FREQ S* ]
            //  | STRING S* | IDENT S* | URI S* | hexcolor | function
            //  ;

            termGr1_1.Rule = NUMBER + S_star
                             | PERCENTAGE + S_star
                             | LENGTH + S_star
                             | EMS + S_star
                             | EXS + S_star
                             | ANGLE + S_star
                             | TIME + S_star
                             | FREQ + S_star;

            term.Rule = unary_operator.Q() + termGr1_1
                        | STRING + S_star
                        | IDENT + S_star
                        | URI + S_star
                        | hexcolor
                        | function;

            #endregion

            #region function rule

            // BNF form:
            //function
            //  : FUNCTION S* expr ')' S*
            //  ;

            function.Rule = FUNCTION + S_star + expr + ")" + S_star;

            #endregion

            #region hexcolor

            // BNF form:
            //hexcolor
            //  : HASH S*
            //  ;

            hexcolor.Rule = HASH + S_star;

            #endregion

            this.Root = stylesheet;

            #endregion

        }
    }
}
