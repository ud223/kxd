using System;
using System.Data;
using System.Security.Cryptography;

namespace CommonLib.Common
{
    /// <summary>
    /// ͨ��С����
    /// </summary>
    public class Function
    {
        /// <summary>
        /// �ж϶����Ƿ�Ϊ��,ͬʱ���ض����ַ���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string IsNull(object obj)
        {
            if (obj == null || obj == System.DBNull.Value)
                return "";

            return obj.ToString();
        }

        /// <summary>
        /// �жϱ��м������Ƿ�������ֶ���,�����ھͷ���null,���ھͷ���Ӧ�Զ���
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static object Contains(DataRow dr, string ColumnName)
        {
            if (!dr.Table.Columns.Contains(ColumnName) || dr[ColumnName] == System.DBNull.Value)
                return null;

            return dr[ColumnName];
        }

        /// <summary>
        /// �ж�HashTable���Ƿ������key��value,���ھͷ��ض�Ӧֵ,�����ھͷ���null
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Contains(System.Collections.Hashtable ht, string key)
        {
            if (ht.Contains(key))
                return ht[key];

            return null;
        }

        /// <summary>
        /// ��������ת�����ַ���
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string BooleanToIntString(bool b)
        {
            if (b)
                return "1";
            else
                return "0";
        }

        /// <summary>
        /// �ַ���md5 32λ����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string toMD5String(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//ʵ����һ��md5����
            // ���ܺ���һ���ֽ����͵����飬����Ҫע�����UTF8/Unicode�ȵ�ѡ��
            byte[] s = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cl));
            // ͨ��ʹ��ѭ�������ֽ����͵�����ת��Ϊ�ַ��������ַ����ǳ����ַ���ʽ������
            for (int i = 0; i < s.Length; i++)
            {
                // ���õ����ַ���ʹ��ʮ���������͸�ʽ����ʽ����ַ���Сд����ĸ�����ʹ�ô�д��X�����ʽ����ַ��Ǵ�д�ַ� 
                pwd = pwd + s[i].ToString("X");

            }

            return pwd;
        }
    }
}
