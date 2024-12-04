package advc_2024.day04;

import advc_utils.Grid.*;
import advc_utils.Points.*;
import java.util.List;

public class WordSearch 
{
    private IGrid<Character> m_grid = new Grid<>();
    
    public WordSearch(List<String> lines)
    {
        m_grid.initialiseGrid(lines, c -> c);
    }

    public int findAllWord(String word)
    {
        final IPoint size = m_grid.getSize();
        int wordCount = 0;

        for (int y = 0; y < size.getY(); y ++)
        {
            for (int x = 0; x < size.getX(); x++)
            {
                IPoint p = new Point(x, y);
                if (m_grid.getTile(p) == word.charAt(0))
                {
                    for (IPoint dir : PointStatic.Dir8Points)
                    {
                        if (findWordInDirection(word, p, dir))
                        {
                            wordCount ++;
                        }
                    }

                }
            }
        }
        return wordCount;
    }

    public int findAllXWords(String word) // X-shaped word 
    {
        final IPoint size = m_grid.getSize();
        final int offset = word.length() / 2;
        final char startingChar = word.charAt(offset);
        final Point[] searchPoints  = { 
            new Point(-offset, -offset),   // top left
            new Point(offset, -offset), // top right
            new Point(offset, offset), // botton right
            new Point(-offset, offset)   // botton left
        };

        int wordCount = 0;
        for (int y = offset; y < size.getY() - offset; y ++)
        {
            for (int x = offset; x < size.getX() - offset; x++)
            {
                IPoint curLoc = new Point(x,  y);
                if (m_grid.getTile(curLoc) == startingChar)
                {
                    int foundMatchCount = 0; // must be 2 to make a cross
                    for (IPoint sp : searchPoints)
                    {
                        IPoint newLoc = curLoc.getAdded(sp);

                        IPoint dir = new Point(sp.getX() / (-offset), sp.getY() / (-offset));
                        if (findWordInDirection(word, newLoc, dir))
                        {
                            foundMatchCount ++;
                        }
                    }
                    if (foundMatchCount == 2)
                    {
                        wordCount ++;
                    }
                }
            }
        }
        return wordCount;
    }

    private boolean findWordInDirection(String word, IPoint startingPoint, IPoint dir)
    {
        IPoint curLoc = new Point(startingPoint);

        for (int i = 0; i < word.length(); i ++)
        {
            if (!m_grid.isValid(curLoc) || word.charAt(i) != m_grid.getTile(curLoc))
            {
                return false;
            }
            curLoc = curLoc.getAdded(dir);
        }

        return true;
    }
    
}
