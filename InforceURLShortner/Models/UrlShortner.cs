namespace InforceURLShortner.Models
{
    public static class UrlShortner
    {
        public static string IdToShortURL(int n)
        {
            char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

            string shorturl = "";
            n = Math.Abs(n);

            while (n > 0)
            {
                shorturl += (map[n % 62]);
                n /= 62;
            }

            return reverse(shorturl);
        }
        private static string reverse(string input)
        {
            char[] a = input.ToCharArray();
            int l, r = a.Length - 1;
            for (l = 0; l < r; l++, r--)
            {
                (a[r], a[l]) = (a[l], a[r]);
            }
            return string.Join("", a);
        }

        public static int ShortURLtoID(string shortURL)
        {
            int id = 0;

            for (int i = 0; i < shortURL.Length; i++)
            {
                if ('a' <= shortURL[i] &&
                           shortURL[i] <= 'z')
                    id = id * 62 + shortURL[i] - 'a';
                if ('A' <= shortURL[i] &&
                           shortURL[i] <= 'Z')
                    id = id * 62 + shortURL[i] - 'A' + 26;
                if ('0' <= shortURL[i] &&
                           shortURL[i] <= '9')
                    id = id * 62 + shortURL[i] - '0' + 52;
            }
            return id;
        }
    }
}
