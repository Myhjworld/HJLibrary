using HJLibrary.IDAL;
using HJLibrary.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HJLibrary.DAL
{
    public class BaseDAL : IBaseDAL
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["Mssql"].ConnectionString;

        public bool Add<T>(T t) where T : BaseModel
        {
            bool result = false;
            Type type = t.GetType();
            //string ColumnStrings = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
            //    //.Where(p => p.Name.Equals("ID"))
            //    .Select(p => $"[{p.Name}]")
            //    );
            //string ValueColumn = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
            //    .Select(p => $"@{ p.Name}")
            //    );
            List<SqlParameter> lSqlParameters = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => new SqlParameter($"@{p.Name}", p.GetValue(t) ?? DBNull.Value))
                .ToList();


            string sql = SqlBuilder<T>.AddSql;//$"insert into [{type.Name}] ({ColumnStrings}) values ({ValueColumn})";
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddRange(lSqlParameters.ToArray());
                result = sqlCommand.ExecuteNonQuery() > 0 ? true : false;
            }
            return result;
        }

        public bool Delete<T>(T t) where T : BaseModel
        {
            bool result = false;
            //Type type = typeof(T);
            string sql = SqlBuilder<T>.DeleteSql;//$"delete * from [{type.Name}] where id=[{t.Id}]";
            SqlParameter sqlParameter = new SqlParameter("@Id", t.Id);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                result = sqlCommand.ExecuteNonQuery() > 0 ? true : false;
            }

            return result;
        }

        public T Find<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            string sql = SqlBuilder<T>.FindSql;//$"select {string.Join(",", type.GetProperties().Select(p => $"[{ p.Name}]"))} from [{type.Name}] where ID={id}";
            //object Object = Activator.CreateInstance(type);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                var reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    return Trans<T>(type, reader);
                }
                else
                {
                    return null;
                }
            }
            //throw new NotImplementedException();
        }

        public List<T> FindAll<T>() where T : BaseModel
        {
            Type type = typeof(T);
            string sql = SqlBuilder<T>.FindAllSql;//$"select {string.Join(",", type.GetProperties().Select(p => $"[{ p.Name}]"))} from [{type.Name}]";
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                var reader = sqlCommand.ExecuteReader();
                List<T> tLIst = new List<T>();
                while (reader.Read())
                {
                    tLIst.Add(Trans<T>(type, reader));
                }
                return tLIst;
            }
        }

        public bool Update<T>(T t) where T : BaseModel
        {
            bool result = false;
            Type type = t.GetType();
            
            List<SqlParameter> lSqlParameters = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => new SqlParameter($"@{p.Name}", p.GetValue(t) ?? DBNull.Value))
                .ToList();

            //string UpdateValues = string.Join(",",type.GetProperties(BindingFlags.DeclaredOnly|BindingFlags.Instance|BindingFlags.Public)
            //    .Select(p=>$"{p.Name}=@{p.Name}")
            //    );

            string sql = SqlBuilder<T>.UpdateSql;//$"update [{type.Name}] set {UpdateValues} where ID={t.Id}";
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddRange(lSqlParameters.ToArray());
                sqlCommand.Parameters.Add(new SqlParameter("@Id", t.Id));
                result = sqlCommand.ExecuteNonQuery() > 0 ? true : false;
            }
            return result;
        }

        #region Private Method

        private T Trans<T>(Type type, SqlDataReader sqlDataReader)
        {
            object Object = Activator.CreateInstance(type);
            foreach (var prop in type.GetProperties())
            {
                prop.SetValue(Object, sqlDataReader[prop.Name] is DBNull ? null : sqlDataReader[prop.Name]);
            }
            return (T)Object;
        }

        #endregion
    }
}
