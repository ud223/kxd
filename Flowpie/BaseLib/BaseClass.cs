using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLib
{
    public class BaseClass
    {
        private string _sMessage = "�����ɹ�!";

        /// <summary>
        /// ��������ڲ���Ϣ����
        /// </summary>
        public string Message
        { get { return this._sMessage; } set { this._sMessage = value; } }

        private bool _bResult = true;

        /// <summary>
        /// ��������ڲ������������
        /// </summary>
        public bool Result
        { get { return this._bResult; } set { this._bResult = value; } }

        private SystemType _SystemType;

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public SystemType SystemType
        { get { return this._SystemType; } set { this._SystemType = value; } }

        private string _sClassName;

        /// <summary>
        /// ������
        /// </summary>
        public string ClassName
        { get { return this._sClassName; } set { this._sClassName = value; } }
    }
}
