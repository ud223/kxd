using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DatabaseLib.Client
{
    /// <summary>
    /// Sql2000���ݿ������
    /// </summary>
    public class SqlClient : IDatabase
    {
        #region ���ݳ�Ա

        private StringBuilder _sConnectionString = null;

        /// <summary>
        /// ��ȡ���ݿ������ַ���
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                if (_sConnectionString == null || _sConnectionString.Length == 0)
                    GetConnectionString();

                return _sConnectionString.ToString();
            }
            set
            {
                if (this._sConnectionString.Length > 0)
                    this._sConnectionString.Remove(0, this._sConnectionString.Length);

                this._sConnectionString.Append(value);
            }
        }

        #endregion

        #region ���߷���

        private void GetConnectionString()
        {
            if (this._sConnectionString == null)
            {
                //ʵ�������Ӷ���
                _sConnectionString = new StringBuilder();
            }
            else
            {
                this._sConnectionString.Remove(0, this._sConnectionString.Length);
            }

            //ƴ�����ݿ����Ӷ���
            _sConnectionString.Append("server=");
            _sConnectionString.Append(this.Server);
            _sConnectionString.Append(";uid=");
            _sConnectionString.Append(this.Uid);
            _sConnectionString.Append(";pwd=");
            _sConnectionString.Append(this.Password);
            _sConnectionString.Append(";database=");
            _sConnectionString.Append(this.DatabaseName);
        }

        /// <summary>
        /// �������״洢���̲���
        /// </summary>
        /// <param name="ParaName">������</param>
        /// <param name="Value">������ֵ</param>
        /// <returns></returns>
        public override IDataParameter MakeParam(string ParaName, object Value)
        {
            return new SqlParameter(ParaName, Value);
        }

        /// <summary>
        /// ��DbType����ת��ΪSqlDbType����
        /// </summary>
        /// <param name="pSourceType">DbType����</param>
        /// <returns>ת�����SqlDbType����</returns>
        private SqlDbType ConvertToSqlDbType(DbType pSourceType)
        {
            SqlParameter paraConver = new SqlParameter();

            paraConver.DbType = pSourceType;

            return paraConver.SqlDbType;
        }

        #endregion

        #region ʵ�ֵļ̳еĳ��󷽷�

        /// <summary>
        /// ����һ�����Ӷ���
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection CreateConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        /// <summary>
        /// ����һ�����������
        /// </summary>
        /// <returns></returns>
        protected override IDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter(this.SqlText, (SqlConnection)this.Connection);
        }

        /// <summary>
        /// ����һ�����ݶ�ȡ��
        /// </summary>
        /// <returns></returns>
        protected override IDataReader CreateDataReader()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ����һ������ִ�ж���
        /// </summary>
        /// <returns></returns>
        protected override IDbCommand CreateCommand()
        {
            return new SqlCommand(this.SqlText, (SqlConnection)this.Connection);
        }

        /// <summary>
        /// �������ݿ��������
        /// </summary>
        /// <returns></returns>
        protected override IDbTransaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Command��ʽ��ѯ����
        /// </summary>
        /// <param name="pParams"></param>
        /// <returns></returns>
        public override DataSet Query(IDataParameter[] pParams,CommandType commandType)
        {
            SqlCommand dCommand = (SqlCommand)this.CreateCommand(pParams, commandType);

            SqlDataAdapter dDataAdapter = new SqlDataAdapter(dCommand);

            //������Լ�������������ѯ���ݼ�
            dDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            DataSet ds = new DataSet();

            try
            {
                this.Count = dDataAdapter.Fill(ds);

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                this.Close();
            }

            return ds;
        }

        /// <summary>
        /// ��ѯ��������
        /// </summary>
        /// <param name="nStart">��ѯ��¼����ʵλ��</param>
        /// <param name="nEnd">��ѯ��¼�Ľ���λ��</param>
        /// <returns></returns>
        public override DataSet Query(int nStart, int nEnd)
        {
            SqlDataAdapter dDataAdapter = new SqlDataAdapter(this.SqlText, (SqlConnection)this.Connection);

            //������Լ�������������ѯ���ݼ�
            dDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            DataSet ds = new DataSet();

            try
            {
                this.Count = dDataAdapter.Fill(ds, nStart, nEnd, "temp");

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;

                this.Result = false;
            }
            finally
            {
                this.Close();
            }

            return ds;
        }

        /// <summary>
        /// �����洢���̲���
        /// </summary>
        /// <param name="ParamName">������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">���ݴ�С</param>
        /// <param name="Direction">���ݷ���</param>
        /// <param name="Value">����ֵ</param>
        /// <returns></returns>
        public override IDataParameter MakeParam(string ParamName, DbType DbType, int Size, ParameterDirection Direction, object Value)
        {
            SqlParameter pParam;

            //�жϲ����Ƿ��д�С���ã�-1Ϊû��
            if (Size != -1)
                pParam = new SqlParameter(ParamName, ConvertToSqlDbType(DbType), Size);
            else
                pParam = new SqlParameter(ParamName, ConvertToSqlDbType(DbType));

            pParam.Direction = Direction;

            //�жϲ������������뻹���������
            if (!(Direction == ParameterDirection.Output && Value == null))
                pParam.Value = Value;

            return pParam;
        }

        /// <summary>
        /// �����µ����ݿ�
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public override string CreateDatabase(CommandType commandType)
        {
            string strConnectonString = "DATA SOURCE=@Server;USER ID=@Uid;PASSWORD=@Pwd;PERSIST SECURITY INFO=FALSE;PACKET SIZE=4096";
            string strSql = "CREATE DATABASE @DatabaseName";

            strConnectonString = strConnectonString.Replace("@Server", this.Server);
            strConnectonString = strConnectonString.Replace("@Uid", this.Uid);
            strConnectonString = strConnectonString.Replace("@Pwd", this.Password);

            strSql = strSql.Replace("@DatabaseName", this.DatabaseName);

            this.Connection.ConnectionString = strConnectonString;

            //�������ݿ��������
            IDbCommand dCommand = this.CreateCommand(null, commandType);

            dCommand.Connection = this.Connection;
            dCommand.CommandText = strSql;

            try
            {
                dCommand.Connection.Open();

                dCommand.ExecuteNonQuery();

                this.Message = "�����ɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                this.Result = false;
            }
            finally
            {
                this.Close();
            }

            return strConnectonString;
        }

        /// <summary>
        /// �Ƿ�������ָ�����ݿ�
        /// </summary>
        /// <returns></returns>
        public override void IsCanConnection()
        {
            string strConnectonString = "DATA SOURCE=@Server;USER ID=@Uid;PASSWORD=@Pwd;@DatabaseName;PERSIST SECURITY INFO=FALSE;PACKET SIZE=4096";

            strConnectonString = strConnectonString.Replace("@Server", this.Server);
            strConnectonString = strConnectonString.Replace("@Uid", this.Uid);
            strConnectonString = strConnectonString.Replace("@Pwd", this.Password);

            if (this.DatabaseName != null && this.DatabaseName != "")
                strConnectonString = strConnectonString.Replace("@DatabaseName", "INITIAL CATALOG=" + this.DatabaseName);
            else
                strConnectonString = strConnectonString.Replace("@DatabaseName;", "");

            this.Connection.ConnectionString = strConnectonString;

            try
            {
                this.Open();

                this.Message = "���������ӳɹ�!";

                this.Result = true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                this.Result = false;
            }
            finally
            {
                this.Close();
            }
        }

        #endregion
    }
}
