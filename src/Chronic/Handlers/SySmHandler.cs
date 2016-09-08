using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class SySmHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var year = tokens[0].GetTag<ScalarYear>().Value;
            var month = (int)tokens[1].GetTag<ScalarMonth>().Value;            

            try
            {
                var start = Time.New(year, month);
                return new Span(start, start.AddMonths(1));
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
