using Chronic.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic
{
    public class SignScanner : ITokenScanner
    {
        static readonly dynamic[] Patterns = new dynamic[]
            {
                new { Pattern = @"^\+$".Compile(), Tag = new SignPlus() },
                new { Pattern = @"^-$".Compile(), Tag = new SignMinus() },                
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

        public override string ToString()
        {
            return "sign";
        }
    }
}
