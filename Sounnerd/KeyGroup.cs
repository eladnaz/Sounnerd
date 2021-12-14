using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sounnerd
{
    public enum ModKey
    {
        SHIFT = ModifierKeys.Shift,
        CTRL = ModifierKeys.Control,
        ALT = ModifierKeys.Alt,
        SHIFTALT = (ModifierKeys.Shift | ModifierKeys.Alt),
        CTRLALT = (ModifierKeys.Control | ModifierKeys.Alt),
        NONE = ModifierKeys.None,
    }
    class KeyGroup
    {
        private ModKey mod { get; set; }
        private Key key { get; set; }
        public KeyGroup(ModKey mod,Key key)
        {
            this.mod = mod;
            this.key = key;
        }
    }
}
