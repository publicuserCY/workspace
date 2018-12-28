using Pinyin4net;
using Pinyin4net.Format;

namespace Demo4OAuth.Tools
{
    public class PinYinTool
    {
        private static HanyuPinyinOutputFormat format = new HanyuPinyinOutputFormat()
        {
            CaseType = HanyuPinyinCaseType.LOWERCASE,
            ToneType = HanyuPinyinToneType.WITHOUT_TONE,
            VCharType = HanyuPinyinVCharType.WITH_V
        };

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="character">传入字符</param>
        /// <param name="isAcronym">true:首拼  false:全拼</param>
        /// <returns></returns>
        public static string GetPinYin(string character, bool isAcronym = false)
        {

            string result = string.Empty;
            var array = character.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                var pinyin = PinyinHelper.ToHanyuPinyinStringArray(array[i], format);
                if (pinyin == null)
                {
                    result += array[i];
                }
                else
                {
                    result += isAcronym ? pinyin[0].ToCharArray()[0].ToString() : pinyin[0];
                }

            }
            return result;
        }
    }
}