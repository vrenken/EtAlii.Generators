namespace EtAlii.Generators
{
    using System;
    using System.Text;

    //Generates a short, more or less human readable uid.
    //Not cryptographic secure but enough for e.g. correlation ids
    public static class ShortId
    {
        private static readonly char[] _base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        private static readonly Random _random = new ();

        public static string GetId(int length = 6)
        {
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
                sb.Append(_base62Chars[_random.Next(62)]);

            return sb.ToString();
        }
    }
}
