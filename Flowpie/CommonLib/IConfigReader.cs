using System;
using System.Collections;
using System.Configuration;

namespace CommonLib
{
    /// <summary>
    /// �����ļ���ȡ��������
    /// </summary>
    public abstract class IConfigReader
    {
        /// <summary>
        /// ��ȡWeb.Config�ļ�ʵ��
        /// </summary>
        protected Configuration objConfig;

        /// <summary>
        /// ��ȡWeb.Config�ļ���appSettings�ڵ�
        /// </summary>
        protected AppSettingsSection objAppsettings;

        #region �������鷽��(��ʵ��)

        protected virtual void InitAppSettings()
        {
            this.objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
        }

        /// <summary>
        /// ��ȡһ���ڵ��ֵ
        /// </summary>
        /// <param name="strKey">�ڵ��key</param>
        /// <returns></returns>
        public virtual string Read(string strKey)
        {
            if (objAppsettings != null)
            {
                return objAppsettings.Settings[strKey].Value;
            }

            return "";
        }

        /// <summary>
        /// ��ѯ���нڵ㼯��
        /// </summary>
        /// <returns></returns>
        public virtual Hashtable Query()
        {
            Hashtable Items = new Hashtable();

            if (objAppsettings != null)
            {
                foreach (KeyValueConfigurationElement node in objAppsettings.Settings)
                {
                    Items.Add(node.Key, node.Value);
                }
            }

            return Items;
        }

        /// <summary>
        /// ����ڵ�
        /// </summary>
        /// <param name="strKey">����ڵ��key</param>
        /// <param name="strValue">����ڵ��value</param>
        public virtual void Insert(string strKey, string strValue)
        {
            if (objAppsettings != null)
            {
                //�жϲ���ڵ��Ƿ��Ѿ�����
                if (IsExist(strKey))
                {
                    throw new Exception("�ڵ��Ѿ�����!");
                }

                //����ڵ�
                objAppsettings.Settings.Add(strKey, strValue);

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// �����������ýڵ�
        /// </summary>
        /// <param name="hasItems">�ڵ㼯��</param>
        public virtual void Insert(Hashtable hasItems)
        {
            if (objAppsettings != null)
            {
                //ѭ��HashTable
                foreach (System.Collections.DictionaryEntry item in hasItems)
                {
                    if (!IsExist(item.Key.ToString()))
                        //����Ӧ�Ľڵ㸳ֵ
                        objAppsettings.Settings.Add(item.Key.ToString(), item.Value.ToString());
                }

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// �Ƴ������ļ��ڵ�
        /// </summary>
        /// <param name="strKey">�ڵ��key</param>
        public virtual void Remove(string strKey)
        {
            if (objAppsettings.Settings != null)
            {
                //�Ƴ�key��Ӧ�Ľڵ�
                objAppsettings.Settings.Remove(strKey);

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// �����������ýڵ�
        /// </summary>
        /// <param name="hasItems">�ڵ㼯��</param>
        public virtual void Remove(Hashtable hasItems)
        {
            if (objAppsettings != null)
            {
                //ѭ��HashTable
                foreach (System.Collections.DictionaryEntry item in hasItems)
                {
                    if (IsExist(item.Key.ToString()))
                        //����Ӧ�Ľڵ㸳ֵ
                        objAppsettings.Settings.Remove(item.Key.ToString());
                }

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// ���½ڵ�
        /// </summary>
        /// <param name="strKey">Ҫ���½ڵ��Ӧkey</param>
        /// <param name="strValue">���µ�ֵ</param>
        public virtual void Update(string strKey, string strValue)
        {
            if (objAppsettings != null)
            {
                //����Ӧ�Ľڵ㸳ֵ
                objAppsettings.Settings[strKey].Value = strValue;

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// �����������ýڵ�
        /// </summary>
        /// <param name="hasItems">�ڵ㼯��</param>
        public virtual void Update(Hashtable hasItems)
        {
            if (objAppsettings != null)
            {
                //ѭ��HashTable
                foreach (System.Collections.DictionaryEntry item in hasItems)
                {
                    //����Ӧ�Ľڵ㸳ֵ
                    objAppsettings.Settings[item.Key.ToString()].Value = item.Value.ToString();
                }

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// �жϽڵ��Ƿ����
        /// </summary>
        /// <param name="strKey">�ڵ��key</param>
        /// <returns></returns>
        public virtual bool IsExist(string strKey)
        {
            if (objAppsettings != null)
            {
                if (objAppsettings.Settings[strKey] != null)
                    return true;
            }

            return false;
        }

        #endregion

        #region ���󷽷�

        /// <summary>
        /// ʵ����������
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// ���������ļ������ýڵ�
        /// </summary>
        public virtual void EncryptAppSettings()
        {
           if (!objAppsettings.SectionInformation.IsProtected)
            {
                //ѡ�Ӽ����ṩ��ʽ
                objAppsettings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                objAppsettings.SectionInformation.ForceSave = true;

                //������ܺ�������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        public virtual void DecryptAppSettings()
        {
            //�ж��Ƿ����
            if (objAppsettings.SectionInformation.IsProtected)
            {
                //���������ļ�
                objAppsettings.SectionInformation.UnprotectSection();
                objAppsettings.SectionInformation.ForceSave = true;

                //���������ļ�
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
        }

        #endregion
    }
}
