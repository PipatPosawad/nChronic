namespace Chronic
{
    public class Separator : Tag<Separator.Type>
    {
        public Separator(Type value)
            : base(value)
        {

        }

        public enum Type
        {
            Comma,
            Dot,
            Colon,
            Space,            
            Slash,
            Dash,
            SingleQuote,
            DoubleQuote,
            At,            
            In,
            On,
            And,
            T,
            W
        }
    }
}