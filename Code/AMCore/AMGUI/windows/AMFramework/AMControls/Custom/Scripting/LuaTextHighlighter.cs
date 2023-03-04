using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Text.RegularExpressions;
using System.Windows.Media;
using SharpDX.Direct3D9;

namespace AMControls.Custom.Scripting
{
    internal class LuaTextHighlighter : IHighlightingDefinition
    {

        public LuaTextHighlighter()
        {
            _namedHighlightingColors = new List<HighlightingColor>()
            {
                new HighlightingColor { Name = "Keyword", Foreground = new SimpleHighlightingBrush(Colors.DodgerBlue) },
                new HighlightingColor { Name = "String", Foreground = new SimpleHighlightingBrush(Colors.DarkRed) },
                new HighlightingColor { Name = "Comment", Foreground = new SimpleHighlightingBrush(Colors.Green) },
                new HighlightingColor { Name = "Variable", Foreground = new SimpleHighlightingBrush(Colors.HotPink) },
                new HighlightingColor { Name = "Class", Foreground = new SimpleHighlightingBrush(Colors.DarkOliveGreen) },
                new HighlightingColor { Name = "Function", Foreground = new SimpleHighlightingBrush(Colors.DarkOrange) },
                new HighlightingColor { Name = "Number", Foreground = new SimpleHighlightingBrush(Colors.DarkMagenta) },
                new HighlightingColor { Name = "Operator", Foreground = new SimpleHighlightingBrush(Colors.White) },
                new HighlightingColor { Name = "LocalVariable", Foreground = new SimpleHighlightingBrush(Colors.DeepSkyBlue) },
        };

            _mainRuleSet = new() { Name = "Lua" };

            

            _mainRuleSet.Rules.Add(new HighlightingRule
            {
                Regex = new Regex(@"\b(and|break|do|else|elseif|end|false|for|function|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b"),
                Color = new HighlightingColor { Foreground = GetNamedColor("Keyword").Foreground, FontWeight = System.Windows.FontWeights.SemiBold },
            });

            _mainRuleSet.Rules.Add(new HighlightingRule
            {
                Regex = new Regex(@"""[^\n\r]*"""),
                Color = new HighlightingColor { Foreground = GetNamedColor("String").Foreground, FontWeight = System.Windows.FontWeights.SemiBold }
            });

            _mainRuleSet.Rules.Add(new HighlightingRule()
            {
                Regex = new Regex(@"--.*$"),
                Color = new HighlightingColor { Foreground = GetNamedColor("Comment").Foreground }
            });

            _mainRuleSet.Rules.Add(new HighlightingRule()
            {
                Regex = new Regex(@"[-+*/%^#=<>!&|]+"),
                Color = new HighlightingColor { Foreground = GetNamedColor("Operator").Foreground, FontWeight = System.Windows.FontWeights.SemiBold }
            });

            _mainRuleSet.Rules.Add(new HighlightingRule()
            {
                Regex = new Regex(@"\d+"),
                Color = new HighlightingColor { Foreground = GetNamedColor("Number").Foreground, FontWeight = System.Windows.FontWeights.Medium }
            });

            _mainRuleSet.Rules.Add(new HighlightingRule()
            {
                Regex = new Regex(@"(?<=\W)([a-zA-Z_][a-zA-Z0-9_]*)(?=\s*\()"),
                Color = new HighlightingColor { Foreground = GetNamedColor("Function").Foreground, FontWeight = System.Windows.FontWeights.Medium }
            });


            _properties = new Dictionary<string, string>();
        }

        private HighlightingRuleSet _mainRuleSet;
        private IEnumerable<HighlightingColor> _namedHighlightingColors;
        private IDictionary<string, string> _properties;
        public string Name => "LUA";
        public HighlightingRuleSet MainRuleSet => _mainRuleSet;

        public IEnumerable<HighlightingColor> NamedHighlightingColors => _namedHighlightingColors;

        public IDictionary<string, string> Properties => _properties;

        public HighlightingColor GetNamedColor(string name)
        {
            return NamedHighlightingColors.FirstOrDefault(e => e.Name == name) ?? new HighlightingColor();
        }

        public HighlightingRuleSet GetNamedRuleSet(string name)
        {
            throw new NotImplementedException();
        }
    }
}
