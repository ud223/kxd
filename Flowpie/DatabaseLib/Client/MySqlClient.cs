using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DatabaseLib.Client
{
    /// <summary>
    /// MySql���ݲ�����
    /// </summary>
    public class MySqlClient : IDatabase
    {
        private StringBuilder _sConnectionString = new StringBuilder();

        /// <summary>
        /// ��ȡ���ݿ������ַ���
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                if (_sConnectionString == null || _sConnectionString.Length == 0 || this._sConnectionString.ToString().IndexOf(this.DatabaseName) < 0)
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
            _sConnectionString.Append("Server=");
            _sConnectionString.Append(this.Server);
            _sConnectionString.Append(";Database=");
            _sConnectionString.Append(this.DatabaseName);
            _sConnectionString.Append(";User ID=");
            _sConnectionString.Append(this.Uid);
            _sConnectionString.Append(";password=");
            _sConnectionString.Append(this.Password);
            _sConnectionString.Append(";Port=");
            _sConnectionString.Append(this.Port);
            _sConnectionString.Append(";pooling=false;charset=utf8");
        }

        protected override IDbConnection CreateConnection()
        {
            return new MySqlConnection(this.ConnectionString); ////
        }

        protected override IDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter(this.SqlText, (MySqlConnection)this.Connection);
        }

        protected override IDataReader CreateDataReader()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IDbCommand CreateCommand()
        {
            return new MySqlCommand(this.SqlText, (MySqlConnection)this.Connection);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        protected override IDbTransaction CreateTransaction()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //public override DataSet Query()
        //{
        //    //MySqlCommand cmd = new MySqlCommand("set names gb2312", this.Connection);
        //    //ʵ��һ����������
        //    DataSet dDataSet = new DataSet();

        //    //������Լ�������������ѯ���ݼ�
        //    this.DataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

        //    try
        //    {
        //        this.Open();

        //        cmd.ExecuteNonQuery();

        //        //�������
        //        this.Count = this.DataAdapter.Fill(dDataSet);

        //        this.Result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //�쳣����յ���������
        //        dDataSet = null;

        //        //��ȡ�쳣��Ϣ
        //        this.Message = ex.Message;
        //    }
        //    finally
        //    {
        //        //�ر����ݿ�����
        //        this.Close();
        //    }

        //    return dDataSet;
        //}

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="pParams"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public override DataSet Query(IDataParameter[] pParams, CommandType commandType)
        {
            MySqlCommand dCommand = (MySqlCommand)this.CreateCommand(pParams, commandType);

            MySqlDataAdapter dDataAdapter = new MySqlDataAdapter(dCommand);

            //������Լ�������������ѯ���ݼ�
            dDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            DataSet ds = new DataSet();

            try
            {
                this.Count = dDataAdapter.Fill(ds);
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
        /// ��������
        /// </summary>
        /// <param name="ParaName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override IDataParameter MakeParam(string ParaName, object Value)
        {
            return new MySqlParameter(ParaName, Value);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Direction"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override IDataParameter MakeParam(string ParamName, DbType DbType, int Size, ParameterDirection Direction, object Value)
        {
            MySqlParameter pParam;

            //�жϲ����Ƿ��д�С���ã�-1Ϊû��
            if (Size != -1)
                pParam = new MySqlParameter(ParamName, this.ConvertToSqlDbType(DbType));
            else
                pParam = new MySqlParameter(ParamName, this.ConvertToSqlDbType(DbType), Size);

            pParam.Direction = Direction;

            //�жϲ������������뻹���������
            if (!(Direction == ParameterDirection.Output && Value == null))
                pParam.Value = Value;

            return pParam;
        }

        /// <summary>
        /// ����SQL�������
        /// </summary>
        /// <param name="pParams"></param>
        /// <param name="commondType"></param>
        /// <returns></returns>
        public override IDbCommand CreateCommand(IDataParameter[] pParams, CommandType commondType)
        {
            return new MySqlCommand(this.SqlText, (MySqlConnection)this.Connection);
        }

        public override void IsCanConnection()
        {
            throw new Exception("û��ʵ�ָ÷���!");
        }

        public override DataSet Query(int nStart, int nEnd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //protected override void Execute(IDbCommand cmd)
        //{
        //    try
        //    {
        //        //�����ݿ�
        //        this.Open();

        //        MySQLCommand cmdChar = new MySQLCommand("set charset gb2312", (MySQLConnection)cmd.Connection);

        //        cmdChar.ExecuteNonQuery();

        //        //ִ��SQL���
        //        this.Count = cmd.ExecuteNonQuery();

        //        this.Result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //�׳��쳣
        //        this.Message = ex.Message;
        //    }
        //    finally
        //    {
        //        //�ر����ݿ�
        //        this.Close();
        //    }
        //}

        /// <summary>
        /// ��DbType����ת��ΪSqlDbType����
        /// </summary>
        /// <param name="pSourceType">DbType����</param>
        /// <returns>ת�����SqlDbType����</returns>
        private MySqlDbType ConvertToSqlDbType(DbType pSourceType)
        {
            MySqlParameter paraConver = new MySqlParameter();

            paraConver.DbType = pSourceType;

            return paraConver.MySqlDbType;
        }
    }
}
