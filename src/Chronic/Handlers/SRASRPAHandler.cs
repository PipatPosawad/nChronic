using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class SRASRPAHandler : SRPHandler
    {
        public override Span Handle(IList<Token> tokens, Options options)
        {
            var anchorSpan = tokens.Skip(4).GetAnchor(options);

            var span = Handle(tokens.ToList().GetRange(0, 2).Union(tokens.ToList().GetRange(4, 3)).ToList(), anchorSpan, options);

            return Handle(tokens.ToList().GetRange(2, 1).Union(tokens.ToList().GetRange(4, 3)).ToList(), span, options);
        }
    }
}
