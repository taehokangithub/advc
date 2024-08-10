package advc_2018.day05;

public class Reduction 
{
    public static StringBuilder reduceSpecific(StringBuilder str, char target)
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

        return sb;
    }

    public static int reduce(StringBuilder sbSource)
    {
        boolean hasReacted = true;

        StringBuilder sbTarget = new StringBuilder(sbSource.length());

        while (hasReacted)
        {
            hasReacted = false;
            
            int i;
            for (i = 0; i < sbSource.length() - 1; i++)
            {
                final char a = sbSource.charAt(i);
                final char b = sbSource.charAt(i + 1);

                if (canReact(a, b))
                {
                    hasReacted = true;
                    i ++;
                }
                else
                {
                    sbTarget.append(a);
                }
            }
            if (i == sbSource.length() - 1)
            {
                sbTarget.append(sbSource.charAt(i));
            }
            
            // swap & reuse
            StringBuilder tmp = sbSource;
            sbSource = sbTarget;
            sbTarget = tmp;
            sbTarget.setLength(0);
        }

        return sbSource.length();
    }

    public static int reduceBestStrategy(StringBuilder sb)
    {
        int minLen = sb.length();

        for (char c = 'a'; c <= 'z'; c ++)
        {
            final int ret = reduce(reduceSpecific(sb, c));

            minLen = Math.min(minLen, ret);
        }
        
        return minLen;
    }

    private static boolean canReact(final char a, final char b)
    {
        return Character.toLowerCase(a) == Character.toLowerCase(b) && a != b;
    }
}
