using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 传奇GM工具
{
    class User
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string nick;

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }
        private int diamond;

        public int Diamond
        {
            get { return diamond; }
            set { diamond = value; }
        }
        private int gold;

        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        private int vip;

        public int Vip
        {
            get { return vip; }
            set { vip = value; }
        }
    }
}
