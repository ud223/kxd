using System;
using System.Reflection;

namespace DatabaseLib
{
    /// <summary>
    /// ���ݿ⹤����
    /// </summary>
    public class DatabaseFactory : BaseLib.BaseClass
    {
        private string _sNameSpace = "DatabaseLib";
        private string _sClientPath = "Client";
        private string _sClassPath = "DatabaseLib.Client.";
        private string _sClassName = "";

        /// <summary>
        /// ��ʼ������֤���ݿ�����
        /// </summary>
        /// <returns></returns>
        public void GetDataType(BaseLib.SystemType type, string strDataType)
        {
            int nDataType = 0;

            if (strDataType == null || strDataType == "")
                strDataType = CommonLib.Common.ConfigReader.Read("DataType", type);

            if (!CommonLib.Common.Validate.IsNumber(strDataType))
            {
                this.Message = "���ݿ��������ô���!";
                this.Result = false;
            }

            nDataType = Int32.Parse(strDataType);

            _sClassName = ((DataType)nDataType).ToString();

            if (CommonLib.Common.Validate.IsNumber(_sClassName))
            {
                _sClassName = "";

                this.Message = "���ݿ��������ô���!";
                this.Result = false;
            }
        }

        /*!----------------------------------�쳣δ����-------------------------------------------*/
        public IDatabase CreateClient(BaseLib.SystemType tmpSystemType)
        {
            IDatabase dDatabase = null;

            try
            {
                //��ȡ��Ӧ�����ݿ�����
                Type ClientType = Type.GetType(_sClassPath + ((DataType)(Int32.Parse(CommonLib.Common.ConfigReader.Read("DataType", tmpSystemType)))).ToString(), true);

                //������Ӧ�����ݿ����ʵ�������ظ��ӿ�ʹ������ͨ���ӿ����ҵ����ݿ����ʵ�ִ���
                dDatabase = (IDatabase)Activator.CreateInstance(ClientType);

                dDatabase.SystemType = tmpSystemType;

                dDatabase.Server = CommonLib.Common.ConfigReader.Read("Server", tmpSystemType);
                dDatabase.Uid = CommonLib.Common.ConfigReader.Read("UID", tmpSystemType);
                dDatabase.Password = CommonLib.Common.ConfigReader.Read("PWD", tmpSystemType);
                dDatabase.DatabaseName = CommonLib.Common.ConfigReader.Read("Database", tmpSystemType);
                dDatabase.Port = CommonLib.Common.ConfigReader.Read("Port", tmpSystemType);
            }
            catch (Exception ex)
            {
                //��ȡ�����쳣�Ĵ�����Ϣ
                this.Message = ex.Message;
                this.Result = false;
            }

            //���ز���ʵ������
            return dDatabase;
        }

        public IDatabase CreateClient(DatabaseLib.Entity.DatabaseEntity dbEntity, BaseLib.SystemType tmpSystemType)
        {
            IDatabase dDatabase = null;

            try
            {
                //��ȡ��Ӧ�����ݿ�����
                Type ClientType = Type.GetType(_sClassPath + ((DataType)(Int32.Parse(dbEntity.Data_Type))).ToString(), true);

                //������Ӧ�����ݿ����ʵ�������ظ��ӿ�ʹ������ͨ���ӿ����ҵ����ݿ����ʵ�ִ���
                dDatabase = (IDatabase)Activator.CreateInstance(ClientType);

                dDatabase.SystemType = tmpSystemType;

                dDatabase.Server = dbEntity.Server;
                dDatabase.Uid = dbEntity.Uid;
                dDatabase.Password = dbEntity.Pwd;
                dDatabase.DatabaseName = dbEntity.Name;
                dDatabase.Port = dbEntity.Port;
            }
            catch (Exception ex)
            {
                //��ȡ�����쳣�Ĵ�����Ϣ
                this.Message = ex.Message;
                this.Result = false;
            }

            //���ز���ʵ������
            return dDatabase;
        }
    }
}
