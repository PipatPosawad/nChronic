using Chronic.Tags.Repeaters;
using System;
using System.Collections.Generic;

namespace Chronic.Handlers
{
    public class EndianSpecificRegistry : HandlerRegistry
    {
        public EndianSpecificRegistry(EndianPrecedence precedence)
        {
            var handlers = new List<ComplexHandler>()
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

            switch (precedence)
            {
                case EndianPrecedence.Little:
                    {
                        handlers.Reverse();
                        Add(HandlerType.Endian, handlers);
                        break;
                    }
                case EndianPrecedence.Middle:
                    Add(HandlerType.Endian, handlers);
                    break;
                default:
                    throw new ArgumentException($"Unknown endian value {precedence}");
            }
        }

    }
}