using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FindCoin.Mysql
{
    class MysqlConn
    {
        public static string conf = "";

        public static void CreateTable(string createSql)
        {
            //string createTableSql_block = "create table block (id bigint(20) primary key auto_increment, hash varchar(255), size varchar(255), version tinyint(3)," +
            //    " previousblockhash varchar(255), merkleroot varchar(255)," +
            //    " time int(11), indexx int(11), nonce varchar(255), nextconsensus varchar(255), script varchar(2048), tx longtext)";
            //string createTableSql_address = "create table address (id int(11) primary key auto_increment, addr varchar(255)," +
            //    " firstuse varchar(255), lastuse varchar(255), txcount int(11))";
            //string createTableSql_address_tx = "create table address_tx (id int(11) primary key auto_increment, addr varchar(255)," +
            //    " txid varchar(255), blockindex int(11), blocktime varchar(255))";
            //string createTableSql_tx = "create table tx (id int(11) primary key auto_increment, txid varchar(255)," +
            //    " size int(11), type varchar(45), version tinyint(3), attributes varchar(2048), vin varchar(2048), vout varchar(2048)," +
            //    " sys_fee int(11), net_fee int(11), scripts varchar(2048), nonce varchar(255), blockheight varchar(45))";
            //string createTableSql_notify = "create table notify (id bigint(20) primary key auto_increment, txid varchar(255), vmstate varchar(255), gas_consumed varchar(255)," +
            //    " stack varchar(2048), notifications varchar(255), blockindex int(11))";
            //string createTableSql_nep5transfer = "create table nep5transfer (id bigint(20) primary key auto_increment, blockindex int(11), txid varchar(255)," +
            //    " n tinyint(3), asset varchar(255), from varchar(255), to varchar(255)), value varchar(255))";
            //string createTableSql_nep5asset = "create table nep5asset (id int(11) primary key auto_increment, assetid varchar(45), totalsupply varchar(45)," +
            //    " name varchar(45), symbol varchar(45), decimals varchar(45))";
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(createSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("建表成功");
                }
                catch (Exception)
                {
                    Console.WriteLine("建表失败");
                }
            }
        }

        public static DataSet ExecuteDataSet(string tableName, Dictionary<string, string> where) {
            using (MySqlConnection conn = new MySqlConnection(conf)) {
                conn.Open();
                string select = "select * from " + tableName + " where";
                foreach (var dir in where)
                {
                    select += " " + dir.Key + "='" + dir.Value + "'";
                    select += " and";
                }
                select = select.Substring(0, select.Length - 4);
                MySqlDataAdapter adapter = new MySqlDataAdapter(select, conf);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public static int ExecuteDataInsert(string tableName, List<string> parameter)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string mysql = $"insert into " + tableName + " values (null,";
                foreach (string param in parameter) {
                    mysql += "'" + param + "',";
                }               
                mysql = mysql.Substring(0, mysql.Length - 1);
                mysql += ");";
                MySqlCommand mc = new MySqlCommand(mysql, conn);
                int count = mc.ExecuteNonQuery();
                return count;
            }
        }

        /// <summary>
        /// 插入多条数据
        /// </summary>
        public static void InsertCollection(MySqlConnection connection)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = "INSERT INTO person VALUES ( null,?name, ?birthday)";
            command.Parameters.Add("?name", MySqlDbType.VarChar);
            command.Parameters.Add("?birthday", MySqlDbType.DateTime);

            for (int x = 0; x < 30; x++)
            {
                command.Parameters[0].Value = "name" + x;
                command.Parameters[1].Value = DateTime.Now;
                command.ExecuteNonQuery();
            }

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        public static int Update(string tableName, Dictionary<string, string> dirs, Dictionary<string, string> where)
        {
            using (MySqlConnection conn = new MySqlConnection(conf))
            {
                conn.Open();
                string update = $"update " + tableName + " set ";
                foreach (var dir in dirs)
                {
                    update += dir.Key + "='" + dir.Value + "',";
                }
                update = update.Substring(0, update.Length - 1);
                update += " where";
                foreach (var dir in where)
                {
                    update += " " + dir.Key + "='" + dir.Value + "'";
                    update += " and";
                }
                update = update.Substring(0, update.Length - 4);
                update += ";";
                MySqlCommand command = new MySqlCommand(update, conn);
                int count = command.ExecuteNonQuery();
                conn.Close();
                return count;
            }
        }
    }
}
