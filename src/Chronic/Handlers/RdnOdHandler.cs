using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Handlers
{
    public class RdnOdHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var day = tokens[1].GetTag<OrdinalDay>().Value;
            var time_tokens = tokens.Skip(2).ToList();
            var year = options.Clock().Year;
            var month = options.Clock().Month;

            if(options.Context == Pointer.Type.Future)
            {
                month = options.Clock().Day > day ? month + 1 : month;
            }

            if (Time.IsMonthOverflow(year, month, day))
            {
                return null;
            }

            try
            {
                if(time_tokens == null || time_tokens.Count == 0)
                {
                    var start = Time.New(year, month, day);
                    var end = start.AddDays(1);
                    return new Span(start, end);
                }
                else
                {
                    var dayStart = Time.New(year, month, day);
                    return Utils.DayOrTime(dayStart, time_tokens, options);
                }
                
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
