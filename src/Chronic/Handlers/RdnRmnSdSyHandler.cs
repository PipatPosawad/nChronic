using Chronic.Tags.Repeaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class RdnRmnSdSyHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var month = tokens[1].GetTag<RepeaterMonthName>();
            var day = tokens[2].GetTag<ScalarDay>().Value;
            var year = tokens[2].GetTag<ScalarYear>().Value;

            if (Time.IsMonthOverflow(year, (int)month.Value, day))
            {
                return null;
            }
            try
            {
                var start = Time.New(year, (int)month.Value, day);
                var end = start.AddDays(1);
                return new Span(start, end);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
