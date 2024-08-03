package advc_utils.Etc;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;

public class AdvcHelper implements IAdvcHelper
{
    private final String m_name;
    private int m_part = 0;
    private enum EThrowException { Yes, No };
    private enum EIncreasePartNum { Yes, No }
    private final String colourDefault = "\033[0m";
    private final String colourRed= "\033[31m";
    private final String colourBlue = "\033[34m";

    public AdvcHelper(String name)
    {
        m_name = name;
    }

    public List<String> readLinesFromFile(String filename)
    {
        Path path = Paths.get(String.format("data/%s/%s", m_name, filename));

        try
        {
            return Files.readAllLines(path);
        }
        catch(IOException e)
        {
            System.err.println(e.toString());
            System.err.println("Current dir:" + Paths.get("").toAbsolutePath());
            System.err.println("Searching at:" + path.toAbsolutePath());
        }
        return null;
    }

    public <T> void answerChecker(T answer, T target)
    {
        answerCheckerInternal(answer, target, EThrowException.Yes, EIncreasePartNum.Yes);
    }

    public <T> void answerCheckerDontThrow(T answer, T target)
    {
        answerCheckerInternal(answer, target,  EThrowException.No, EIncreasePartNum.Yes);
    }

    public <T> void answerCheckerTestInput(T answer, T target)
    {
        answerCheckerInternal(answer, target,  EThrowException.Yes, EIncreasePartNum.No);
    }
   
    private <T> void answerCheckerInternal(T answer, T target, EThrowException eThrowException, EIncreasePartNum eIncreasePartNum)
    {
        if (eIncreasePartNum == EIncreasePartNum.Yes)
        {
            m_part ++;
        }

        String header = String.format("%s[%s/part%d]%s ", colourBlue, m_name, m_part, colourDefault);

        if (!answer.toString().equals(target.toString()))
        {
            String msg = String.format("%s%sWrong answer!%s %s != %s", header, colourRed, colourDefault, answer.toString(), target.toString());

            if (eThrowException == EThrowException.Yes)
            {
                throw new IllegalArgumentException(msg);
            }
            else
            {
                System.err.println(msg);
            }            
        }
        else
        {
            System.out.println(String.format(header + answer.toString()));
        }
    } 
}
