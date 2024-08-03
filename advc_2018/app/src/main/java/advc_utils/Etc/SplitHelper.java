package advc_utils.Etc;

public class SplitHelper 
{
    public static String[] CheckSplit(String source, String separator, int expected)
    {
        String[] result = source.split(separator);
        if (result.length != expected)
        {
            throw new IllegalArgumentException(String.format("Split error: expected %d, result %d, source [%s], separator [%s]", 
                    expected, result.length, source, separator));
        }
        return result;
    }
}
