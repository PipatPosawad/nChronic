namespace Chronic
{
    public class SeparatorComma : Separator
    {
        public SeparatorComma() : base(Separator.Type.Comma) { }
    }

    public class SeparatorDot : Separator
    {
        public SeparatorDot() : base(Separator.Type.Dot) { }
    }

    public class SeparatorColon : Separator
    {
        public SeparatorColon() : base(Separator.Type.Colon) { }
    }

    public class SeparatorSpace : Separator
    {
        public SeparatorSpace() : base(Separator.Type.Space) { }
    }

    public class SeparatorSlash : Separator
    {
        public SeparatorSlash() : base(Separator.Type.Slash) { }
    }

    public class SeparatorDash : Separator
    {
        public SeparatorDash() : base(Separator.Type.Dash) { }
    }

    public class SeparatorQuote : Separator
    {
        public SeparatorQuote(Separator.Type value) : base(value) { }
    }

    public class SeparatorAt : Separator
    {
        public SeparatorAt() : base(Separator.Type.At) { }
    }

    public class SeparatorIn : Separator
    {
        public SeparatorIn() : base(Separator.Type.In) { }
    }

    public class SeparatorOn : Separator
    {
        public SeparatorOn() : base(Separator.Type.On) { }
    }

    public class SeparatorAnd : Separator
    {
        public SeparatorAnd() : base(Separator.Type.And) { }
    }

    public class SeparatorT : Separator
    {
        public SeparatorT() : base(Separator.Type.T) { }
    }

    public class SeparatorW : Separator
    {
        public SeparatorW() : base(Separator.Type.W) { }
    }

    public class SeparatorDate : Separator
    {
        public SeparatorDate(Separator.Type value) : base(value) { }
    }

    
}