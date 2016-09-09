using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronic
{
    public class Dictionary
    {
        protected Options options;
        protected List<Definitions> definedItems;

        public Dictionary(Options options)
        {
            this.options = options;
            definedItems = new List<Definitions>();
        }

        public void Definitions()
        {
            foreach(var item in definedItems)
            {
                item.DefineHandlers();
            }
        }
    }

    public class SpanDictionary : Dictionary
    {
        public SpanDictionary(Options options) :base(options)
        {
            definedItems = new List<Definitions>
            {
                new TimeDefinitions(options),
                new DateDefinitions(options),
                new AnchorDefinitions(options),
                new ArrowDefinitions(options),
                new NarrowDefinitions(options),
                new EndianDefinitions(options)
            };
        }

        public Definitions GetDefinition(Type type)
        {
            return definedItems.SingleOrDefault(d => d.GetType() == type);
        }
    }
}
