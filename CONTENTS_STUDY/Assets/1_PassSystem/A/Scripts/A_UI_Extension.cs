// 내편의를 위한 Extension 정의

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
                    // 일로 표시
                    remainStr = string.Format(remainStr, diffTime.Days.ToString(), "일");
                }
                else if(diffTime.Hours > 0)
                {
                    // 시간으로 표시
                    remainStr = string.Format(remainStr, diffTime.Hours.ToString(), "시간");
                }
                else if(diffTime.Minutes > 0)
                {
                    // 분으로 표시
                    remainStr = string.Format(remainStr, diffTime.Minutes.ToString(), "분");
                }
                else if(diffTime.Seconds > 0)
                {
                    // 초로 표시
                    remainStr = string.Format(remainStr, diffTime.Seconds.ToString(), "초");
                }
                else
                {
                    // 나머지는 없어야 하는경우임으로 에러표시
                    Debug.LogError("Check Time Data");
                    return;
                }

                txt.text = remainStr;
            }
        }
    }
}
