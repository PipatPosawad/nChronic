using System;
using System.Collections.Generic;
using System.Linq;
using Chronic.Tags.Repeaters;

namespace Chronic
{
    public class Tokenizer
    {
        static readonly List<ITokenScanner> _scanners = new List<ITokenScanner>
        {
            new RepeaterScanner(),
            new GrabberScanner(),
            new PointerScanner(),
            new ScalarScanner(), 
            new OrdinalScanner(), 
            new SeparatorScanner(),
            new SignScanner(),
            new TimeZoneScanner(),
        };

        IList<Token> TokenizeInternal(string phrase, Options options)
        {
            var tokens = phrase
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => new Token(part))
                .ToList();
            return tokens;
        }

        public IList<Token> Tokenize(string phrase, Options options)
        {
            options.OriginalPhrase = phrase;
            Logger.Log(() => phrase);

            phrase = Normalize(phrase);
            Logger.Log(() => phrase);

            var tokens = TokenizeInternal(phrase, options);
            _scanners.ForEach(scanner => scanner.Scan(tokens, options));
            var taggedTokens = tokens.Where(token => token.HasTags()).ToList();
            Logger.Log(() => String.Join(",", taggedTokens.Select(t => t.ToString())));

            return taggedTokens;
        }

        public static string Normalize(string phrase)
        {
            var normalized = phrase.ToLower();
            normalized = normalized
                //text.gsub!(/\b(\d{2})\.(\d{2})\.(\d{4})\b/, '\3 / \2 / \1')
                .ReplaceAll(@"\b(\d{2})\.(\d{2})\.(\d{4})\b", "$3 / $2 / $1")
                //text.gsub!(/\b([ap])\.m\.?/, '\1m')
                .ReplaceAll(@"\b([ap])\.m\.?", "$1m")
                //text.gsub!(/(\s+|:\d{2}|:\d{2}\.\d{3})\-(\d{2}:?\d{2})\b/, '\1tzminus\2')
                .ReplaceAll(@"(\s+|:\d{2}|:\d{2}\.\d{3})\-(\d{2}:?\d{2})\b", "$1tzminus$2")
                //text.gsub!(/\./, ':')
                .ReplaceAll(@"\.", ":")
                //text.gsub!(/([ap]):m:?/, '\1m')
                .ReplaceAll(@"([ap]):m:?", "$1m")
                //text.gsub!(/['"]/, '')
                .ReplaceAll(@"['""]", "")
                //text.gsub!(/,/, ' ')
                .ReplaceAll(@",", " ")
                //text.gsub!(/^second /, '2nd ')
                .ReplaceAll(@"^second ", "2nd ")
                .ReplaceAll(@"\bsecond (of|day|month|hour|minute|second)\b", "2nd $1")
                .Numerize()
                //.ReplaceAll(@"['""\.,]", "")
                .ReplaceAll(@"([/\-,@])", " " + "$1" + " ")
                .ReplaceAll(@"(?:^|\s)0(\d+:\d+\s*pm?\b)", "$1")
                .ReplaceAll(@"\btoday\b", "this day")
                .ReplaceAll(@"\btomm?orr?ow\b", "next day")
                .ReplaceAll(@"\byesterday\b", "last day")
                .ReplaceAll(@"\bnoon\b", "12:00")
                .ReplaceAll(@"\bmidnight\b", "24:00")
                .ReplaceAll(@"\bnow\b", "this second")
                //text.gsub!('quarter', '15')
                .ReplaceAll(@"quarter", "15")
                //text.gsub!('half', '30')
                .ReplaceAll(@"half", "30")
                //text.gsub!(/(\d{1,2}) (to|till|prior to|before)\b/, '\1 minutes past')
                .ReplaceAll(@"(\d{1,2}) (to|till|prior to|before)\b", "$1 minutes past")
                //text.gsub!(/(\d{1,2}) (after|past)\b/, '\1 minutes future')
                .ReplaceAll(@"(\d{1,2}) (after|past)\b", "$1 minutes future")
                //text.gsub!(/\b(?:ago|before(?: now)?)\b/, 'past')
                .ReplaceAll(@"\b(?:ago|before(?: now)?)\b", "past")
                //text.gsub!(/\bthis (?:last|past)\b/, 'last')
                .ReplaceAll(@"\bthis (?:last|past)\b", "last")
                .ReplaceAll(@"\b(?:in|during) the (morning)\b", "$1")
                .ReplaceAll(@"\b(?:in the|during the|at) (afternoon|evening|night)\b", "$1")
                .ReplaceAll(@"\btonight\b", "this night")
                //text.gsub!(/\b\d+:?\d*[ap]\b/,'\0m')
                .ReplaceAll(@"\b\d+:?\d*[ap]\b", "$0m")
                //text.gsub!(/\b(\d{2})(\d{2})(am|pm)\b/, '\1:\2\3')
                .ReplaceAll(@"\b(\d{2})(\d{2})(am|pm)\b", "$1:$2$3")
                .ReplaceAll(@"(\d)([ap]m|oclock)\b", "$1 $2")
                .ReplaceAll(@"\b(hence|after|from)\b", "future")
                //text.gsub!(/^\s?an? /i, '1 ')
                .ReplaceAll(@"^\s?an? ", "1 ")
                //text.gsub!(/\b(\d{4}):(\d{2}):(\d{2})\b/, '\1 / \2 / \3') # DTOriginal
                .ReplaceAll(@"\b(\d{4}):(\d{2}):(\d{2})\b", "$1 / $2 / $3") // DTOriginal
                //text.gsub!(/\b0(\d+):(\d{2}):(\d{2}) ([ap]m)\b/, '\1:\2:\3 \4')
                .ReplaceAll(@"\b0(\d+):(\d{2}):(\d{2}) ([ap]m)\b", "$1:$2:$3 $4")                
                //.ReplaceAll(@" \-(\d{4})\b", " tzminus$1")   
                //.ReplaceAll(@"\bbefore now\b", "past")
                //.ReplaceAll(@"\b(ago|before)\b", "past")
                //.ReplaceAll(@"\bthis past\b", "last")
                //.ReplaceAll(@"\bthis last\b", "last")
                ;

            return normalized;
        }
    }
}