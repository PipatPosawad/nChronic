using Chronic.Tags.Repeaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class SmRmnSyHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var day = tokens[0].GetTag<ScalarDay>().Value;
            var month = tokens[1].GetTag<RepeaterMonthName>();            
            var year = tokens[2].GetTag<ScalarYear>().Value;

            DateTime? start;
            DateTime end;

            if (tokens.Count() > 3)
            {
                start = tokens.Skip(3).GetAnchor(options).Start;

                var h = start.Value.Hour;
                var m = start.Value.Minute;
                var s = start.Value.Second;

                start = Time.New(year, (int)month.Value, day, h, m, s);
                end = Time.New(year, (int)month.Value, day + 1, h, m, s);
            } else
            {
                start = Time.New(year, (int)month.Value, day);
                day = day >= 31 ? day : day + 1;
                end = Time.New(year, (int)month.Value, day);
            }
            return new Span(start.Value, end);
        }
    }
}
