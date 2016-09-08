using Chronic.Tags.Repeaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class OdRmHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var day = tokens[0].GetTag<OrdinalDay>().Value;
            var month = tokens[2].GetTag<RepeaterMonthName>();
            
            return Utils.HandleMD(month, day, tokens.Skip(3).ToList(), options);
        }
    }
}
