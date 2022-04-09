using System;

namespace MediaDownloaderLib
{
    public static class StringExtensions
    {
        public static string RepeatedlyReplace(this string input, string oldValue, string newValue)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            
            while (input.Contains(oldValue))
            {
                input = input.Replace(oldValue, newValue);
            }

            return input;
        }
    }
}