using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chronic
{
    public class SeparatorScanner : ITokenScanner
    {
        static readonly dynamic[] Patterns = new dynamic[]
            {
                new { Pattern = @"^,$".Compile(), Tag = new SeparatorComma() },
                new { Pattern = @"^\.$".Compile(), Tag = new SeparatorDot() },
                new { Pattern = @"^:$".Compile(), Tag = new SeparatorColon() },
                new { Pattern = @"^ $".Compile(), Tag = new SeparatorSpace() },
                new { Pattern = @"^\/$".Compile(), Tag = new SeparatorSlash() },
                new { Pattern = @"^-$".Compile(), Tag = new SeparatorDash() },
                new { Pattern = @"^'$".Compile(), Tag = new SeparatorQuote(Separator.Type.SingleQuote) },
                new { Pattern = @"^""$".Compile(), Tag = new SeparatorQuote(Separator.Type.DoubleQuote) },
                new { Pattern = @"^(at|@)$".Compile(), Tag = new SeparatorAt() },
                new { Pattern = @"^in$".Compile(), Tag = new SeparatorIn() },                
                new { Pattern = @"^on$".Compile(), Tag = new SeparatorOn() },
                new { Pattern = @"^and$".Compile(), Tag = new SeparatorAnd() },
                new { Pattern = @"^t$$".Compile(), Tag = new SeparatorT() },
                new { Pattern = @"^w$".Compile(), Tag = new SeparatorW() },
            };

        public IList<Token> Scan(IList<Token> tokens, Options options)
        {
            tokens.ForEach(ApplyTags);
            return tokens;
        }

        static void ApplyTags(Token token)
        {
            foreach (var pattern in Patterns)
            {
                if (pattern.Pattern.IsMatch(token.Word))
                {
                    token.Tag(pattern.Tag);
                }
            }
        }
    }
}