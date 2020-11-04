using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 传奇GM工具
{
    class Tool
    {
        private string dbHost;

        public string DbHost
        {
            get { return dbHost; }
            set { dbHost = value; }
        }
        private string dbPort;

        public string DbPort
        {
            get { return dbPort; }
            set { dbPort = value; }
        }
        private string dbUser;

        public string DbUser
        {
            get { return dbUser; }
            set { dbUser = value; }
        }
        private string dbPassword;

        public string DbPassword
        {
            get { return dbPassword; }
            set { dbPassword = value; }
        }
        private string mainData;

        public string MainData
        {
            get { return mainData; }
            set { mainData = value; }
        }
        private string loginData;

        public string LoginData
        {
            get { return loginData; }
            set { loginData = value; }
        }
        private string serverData;

        public string ServerData
        {
            get { return serverData; }
            set { serverData = value; }
        }
    }
}
