using System;
using System.Text;

namespace Translumo.Utils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Jaro distance algorithm
        /// </summary>
        /// <returns>Distance from 0.0 to 1.0</returns>
        public static double GetSimilarity(this string str, string anotherStr)
        {
            if (string.IsNullOrEmpty(anotherStr))
            {
                return 0.0;
            }

            int distanceSep = (Math.Min(str.Length, anotherStr.Length) / 2) + 1;
            StringBuilder builder = GetCommonCharacters(str, anotherStr, distanceSep);
            int length = builder.Length;
            if (length == 0)
            {
                return 0.0;
            }

            StringBuilder builder2 = GetCommonCharacters(anotherStr, str, distanceSep);
            if (length != builder2.Length)
            {
                return 0.0;
            }

            int num3 = 0;
            for (int i = 0; i < length; i++)
            {
                if (builder[i] != builder2[i])
                {
                    num3++;
                }
            }
            num3 /= 2;

            return (((((double)length) / (3.0 * str.Length)) + (((double)length) / (3.0 * anotherStr.Length))) + (((double)(length - num3)) / (3.0 * length)));
        }

        private static StringBuilder GetCommonCharacters(string firstStr, string secondStr, int distanceSep)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder(secondStr);
            for (int i = 0; i < firstStr.Length; i++)
            {
                char ch = char.ToLowerInvariant(firstStr[i]);
                bool flag = false;
                for (int j = Math.Max(0, i - distanceSep); !flag && (j < Math.Min(i + distanceSep, secondStr.Length)); j++)
                {
                    if (char.ToLowerInvariant(builder2[j]) == ch)
                    {
                        flag = true;
                        builder.Append(ch);
                        builder2[j] = 'ʔ';
                    }
                }
            }
            return builder;
        }
    }
}
