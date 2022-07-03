// �����Ǹ� ���� Extension ����

using System;

namespace UnityEngine.UI.Extensions
{
    public static class A_UI_Extension
    {
        public static void SetTextWithStringKey(this Text txt, string key)
        {
            if (txt == null)
            {
                Debug.LogError("text is null");
            }

            var str = A_StringManager.Instance.GetString(key);
            txt.text = str;
        }

        public static void SetTextWithString(this Text txt, string str)
        {
            if (txt == null)
            {
                Debug.LogError("text is null");
            }

            txt.text = str;
        }

        public static void SetTextForRemainTime(this Text txt, string limit)
        {
            if (txt == null)
            {
                Debug.LogError("text is null");
            }

            var remainStr = A_StringManager.Instance.GetString("ui_pass_002");
            if (remainStr != null)
            {
                var dateTime = DateTime.Now;
                var limitTime = A_StringManager.Instance.ConvertStringTimeToDate(limit);
                var diffTime = limitTime - dateTime;

                if(diffTime.Days > 0)
                {
                    // �Ϸ� ǥ��
                    remainStr = string.Format(remainStr, diffTime.Days.ToString(), "��");
                }
                else if(diffTime.Hours > 0)
                {
                    // �ð����� ǥ��
                    remainStr = string.Format(remainStr, diffTime.Hours.ToString(), "�ð�");
                }
                else if(diffTime.Minutes > 0)
                {
                    // ������ ǥ��
                    remainStr = string.Format(remainStr, diffTime.Minutes.ToString(), "��");
                }
                else if(diffTime.Seconds > 0)
                {
                    // �ʷ� ǥ��
                    remainStr = string.Format(remainStr, diffTime.Seconds.ToString(), "��");
                }
                else
                {
                    // �������� ����� �ϴ°�������� ����ǥ��
                    Debug.LogError("Check Time Data");
                    return;
                }

                txt.text = remainStr;
            }
        }
    }
}
