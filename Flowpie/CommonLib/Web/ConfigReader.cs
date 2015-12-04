using System;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;


namespace CommonLib.Web
{
    /// <summary>
    /// ASP.Net(2.0-3.5)Web.Config��ȡ��
    /// </summary>
    public class ConfigReader : IConfigReader
    {
        public ConfigReader()
        {
            this.Init();
        }

        /// <summary>
        /// ʵ������������
        /// </summary>
        public override void Init()
        {
            try
            {
                this.objConfig = WebConfigurationManager.OpenWebConfiguration("~");

                this.InitAppSettings();
            }
            catch (Exception ex)
            { 
            
            }
        }

        /// <summary>
        /// ���������ļ������ýڵ�
        /// </summary>
        public override void EncryptAppSettings()
        {
            //��ȡ�����ļ�������·��
            Configuration objEncryConfig = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);

            //��ȡ�ļ��е�appSettings�ڵ�
            AppSettingsSection objEncryAppsettings = (AppSettingsSection)objEncryConfig.GetSection("appSettings");

            if (!objEncryAppsettings.SectionInformation.IsProtected)
            {
                //ѡ�Ӽ����ṩ��ʽ
                objEncryAppsettings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                objEncryAppsettings.SectionInformation.ForceSave = true;

                //������ܺ�������ļ�
                objEncryConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        public override void DecryptAppSettings()
        {
            //��ȡ�����ļ�������·��
            Configuration objDecConfig = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);

            //��ȡ�ļ��е�appSettings�ڵ�
            AppSettingsSection objDecAppsettings = (AppSettingsSection)objDecConfig.GetSection("appSettings");

            //�ж��Ƿ����
            if (objDecAppsettings.SectionInformation.IsProtected)
            {
                //���������ļ�
                objDecAppsettings.SectionInformation.UnprotectSection();
                objDecAppsettings.SectionInformation.ForceSave = true;

                //���������ļ�
                objDecConfig.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}
