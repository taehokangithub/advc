package advc_utils.Etc;

public class Logger {
    public enum LoggerType { DummyLogger, TextLogger } // can add more types..

    private LoggerType m_type;
    
    public Logger(LoggerType type)
    {
        m_type = type;
    }

    public void log(String format, Object... args) 
    {
        if (m_type == LoggerType.TextLogger)
        {
            System.out.println(String.format(format, args));
        }
    }
}
