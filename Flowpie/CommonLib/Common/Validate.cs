using System;
using System.Text.RegularExpressions;

namespace CommonLib.Common
{
    /// <summary>
    /// ��֤�ж���
    /// </summary>
    public class Validate
    {
        #region Validate Condition ��֤����

        private static string _sNumber = "^[0-9]+$";

        /// <summary>
        /// �����������ַ�����ʽ
        /// </summary>
        public static string Number
        { get { return _sNumber; } }

        private static string _sNumberSign = "^[+-]?[0-9]+$";

        /// <summary>
        /// �з������������ַ�����ʽ
        /// </summary>
        public static string NumberSign
        { get { return _sNumberSign; } }

        private static string _sDecimal = "^[0-9]+[.]?[0-9]+$";

        /// <summary>
        /// �����������ַ�����ʽ
        /// </summary>
        public static string Decimal
        { get { return _sDecimal; } }

        private static string _sDecimalSign = "^[+-]?[0-9]+[.]?[0-9]+$";

        /// <summary>
        /// �з��Ÿ����������ַ�����ʽ
        /// </summary>
        public static string DecimalSign
        { get { return _sDecimalSign; } }

        private static string _sEmail = "^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$";

        /// <summary>
        /// �������������ַ�����ʽ
        /// </summary>
        public static string Email
        { get { return _sEmail; } }

        private static string _sCHZN = "[\u4e00-\u9fa5]";

        /// <summary>
        /// ���ļ��������ַ�����ʽ
        /// </summary>
        public static string CHZN
        { get { return _sCHZN; } }

        private static string _sChar = "^[A-Za-z]+$";

        /// <summary>
        /// Ӣ���ַ������ַ�����ʽ
        /// </summary>
        public static string Char
        { get { return _sChar; } }

        private static string _sBigChar = "^[A-Z]+$";

        /// <summary>
        /// ��д�ַ������ַ�����ʽ
        /// </summary>
        public static string BigChar
        { get { return _sBigChar; } }

        private static string _sSmallChar = "^[a-z]+$";

        /// <summary>
        /// Сд�ַ������ַ�����ʽ
        /// </summary>
        public static string SmallChar
        { get { return _sSmallChar; } }

        private static string _sCall = "^\\w+$";

        /// <summary>
        /// �绰���������ַ�����ʽ
        /// </summary>
        public static string Call
        { get { return _sCall; } }

        private static string _sUrl = "^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$";

        /// <summary>
        /// ��ַ�����ַ�����ʽ
        /// </summary>
        public static string Url
        { get { return _sUrl; } }

        private static Regex RegNumber = new Regex(_sNumber);

        private static Regex RegNumberSign = new Regex(_sNumberSign);

        private static Regex RegDecimal = new Regex(_sDecimal);

        private static Regex RegDecimalSign = new Regex(_sDecimalSign); //�ȼ���^[+-]?\d+[.]?\d+$ 

        private static Regex RegEmail = new Regex(_sEmail);//w Ӣ����ĸ�����ֵ��ַ������� [a-zA-Z0-9] �﷨һ�� 

        private static Regex RegCHZN = new Regex(_sCHZN);

        private static Regex RegexChar = new Regex(_sChar);

        private static Regex RegexBigChar = new Regex(_sBigChar);

        private static Regex RegexSmallChar = new Regex(_sSmallChar);

        private static Regex RegexCall = new Regex(_sCall);

        private static Regex RegexUrl = new Regex(_sUrl);

        #endregion

        #region �����ַ������

        /// <summary> 
        /// �Ƿ������ַ��� 
        /// </summary> 
        /// <param name="inputData">�����ַ���</param> 
        /// <returns></returns> 
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);

            return m.Success;
        }

        /// <summary> 
        /// �Ƿ������ַ����ɴ������� 
        /// </summary> 
        /// <param name="inputData">�����ַ���</param> 
        /// <returns></returns> 
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);

            return m.Success;
        }



        /// <summary> 
        /// �Ƿ��Ǹ����� 
        /// </summary> 
        /// <param name="inputData">�����ַ���</param> 
        /// <returns></returns> 
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);

            return m.Success;
        }



        /// <summary> 
        /// �Ƿ��Ǹ������ɴ������� 
        /// </summary> 
        /// <param name="inputData">�����ַ���</param> 
        /// <returns></returns> 
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);

            return m.Success;
        }

        #endregion

        #region Ӣ���ַ����

        /// <summary>
        /// �Ƿ���26����ĸ���
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsChar(string inputData)
        {
            Match m = RegexChar.Match(inputData);

            return m.Success;
        }


        /// <summary>
        /// �Ƿ�����26����д��ĸ���
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsBigChar(string inputData)
        {
            Match m = RegexBigChar.Match(inputData);

            return m.Success;
        }

        /// <summary>
        /// �Ƿ�����26��Сд��ĸ���
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsSmallChar(string inputData)
        {
            Match m = RegexSmallChar.Match(inputData);

            return m.Success;
        }

        /// <summary>
        /// �Ƿ�������ĸ�����»������
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsCall(string inputData)
        {
            Match m = RegexCall.Match(inputData);

            return m.Success;
        }

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsUrl(string inputData)
        {
            Match m = RegexUrl.Match(inputData);

            return m.Success;
        }

        #endregion

        #region ���ļ��

        /// <summary> 
        /// ����Ƿ��������ַ� 
        /// </summary> 
        /// <param name="inputData"></param> 
        /// <returns></returns> 
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);

            return m.Success;
        }

        #endregion

        #region �ʼ���ַ

        /// <summary> 
        /// �Ƿ���ȷ���ʼ���ַ 
        /// </summary> 
        /// <param name="inputData">�����ַ���</param> 
        /// <returns></returns> 
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);

            return m.Success;
        }

        #endregion

        #region ����

        /// <summary>
        /// �ж��ַ��������Ƿ�Ϸ�
        /// </summary>
        /// <param name="strTxt">Ҫ�жϵ��ַ���</param>
        /// <param name="nLength">��߳���</param>
        /// <returns>1Ϊ����0ΪС��2Ϊ����</returns>
        public static int EqualLength(string strTxt, int nLength)
        {
            if (strTxt.Length > nLength)
                return 1;
            else if (strTxt.Length < nLength)
                return 0;
            else
                return 2;
        }

        /// <summary>
        /// �ж��ַ����Ƿ�Ϊ�գ�Ϊ���򷵻ؿ��ַ���
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string IsNullString(object val)
        {
            if (val == null || val == System.DBNull.Value || val.ToString().Trim() == "")
                return "";

            return val.ToString();
        }

        /// <summary>
        /// �ж��ַ����Ƿ�Ϊ�գ�Ϊ���򷵻ؿ��ַ���
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string IsNullString(object val, string replace)
        {
            if (val == null || val == System.DBNull.Value || val.ToString() == "")
                return replace;

            return val.ToString();
        }

        /// <summary>
        /// �ж��ַ��������Ƿ�Ϊ�գ�Ϊ���򷵻ؿ��ַ���
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string IsNullDateString(object val, string format)
        {
            if (val == null || val == System.DBNull.Value || val.ToString() == "")
                return "";

            if (format == null || format == "")
                return val.ToString();

            return Convert.ToDateTime(val).ToString(format);
        }

        public static string IsEnableString(object val, string value1, string value2)
        {
            if (val == null || val == System.DBNull.Value || val.ToString() == "")
                return value2;

            if (val.ToString() == "1")
                return value1;

            return value2;
        }

        public static string filterEmoji(string str)
        {
            string result = Regex.Replace(str, @"\p{Cs}", "");
            return result;
        }

        #endregion
    }
}
