using System;
using System.Data;

namespace ToolsLib.Data
{
    public interface IDatabase
    {
        /// <summary>
        /// �Ѷ�ȡ��DataReader����
        /// </summary>
        System.Data.IDataReader DataReader
        { get; }

        string ConnectionString { get; set; }

        /// <summary>
        /// ���ز��������Ϣ
        /// </summary>
        string Message { get; }

        /// <summary>
        /// ���ز������
        /// </summary>
        bool Result { get; }

        /// <summary>
        /// �����ݿ�����
        /// </summary>
        void Open();

        /// <summary>
        /// �ر����в�����Դ
        /// </summary>
        void Close();

        /// <summary>
        /// ��DataReader�ķ�ʽ��ȡ���ݿ�
        /// </summary>
        /// <param name="strSql"></param>
        void Read(string strSql);

        /// <summary>
        /// ��ѯ���ݼ���
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        DataSet Query(string strSql);

        /// <summary>
        /// ִ�����ݿ����
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        int Execute(string strSql);

        /// <summary>
        /// ִ�����ݿ����
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="oParams"></param>
        /// <returns></returns>
        int Execute(string strSql, IDataParameter[] oParams);

        /// <summary>
        /// ���ص��������ַ���
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        object ExecuteScalarSQL(string strSql);

        /// <summary>
        /// �����洢���̲���
        /// </summary>
        /// <param name="ParamName">��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Direction">��������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>����һ���µĲ���</returns>
        IDataParameter MakeParam(string ParamName, object Value);
    }
}
