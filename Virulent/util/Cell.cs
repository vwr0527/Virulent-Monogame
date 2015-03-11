using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virulent
{
    class Cell<T>
    {
        private T data;
        private bool isEmpty;
        public void Activate()
        {
            isEmpty = false;
        }
        public void Deactivate()
        {
            isEmpty = true;
        }
        public bool IsActive()
        {
            return !isEmpty;
        }
        public void SetData(T param)
        {
            data = param;
        }
        public void CopyData(T param, Action<T, T> copyMethod)
        {
            if (data != null)
            {
                copyMethod(data, param);
            }
        }
        public void CreateCopy(T param, Func<T, T> createCopyMethod)
        {
            data = createCopyMethod(param);
        }

        public T GetData()
        {
            return data;
        }
    }
}
