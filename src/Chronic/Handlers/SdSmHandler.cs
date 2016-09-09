using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class SdSmHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var newTokens = new List<Token> { tokens[1], tokens[0] };
            var timeTokens = tokens.Skip(2);

            newTokens.AddRange(timeTokens);

            return new SmSdHandler().Handle(newTokens, options);
        }
    }
}
