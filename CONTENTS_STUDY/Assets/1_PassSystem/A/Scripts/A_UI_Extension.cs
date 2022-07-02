// 내편의를 위한 Extension 정의

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

            var stringtable = ExcelParser.Read("STRINGTABLE");
            if (stringtable.TryGetValue(key, out var fortext) == true)
            {
                txt.text = fortext["DESCRIPTION"].ToString();
            }
            else
            {
                Debug.LogError($"Please Check stringkey : {key}");
            }
        }

        public static void SetTextWithString(this Text txt, string str)
        {
            if (txt == null)
            {
                Debug.LogError("text is null");
            }

            txt.text = str;
        }
    }
}
