using HJLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJLibrary.IDAL
{
    public interface IBaseDAL
    {
        T Find<T>(int id) where T : BaseModel;

        List<T> FindAll<T>() where T : BaseModel;

        bool Add<T>(T t) where T : BaseModel;

        bool Update<T>(T t) where T : BaseModel;

        bool Delete<T>(T t) where T : BaseModel;

    }
}
