using System;
using Irony.Parsing;

namespace CssIronyParser
{
    [Language("CSS", "2.2", "")]
    public class CssGrammar : Grammar
    {

        // Grammar version 2.2 http://dev.w3.org/csswg/css2/syndata.html
        //                     http://dev.w3.org/csswg/css2/grammar.html

        public CssGrammar()
        {

            #region 1. Terminals

            var ident = new RegexBasedTerminal(@"[-]?([_a-z]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))([_a-zA-Z0-9-]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*");
            var name = new RegexBasedTerminal(@"([_a-zA-Z0-9-]|([^\0-\177])|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))+");
            var nmstart = new RegexBasedTerminal(@"[_a-z]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])");
            var nonascii = new RegexBasedTerminal(@"[^\0-\177]");
            var unicode = new RegexBasedTerminal(@"\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?");
            var escape = new RegexBasedTerminal(@"(\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]");
            var nmchar = new RegexBasedTerminal(@"[_a-zA-Z0-9-]|[^\0-\177]|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])");
            var num = new RegexBasedTerminal(@"[+-]?([0-9]+|[0-9]*\.[0-9]+)(e[+-]?[0-9]+)?");
            var string1 = new RegexBasedTerminal(@"""([^\n\r\f""]|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])*""");
            var string2 = new RegexBasedTerminal(@"'([^\n\r\f']|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])*'");
            var String = string1 | string2;
            var badstring1 = new RegexBasedTerminal(@"""([^\n\r\f""]|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])*\?");
            var badstring2 = new RegexBasedTerminal(@"'([^\n\r\f']|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f])*\?");
            var badstring = badstring1 | badstring2;
            var badcomment1 = new RegexBasedTerminal(@"\/\*[^*]*\*+([^/*][^*]*\*+)*");
            var badcomment2 = new RegexBasedTerminal(@"\/\*[^*]*(\*+[^/*][^*]*)*");
            var badcomment = badstring1 | badstring2;
            var baduri1 = new RegexBasedTerminal("(u|\\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\\r)(l|\\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)([ \t\r\n\f]*)([!#$%&*-~]|([])|((\\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*([ \t\r\n\f]*)");
            var baduri2 = new RegexBasedTerminal("(u|\\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\\r)(l|\\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)([ \t\r\n\f]*)((\"([^\n\r\f\"]|\\(\n|\r\n|\r|\f)|((\\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*\")|('([^\n\r\f']|\\(\n|\r\n|\r|\f)|((\\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*'))([ \t\r\n\f]*)");
            var baduri3 = new RegexBasedTerminal("(u|\\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\\r)(l|\\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)([ \t\r\n\f]*)((\"([^\n\r\f\"]|\\(\n|\r\n|\r|\f)|((\\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*\\?)|('([^\n\r\f']|\\(\n|\r\n|\r|\f)|((\\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*\\?))");
            var baduri = baduri1 | baduri2 | baduri3;
            var nl = new RegexBasedTerminal(@"\n|\r\n|\r|\f");
            var w = new RegexBasedTerminal(@"[ \t\r\n\f]*");
            var L = new RegexBasedTerminal(@"l|\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l");
            var R = new RegexBasedTerminal(@"r|\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\\r");
            var U = new RegexBasedTerminal(@"u|\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u");
            var S = new RegexBasedTerminal(@"[ \t\r\n\f]+");
            var COMMENT = new CommentTerminal("COMMENT", "/*", "*/");
            NonGrammarTerminals.Add(COMMENT);
            var DASHMATCH = ToTerm("|=");
            var INCLUDES = ToTerm("~=");
            
            #endregion

            #region 2. Non-terminals

            var IDENT = new NonTerminal("IDENT", ident);
            var ATKEYWORD = new NonTerminal("ATKEYWORD", ToTerm("@") + ident);
            var STRING = new NonTerminal("STRING", String);
            var BAD_STRING = new NonTerminal("BAD_STRING", badstring);
            var BAD_URI = new NonTerminal("BAD_URI", baduri);
            var BAD_COMMENT = new NonTerminal("BAD_COMMENT", badcomment);
            var HASH = new NonTerminal("HASH", ToTerm("#") + name);
            var NUMBER = new NonTerminal("NUMBER", num);
            var PERCENTAGE = new NonTerminal("PERCENTAGE", num + "%");
            var DIMENSION = new NonTerminal("DIMENSION", num + ident);
            var URI = new NonTerminal("URI");
            var UNICODE_RANGE = new NonTerminal("UNICODE-RANGE");
            var CDO = new NonTerminal("CDO", ToTerm("<!--"));
            var CDC = new NonTerminal("CDC", ToTerm("-->"));
            var FUNCTION = new NonTerminal("FUNCTION", ident + "(");

            var stylesheet = new NonTerminal("stylesheet");
            var statement = new NonTerminal("statement");
            var at_rule = new NonTerminal("at-rule");
            var block = new NonTerminal("block");
            var ruleset = new NonTerminal("ruleset");
            var selector = new NonTerminal("selector");
            var declaration = new NonTerminal("declaration");
            var property = new NonTerminal("property");
            var value = new NonTerminal("value");
            var any = new NonTerminal("any");
            var unused = new NonTerminal("unused");
            var any_star = new NonTerminal("any_star");
            var any_plus = new NonTerminal("any_star");

            #endregion

            #region 3. BNF-rules

            URI.Rule = new RegexBasedTerminal(@"(u|\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\r)(l|\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)\(([ \t\r\n\f]*)((""([^\n\r\f\""]|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*"")|('([^\n\r\f\']|\(\n|\r\n|\r|\f)|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\[^\n\r\f0-9a-f]))*'([ \t\r\n\f]*)\)")
                        | new RegexBasedTerminal(@"(u|\0(0,4)(55|75)(\r\n|[ \t\r\n\f])?|\\u)(r|\0(0,4)(52|72)(\r\n|[ \t\r\n\f])?|\r)(l|\0(0,4)(4c|6c)(\r\n|[ \t\r\n\f])?|\\l)\(([ \t\r\n\f]*)([!#$%&*-\[\]-~]|([^\0-\177])|((\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?)|\\[^\n\r\f0-9a-f]))*([ \t\r\n\f]*)\)");

            UNICODE_RANGE.Rule = new RegexBasedTerminal(@"u\+[?]{1,6}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{1}[?]{0,5}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{2}[?]{0,4}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{3}[?]{0,3}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{4}[?]{0,2}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{5}[?]{0,1}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{6}")
                                | new RegexBasedTerminal(@"u\+[0-9a-f]{1,6}-[0-9a-f]{1,6}");

            //stylesheet  : [ CDO | CDC | S | statement ]*;
            //
            var stylesheet_nonstar = CDO | CDC | S | statement;
            stylesheet.Rule = MakeStarRule(stylesheet, stylesheet_nonstar);

            statement.Rule = ruleset | at_rule;

            //at-rule     : ATKEYWORD S* any* [ block | ';' S* ];
            //
            var S_star = new NonTerminal("S_star");
            S_star.Rule = MakeStarRule(S_star, S);
            var at_rule_grp = block | ";" + S_star;
            at_rule.Rule = ATKEYWORD + S_star + any_star + at_rule_grp;

            //block       : '{' S* [ any | block | ATKEYWORD S* | ';' S* ]* '}' S*;
            //
            var block_grp = any | block | ATKEYWORD + S_star | ";" + S_star;
            var block_grp_star = new NonTerminal("block_grp_star");
            block_grp_star.Rule = MakeStarRule(block_grp_star, block_grp);
            block.Rule = ToTerm("{") + S_star + block_grp_star + "}" + S_star;

            //ruleset     : selector? '{' S* declaration? [ ';' S* declaration? ]* '}' S*;
            //
            var ruleset_grp = ToTerm(";") + S_star + declaration.Q();
            var ruleset_grp_star = new NonTerminal("ruleset_grp_star");
            ruleset_grp_star.Rule = MakeStarRule(ruleset_grp_star, ruleset_grp);
            ruleset.Rule = selector.Q() + "{" + S_star + declaration.Q() + ruleset_grp_star + "}" + S_star;

            //selector    : any+;
            //
            //var selector_plus = new NonTerminal("selector_plus");
            //selector_plus.Rule = MakePlusRule(selector_plus, selector);
            selector.Rule = any_plus;

            //declaration : property S* ':' S* value;
            //
            declaration.Rule = property + S_star + ":" + S_star + value;

            //property    : IDENT;
            //
            property.Rule = IDENT;

            //value       : [ any | block | ATKEYWORD S* ]+;
            //
            var value_grp = any | block | ATKEYWORD + S_star;
            var value_grp_plus = new NonTerminal("value_grp_plus");
            value_grp_plus.Rule = MakePlusRule(value_grp_plus, value_grp);
            value.Rule = value_grp_plus;

            /*any         : [ IDENT | NUMBER | PERCENTAGE | DIMENSION | STRING
                            | DELIM | URI | HASH | UNICODE-RANGE | INCLUDES
                            | DASHMATCH | ':' | FUNCTION S* [any|unused]* ')'
                            | '(' S* [any|unused]* ')' | '[' S* [any|unused]* ']'
                            ] S*; 
            */
            var anyORunused = any | unused;
            var anyORunused_star = new NonTerminal("anyORunused_star");
            anyORunused_star.Rule = MakeStarRule(anyORunused_star, anyORunused);

            var any_grp = IDENT | NUMBER | PERCENTAGE | DIMENSION | STRING
                        | URI | HASH | UNICODE_RANGE | INCLUDES
                        | DASHMATCH | ":" | FUNCTION + S_star + anyORunused_star + ")"
                        | "(" + S_star + anyORunused_star + ")"
                        | "[" + S_star + anyORunused_star + "]"
                        ;

            any.Rule = any_grp + S_star;
            any_star.Rule = MakeStarRule(any_star, any);
            any_plus.Rule = MakePlusRule(any_plus, any);

            //unused      : block | ATKEYWORD S* | ';' S* | CDO S* | CDC S*;
            //
            unused.Rule = block | ATKEYWORD + S_star | ";" + S_star | CDO + S_star | CDC + S_star;

            this.Root = stylesheet;

            #endregion
        }
    }
}
