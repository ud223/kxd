using System;
using System.Data;

namespace DatabaseLib
{
    /// <summary>
    /// �������ݷ�����
    /// </summary>
    public abstract class IDatabase : CommonLib.BaseClass
    {
        #region �ύ����

        private string _sSqlText = null;

        /// <summary>
        /// Ҫִ�е�SQL���
        /// </summary>
        public string SqlText
        { get { return this._sSqlText; } set { this._sSqlText = value; } }

        private string _sServer;

        /// <summary>
        /// ����·��
        /// </summary>
        public string Server
        { 
            get 
            {
                if (this._sServer == null)
                    this._sServer = CommonLib.Common.ConfigReader.Read("Server", this.SystemType);

                return this._sServer; 
            } 
            set 
            { 
                this._sServer = value; 
            } 
        }

        private string _sDatabaseName;

        /// <summary>
        /// ���ݿ���
        /// </summary>
        public string DatabaseName
        { 
            get 
            {
                if (this._sDatabaseName == null)
                    this._sDatabaseName = CommonLib.Common.ConfigReader.Read("Database", this.SystemType);

                return this._sDatabaseName; 
            } 
            set 
            { 
                this._sDatabaseName = value; 
            } 
        }

        private string _sUid;

        /// <summary>
        /// ���ݿ�����û�ID
        /// </summary>
        public string Uid
        { 
            get 
            {
                if (this._sUid == null)
                    this._sUid = CommonLib.Common.ConfigReader.Read("UID", this.SystemType);

                return this._sUid; 
            } 
            set 
            { 
                this._sUid = value; 
            } 
        }

        private string _sPassword;

        /// <summary>
        /// ���ݿ��������
        /// </summary>
        public string Password
        { 
            get 
            {
                if (this._sPassword == null)
                    this._sPassword = CommonLib.Common.ConfigReader.Read("PWD", this.SystemType);

                return this._sPassword; 
            } 
            set 
            { 
                this._sPassword = value; 
            } 
        }

        private DataType _DatabaseType;

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DataType DatabaseType
        { 
            get 
            {
                if (this._DatabaseType == DatabaseLib.DataType.None)
                    this._DatabaseType = (DatabaseLib.DataType)Int32.Parse(CommonLib.Common.ConfigReader.Read("DataType", this.SystemType));

                return this._DatabaseType; 
            } 
            set 
            { 
                this._DatabaseType = value; 
            } 
        }

        private string _sPort;

        /// <summary>
        /// ���ݿ�˿ں�
        /// </summary>
        public string Port
        {
            get
            {
                if (this._sPort == null || this._sPort == "")
                    this._sPort = CommonLib.Common.ConfigReader.Read("Port", this.SystemType);

                return this._sPort;
            }
            set
            {
                this._sPort = value;
            }
        }

        #endregion

        #region ������Ա

        private IDbConnection _con;

        /// <summary>
        /// ��������
        /// </summary>
        protected IDbConnection Connection
        {
            get
            {
                if (this._con == null || this._con.ConnectionString.IndexOf(this.DatabaseName) < 0)
                    this._con = CreateConnection();

                return this._con;
            }
            set
            {
                this._con = value;
            }
        }

        //private IDbCommand _cmd;

        //protected IDbCommand Command
        //{
        //    get
        //    {
        //        if (this._con == null)
        //            this._cmd = CreateCommand();

        //        return this._cmd;
        //    }
        //    set
        //    {
        //        this._cmd = value;
        //    }
        //}

        private IDataAdapter _adapter;

        /// <summary>
        /// ���ݶ�ȡ��
        /// </summary>
        protected IDataAdapter DataAdapter
        {
            get
            {
                this._adapter = CreateDataAdapter();

                return _adapter;
            }
            set
            {
                this._adapter = value;
            }
        }

        private IDataReader _dataReader;

        /// <summary>
        /// ���ݶ�ȡ��
        /// </summary>
        public IDataReader DataReader
        { get { return this._dataReader; } set { this._dataReader = value; } }

        private string _sConnectionString;


        public virtual string ConnectionString
        { get { return this._sConnectionString; } set { this._sConnectionString = value; } }

        private int _nCount = -1;

        /// <summary>
        /// ������¼������
        /// </summary>
        public int Count
        { get { return this._nCount; } set { this._nCount = value; } }

        /// <summary>
        /// �����Ƿ�ɹ�
        /// </summary>
        public new bool Result
        { get { return base.Result; } set { base.Result = value; } }

        /// <summary>
        /// �ڲ�������Ϣ
        /// </summary>
        public new string Message
        { get { return base.Message; } set { base.Message = value; } }

        #endregion

        #region ���󷽷�

        /// <summary>
        /// �������ݿ����Ӷ���
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection CreateConnection();

        /// <summary>
        /// �������������
        /// </summary>
        /// <param name="con">���ݿ����Ӷ���</param>
        /// <returns></returns>
        protected abstract IDataAdapter CreateDataAdapter();

        /// <summary>
        /// ����DataReader����
        /// </summary>
        /// <returns></returns>
        protected abstract IDataReader CreateDataReader();

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        protected abstract IDbCommand CreateCommand();

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        protected abstract IDbTransaction CreateTransaction();

        /// <summary>
        /// �ô洢���̲�ѯ���ݷ���һ�����ݼ� 
        /// </summary>
        /// <returns>����һ����ͼ��������</returns>
        public abstract DataSet Query(IDataParameter[] pParams, CommandType commandType);

        /// <summary>
        /// ��ѯ�ض���Χ�����ݼ�(��Ч)
        /// </summary>
        /// <param name="nStart">����ͷ</param>
        /// <param name="nEnd">����β</param>
        /// <returns>����һ����ͼ��������</returns>
        public abstract DataSet Query(int nStart, int nEnd);

        #endregion

        #region ���߷���

        /// <summary>
        /// �����洢���̼��ײ���
        /// </summary>
        /// <param name="ParaName">������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns></returns>
        public abstract IDataParameter MakeParam(string ParaName, object Value);

        /// <summary>
        /// �����洢���̲���
        /// </summary>
        /// <param name="ParamName">��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Direction">��������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>����һ���µĲ���</returns>
        public abstract IDataParameter MakeParam(string ParamName, DbType DbType, Int32 Size, ParameterDirection Direction, object Value);

        /// <summary>
        /// �ر��������Ӷ���
        /// </summary>
        public virtual void Close()
        {
            if (this._dataReader != null && !this._dataReader.IsClosed)
                this._dataReader.Close();

            if (this._con.State == ConnectionState.Open)
                this._con.Close();

            if (this._con != null)
                this._con.Close();
        }

        /// <summary>
        /// �����������ݿ����Ӷ���
        /// </summary>
        public virtual void Open()
        {
            if (this.Connection.State == ConnectionState.Closed)
                this.Connection.Open();
        }

        /// <summary>
        /// �ͷ����ݿ���Դ
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// �����洢��������ʹ�ö���
        /// </summary>
        /// <param name="procName">������</param>
        /// <param name="prams">���̲�������</param>
        /// <returns>�������</returns>
        public virtual IDbCommand CreateCommand(IDataParameter[] pParams, CommandType commondType)
        {
            IDbCommand dCommand = this.CreateCommand();

            //����һ�����ݿ�����������
            //�����ݿ���������������͸��洢��������
            dCommand.CommandType = commondType;

            //ѭ����ӹ��̲���
            if (pParams != null)
            {
                foreach (IDataParameter parameter in pParams)
                    dCommand.Parameters.Add(parameter);
            }

            // ��Ӵ����Ĺ��̲�������
            dCommand.Parameters.Add(this.MakeReturnParam("ReturnValue", DbType.Int32, 4));
            //���ع��̶���
            return dCommand;
        }

        /// <summary>
        /// �����洢��������ʹ�ö���
        /// </summary>
        /// <param name="procName">������</param>
        /// <param name="prams">���̲�������</param>
        /// <returns>�������</returns>
        public virtual IDbCommand HashCreateCommand(System.Collections.Hashtable pParams, CommandType commondType)
        {
            IDbCommand dCommand = this.CreateCommand();

            //����һ�����ݿ�����������
            //�����ݿ���������������͸��洢��������
            dCommand.CommandType = commondType;

            //ѭ����ӹ��̲���
            if (pParams != null)
            {
                foreach (System.Collections.DictionaryEntry parameter in pParams)
                    dCommand.Parameters.Add(MakeParam("@" + parameter.Key,parameter.Value));
            }

            // ��Ӵ����Ĺ��̲�������
            dCommand.Parameters.Add(this.MakeReturnParam("ReturnValue", DbType.Int32, 4));
            //���ع��̶���
            return dCommand;
        }

        /// <summary>
        /// ����һ���洢�����������
        /// </summary>
        /// <param name="ParamName">��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>����һ������</returns>
        public virtual IDataParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value); ;
        }

        /// <summary>
        /// ����һ���洢���̷��ز���
        /// </summary>
        /// <param name="ParamName">��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns>����һ������</returns>
        public virtual IDataParameter MakeOutParam(string ParamName, DbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// �������̷���ֵ
        /// </summary>
        /// <param name="ParaName">��������</param>
        /// <param name="DbType">�������</param>
        /// <param name="Size">������С</param>
        /// <returns>����ֵ</returns>
        public virtual IDataParameter MakeReturnParam(string ParaName, DbType DbType, int Size)
        {
            return MakeParam(ParaName, DbType, Size, ParameterDirection.ReturnValue, null);
        }

        #endregion

        #region ��ѯ����

        /// <summary>
        /// ��SQL����ѯ���ݷ���һ����ͼ 
        /// </summary>
        /// <returns>����һ����ͼ��������</returns>
        public virtual DataSet Query()
        {
            //ʵ��һ����������
            DataSet dDataSet = new DataSet();

            //������Լ�������������ѯ���ݼ�
            this.DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            try
            {
                //�������
                this._nCount = this.DataAdapter.Fill(dDataSet);

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                //�쳣����յ���������
                dDataSet = null;

                //��ȡ�쳣��Ϣ
                this.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                //�ر����ݿ�����
                this.Close();
            }

            return dDataSet;
        }

        /// <summary>
        /// �ô洢���̲�ѯ���ݼ�
        /// </summary>
        /// <param name="procName">�洢��������</param>
        /// <param name="dDataReader">DataReader��������</param>
        public virtual void Read(CommandType commandType)
        {
            //�洢���̲�ѯ����
            ReadData(null,commandType);
        }

        /// <summary>
        /// �ô������Ĵ洢���̲�ѯ���ݼ�
        /// </summary>
        /// <param name="prams">�洢���̲���</param>
        public virtual void Read(IDataParameter[] pParams, CommandType commandType)
        {
            //�洢���̲�ѯ����
            ReadData(pParams, commandType);
        }

        /// <summary>
        /// �ô������Ĵ洢���̲�ѯ���ݼ�
        /// </summary>
        /// <param name="prams">�洢���̲���</param>
        public virtual void Read(System.Collections.Hashtable pParams, CommandType commandType)
        {
            //�洢���̲�ѯ����
            HashReadData(pParams, commandType);
        }

        #endregion

        #region ��������

        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="cmd">Ҫִ�е�Command����</param>
        protected virtual void Execute(IDbCommand cmd)
        {
            try
            {
                //�����ݿ�
                this.Open();

                //ִ��SQL���
                this._nCount = cmd.ExecuteNonQuery();

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                //�׳��쳣
                this.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                //�ر����ݿ�
                this.Close();
            }
        }

        public virtual string ExecuteScalar(IDbCommand cmd)
        {
            string strValue = "";

            try
            {
                //�����ݿ�
                this.Open();

                //ִ��SQL���
                strValue = cmd.ExecuteScalar().ToString();

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                //�׳��쳣
                this.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                //�ر����ݿ�
                this.Close();
            }

            return strValue;
        }

        /// <summary>
        /// ��SQL����������
        /// </summary>
        public virtual void Execute(CommandType commandType)
        {
            //�������ݿ��������
            IDbCommand dCommand = this.CreateCommand(null,commandType);

            //���в�����Command�������ִ�д���
            Execute(dCommand);
        }

        /// <summary>
        /// ������ֵ��ִ�����
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual string ExecuteScalar(CommandType commandType)
        {
            //�������ݿ��������
            IDbCommand dCommand = this.CreateCommand(null, commandType);

            //���в�����Command�������ִ�д���
            return ExecuteScalar(dCommand);
        }

        /// <summary>
        /// �ô洢���̲�������
        /// </summary>
        /// <param name="prams">�洢���̲���</param>
        /// <returns>����Ӱ�������</returns>
        public virtual void  Execute(IDataParameter[] pParams, CommandType commandType)
        {
            //�������ݿ��������
            IDbCommand dCommand = this.CreateCommand(pParams, commandType);

            //���в�����Command�������ִ�д���
            Execute(dCommand);
        }

        /// <summary>
        /// �������Ĵ洢���̲�������
        /// </summary>
        /// <param name="prams">�洢���̲���</param>
        /// <returns>����Ӱ�������</returns>
        public virtual string ExecuteScalar(IDataParameter[] pParams, CommandType commandType)
        {
            //�������ݿ��������
            IDbCommand dCommand = this.CreateCommand(pParams, commandType);

            //���в�����Command�������ִ�д���
            return ExecuteScalar(dCommand);
        }

        /// <summary>
        /// �ô洢���̲�������
        /// </summary>
        /// <param name="prams">�洢���̲���</param>
        /// <returns>����Ӱ�������</returns>
        public virtual void Execute(System.Collections.Hashtable pParams, CommandType commandType)
        {
            //�������ݿ��������
            IDbCommand dCommand = this.HashCreateCommand(pParams, commandType);

            //���в�����Command�������ִ�д���
            Execute(dCommand);
        }

        /// <summary>
        /// �������洢���̲�������
        /// </summary>
        /// <param name="prams">�洢���̲���</param>
        /// <returns>����Ӱ�������</returns>
        public virtual string ExecuteScalar(System.Collections.Hashtable pParams, CommandType commandType)
        {
            //�������ݿ��������
            IDbCommand dCommand = this.HashCreateCommand(pParams, commandType);

            //���в�����Command�������ִ�д���
            return ExecuteScalar(dCommand);
        }

        /// <summary>
        /// ������ִ�д洢���̲�����һ��DataReader�����ݼ���
        /// </summary>
        /// <param name="prams">��������</param>
        public void ReadData(IDataParameter[] pParams, CommandType commandType)
        {
            IDbCommand dCommand = this.CreateCommand(pParams, commandType);

            try
            {
                //�����ݿ�
                this.Open();

                //ִ�в��������ݶ�ȡ��
                this._dataReader = dCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;

                this.Result = false;
            }
        }

        /// <summary>
        /// ������ִ�д洢���̲�����һ��DataReader�����ݼ���
        /// </summary>
        /// <param name="prams">��������</param>
        public void HashReadData(System.Collections.Hashtable pParams, CommandType commandType)
        {
            IDbCommand dCommand = this.HashCreateCommand(pParams, commandType);

            try
            {
                //�����ݿ�
                this.Open();

                //ִ�в��������ݶ�ȡ��
                this._dataReader = dCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;

                this.Result = false;
            }
        }

        #endregion

        #region �ж������Ƿ����

        /// <summary>
        /// ��SQL����ѯ�Ƿ�����ͬ�ļ�¼
        /// </summary>
        /// <returns>����һ��true or false</returns>
        public virtual bool IsExist(CommandType commandType)
        {
            //�������ΪĬ�ϲ���ͬ
            bool bResult = false;

            try
            {
                //����ִ�ж�ȡ������
                this.Read(commandType);

                //�ж��Ƿ���ͬ
                if (this._dataReader.Read())
                    bResult = true;

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                //�׳��쳣
                this.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                //�ر���������
                this.Close();
            }

            //���ؽ��
            return bResult;
        }

        /// <summary>
        /// �ô洢���̲�ѯ�Ƿ�����ͬ�ļ�¼
        /// </summary>
        /// <param name="prams">��������</param>
        /// <returns>����һ��true or false</returns>
        public virtual bool IsExist(IDataParameter[] pParams,CommandType commandType)
        {
            //�������ΪĬ�ϲ���ͬ
            bool bResult = false;

            try
            {
                //����ִ�ж�ȡ������
                this.Read(pParams,commandType);

                //�ж��Ƿ���ͬ
                if (this._dataReader.Read())
                    bResult = true;

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                //�׳��쳣
                base.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                //�ر���������
                this.Close();
            }

            //���ؽ��
            return bResult;
        }

        /// <summary>
        /// �ô洢���̲�ѯ�Ƿ�����ͬ�ļ�¼
        /// </summary>
        /// <param name="prams">��������</param>
        /// <returns>����һ��true or false</returns>
        public virtual bool IsExist(System.Collections.Hashtable pParams, CommandType commandType)
        {
            //�������ΪĬ�ϲ���ͬ
            bool bResult = false;

            try
            {
                //����ִ�ж�ȡ������
                this.Read(pParams, commandType);

                //�ж��Ƿ���ͬ
                if (this._dataReader.Read())
                    bResult = true;

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                //�׳��쳣
                base.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                //�ر���������
                this.Close();
            }

            //���ؽ��
            return bResult;
        }

        #endregion

        #region ���ݿ��ڲ�����

        /// <summary>
        /// �����µ����ݿ�
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual string CreateDatabase(CommandType commandType)
        {
            throw new Exception("Null Function");
        }

        /// <summary>
        /// �Ƿ����������ݿ�
        /// </summary>
        /// <returns></returns>
        public virtual void IsCanConnection()
        {
            throw new Exception("Null Function");
        }

        #endregion
    }
}
