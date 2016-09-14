using System;
using System.Collections.Generic;
using Chronic.Tags.Repeaters;
using System.Linq;

namespace Chronic.Handlers
{
    public class RdnRmnSdHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            var month = tokens[1].GetTag<RepeaterMonthName>();
            var day = tokens[2].GetTag<ScalarDay>().Value;
            var time_tokens = tokens.Skip(3).ToList();
            var year = options.Clock().Year;

            if (Time.IsMonthOverflow(year, (int) month.Value, day))
            {
                return null;
            }
            try
            {
                if (time_tokens == null || time_tokens.Count == 0)
                {
                    var start = Time.New(year, (int)month.Value, day);
                    var end = start.AddDays(1);
                    return new Span(start, end);
                }
                else
                {
                    var dayStart = Time.New(year, (int)month.Value, day);
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