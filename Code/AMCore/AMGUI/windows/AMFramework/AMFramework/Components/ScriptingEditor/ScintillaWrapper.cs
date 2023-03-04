using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMFramework.Components.ScriptingEditor
{
    public class ScintillaWrapper : ScintillaNET.Scintilla
    {

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle &= ~WS_EX_TOPMOST;
                return createParams;
            }
        }

        private const int WS_EX_TOPMOST = 0x00000008;

    }
}
