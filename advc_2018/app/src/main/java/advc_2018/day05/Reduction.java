package advc_2018.day05;

public class Reduction 
{
    public static String reduceSpecific(String str, char target)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < str.length(); i++)
        {
            final char a = str.charAt(i);
            if (Character.toLowerCase(a) != target)
            {
                sb.append(a);
            }
        }

        str = sb.toString();
        return str;
    }

    public static String reduce(String str)
    {
        boolean hasReacted = true;

        while (hasReacted)
        {
            StringBuilder sb = new StringBuilder(str.length());
            hasReacted = false;

            int i;
            for (i = 0; i < str.length() - 1; i++)
            {
                final char a = str.charAt(i);
                final char b = str.charAt(i + 1);

                if (canReact(a, b))
                {
                    hasReacted = true;
                    i ++;
                }
                else
                {
                    sb.append(a);
                }
            }
            if (i == str.length() - 1)
            {
                sb.append(str.charAt(i));
            }
            str = sb.toString();
        }

        return str;
    }

    public static String reduceBestStrategy(String str)
    {
        String minStr = str;

        for (char c = 'a'; c <= 'z'; c ++)
        {
            String newStr1 = reduceSpecific(str, c);
            String newStr2 = reduce(newStr1);

            if (newStr2.length() < minStr.length())
            {
                minStr = newStr2;
            }
        }
        return minStr;
    }

    private static boolean canReact(final char a, final char b)
    {
        return Character.toLowerCase(a) == Character.toLowerCase(b) && a != b;
    }
}
