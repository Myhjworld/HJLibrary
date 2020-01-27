using HJLibrary.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Framework;

namespace HJLibrary.Factory
{
    public class SimpleFactory
    {

        private static string DLLName = StaticConstraint.IBaseDALConfig.Split(',')[1];

        private static string TypeName = StaticConstraint.IBaseDALConfig.Split(',')[0];

        public static IBaseDAL CreateInstance()
        {
            Assembly assembly = Assembly.Load(DLLName);
            Type type = assembly.GetType(TypeName);
            return (IBaseDAL)Activator.CreateInstance(type);
        }
    }
}
