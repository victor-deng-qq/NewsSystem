﻿using Itcast.CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace Itcast.CMS.DAL
{
    public class NewsInfoDal
    {
        public List<NewsInfo> GetPageList(int start,int end)
        {
            string sql = "select * from (select row_number() over (order by id) as num,* from T_News) as t where t.num >= @start and t.num <= @end";
            SqlParameter[] pars = {
                new SqlParameter("@start",SqlDbType.Int),
                new SqlParameter("@end",SqlDbType.Int)
            };
            pars[0].Value = start;
            pars[1].Value = end;
            DataTable da = SqlHelper.GetTable(sql,CommandType.Text,pars);
            List<NewsInfo> list = null;
            if (da.Rows.Count>0) {
                list = new List<NewsInfo>();
                NewsInfo newsInfo = null;
                foreach (DataRow row in da.Rows)
                {
                    newsInfo = new NewsInfo();
                    LoadEntity(row,newsInfo);
                    list.Add(newsInfo);
                }
            }
            return list;
        }

        private void LoadEntity(DataRow row, NewsInfo newsInfo)
        {
            newsInfo.Id = Convert.ToInt32(row["Id"]);
            newsInfo.Author = row["Author"] != DBNull.Value ? row["Author"].ToString() : string.Empty;
            newsInfo.Title = row["Title"] != DBNull.Value ? row["Title"].ToString() : string.Empty;
            newsInfo.Msg = row["Msg"] != DBNull.Value ? row["Msg"].ToString() : string.Empty;
            newsInfo.ImagePath = row["ImagePath"] != DBNull.Value ? row["ImagePath"].ToString() : string.Empty;
            newsInfo.SubDateTime = Convert.ToDateTime(row["SubDateTime"]);
        }

        // 获取总的记录数
        public int GetRecordCount()
        {
            string sql = "select count(*) from T_News";
            return Convert.ToInt32(SqlHelper.ExecuteScalare(sql, CommandType.Text));
        }

        //获取一条记录
        public NewsInfo GetModel(int id)
        {
            string sql = "select * from T_News where id=@id";
            SqlParameter[] pars = {
                                 new SqlParameter("@id",SqlDbType.Int)
                                 };
            pars[0].Value = id;
            DataTable da = SqlHelper.GetTable(sql, CommandType.Text, pars);
            NewsInfo newInfo = null;
            if (da.Rows.Count > 0)
            {
                newInfo = new NewsInfo();
                LoadEntity(da.Rows[0], newInfo);
            }
            return newInfo;
        }

        //删除一条记录
        public int DeleteInfo(int id)
        {
            string sql = "delete from T_News where id=@id";
            return SqlHelper.ExecuteNonquery(sql,CommandType.Text,new SqlParameter("@id",id));
        }

    }
}