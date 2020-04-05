using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gooios.UserService.Domain
{
    public abstract class Entity<T>: IEntity
    {
        T _id;

        public virtual T Id
        {
            get { return _id; }
            protected set { _id = value; }
        }
    }
}
