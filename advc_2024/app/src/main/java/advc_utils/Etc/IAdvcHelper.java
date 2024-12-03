package advc_utils.Etc;

import java.util.List;

public interface IAdvcHelper 
{
    List<String> readLinesFromFile(String filename);
    
    <T> void answerChecker(T answer, T target);
    <T> void answerCheckerDontThrow(T answer, T target);
    <T> void answerCheckerTestInput(T answer, T target);
}
