using System.Collections.Generic;

namespace SCPaker.BlocksDataEdit
{
    public struct Data
    {
        public string ClassName
        {
            get;
            set;
        }
        public string ChineseName
        {
            get;
            set;
        }
        public bool EditTextPower
        {
            get;
            set;
        }
        public bool ButtonClickPower
        {
            get;
            set;
        }
        public List<string> Shortcut
        {
            get;
            set;
        }
    }
}