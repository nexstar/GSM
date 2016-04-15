using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSM_GUI
{
    class Com
    {
        static int Comport;
        public static void SetCom(int com1) 
        {
            Comport = com1;
        }
        public static int GetCom() 
        {
            return Comport;
        }
    }
}
