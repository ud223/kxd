using System;

namespace DatabaseLib
{
    /// <summary>
    /// ���ݿ�����ö��
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// û��ָ�����ݿ�
        /// </summary>
        None = 0,
        /// <summary>
        /// Sql2000���ݿ�
        /// </summary>
        SqlClient = 1,
        /// <summary>
        /// Oracle���ݿ�
        /// </summary>
        OracleClient = 2,
        /// <summary>
        /// MySql���ݿ�
        /// </summary>
        MySqlClient = 3
    }
}
