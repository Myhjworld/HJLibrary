using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class StaticConstraint
    {
        /// <summary>
        /// 工厂生成DAL的配置文件
        /// </summary>
        public readonly static string IBaseDALConfig = ConfigurationManager.AppSettings["IBaseDALConfig"];
    }
}
