using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic.Tags
{
    public class Sign : Tag<Sign.Type>
    {
        public Sign(Sign.Type value) : base(value)
        {
        }

        public override string ToString()
        {
            return "sign";
        }

        public enum Type
        {
            Plus,
            Minus
        }
    }

    public class SignPlus : Sign
    {
        public SignPlus() : base(Type.Plus)
        {
        }

        public override string ToString()
        {
            return base.ToString() + "-plus";
        }
    }

    public class SignMinus : Sign
    {
        public SignMinus() : base(Type.Minus)
        {
        }

        public override string ToString()
        {
            return base.ToString() + "-minus";
        }
    }
}
