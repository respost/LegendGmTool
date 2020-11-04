using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;

namespace 传奇GM工具
{
    public partial class Form1 : Form
    {
        //工具参数
        private Tool tool;
        //账号数据库对象
        private MysqlBase mysql_login;
        //服务数据库对象
        private MysqlBase mysql_server;
        //当前查询玩家Id
        private int accountId;
        //用户对象
        private User user;
        public Form1()
        {
            //加载嵌入资源
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            InitializeComponent();
        }
        /// <summary>
        /// 加载嵌入资源中的全部dll文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }
        /// <summary>
        /// 获取工具参数
        /// </summary>
        /// <returns></returns>
        private Tool getTool()
        {
            Tool tool = new Tool();
            //数据库IP
            string txtDbHost = this.txtDbHost.Text.Trim();
            if (txtDbHost == string.Empty)
            {
                txtDbHost = "127.0.0.1";
            }
            tool.DbHost = txtDbHost;
            //数据库端口
            string txtDbPort = this.txtDbPort.Text.Trim();
            if (txtDbPort == string.Empty)
            {
                txtDbHost = "3306";
            }
            tool.DbPort = txtDbPort;
            //数据库用户
            string txtDbUser = this.txtDbUser.Text.Trim();
            if (txtDbUser == string.Empty)
            {
                txtDbUser = "root";
            }
            tool.DbUser = txtDbUser;
            //数据库密码
            string txtDbPassword = this.txtDbPassword.Text.Trim();
            if (txtDbPassword == string.Empty)
            {
                txtDbPassword = "root";
            }
            tool.DbPassword = txtDbPassword;
            //主数据库
            string txtMainData = this.txtMainData.Text.Trim();
            if (txtMainData == string.Empty)
            {
                MessageBox.Show("主数据库名称不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            tool.MainData = txtMainData;
            //登录数据库
            string txtLoginData = this.txtLoginData.Text.Trim();
            if (txtLoginData == string.Empty)
            {
                MessageBox.Show("登录数据库名称不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            tool.LoginData = txtLoginData;
            //服务数据库
            string txtServerData = this.txtServerData.Text.Trim();
            if (txtServerData == string.Empty)
            {
                MessageBox.Show("服务数据库名称不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            tool.ServerData = txtServerData;
            return tool;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            //工具参数
            tool = getTool();
            string loginStr = String.Format("Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port={4}", tool.LoginData, tool.DbHost, tool.DbUser, tool.DbPassword, tool.DbPort);
            string serverStr = String.Format("Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port={4}", tool.ServerData, tool.DbHost, tool.DbUser, tool.DbPassword, tool.DbPort);
            mysql_login = new MysqlBase(loginStr);
            mysql_server = new MysqlBase(serverStr);
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            //查询
            string txtUsername = this.txtUsername.Text.Trim();
            if (txtUsername == string.Empty)
            {
                MessageBox.Show("游戏账号不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //元宝
            string txtDiamond = this.txtDiamond.Text.Trim();
            if (txtDiamond == string.Empty)
            {
                MessageBox.Show("元宝数量不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            int diamond = Convert.ToInt32(txtDiamond);
            if (diamond <= 0)
            {
                MessageBox.Show("充值元宝数量必须大于0", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                mysql_server.CreateCommand("select id,vipscore from uw_user where accountid =" + accountId);
                DataTable dt = mysql_server.selectExecute();
                if (dt.Rows.Count != 0)
                {
                    //计算会员积分
                    int vipscore = Convert.ToInt32(dt.Rows[0][1]);
                    vipscore += diamond;
                    int vip = 10;
                    if (vipscore > 0 && vipscore < 61) { vip = 1; }
                    else if (vipscore > 60 && vipscore < 421) { vip = 2; }
                    else if (vipscore > 420 && vipscore < 981) { vip = 3; }
                    else if (vipscore > 980 && vipscore < 1981) { vip = 4; }
                    else if (vipscore > 1980 && vipscore < 4981) { vip = 5; }
                    else if (vipscore > 4980 && vipscore < 9981) { vip = 6; }
                    else if (vipscore > 9980 && vipscore < 20001) { vip = 7; }
                    else if (vipscore > 20000 && vipscore < 36001) { vip = 8; }
                    else if (vipscore > 36000 && vipscore < 58001) { vip = 9; }
                    else if (vipscore > 58000 && vipscore < 98001) { vip = 10; }
                    else if (vipscore > 98000 && vipscore < 150001) { vip = 11; }
                    else if (vipscore > 150000 && vipscore < 210001) { vip = 12; }
                    else if (vipscore > 210000 && vipscore < 280001) { vip = 13; }
                    else if (vipscore > 280000 && vipscore < 360001) { vip = 14; }
                    else if (vipscore > 360000 && vipscore < 450001) { vip = 15; }
                    else if (vipscore > 450000 && vipscore < 550001) { vip = 16; }
                    else if (vipscore > 550000 && vipscore < 720001) { vip = 17; }
                    else if (vipscore > 720000 && vipscore < 980001) { vip = 18; }
                    else if (vipscore > 980000 && vipscore < 1280001) { vip = 19; }
                    else if (vipscore > 1280000 && vipscore < 1980001) { vip = 20; }
                    else if (vipscore >= 1980000) { vip = 21; }
                    else { vip = 0; }
                    //更新
                    string sql_update = String.Format("update uw_user set diamond = diamond+{0}, vipScore=vipScore+{0} where accountid ={1}", diamond, accountId);
                    if (vip > 10)
                    {
                        sql_update = String.Format("update uw_user set diamond = diamond+{0}, vipScore=vipScore+{0},vip={1} where accountid ={2}", diamond, vip, accountId);
                    }
                    mysql_server.CreateCommand(sql_update);
                    int res = mysql_server.commonExecute();
                    if (res > 0)
                    {
                        MessageBox.Show("充值元宝成功");
                        //读取用户信息
                        getUserInfo();
                    }
                    else
                    {
                        MessageBox.Show("充值元宝失败");
                    }
                }
                else
                {
                    MessageBox.Show("账号不存在", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败，请检查是否已导入数据库。", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        private void getUserInfo()
        {
            user = new User();
            try
            {
                //查询
                string txtUsername = this.txtUsername.Text.Trim();
                if (txtUsername != string.Empty)
                {
                    user.Name = txtUsername;
                }
                mysql_login.CreateCommand("select id,userServers from uw_account where name ='" + txtUsername + "'");
                DataTable dt = mysql_login.selectExecute();
                if (dt.Rows.Count != 0)
                {
                    accountId = Convert.ToInt32(dt.Rows[0][0]);
                    user.Id = Convert.ToInt32(dt.Rows[0][0]);
                    string str = dt.Rows[0][1].ToString();
                    List<int> userServersList = JsonConvert.DeserializeObject<List<int>>(str);
                    ArrayList areaList = new ArrayList();
                    for (int i = 0; i < userServersList.Count; i++)
                    {
                        areaList.Add(new DictionaryEntry(userServersList[i], userServersList[i] + "区"));
                    }
                    //设置用户所在区
                    combArea.DataSource = areaList;
                    combArea.DisplayMember = "Value";
                    combArea.ValueMember = "Key";
                    mysql_server.CreateCommand("select nickName,diamond,gold,vip from uw_user where accountid =" + accountId);
                    DataTable dt2 = mysql_server.selectExecute();
                    if (dt2.Rows.Count != 0)
                    {
                        user.Nick = dt2.Rows[0][0].ToString();
                        user.Diamond = Convert.ToInt32(dt2.Rows[0][1]);
                        user.Gold = Convert.ToInt32(dt2.Rows[0][2]);
                        user.Vip = Convert.ToInt32(dt2.Rows[0][3]);
                        //显示到界面上
                        this.panelUserInfo.Visible = true;
                        this.panelAbout.Visible = false;
                        this.labNickname.Text = user.Nick;
                        this.labDiamond.Text = user.Diamond.ToString();
                        this.labGold.Text = user.Gold.ToString();
                        this.labVip.Text = "VIP" + user.Vip.ToString();
                    }
                }
                else
                {
                    this.panelUserInfo.Visible = false;
                    this.panelAbout.Visible = true;
                }
            }
            catch (Exception)
            {
                this.panelUserInfo.Visible = false;
                this.panelAbout.Visible = true;
                MessageBox.Show("数据库连接失败，请检查是否已导入数据库。", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //工具参数
            tool = getTool();
            string loginStr = String.Format("Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port={4}", tool.LoginData, tool.DbHost, tool.DbUser, tool.DbPassword, tool.DbPort);
            string serverStr = String.Format("Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port={4}", tool.ServerData, tool.DbHost, tool.DbUser, tool.DbPassword, tool.DbPort);
            mysql_login = new MysqlBase(loginStr);
            mysql_server = new MysqlBase(serverStr);
            //等级下拉框默认选择
            this.combVip.SelectedIndex = 0;        
            //读取用户信息
            string txtUsername = this.txtUsername.Text.Trim();
            if (txtUsername != string.Empty)
            {
                getUserInfo();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            //读取用户信息
            getUserInfo();
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            //查询
            string txtUsername = this.txtUsername.Text.Trim();
            if (txtUsername==string.Empty)
            {
                MessageBox.Show("游戏账号不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //等级选择
            string combVip = this.combVip.Text;
            int vip = Convert.ToInt32(combVip.Substring(combVip.Length - 2, 2));
            try
            {
                string sql_update = String.Format("update uw_user set vip={0} where accountid ={1}", vip, accountId);
                mysql_server.CreateCommand(sql_update);
                int res = mysql_server.commonExecute();
                if (res > 0)
                {
                    MessageBox.Show("提升等级成功");
                    //读取用户信息
                    getUserInfo();
                }
                else
                {
                    MessageBox.Show("提升等级失败");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败，请检查是否已导入数据库。", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnGold_Click(object sender, EventArgs e)
        {
            //查询
            string txtUsername = this.txtUsername.Text.Trim();
            if (txtUsername == string.Empty)
            {
                MessageBox.Show("游戏账号不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //金币
            string txtGold = this.txtGold.Text.Trim();
            if (txtGold == string.Empty)
            {
                MessageBox.Show("金币数量不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            int gold = Convert.ToInt32(txtGold);
            if (gold <= 0)
            {
                MessageBox.Show("充值金币数量必须大于0", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                mysql_server.CreateCommand("select id from uw_user where accountid =" + accountId);
                DataTable dt = mysql_server.selectExecute();
                if (dt.Rows.Count != 0)
                {
                    //更新
                    string sql_update = String.Format("update uw_user set gold = gold+{0} where accountid ={1}", gold, accountId);
                    mysql_server.CreateCommand(sql_update);
                    int res = mysql_server.commonExecute();
                    if (res > 0)
                    {
                        MessageBox.Show("充值金币成功");
                        //读取用户信息
                        getUserInfo();
                    }
                    else
                    {
                        MessageBox.Show("充值金币失败");
                    }
                }
                else
                {
                    MessageBox.Show("账号不存在", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败，请检查是否已导入数据库。", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnGiftPackage_Click(object sender, EventArgs e)
        {
            //查询
            string txtUsername = this.txtUsername.Text.Trim();
            if (txtUsername == string.Empty)
            {
                MessageBox.Show("游戏账号不能为空", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //获取玩家所在区
            string area = this.combArea.SelectedValue.ToString();
            try
            {
                string sql_insert = String.Format("INSERT INTO `uw_recharge`(userId,diamond,effTime,payMoney,userLvl) VALUES ({0},{1},'{2}',{3},{4})", accountId, 1000, DateTime.Now.ToString(), 100,user.Vip);
                mysql_server.CreateCommand(sql_insert);
                int res = mysql_server.commonExecute();
                if (res > 0)
                {
                    MessageBox.Show("赠送礼包成功");
                }
                else
                {
                    MessageBox.Show("提赠送礼包失败");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败，请检查是否已导入数据库。", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsslUrl_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.zy13.net");
        }

    }
}
