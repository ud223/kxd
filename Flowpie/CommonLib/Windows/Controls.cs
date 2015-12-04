using System;

namespace CommonLib.Windows
{
    /// <summary>
    /// Windows����������
    /// </summary>
    public class Controls
    {
        public delegate object CustomEventHandler(System.Windows.Forms.Control control,object[] param);//���ȶ���һ��ί�����͵Ķ���CustomEventHandler

        //��delegate�������������¼���Ҫ��event�ؼ��֣����ﶨ���������ּ�;
        public event CustomEventHandler CustomEvent;

        /// <summary>
        /// ѭ���ռ伯�ϲ�ִ��ί�еķ���
        /// </summary>
        /// <param name="controls"></param>
        public object LoopControls(System.Windows.Forms.Control.ControlCollection controls,object[] param)
        {
            System.Collections.Hashtable hasFields = new System.Collections.Hashtable();

            foreach (System.Windows.Forms.Control control in controls)
            {
                CustomEvent(control,param);

                LoopControls(control.Controls, param);
            }

            return hasFields;
        }

        /// <summary>
        /// ��������ı���
        /// </summary>
        /// <param name="controls"></param>
        public void ClearText(System.Windows.Forms.Control.ControlCollection controls)
        {
            this.CustomEvent += new CommonLib.Windows.Controls.CustomEventHandler(this.ClearText);

            this.LoopControls(controls, null);
        }
      
        /// <summary>
        /// ����ѭ����������ı���
        /// </summary>
        /// <param name="control"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private object ClearText(System.Windows.Forms.Control control,object[] param)
        {
            switch (control.GetType().ToString())
            {
                case "System.Windows.Forms.TextBox":
                    {
                        control.Text = ""; break;
                    }
                default:
                    {
                        break;
                    }
            }

            return null;
        }
    }
}
