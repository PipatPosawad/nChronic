using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Chronic.Tests.Parsing
{
    public class KnownPatternsParsingTests : ParsingTestsBase
    {
        protected override DateTime Now()
        {
            return Time.New(2006, 8, 16, 14, 0, 0);
        }

        [Fact]
        public void generic()
        {
            Parse("2012-08-02T13:00:00")
                .AssertEquals(Time.New(2012, 8, 2, 13));

            //time = Chronic.parse("2012-08-02T13:00:00+01:00")
            //assert_equal Time.utc(2012, 8, 2, 12), time

            //time = Chronic.parse("2012-08-02T08:00:00-04:00")
            //assert_equal Time.utc(2012, 8, 2, 12), time

            //time = Chronic.parse("2013-08-01T19:30:00.345-07:00")
            //time2 = Time.parse("2013-08-01 019:30:00.345-07:00")
            //assert_in_delta time, time2, 0.001

            //time = Chronic.parse("2012-08-02T12:00:00Z")
            //assert_equal Time.utc(2012, 8, 2, 12), time

            //time = Chronic.parse("2012-01-03 01:00:00.100")
            //time2 = Time.parse("2012-01-03 01:00:00.100")
            //assert_in_delta time, time2, 0.001

            //time = Chronic.parse("2012-01-03 01:00:00.234567")
            //time2 = Time.parse("2012-01-03 01:00:00.234567")
            //assert_in_delta time, time2, 0.000001

            //assert_nil Chronic.parse("1/1/32.1")

            //time = Chronic.parse("28th", {:guess => :begin})
            //assert_equal Time.new(Time.now.year, Time.now.month, 28), time
        }

        [Fact]
        public void rmn_sd()
        {
            Parse("aug 3")
                .AssertEquals(Time.New(2007, 8, 3, 12));

            Parse("aug 3", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 3, 12));

            Parse("aug. 3")
                .AssertEquals(Time.New(2007, 8, 3, 12));

            Parse("aug 20")
                .AssertEquals(Time.New(2006, 8, 20, 12));

            Parse("aug-20")
                .AssertEquals(Time.New(2006, 8, 20, 12));

            Parse("aug 20", new { Context = Pointer.Type.Future })
                .AssertEquals(Time.New(2006, 8, 20, 12));

            Parse("may 27")
                .AssertEquals(Time.New(2007, 5, 27, 12));

            Parse("may 28", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 28, 12));

            Parse("may 28 5pm", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 28, 17));

            Parse("may 28 at 5pm", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 28, 17));

            Parse("may 28 at 5:32.19pm", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 28, 17, 32, 19));

            Parse("may 28 at 5:32:19.764")
                .AssertEquals(Time.New(2007, 5, 28, 17, 32, 19, 764000));
        }

        [Fact]
        public void rmn_sd_on()
        {
            Parse("5pm on may 28")
                .AssertEquals(Time.New(2007, 5, 28, 17));

            Parse("5pm may 28")
                .AssertEquals(Time.New(2007, 5, 28, 17));

            Parse("5 on may 28", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2007, 5, 28, 05));
        }

        [Fact]
        public void rmn_od()
        {
            Parse("may 27th")
                .AssertEquals(Time.New(2007, 5, 27, 12));

            Parse("may 27th", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 27, 12));

            Parse("may 27th 5:00 pm", new { Context = Pointer.Type.Past })
                    .AssertEquals(Time.New(2006, 5, 27, 17));

            Parse("may 27th at 5pm", new { Context = Pointer.Type.Past })
                    .AssertEquals(Time.New(2006, 5, 27, 17));

            Parse("may 27th at 5", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2007, 5, 27, 5));
        }

        [Fact]
        public void od_rm()
        {
            Parse("fifteenth of this month")
                .AssertEquals(Time.New(2007, 8, 15, 12));
        }

        [Fact]
        public void od_rmn()
        {
            Parse("22nd February")
                .AssertEquals(Time.New(2007, 2, 22, 12));

            Parse("31st of may at 6:30pm")
                .AssertEquals(Time.New(2007, 5, 31, 18, 30));

            Parse("11th december 8am")
                .AssertEquals(Time.New(2006, 12, 11, 8));
        }

        [Fact]
        public void sy_rmn_od()
        {
            Parse("2009 May 22nd")
                .AssertEquals(Time.New(2009, 05, 22, 12));
        }

        [Fact]
        public void sd_rmn()
        {
            Parse("22 February")
                .AssertEquals(Time.New(2007, 2, 22, 12));

            Parse("22 feb")
                .AssertEquals(Time.New(2007, 2, 22, 12));

            Parse("22-feb")
                .AssertEquals(Time.New(2007, 2, 22, 12));

            Parse("31 of may at 6:30pm")
                .AssertEquals(Time.New(2007, 5, 31, 18, 30));

            Parse("11 december 8am")
                .AssertEquals(Time.New(2006, 12, 11, 8));
        }

        [Fact]
        public void rmn_od_on()
        {
            Parse("5:00 pm may 27th", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 27, 17));

            Parse("05:00 pm may 27th", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 27, 17));

            Parse("5pm on may 27th", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 27, 17));

            Parse("5 on may 27th", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2007, 5, 27, 5));
        }

        [Fact]
        public void rmn_sy()
        {
            Parse("may 97")
                .AssertEquals(Time.New(1997, 5, 16, 12));

            Parse("may 33", new { AmbiguousYearFutureBias = 10 })
                .AssertEquals(Time.New(2033, 5, 16, 12));

            Parse("may 32")
                .AssertEquals(Time.New(2032, 5, 16, 12, 0, 0));
        }

        [Fact]
        public void rdn_rmn_sd_t_tz_sy()
        {
            Parse("Mon Apr 02 17:00:00 PDT 2007")
               .AssertEquals(1175558400);
        }

        [Fact]
        public void sy_sm_sd_t_tz()
        {
            Parse("2011-07-03 22:11:35 +0100")
               .AssertEquals(1309727495);

            Parse("2011-07-03 22:11:35 +01:00")
               .AssertEquals(1309727495);

            Parse("2011-07-03 16:11:35 -05:00")
                 .AssertEquals(1309727495);

            Parse("2011-07-03 21:11:35 UTC")
                 .AssertEquals(1309727495);

            Parse("2011-07-03 21:11:35.362 UTC")
                 .AssertEquals(1309727495);

            //time = parse_now("2011-07-03 21:11:35.362 UTC")
            //assert_in_delta 1309727495.362, time.to_f, 0.001
        }

        [Fact]
        public void rmn_sd_sy()
        {
            Parse("November 18, 2010")
                .AssertEquals(Time.New(2010, 11, 18, 12));

            Parse("Jan 1,2010")
                .AssertEquals(Time.New(2010, 1, 1, 12));

            Parse("February 14, 2004")
                .AssertEquals(Time.New(2004, 2, 14, 12));

            Parse("jan 3 2010")
                .AssertEquals(Time.New(2010, 1, 3, 12));

            Parse("jan 3 2010 midnight")
                .AssertEquals(Time.New(2010, 1, 4, 0));

            Parse("jan 3 2010 at midnight")
                .AssertEquals(Time.New(2010, 1, 4, 0));

            Parse("jan 3 2010 at 4", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2010, 1, 3, 4));

            Parse("may 27, 1979")
                .AssertEquals(Time.New(1979, 5, 27, 12));

            Parse("may 27 79")
                .AssertEquals(Time.New(1979, 5, 27, 12));

            Parse("may 27 79 4:30")
                .AssertEquals(Time.New(1979, 5, 27, 16, 30));

            Parse("may 27 79 at 4:30", new { AmbiguousTimeRange = 0 }).
                AssertEquals(Time.New(1979, 5, 27, 4, 30));

            Parse("may 27 32")
                .AssertEquals(Time.New(2032, 5, 27, 12, 0, 0));

            Parse("oct 5 2012 1045pm")
                .AssertEquals(Time.New(2012, 10, 5, 22, 45));
        }

        [Fact]
        public void rmn_od_sy()
        {
            Parse("may 1st 01")
                .AssertEquals(Time.New(2001, 5, 1, 12));

            Parse("November 18th 2010")
                .AssertEquals(Time.New(2010, 11, 18, 12));

            Parse("November 18th, 2010")
                .AssertEquals(Time.New(2010, 11, 18, 12));

            Parse("November 18th 2010 midnight")
                .AssertEquals(Time.New(2010, 11, 19, 0));

            Parse("November 18th 2010 at midnight")
                .AssertEquals(Time.New(2010, 11, 19, 0));

            Parse("November 18th 2010 at 4")
                .AssertEquals(Time.New(2010, 11, 18, 16));


            Parse("November 18th 2010 at 4", new { AmbiguousTimeRange = 0 }).
                AssertEquals(Time.New(2010, 11, 18, 4));

            Parse("March 30th, 1979")
                .AssertEquals(Time.New(1979, 3, 30, 12));

            Parse("March 30th 79")
                .AssertEquals(Time.New(1979, 3, 30, 12));

            Parse("March 30th 79 4:30")
                .AssertEquals(Time.New(1979, 3, 30, 16, 30));

            Parse("March 30th 79 at 4:30", new { AmbiguousTimeRange = 0 }).
                AssertEquals(Time.New(1979, 3, 30, 4, 30));
        }

        [Fact]
        public void od_rmn_sy()
        {
            Parse("22nd February 2012")
                .AssertEquals(Time.New(2012, 2, 22, 12));

            Parse("11th december 79")
                .AssertEquals(Time.New(1979, 12, 11, 12));
        }

        [Fact]
        public void sd_rmn_sy()
        {
            Parse("3 jan 2010")
                .AssertEquals(Time.New(2010, 1, 3, 12));

            Parse("3 jan 2010 4pm")
                .AssertEquals(Time.New(2010, 1, 3, 16));

            Parse("27 Oct 2006 7:30pm")
                .AssertEquals(Time.New(2006, 10, 27, 19, 30));

            Parse("3 jan 10")
                .AssertEquals(Time.New(2010, 1, 3, 12));

            Parse("3 jan 10", new { EndianPrecedence = EndianPrecedence.Little })
                .AssertEquals(Time.New(2010, 1, 3, 12));

            Parse("3 jan 10", new { EndianPrecedence = EndianPrecedence.Middle })
                .AssertEquals(Time.New(2010, 1, 3, 12));
        }

        [Fact]
        public void sm_sd_sy()
        {
            Parse("5/27/1979")
                .AssertEquals(Time.New(1979, 5, 27, 12));

            Parse("5/27/1979 4am")
                .AssertEquals(Time.New(1979, 5, 27, 4));

            Parse("7/12/11")
                .AssertEquals(Time.New(2011, 7, 12, 12));

            Parse("7/12/11", new { EndianPrecedence = EndianPrecedence.Little }).
                AssertEquals(Time.New(2011, 12, 7, 12));

            Parse("9/19/2011 6:05:57 PM")
                .AssertEquals(Time.New(2011, 9, 19, 18, 05, 57));

            // month day overflows
            Parse("30/2/2000").AssertIsNull();

            Parse("2013-03-12 17:00", new { Context = Pointer.Type.Past }).
                AssertEquals(Time.New(2013, 3, 12, 17, 0, 0));
        }

        [Fact]
        public void sd_sm_sy()
        {
            Parse("27/5/1979")
                .AssertEquals(Time.New(1979, 5, 27, 12));

            Parse("27/5/1979 @ 0700")
                .AssertEquals(Time.New(1979, 5, 27, 7));

            Parse("03/18/2012 09:26 pm")
                .AssertEquals(Time.New(2012, 3, 18, 21, 26));

            Parse("30.07.2013 16:34:22")
                .AssertEquals(Time.New(2013, 7, 30, 16, 34, 22));

            Parse("09.08.2013")
                .AssertEquals(Time.New(2013, 8, 9, 12));

            Parse("30-07-2013 21:53:49")
                .AssertEquals(Time.New(2013, 7, 30, 21, 53, 49));           
        }

        [Fact]
        public void sy_sm_sd()
        {
            Parse("2000-1-1")
                .AssertEquals(Time.New(2000, 1, 1, 12));

            Parse("2006-08-20")
                .AssertEquals(Time.New(2006, 8, 20, 12));

            Parse("2006-08-20 7pm")
                .AssertEquals(Time.New(2006, 8, 20, 19));

            Parse("2006-08-20 03:00")
                .AssertEquals(Time.New(2006, 8, 20, 3));

            Parse("2006-08-20 03:30:30")
                .AssertEquals(Time.New(2006, 8, 20, 3, 30, 30));

            Parse("2006-08-20 15:30:30")
                .AssertEquals(Time.New(2006, 8, 20, 15, 30, 30));

            Parse("2006-08-20 15:30.30")
                .AssertEquals(Time.New(2006, 8, 20, 15, 30, 30));

            Parse("2006-08-20 15:30:30:000536")
                .AssertEquals(Time.New(2006, 8, 20, 15, 30, 30, 536));

            Parse("1902-08-20")
                .AssertEquals(Time.New(1902, 8, 20, 12, 0, 0));

            Parse("2013.07.30 11:45:23")
                .AssertEquals(Time.New(2013, 7, 30, 11, 45, 23));

            Parse("2013.08.09")
                .AssertEquals(Time.New(2013, 8, 9, 12, 0, 0));

            Parse("2012:05:25 22:06:50")
                .AssertEquals(Time.New(2012, 5, 25, 22, 6, 50));
        }

        [Fact]
        public void sm_sd()
        {
            Parse("05/06")
                .AssertEquals(Time.New(2007, 5, 6, 12));

            Parse("05/06")
                .AssertEquals(Time.New(2007, 6, 5, 12));
            //time = parse_now("05/06", :endian_precedence => [:little, :medium])
            //assert_equal Time.local(2007, 6, 5, 12), time

            Parse("05/06 6:05:57 PM")
                .AssertEquals(Time.New(2007, 5, 6, 18, 05, 57));

            Parse("05/06 6:05:57 PM")
                .AssertEquals(Time.New(2007, 5, 6, 18, 05, 57));
            //time = parse_now("05/06 6:05:57 PM", :endian_precedence => [:little, :medium])
            //assert_equal Time.local(2007, 6, 5, 18, 05, 57), time

            Parse("13/09")
                .AssertEquals(Time.New(2006, 9, 13, 12));

            Parse("05/06", new { Context = Pointer.Type.Future })
                .AssertEquals(Time.New(2007, 5, 6, 12));

            Parse("1/13", new { Context = Pointer.Type.Future })
                .AssertEquals(Time.New(2007, 1, 13, 12));

            Parse("3/13", new { Context = Pointer.Type.None })
                .AssertEquals(Time.New(2006, 3, 13, 12));
        }

        //[Fact]
        //public void sm_sy()
        //{
        //    Parse("05/06")
        //        .AssertEquals(Time.New(2006, 5, 16, 12));

        //    Parse("12/06")
        //        .AssertEquals(Time.New(2006, 12, 16, 12));

        //    Parse("13/06").AssertIsNull();
        //}

        [Fact]
        public void sy_sm()
        {
            Parse("2012-06")
                .AssertEquals(Time.New(2012, 06, 16));

            Parse("2013/12")
                .AssertEquals(Time.New(2013, 12, 16, 12, 0));           
        }

        [Fact]
        public void r()
        {
            Parse("9am on Saturday")
                .AssertEquals(Time.New(2006, 8, 19, 9));

            Parse("on Tuesday")
                .AssertEquals(Time.New(2006, 8, 22, 12));

            Parse("1:00:00 PM")
                .AssertEquals(Time.New(2006, 8, 16, 13));

            Parse("01:00:00 PM")
                .AssertEquals(Time.New(2006, 8, 16, 13));

            Parse("today at 02:00:00", new { Hours24 = false })
                .AssertEquals(Time.New(2006, 8, 16, 14));

            Parse("today at 02:00:00 AM", new { Hours24 = false })
                .AssertEquals(Time.New(2006, 8, 16, 2));

            Parse("today at 3:00:00", new { Hours24 = true })
                .AssertEquals(Time.New(2006, 8, 16, 3));

            Parse("today at 03:00:00", new { Hours24 = true })
                .AssertEquals(Time.New(2006, 8, 16, 3));

            Parse("tomorrow at 4a.m.")
                .AssertEquals(Time.New(2006, 8, 17, 4));
        }

        [Fact]
        public void r_g_r()
        {
        }

        [Fact]
        public void srp()
        {
        }

        [Fact]
        public void s_r_p()
        {
        }

        [Fact]
        public void p_s_r()
        {
        }

        [Fact]
        public void s_r_p_a()
        {
            var time1 = Parse("two days ago 0:0:0am");
            var time2 = Parse("two days ago 00:00:00am");

            Assert.Equal(time1, time2);
        }
    
        [Fact]
        public void orr()
        {
            Parse("5th tuesday in january")
                .AssertEquals(Time.New(2007, 01, 30, 12));

            Parse("5th tuesday in february")
                .AssertIsNull();

            //% W(jan feb march april may june july aug sep oct nov dec).each_with_index do | month, index |
            //    time = parse_now("5th tuesday in #{month}")

            //  if time then
            //    assert_equal time.month, index + 1
            //  end
            //end

            var months = new List<string> { "jan", "feb", "march", "april", "may", "june", "july", "aug", "sep", "oct", "nov", "dec" };
            for (var i = 0; i < months.Count; i++)
            {
                var time = Parse($"5th tuesday in {months[i]}");
                
                if(time != null)
                {
                    Assert.Equal(i + 1, time.Start.Value.Month);
                }
            }
        }

        [Fact]
        public void o_r_s_r()
        {
            Parse("3rd wednesday in november").
                AssertEquals(Time.New(2006, 11, 15, 12));

            Parse("10th wednesday in november").AssertIsNull();

            // Parse("3rd wednesday in 2007").AssertEquals(
            // Time.New(2007, 1, 20, 12));
        }

        [Fact]
        public void o_r_g_r()
        {
            Parse("3rd month next year")
                .AssertStartsAt(Time.New(2007, 3));

            Parse("3rd month next year")
                .AssertStartsAt(Time.New(2007, 3, 1));

            Parse("3rd thursday this september")
                .AssertEquals(Time.New(2006, 9, 21, 12));

            var now = new DateTime(2010, 2, 1);
            Parse("3rd thursday this november", new Options { Clock = () => now })
                .AssertEquals(Time.New(2010, 11, 18, 12));

            Parse("4th day last week")
                .AssertEquals(Time.New(2006, 8, 9, 12));
        }        

        [Fact]
        public void sm_rmn_sy()
        {
            Parse("30-Mar-11")
                .AssertEquals(Time.New(2011, 3, 30, 12));

            Parse("31-Aug-12")
                .AssertEquals(Time.New(2012, 8, 31));
        }

        // end of testing handlers
        
        [Fact]                
        public void guess_r()
        {
            Parse("friday")
                .AssertEquals(Time.New(2006, 8, 18, 12));   

            Parse("tue")
                .AssertEquals(Time.New(2006, 8, 22, 12));

            Parse("5")
                .AssertEquals(Time.New(2006, 8, 16, 17));

            Parse("5", new { Clock = Time.New(2006, 8, 16, 3, 0, 0, 0), AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2006, 8, 16, 5));

            Parse("13:00")
                .AssertEquals(Time.New(2006, 8, 17, 13));

            Parse("13:45")
                .AssertEquals(Time.New(2006, 8, 17, 13, 45));

            Parse("1:01pm")
                .AssertEquals(Time.New(2006, 8, 16, 13, 01));

            Parse("2:01pm")
                .AssertEquals(Time.New(2006, 8, 16, 14, 01));

            Parse("november")
                .AssertEquals(Time.New(2006, 11, 16));
        }

        [Fact]
        public void guess_rr()
        {
            Parse("friday 13:00")
                .AssertEquals(Time.New(2006, 8, 18, 13));

            Parse("monday 4:00")
                .AssertEquals(Time.New(2006, 8, 21, 16));

            Parse("sat 4:00", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2006, 8, 19, 4));

            Parse("sunday 4:20", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2006, 8, 20, 4, 20));

            Parse("4 pm")
                .AssertEquals(Time.New(2006, 8, 16, 16));

            Parse("4 am", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2006, 8, 16, 4));

            Parse("12 pm")
                .AssertEquals(Time.New(2006, 8, 16, 12));

            Parse("12:01 pm")
                .AssertEquals(Time.New(2006, 8, 16, 12, 1));

            Parse("12:01 am")
                .AssertEquals(Time.New(2006, 8, 16, 0, 1));

            Parse("12 am")
                .AssertEquals(Time.New(2006, 8, 16));

            Parse("4:00 in the morning")
                .AssertEquals(Time.New(2006, 8, 16, 4));

            Parse("0:10")
                .AssertEquals(Time.New(2006, 8, 17, 0, 10));

            Parse("november 4")
                .AssertEquals(Time.New(2006, 11, 4, 12));

            Parse("aug 24")
                .AssertEquals(Time.New(2006, 8, 24, 12));
        }

        [Fact]
        public void guess_rrr()
        {
            Parse("friday 1 pm")
                .AssertEquals(Time.New(2006, 8, 18, 13));

            Parse("friday 11 at night")
                .AssertEquals(Time.New(2006, 8, 18, 23));

            Parse("friday 11 in the evening")
                .AssertEquals(Time.New(2006, 8, 18, 23));

            Parse("sunday 6am")
                .AssertEquals(Time.New(2006, 8, 20, 6));

            Parse("friday evening at 7")
                .AssertEquals(Time.New(2006, 8, 18, 19));
        }

        [Fact]
        public void guess_gr()
        {
            // year
            Parse("this year", new { Guess = false })
                .AssertEquals(Time.New(2006, 8, 17));

            Parse("this year", new { Context = Pointer.Type.Past, Guess = false })
                .AssertEquals(Time.New(2006, 1, 1));

            // month
            Parse("this month")
                .AssertEquals(Time.New(2006, 8, 24, 12));

            Parse("this month", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 8, 12));

            Parse("next month", new { Clock = Time.New(2006, 11, 15) })
                .AssertEquals(Time.New(2006, 12, 16, 12));

            // month name
            Parse("last november")
                .AssertEquals(Time.New(2005, 11, 16));

            // fortnight
            Parse("this fortnight")
                .AssertEquals(Time.New(2006, 8, 21, 19, 30));

            Parse("this fortnight", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 14, 19));

            // week

            Parse("this week")
                .AssertEquals(Time.New(2006, 8, 18, 7, 30));

            Parse("this week", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 14, 19));

            //Parse("this week", :context => :past, :guess => :begin)
            //.AssertEquals(Time.New(2006, 8, 13));

            //Parse("this week", :context => :past, :guess => :begin, :week_start => :monday)
            //.AssertEquals(Time.New(2006, 8, 14));

            // weekend

            Parse("this weekend")
                .AssertEquals(Time.New(2006, 8, 20));

            Parse("this weekend", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 13));

            Parse("last weekend")
                .AssertEquals(Time.New(2006, 8, 13));

            // day

            Parse("this day")
                .AssertEquals(Time.New(2006, 8, 16, 19));

            Parse("this day", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 16, 7));

            Parse("today")
                .AssertEquals(Time.New(2006, 8, 16, 19));

            Parse("yesterday")
                .AssertEquals(Time.New(2006, 8, 15, 12));

            Parse("yesterday", new { Clock = Parse("2011-05-27 23:10") }) // after 11pm
                .AssertEquals(Time.New(2011, 05, 26, 12));

            Parse("tomorrow")
            .AssertEquals(Time.New(2006, 8, 17, 12));

            // day name

            Parse("this tuesday")
                .AssertEquals(Time.New(2006, 8, 22, 12));

            Parse("next tuesday")
                .AssertEquals(Time.New(2006, 8, 22, 12));

            Parse("last tuesday")
                .AssertEquals(Time.New(2006, 8, 15, 12));

            Parse("this wed")
                .AssertEquals(Time.New(2006, 8, 23, 12));

            Parse("next wed")
                .AssertEquals(Time.New(2006, 8, 23, 12));

            Parse("last wed")
                .AssertEquals(Time.New(2006, 8, 9, 12));

            var monday = Time.New(2006, 8, 21, 12);
            Assert.Equal(monday, Parse("mon").Start.Value);
            Assert.Equal(monday, Parse("mun").Start.Value);

            var tuesday = Time.New(2006, 8, 22, 12);
            Assert.Equal(tuesday, Parse("tue").Start.Value);
            Assert.Equal(tuesday, Parse("tus").Start.Value);

            var wednesday = Time.New(2006, 8, 23, 12);
            Assert.Equal(wednesday, Parse("wed").Start.Value);
            Assert.Equal(wednesday, Parse("wenns").Start.Value);

            var thursday = Time.New(2006, 8, 17, 12);
            Assert.Equal(thursday, Parse("thu").Start.Value);
            Assert.Equal(thursday, Parse("thur").Start.Value);

            var friday = Time.New(2006, 8, 18, 12);
            Assert.Equal(friday, Parse("fri").Start.Value);
            Assert.Equal(friday, Parse("fry").Start.Value);

            var saturday = Time.New(2006, 8, 19, 12);
            Assert.Equal(saturday, Parse("sat").Start.Value);
            Assert.Equal(saturday, Parse("satterday").Start.Value);

            var sunday = Time.New(2006, 8, 20, 12);
            Assert.Equal(sunday, Parse("sun").Start.Value);
            Assert.Equal(sunday, Parse("sum").Start.Value);

            // day portion

            Parse("this morning")
                .AssertEquals(Time.New(2006, 8, 16, 9));

            Parse("tonight")
                .AssertEquals(Time.New(2006, 8, 16, 22));

            // hour

            Parse("next hr")
                .AssertEquals(Time.New(2006, 8, 16, 15, 30, 0));

            Parse("next hrs")
                .AssertEquals(Time.New(2006, 8, 16, 15, 30, 0));

            // minute

            Parse("next min")
                .AssertEquals(Time.New(2006, 8, 16, 14, 1, 30));

            Parse("next mins")
                .AssertEquals(Time.New(2006, 8, 16, 14, 1, 30));

            Parse("next minute")
                .AssertEquals(Time.New(2006, 8, 16, 14, 1, 30));

            // second

            Parse("next sec")
                .AssertEquals(Time.New(2006, 8, 16, 14, 0, 1));

            Parse("next secs")
                .AssertEquals(Time.New(2006, 8, 16, 14, 0, 1));

            Parse("this second")
                .AssertEquals(Time.New(2006, 8, 16, 14));

            Parse("this second", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 16, 14));

            Parse("next second")
                .AssertEquals(Time.New(2006, 8, 16, 14, 0, 1));

            Parse("last second")
                .AssertEquals(Time.New(2006, 8, 16, 13, 59, 59));
        }

        [Fact]
        public void guess_grr()
        {
            Parse("yesterday at 4:00")
                .AssertEquals(Time.New(2006, 8, 15, 16));

            Parse("today at 9:00")
                .AssertEquals(Time.New(2006, 8, 16, 9));

            Parse("today at 2100")
                .AssertEquals(Time.New(2006, 8, 16, 21));

            Parse("this day at 0900")
                .AssertEquals(Time.New(2006, 8, 16, 9));

            Parse("tomorrow at 0900")
                .AssertEquals(Time.New(2006, 8, 17, 9));

            Parse("yesterday at 4:00", new { AmbiguousTimeRange = 0 })
                .AssertEquals(Time.New(2006, 8, 15, 4));

            Parse("last friday at 4:00")
                .AssertEquals(Time.New(2006, 8, 11, 16));

            Parse("next wed 4:00")
                .AssertEquals(Time.New(2006, 8, 23, 16));

            Parse("yesterday afternoon")
                .AssertEquals(Time.New(2006, 8, 15, 15));

            Parse("last week tuesday")
                .AssertEquals(Time.New(2006, 8, 8, 12));

            Parse("tonight at 7")
                .AssertEquals(Time.New(2006, 8, 16, 19));

            Parse("tonight 7")
                .AssertEquals(Time.New(2006, 8, 16, 19));

            Parse("7 tonight")
                .AssertEquals(Time.New(2006, 8, 16, 19));
        }

        [Fact]
        public void guess_grrr()
        {
            Parse("today at 6:00pm")
                .AssertEquals(Time.New(2006, 8, 16, 18));

            Parse("today at 6:00am")
                .AssertEquals(Time.New(2006, 8, 16, 6));

            Parse("this day 1800")
                .AssertEquals(Time.New(2006, 8, 16, 18));

            Parse("yesterday at 4:00pm")
                .AssertEquals(Time.New(2006, 8, 15, 16));

            Parse("tomorrow evening at 7")
                .AssertEquals(Time.New(2006, 8, 17, 19));

            Parse("tomorrow morning at 5:30")
                .AssertEquals(Time.New(2006, 8, 17, 5, 30));

            Parse("next monday at 12:01 am")
                .AssertEquals(Time.New(2006, 8, 21, 00, 1));

            Parse("next monday at 12:01 pm")
                .AssertEquals(Time.New(2006, 8, 21, 12, 1));

            // with context
            Parse("sunday at 8:15pm", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 13, 20, 15));
        }

        [Fact]
        public void guess_rgr()
        {
            Parse("afternoon yesterday")
                .AssertEquals(Time.New(2006, 8, 15, 15));

            Parse("tuesday last week")
                .AssertEquals(Time.New(2006, 8, 8, 12));
        }

        [Fact]
        public void guess_a_ago()
        {
            Parse("AN hour ago")
                .AssertEquals(Time.New(2006, 8, 16, 13));

            Parse("A day ago")
                .AssertEquals(Time.New(2006, 8, 15, 14));

            Parse("a month ago")
                .AssertEquals(Time.New(2006, 7, 16, 14));

            Parse("a year ago")
                .AssertEquals(Time.New(2005, 8, 16, 14));
        }

        [Fact]
        public void guess_s_r_p()
        {
            //past
            Parse("3 years ago")
                .AssertEquals(Time.New(2003, 8, 16, 14));

            Parse("1 month ago")
                .AssertEquals(Time.New(2006, 7, 16, 14));

            Parse("1 fortnight ago")
                .AssertEquals(Time.New(2006, 8, 2, 14));

            Parse("2 fortnights ago")
                .AssertEquals(Time.New(2006, 7, 19, 14));

            Parse("3 weeks ago")
                .AssertEquals(Time.New(2006, 7, 26, 14));

            Parse("2 weekends ago")
                .AssertEquals(Time.New(2006, 8, 5));

            Parse("3 days ago")
                .AssertEquals(Time.New(2006, 8, 13, 14));

            //time = parse_now("1 monday ago")
            //assert_equal Time.local(2006, 8, 14, 12), time

            Parse("5 mornings ago")
                .AssertEquals(Time.New(2006, 8, 12, 9));

            Parse("7 hours ago")
                .AssertEquals(Time.New(2006, 8, 16, 7));

            Parse("3 minutes ago")
                .AssertEquals(Time.New(2006, 8, 16, 13, 57));

            Parse("20 seconds before now")
                .AssertEquals(Time.New(2006, 8, 16, 13, 59, 40));

            //future
            Parse("3 years from now")
                .AssertEquals(Time.New(2009, 8, 16, 14, 0, 0));

            Parse("6 months hence")
                .AssertEquals(Time.New(2007, 2, 16, 14));

            Parse("3 fortnights hence")
                .AssertEquals(Time.New(2006, 9, 27, 14));

            Parse("1 week from now")
                .AssertEquals(Time.New(2006, 8, 23, 14, 0, 0));

            Parse("1 weekend from now")
                .AssertEquals(Time.New(2006, 8, 19));

            Parse("2 weekends from now")
                .AssertEquals(Time.New(2006, 8, 26));

            Parse("1 day hence")
                .AssertEquals(Time.New(2006, 8, 17, 14));

            Parse("5 mornings hence")
                .AssertEquals(Time.New(2006, 8, 21, 9));

            Parse("1 hour from now")
                .AssertEquals(Time.New(2006, 8, 16, 15));

            Parse("20 minutes hence")
                .AssertEquals(Time.New(2006, 8, 16, 14, 20));

            Parse("20 seconds from now")
                .AssertEquals(Time.New(2006, 8, 16, 14, 0, 20));

            Parse("2 months ago", new { Clock = DateTime.Parse("2007-03-07 23:30") })
                .AssertEquals(Time.New(2007, 1, 7, 23, 30));

            //Two repeaters
            Parse("25 minutes and 20 seconds from now")
                .AssertEquals(Time.New(2006, 8, 16, 14, 25, 20));

            Parse("24 hours and 20 minutes from now")
                .AssertEquals(Time.New(2006, 8, 17, 14, 20, 0));

            Parse("24 hours 20 minutes from now")
                .AssertEquals(Time.New(2006, 8, 17, 14, 20, 0));
        }

        [Fact]
        public void guess_p_s_r()
        {
            Parse("in 3 hours")
                .AssertEquals(Time.New(2006, 8, 16, 17));
        }

        [Fact]
        public void guess_s_r_p_a()
        {
            //past

            Parse("3 years ago tomorrow")
                .AssertEquals(Time.New(2003, 8, 17, 12));

            Parse("3 years ago this friday")
                .AssertEquals(Time.New(2003, 8, 18, 12));

            Parse("3 months ago saturday at 5:00 pm")
                .AssertEquals(Time.New(2006, 5, 19, 17));

            Parse("2 days from this second")
                .AssertEquals(Time.New(2006, 8, 18, 14));

            Parse("7 hours before tomorrow at midnight")
                .AssertEquals(Time.New(2006, 8, 17, 17));

            //future
        }

        [Fact]
        public void guess_o_r_g_r()
        {
            Parse("3rd month next year", new { Guess = false })
                .AssertEquals(Time.New(2007, 3));

            Parse("3rd month next year", new { Guess = false })
                .AssertEquals(Time.New(2007, 3, 1));

            Parse("3rd thursday this september")
                .AssertEquals(Time.New(2006, 9, 21, 12));

            Parse("3rd thursday this november", new { Clock = DateTime.Parse("1/10/2010") })
                .AssertEquals(Time.New(2010, 11, 18, 12));

            Parse("4th day last week")
                .AssertEquals(Time.New(2006, 8, 9, 12));
        }

        [Fact]
        public void guess_nonsense()
        {
            Parse("some stupid nonsense").AssertIsNull();

            Parse("Ham Sandwich").AssertIsNull();
        }

        [Fact]
        public void span()
        {
            var span = Parse("friday", new { Guess = false });
            Assert.Equal(Time.New(2006, 8, 18), span.Start.Value);
            Assert.Equal(Time.New(2006, 8, 19), span.End.Value);

            span = Parse("november", new { Guess = false });
            Assert.Equal(Time.New(2006, 11), span.Start.Value);
            Assert.Equal(Time.New(2006, 12), span.End.Value);

            span = Parse("weekend", new { Clock = Time.New(2006, 8, 16, 14), Guess = false });
            Assert.Equal(Time.New(2006, 8, 19), span.Start.Value);
            Assert.Equal(Time.New(2006, 8, 21), span.End.Value);
        }

        [Fact]
        public void with_endian_precedence()
        {

        }

        [Fact]
        public void words()
        {
            Assert.Equal(Parse("33 days from now"), Parse("thirty-three days from now"));
            Assert.Equal(Parse("2867532 seconds from now"), Parse("two million eight hundred and sixty seven thousand five hundred and thirty two seconds from now"));
            Assert.Equal(Parse("may 10th"), Parse("may tenth"));
            Assert.Equal(Parse("second monday in january"), Parse("2nd monday in january"));
        }

        [Fact]
        public void relative_to_an_hour_before()
        {
            // example prenormalization "10 to 2" becomes "10 minutes past 2"
            Assert.Equal(Time.New(2006, 8, 16, 13, 50), Parse("10 to 2").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 13, 50), Parse("10 till 2").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 13, 50), Parse("10 prior to 2").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 13, 50), Parse("10 before 2").Start.Value);

            // uses the current hour, so 2006-08-16 13:50:00, not 14:50
            Assert.Equal(Time.New(2006, 8, 16, 13, 50), Parse("10 to").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 13, 50), Parse("10 till").Start.Value);

            Assert.Equal(Time.New(2006, 8, 16, 15, 45), Parse("quarter to 4").Start.Value);
        }

        [Fact]
        public void relative_to_an_hour_after()
        {
            // not nil
            Assert.Equal(Time.New(2006, 8, 16, 14, 10), Parse("10 after 2").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 14, 10), Parse("10 past 2").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 14, 30), Parse("half past 2").Start.Value);
        }

        [Fact]
        public void only_complete_pointers()
        {
            Assert.Equal(Parse("eat pasty buns today at 2pm").Start.Value, Time.New(2006, 8, 16, 14));
            Assert.Equal(Parse("futuristically speaking today at 2pm").Start.Value, Time.New(2006, 8, 16, 14));
            Assert.Equal(Parse("meeting today at 2pm").Start.Value, Time.New(2006, 8, 16, 14));
        }

        [Fact]
        public void am_pm()
        {
            Assert.Equal(Time.New(2006, 8, 16), Parse("8/16/2006 at 12am").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 12), Parse("8/16/2006 at 12pm").Start.Value);
        }

        [Fact]
        public void a_p()
        {
            Assert.Equal(Time.New(2006, 8, 16, 0, 15), Parse("8/16/2006 at 12:15a").Start.Value);
            Assert.Equal(Time.New(2006, 8, 16, 18, 30), Parse("8/16/2006 at 6:30p").Start.Value);
        }

        [Fact]
        public void seasons()
        {
            var t = Parse("this spring", new { Guess = false });
            Assert.Equal(Time.New(2007, 3, 20), t.Start.Value);
            Assert.Equal(Time.New(2007, 6, 20), t.End.Value);


            t = Parse("this winter", new { Guess = false });
            Assert.Equal(Time.New(2006, 12, 22), t.Start.Value);
            Assert.Equal(Time.New(2007, 3, 19), t.End.Value);


            t = Parse("last spring", new { Guess = false });
            Assert.Equal(Time.New(2006, 3, 20), t.Start.Value);
            Assert.Equal(Time.New(2006, 6, 20), t.End.Value);


            t = Parse("last winter", new { Guess = false });
            Assert.Equal(Time.New(2005, 12, 22), t.Start.Value);
            Assert.Equal(Time.New(2006, 3, 19), t.End.Value);


            t = Parse("next spring", new { Guess = false });
            Assert.Equal(Time.New(2007, 3, 20), t.Start.Value);
            Assert.Equal(Time.New(2007, 6, 20), t.End.Value);
        }

        [Fact]
        public void days_in_november()
        {
            var t1 = Parse("1st thursday in november", new { Clock = Time.New(2007) });
            Assert.Equal(Time.New(2007, 11, 1, 12), t1.Start.Value);

            t1 = Parse("1st friday in november", new { Clock = Time.New(2007) });
            Assert.Equal(Time.New(2007, 11, 2, 12), t1.Start.Value);

            t1 = Parse("1st saturday in november", new { Clock = Time.New(2007) });
            Assert.Equal(Time.New(2007, 11, 3, 12), t1.Start.Value);
        }

        [Fact]
        public void now_changes()
        {
            var t1 = Parse("now");
            Thread.Sleep(1);
            var t2 = Parse("now");
            Assert.NotEqual(t1, t2);
        }

        [Fact]
        public void noon()
        {
            var t1 = Parse("2011-01-01 at noon", new { AmbiguousTimeRange = 0 });
            Assert.Equal(Time.New(2011, 1, 1, 12, 0), t1.Start.Value);
        }

        [Fact]
        public void rdn_rmn_sd()
        {
            Parse("Thu Aug 10")
                .AssertEquals(Time.New(2006, 8, 10, 12));

            Parse("Thursday July 31")
                .AssertEquals(Time.New(2006, 7, 31, 12));

            Parse("Thursday December 31")
                .AssertEquals(Time.New(2006, 12, 31, 12));
        }

        [Fact]
        public void rdn_rmn_sd_rt()
        {
            Parse("Thu Aug 10 4pm")
                .AssertEquals(Time.New(2006, 8, 10, 16));

            Parse("Thu Aug 10 at 4pm")
                .AssertEquals(Time.New(2006, 8, 10, 16));
        }


        [Fact]
        public void rdn_rmn_od_rt()
        {
            Parse("Thu Aug 10th at 4pm")
                .AssertEquals(Time.New(2006, 8, 10, 16));
        }

        [Fact]
        public void rdn_od_rt()
        {
            Parse("Thu 17th at 4pm")
                .AssertEquals(Time.New(2006, 8, 17, 16));

            Parse("Thu 16th at 4pm")
                .AssertEquals(Time.New(2006, 8, 16, 16));

            Parse("Thu 1st at 4pm")
                .AssertEquals(Time.New(2006, 9, 1, 16));

            Parse("Thu 1st at 4pm", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 8, 1, 16));
        }

        [Fact]
        public void rdn_od()
        {
            Parse("Thu 17th")
                .AssertEquals(Time.New(2006, 8, 17, 12));
        }

        [Fact]
        public void rdn_rmn_sd_sy()
        {
            Parse("Thu Aug 10 2006")
                .AssertEquals(Time.New(2006, 8, 10, 12));

            Parse("Thursday July 31 2006")
                .AssertEquals(Time.New(2006, 7, 31, 12));

            Parse("Thursday December 31 2006")
                .AssertEquals(Time.New(2006, 12, 31, 12));

            Parse("Thursday December 30 2006")
                .AssertEquals(Time.New(2006, 12, 30, 12));
        }

        [Fact]
        public void rdn_rmn_od()
        {
            Parse("Thu Aug 10th")
                .AssertEquals(Time.New(2006, 8, 10, 12));

            Parse("Thursday July 31st")
                .AssertEquals(Time.New(2006, 7, 31, 12));

            Parse("Thursday December 31st")
                .AssertEquals(Time.New(2006, 12, 31, 12));
        }

        [Fact]
        public void rdn_rmn_od_sy()
        {
            Parse("Thu Aug 10th 2005")
                .AssertEquals(Time.New(2005, 8, 10, 12));

            Parse("Thursday July 31st 2005")
                .AssertEquals(Time.New(2005, 7, 31, 12));

            Parse("Thursday December 31st 2005")
                .AssertEquals(Time.New(2005, 12, 31, 12));

            Parse("Thursday December 30th 2005")
                .AssertEquals(Time.New(2005, 12, 30, 12));
        }

        [Fact]
        public void normalizing_day_portions()
        {

        }
       
    }
}