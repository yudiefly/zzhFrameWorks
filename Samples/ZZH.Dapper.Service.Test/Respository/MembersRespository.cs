using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZZH.Dapper.Service.Test.Model;

namespace ZZH.Dapper.Service.Test.Respository
{
    public class MembersRespository:TestAppBaseRepository
    {
        /// <summary>
        /// 获取全部会员
        /// </summary>
        /// <returns></returns>
        public List<Members> GetMembers(string Name = "")
        {
            try
            {
                string sql = $@"SELECT 	Id,NAME, BirthDay, Sex, Memos FROM members ";
                if (!string.IsNullOrEmpty(Name))
                {
                    sql = sql + "Where Name like '%" + Name + "%'";
                }
                var result = this.Query<Members>(sql).ToList();
                return result;
            }
            catch (System.Exception ex)
            {
                var ErrorMessages = ex.Message;
                throw ex;
            }
        }
        /// <summary>
        /// 插入会员
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public int InsertMembers(Members Entity)
        {
            string sqlInserts = $@"INSERT INTO members(Id,NAME,BirthDay,Sex,Memos)VALUES('Id','Name', 'BirthDay', 'Sex', 'Memos');";
            var result = this.Execute(sqlInserts, Entity);
            return result;
        }
    }
}
