// 내편의를 위한 Extension 정의

using System;

namespace UnityEngine.UI.Extensions
{
    public static class A_UI_Extension
    {
        #region TextUtil
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
        #endregion

        #region scrollRectUtil
        public static void SetScrollTo(this ScrollRect rect, int index)
        {
            // 목표지점 위치 저장할곳
            float indexPos = 0;

            // 스크롤 아이템들이 저장되는곳
            var content = rect.content.transform;
            // 스크롤 아이템 사이 간격
            var spacing = rect.content.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing;
            // 원래는 스크롤 padding까지 확인되어야하나 패스..

            for (int i = 0; i<index;i++)
            {
                var child = content.GetChild(i);

                // 해당 스크롤의 자식들의 크기 및 간격을 더해서 위치를 구함.
                indexPos += child.GetComponent<RectTransform>().rect.height;
                indexPos += spacing;
            }

            // 목표 위치 / 전체 스크롤의 크기
            // 출처 : https://mean-dragon.tistory.com/14
            var targetPos = indexPos / (rect.content.rect.height - rect.GetComponent<RectTransform>().rect.height);
            
                // 위치를 설정한다.
            Vector2 targetV2 = Vector2.zero;
            if(rect.vertical == true)
            {
                targetV2 = new Vector2(0, targetPos);
            }
            else
            {
                targetV2 = new Vector2(targetPos, 0);
            }

            rect.normalizedPosition = targetV2;
        }
        #endregion
    }
}
