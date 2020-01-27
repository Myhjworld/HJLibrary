using HJLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HJLibrary.DAL
{
    public class SqlBuilder<T> where T : BaseModel
    {
        public static string FindSql = null;
        public static string FindAllSql = null;
        public static string AddSql = null;
        public static string UpdateSql = null;
        public static string DeleteSql = null;

        static SqlBuilder()
        {
            Type type = typeof(T);
            //查询
            FindSql = $"select {string.Join(",", type.GetProperties().Select(p => $"[{ p.Name}]"))} from [{type.Name}] where ID=@Id";
            
            //查询全部
            FindAllSql= $"select {string.Join(",", type.GetProperties().Select(p => $"[{ p.Name}]"))} from [{type.Name}]";

            //插入
            string ColumnStrings = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                //.Where(p => p.Name.Equals("ID"))
                .Select(p => $"[{p.Name}]")
                );
            string ValueColumn = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => $"@{ p.Name}")
                );
            AddSql = $"insert into [{type.Name}] ({ColumnStrings}) values ({ValueColumn})";

            //更新
            string UpdateValues = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => $"{p.Name}=@{p.Name}")
                );

            UpdateSql = $"update [{type.Name}] set {UpdateValues} where ID=@Id";

            //删除
            DeleteSql = $"delete * from [{type.Name}] where id=@Id";
        }
    }
}
