using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class GenericHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var t = DateTime.Parse(options.OriginalPhrase);
            return new Chronic.Span(t, t.AddSeconds(1));
        }
    }
}
