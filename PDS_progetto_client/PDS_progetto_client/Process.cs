using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_progetto_client
{
    class Process
    {
        string name,wndname,classname;
        int pid;
        public int Pid
        {
            get
            {
                return pid;

            }

            set
            {
                pid = value;
            }
        }
        public string Name
        {
            get
            {
                return name;

            }

            set
            {
                name = value;
            }
        }

        public string wName
        {
            get
            {
                return wndname;

            }

            set
            {
                wndname = value;
            }
        }
        public string cName
        {
            get
            {
                return classname;

            }

            set
            {
                classname = value;
            }
        }
    }
}
