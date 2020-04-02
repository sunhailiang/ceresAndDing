using System;
using System.Collections.Generic;
using System.Text;

namespace Ceres.Domain.Core.Models
{
    public abstract class ValueObject<T> where T:ValueObject<T>
    {
        /// <summary>
        /// 重写方法 相等运算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;
            return !ReferenceEquals(valueObject, null) && EqualsCore(valueObject);
        }

        protected abstract bool EqualsCore(T other);
    }
}
