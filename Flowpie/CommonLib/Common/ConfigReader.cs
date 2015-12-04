using System;
using BaseLib;
using System.Collections;

namespace CommonLib.Common
{
    /// <summary>
    /// ͨ�������ļ���ȡ��
    /// </summary>
    public class ConfigReader
    {
        private static Windows.ConfigReader _oWindowsConfigReader = null;

        private static Web.ConfigReader _oWebConfigReader = null;

        private static IConfigReader _oConfigReader = null;

        /// <summary>
        /// �������ʵ������ļ���ȡ��
        /// </summary>
        /// <param name="type">ϵͳ����</param>
        private static IConfigReader CreateReader(SystemType type)
        {
            switch (type)
            {
                case SystemType.Windows:
                    {
                        if (_oWindowsConfigReader == null)
                            _oWindowsConfigReader = new Windows.ConfigReader();

                        _oWindowsConfigReader.Init();

                        return _oWindowsConfigReader;
                    }
                case SystemType.Web:
                    {
                        if (_oWebConfigReader == null)
                            _oWebConfigReader = new Web.ConfigReader();

                        _oWebConfigReader.Init();

                        return _oWebConfigReader;
                    }
                default:
                    {
                        throw new Exception("����ϵͳ����");
                    }
            }
        }

        /// <summary>
        /// ��ȡһ���ڵ��ֵ
        /// </summary>
        /// <param name="strKey">�ڵ��key</param>
        /// <returns></returns>
        public static string Read(string strKey, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            return _oConfigReader.Read(strKey);
        }

        /// <summary>
        /// ��ѯ���нڵ㼯��
        /// </summary>
        /// <returns></returns>
        public static Hashtable Query(SystemType type)
        {
            _oConfigReader = CreateReader(type);

            return _oConfigReader.Query();
        }

        /// <summary>
        /// ����ڵ�
        /// </summary>
        /// <param name="strKey">����ڵ��key</param>
        /// <param name="strValue">����ڵ��value</param>
        public static void Insert(string strKey, string strValue, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.Insert(strKey, strValue);
        }

        /// <summary>
        /// �����������ýڵ�
        /// </summary>
        /// <param name="hasItems">�ڵ㼯��</param>
        public static void Insert(Hashtable hasItems, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.Insert(hasItems);
        }

        /// <summary>
        /// �Ƴ������ļ��ڵ�
        /// </summary>
        /// <param name="strKey">�ڵ��key</param>
        public static void Remove(string strKey, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.Remove(strKey);
        }

        /// <summary>
        /// �����Ƴ����ýڵ�
        /// </summary>
        /// <param name="hasItems">�ڵ㼯��</param>
        public static void Remove(Hashtable hasItems, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.Remove(hasItems);
        }

        /// <summary>
        /// ���½ڵ�
        /// </summary>
        /// <param name="strKey">Ҫ���½ڵ��Ӧkey</param>
        /// <param name="strValue">���µ�ֵ</param>
        public static void Update(string strKey, string strValue, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.Update(strKey, strValue);
        }

        /// <summary>
        /// �����������ýڵ�
        /// </summary>
        /// <param name="hasItems">�ڵ���</param>
        public static void Update(Hashtable hasItems, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.Update(hasItems);
        }

        /// <summary>
        /// �жϽڵ��Ƿ����
        /// </summary>
        /// <param name="strKey">�ڵ��key</param>
        /// <returns></returns>
        public static bool IsExist(string strKey, SystemType type)
        {
            _oConfigReader = CreateReader(type);

            return _oConfigReader.IsExist(strKey);
        }

        /// <summary>
        /// ���������ļ������ýڵ�
        /// </summary>
        public static void EncryptAppSettings(SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.EncryptAppSettings();
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        public static void DecryptAppSettings(SystemType type)
        {
            _oConfigReader = CreateReader(type);

            _oConfigReader.DecryptAppSettings();
        }
    }
}
