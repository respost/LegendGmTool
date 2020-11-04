using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace 传奇GM工具
{
    class MysqlBase
    {
        private MySqlConnection conn = null;
        private MySqlCommand command = null;
        private MySqlDataReader reader = null;

        /// <summary>
        /// 构造方法里建议连接
        /// </summary>
        /// <param name="connstr"></param>
        public MysqlBase(string connstr)
        {
            conn = new MySqlConnection(connstr);
        }
        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="sql"></param>
        public void CreateCommand(string sql)
        {
            conn.Open();
            command = new MySqlCommand(sql, conn);
        }
        /// <summary>
        /// 增、删、改公共方法
        /// </summary>
        /// <returns></returns>
        public int commonExecute()
        {
            int res = -1;
            try
            {
                res = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("操作失败!" + ex.Message);
            }
            conn.Close();
            return res;
        }
        /// <summary>
        /// 查询方法
        /// 注意：尽量不要用select * from table表（返回的数据过长时，DataTable可能会出错），最好指定要查询的字段。
        /// </summary>
        /// <returns></returns>
        public DataTable selectExecute()
        {
            DataTable dt = new DataTable();
            using (reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(reader);
            }
            return dt;
        }

    }
}