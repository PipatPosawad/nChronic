﻿using Chronic.Handlers;
using Chronic.Tags.Repeaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic
{
    public class Definitions
    {
        public Options options;
        public Definitions(Options options)
        {
            this.options = options;
        }

        public virtual void DefineHandlers()
        {
            new Exception($"definitions are set in subclasses of #{GetType().Name}");
        }
    }

    public class SpanDefinitions : Definitions
    {
        public HandlerRegistry handlers { get; set; }
        public SpanDefinitions(Options options) : base(options)
        {
        }
    }

    public class TimeDefinitions : SpanDefinitions
    {
        public TimeDefinitions(Options options) : base(options)
        {
        }

        public override void DefineHandlers()
        {
            var timeHandlers = new List<ComplexHandler>()
                {
                    Handle
                        .Required<RepeaterTime>()
                        .Optional<IRepeaterDayPortion>()
                        .UsingNothing(),
                };
            handlers.Add(HandlerType.Time, timeHandlers);
        }
    }

    public class DateDefinitions : SpanDefinitions
    {
        public DateDefinitions(Options options) : base(options)
        {
        }

        public override void DefineHandlers()
        {
            var dateHandlers = new List<ComplexHandler>()
                {
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Required<RepeaterTime>()
                        .Optional<SeparatorSlash>()
                        .Optional<SeparatorDash>()
                        .Required<TimeZone>()
                        .Required<ScalarYear>()
                        .Using<GenericHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Using<RdnRmnSdHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Required<ScalarYear>()
                        .Using<RdnRmnSdSyHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Using<RdnRmnOdHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Required<ScalarYear>()
                        .Using<RdnRmnOdSyHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RdnRmnSdHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RdnRmnOdHandler>(),
                    Handle
                        .Required<RepeaterDayName>()
                        .Required<OrdinalDay>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RdnOdHandler>(),
                    Handle
                        .Required<ScalarYear>()
                        .Optional<SeparatorSlash>()
                        .Optional<SeparatorDash>()
                        .Required<ScalarMonth>()
                        .Optional<SeparatorSlash>()
                        .Optional<SeparatorDash>()
                        .Required<ScalarDay>()
                        .Required<RepeaterTime>()
                        .Required<TimeZone>()
                        .Using<GenericHandler>(),
                    Handle
                        .Required<OrdinalDay>()
                        .Using<GenericHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Optional<SeparatorComma>()
                        .Required<ScalarYear>()
                        .Using<RmnSdSyHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Optional<SeparatorComma>()
                        .Required<ScalarYear>()
                        .Using<RmnOdSyHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RmnSdSyHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RmnOdSyHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Repeat(pattern => pattern
                            .Optional<SeparatorSlash>()
                            .Optional<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarDay>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RmnSdHandler>(),
                    Handle
                        .Required<RepeaterTime>()
                        .Optional<IRepeaterDayPortion>()
                        .Optional<SeparatorOn>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Using<RmnSdOnHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RmnOdHandler>(),
                    Handle
                        .Required<OrdinalDay>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<OdRmnSyHandler>(),
                    Handle
                        .Required<OrdinalDay>()
                        .Required<RepeaterMonthName>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<OdRmnHandler>(),

                    Handle
                        .Required<OrdinalDay>()
                        .Optional<Grabber>()
                        .Required<RepeaterMonthName>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<OdRmHandler>(),

                    Handle
                        .Required<ScalarYear>()
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Using<SyRmnOdHandler>(),
                    Handle
                        .Required<RepeaterTime>()
                        .Optional<IRepeaterDayPortion>()
                        .Optional<SeparatorOn>()
                        .Required<RepeaterMonthName>()
                        .Required<OrdinalDay>()
                        .Using<RmnOdOnHandler>(),


                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<ScalarDay>()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<RmnSdSyHandler>(),
                    Handle
                        .Required<RepeaterMonthName>()
                        .Required<ScalarYear>()
                        .Using<RmnSyHandler>(),

                    Handle
                        .Required<ScalarDay>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SdRmnSyHandler>(),
                    Handle
                        .Required<ScalarDay>()
                        .Repeat(pattern => pattern
                            .Optional<SeparatorSlash>()
                            .Optional<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<RepeaterMonthName>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SdRmnHandler>(),
                    Handle

                        .Required<ScalarYear>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarMonth>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarDay>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SySmSdHandler>(),

                    Handle
                        .Required<ScalarYear>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarMonth>()
                        .Using<SySmHandler>(),

                    Handle
                        .Required<ScalarMonth>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarYear>()
                        .Using<SmSyHandler>(),

                    Handle
                        .Required<ScalarYear>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<RepeaterMonthName>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarYear>()
                        .Optional<RepeaterTime>()
                        .Using<SmRmnSyHandler>(),

                    Handle
                        .Required<ScalarYear>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarMonth>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Optional<Scalar>()
                        .Required<TimeZone>()
                        .Using<GenericHandler>()


                    //Handle
                    //    .Required<Scalar>()
                    //    .Required<IRepeater>()
                    //    .Optional<SeparatorComma>()
                    //    .Required<Pointer>()
                    //    .Required(HandlerType.Anchor)
                    //    .Required<SeparatorAt>()
                    //    .Required(HandlerType.Time)
                    //    .Using<SRPAHandler>(),

                    //Handle
                    //    .Repeat(pattern => pattern                        
                    //        .Required<Scalar>()
                    //        .Required<IRepeater>()
                    //        .Optional<SeparatorComma>() 
                    //    ).AnyNumberOfTimes()
                    //    .Required<Pointer>()
                    //    .Optional(HandlerType.Anchor)
                    //    .Optional<SeparatorAt>()
                    //    .Optional(HandlerType.Time)
                    //    .Using<MultiSRHandler>(),                    
                        
                    //Handle
                    //    .Required<ScalarMonth>()
                    //    .Required<SeparatorDate>()
                    //    .Required<ScalarDay>()
                    //    .Using<SmSdHandler>(),

                    //Handle
                    //    .Required<ScalarMonth>()    
                    //    .Required<SeparatorDate>()                        
                    //    .Required<ScalarDay>()
                    //    .Required<SeparatorDate>()     
                    //    .Required<ScalarYear>()
                    //    .Optional<SeparatorAt>()
                    //    .Optional(HandlerType.Time)
                    //    .Using<SmSdSyHandler>(),
                    //Handle
                    //    .Required<ScalarDay>()
                    //    .Required<SeparatorDate>()                        
                    //    .Required<ScalarMonth>()    
                    //    .Required<SeparatorDate>()     
                    //    .Required<ScalarYear>()
                    //    .Optional<SeparatorAt>()
                    //    .Optional(HandlerType.Time)
                    //    .Using<SdSmSyHandler>(),




                };

            handlers.Add(HandlerType.Date, dateHandlers);
        }
    }

    public class AnchorDefinitions : SpanDefinitions
    {
        public AnchorDefinitions(Options options) : base(options)
        {
        }

        public override void DefineHandlers()
        {
            // tonight at 7pm
            var anchorHandlers = new List<ComplexHandler>()
                {
                    Handle
                        .Optional<Grabber>()
                        .Required<IRepeater>()
                        .Optional<SeparatorAt>()
                        .Optional<IRepeater>()
                        .Optional<IRepeater>()
                        .Using<RHandler>(),
                    Handle
                        .Optional<Grabber>()
                        .Required<IRepeater>()
                        .Required<IRepeater>()
                        .Optional<SeparatorAt>()
                        .Optional<IRepeater>()
                        .Optional<IRepeater>()
                        .Using<RHandler>(),
                    Handle
                        .Required<IRepeater>()
                        .Required<Grabber>()
                        .Required<IRepeater>()
                        .Using<RGRHandler>(),
                };
            handlers.Add(HandlerType.Anchor, anchorHandlers);
        }
    }

    public class ArrowDefinitions : SpanDefinitions
    {
        public ArrowDefinitions(Options options) : base(options)
        {
        }

        public override void DefineHandlers()
        {
            var narrowHandlers = new List<ComplexHandler>()
                {
                    Handle
                        .Required<Scalar>()
                        .Required<IRepeater>()
                        .Required<Pointer>()
                        .Using<SRPHandler>(),
                    Handle
                        .Required<Scalar>()
                        .Required<IRepeater>()
                        .Optional<SeparatorAnd>()
                        .Required<Scalar>()
                        .Required<IRepeater>()
                        .Required<Pointer>()
                        .Optional<SeparatorAt>()
                        .Required(HandlerType.Anchor)
                        .Using<SRASRPAHandler>(),
                    Handle
                        .Required<Pointer>()
                        .Required<Scalar>()
                        .Required<IRepeater>()
                        .Using<PSRHandler>(),
                    Handle
                        .Required<Scalar>()
                        .Required<IRepeater>()
                        .Required<Pointer>()
                        .Optional<SeparatorAt>()
                        .Required(HandlerType.Anchor)
                        .Using<SRPAHandler>(),
                };
            handlers.Add(HandlerType.Arrow, narrowHandlers);
        }

    }

    public class NarrowDefinitions : SpanDefinitions
    {
        public NarrowDefinitions(Options options) : base(options)
        {
        }

        public override void DefineHandlers()
        {
            var narrowHandlers = new List<ComplexHandler>()
                {
                    Handle
                        .Required<Ordinal>()
                        .Required<IRepeater>()
                        .Required<SeparatorIn>()
                        .Required<IRepeater>()
                        .Using<ORSRHandler>(),
                    Handle
                        .Required<Ordinal>()
                        .Required<IRepeater>()
                        .Required<Grabber>()
                        .Required<IRepeater>()
                        .Using<ORGRHandler>(),
                };
            handlers.Add(HandlerType.Narrow, narrowHandlers);
        }
    }

    public class EndianDefinitions : SpanDefinitions
    {
        public EndianDefinitions(Options options) : base(options)
        {
        }

        public override void DefineHandlers()
        {
            PreferedEndian();
        }

        private void PreferedEndian()
        {
            var endianhandlers = new List<ComplexHandler>()
                {
                    Handle
                        .Required<ScalarMonth>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarDay>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SmSdSyHandler>(),

                    Handle
                        .Required<ScalarMonth>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarDay>()                      
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SmSdHandler>(),

                    Handle
                        .Required<ScalarDay>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarMonth>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SdSmHandler>(),

                    Handle
                        .Required<ScalarDay>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarMonth>()
                        .Repeat(pattern => pattern
                            .Required<SeparatorSlash>()
                            .Required<SeparatorDash>()
                        ).AnyNumberOfTimes()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()
                        .Optional(HandlerType.Time)
                        .Using<SdSmSyHandler>(),

                    Handle
                        .Required<ScalarDay>()
                        .Required<RepeaterMonthName>()
                        .Required<ScalarYear>()
                        .Optional<SeparatorAt>()                        
                        .Optional(HandlerType.Time)
                        .Using<SdRmnSyHandler>()
                };

            switch (options.EndianPrecedence)
            {
                case EndianPrecedence.Little:
                    {
                        endianhandlers.Reverse();
                        handlers.Add(HandlerType.Endian, endianhandlers);
                        break;
                    }
                case EndianPrecedence.Middle:
                    handlers.Add(HandlerType.Endian, endianhandlers);
                    break;
                default:
                    throw new ArgumentException($"Unknown endian value {options.EndianPrecedence}");
            }
        }
    }
}
