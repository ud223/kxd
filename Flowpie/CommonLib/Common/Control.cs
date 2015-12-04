using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CommonLib.Common
{
    public class Control
    {
        // 
        /// ������ȫ�����ڵ�ǰ�������ȡTYPE zgke@sina.com qq:116149 
        /// 
        /// ��ȫ�� System.Windows.Forms.TextBox 
        /// Type 
        public static Type GetType(string p_TypeFullName)
        {
            Type _TypeInfo = Type.GetType(p_TypeFullName);
            
            if (_TypeInfo != null) 
                return _TypeInfo;
            
            Assembly[] _Assembly = AppDomain.CurrentDomain.GetAssemblies();
            
            for (int i = 0; i != _Assembly.Length; i++)
            {
                _TypeInfo = _Assembly[i].GetType(p_TypeFullName);
                
                if (_TypeInfo != null) 
                    return _TypeInfo;
            }
            
            return null;
        } 
    }
}
