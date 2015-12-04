using System;
using System.Configuration;

namespace CommonLib.Windows
{
    /// <summary>
    /// Config�ļ�������(2.0)
    /// </summary>
    public class ConfigReader : IConfigReader
    {
        /// <summary>
        /// �������ʵ��
        /// </summary>
        public ConfigReader()
        {
            this.Init();
        }

        /// <summary>
        /// ��ʼ����������
        /// </summary>
        public override void Init()
        {
            this.objConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            this.InitAppSettings();
        }
    }
}
