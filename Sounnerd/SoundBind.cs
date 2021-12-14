using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections;
namespace Sounnerd
{
    
    class SoundBind
    {
        private static Dictionary<KeyGroup, string> bindList = new Dictionary<KeyGroup, string>();
        private KeyGroup keybind;
        private string filepath { get; set; }

        public SoundBind(Key key,ModKey mod,string filepath)
        {
            keybind = new KeyGroup(mod, key);
            this.filepath = filepath;
            saveBind(keybind, filepath);
        }

        public static void loadBinds()
        {
            //TODO
        }

        public bool saveBind(KeyGroup keybind,string filepath)
        {
            bool status = true;
            try
            {
                bindList.Add(keybind, filepath);
            }
            catch(ArgumentException)
            {
                status = false;
            }
            return status;
        }

        public string getSound(KeyGroup keybind)
        {
            return "TODO";
        }

    }
}
