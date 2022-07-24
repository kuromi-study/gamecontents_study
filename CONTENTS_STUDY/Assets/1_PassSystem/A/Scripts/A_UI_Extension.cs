// �����Ǹ� ���� Extension ����

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
        #endregion

        #region scrollRectUtil
        public static void SetScrollTo(this ScrollRect rect, int index)
        {
            // ��ǥ���� ��ġ �����Ұ�
            float indexPos = 0;

            // ��ũ�� �����۵��� ����Ǵ°�
            var content = rect.content.transform;
            // ��ũ�� ������ ���� ����
            var spacing = rect.content.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing;
            // ������ ��ũ�� padding���� Ȯ�εǾ���ϳ� �н�..

            for (int i = 0; i<index;i++)
            {
                var child = content.GetChild(i);

                // �ش� ��ũ���� �ڽĵ��� ũ�� �� ������ ���ؼ� ��ġ�� ����.
                indexPos += child.GetComponent<RectTransform>().rect.height;
                indexPos += spacing;
            }

            // ��ǥ ��ġ / ��ü ��ũ���� ũ��
            // ��ó : https://mean-dragon.tistory.com/14
            var targetPos = indexPos / (rect.content.rect.height - rect.GetComponent<RectTransform>().rect.height);
            
                // ��ġ�� �����Ѵ�.
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
