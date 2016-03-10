using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Utilities.StringUtils
{
    public class WordUtil
    {
        public static int GetWordCount(string text)
        {
            int wordCount = 0;
            int index = 0;

            while (index < text.Length)
            {
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
                    index++;

                wordCount++;

                while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
                    index++;
            }
            return wordCount;
        }

        public static string GetSubStringWords(int numberOfWords, string stringWords)
        {
            string[] strArray = stringWords.Split(' ');
            StringBuilder sbReturnString = new StringBuilder();

            for (int i = 0; i < numberOfWords; i++)
                sbReturnString.Append(strArray[i] + " ");

            return sbReturnString.ToString();
        }
    }
}
