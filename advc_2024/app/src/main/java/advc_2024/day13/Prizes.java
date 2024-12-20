package advc_2024.day13;

import java.util.ArrayList;
import java.util.List;

import advc_utils.Etc.SplitHelper;
import advc_utils.Points.*;
import advc_utils.Points.IPoint.EAxis;

class PrizeInfo
{
    IPoint a;        // x, y movement
    IPoint b;        // x, y movement
    IPoint prize;    // x, y prize values
}

public class Prizes
{
    ArrayList<PrizeInfo> m_PrizeInfos = new ArrayList<>();


    public Prizes(List<String> lines)
    {
        PrizeInfo curInfo = null;
        for (var line : lines)
        {
            if (line.isEmpty())
            {
                continue;
            }
            var lineParts = SplitHelper.CheckSplit(line, ": ", 2);
            var lineTitle = lineParts[0];
            var lineValues = lineParts[1];

            if (lineTitle.equals("Button A"))
            {
                curInfo = new PrizeInfo();
                curInfo.a = parseMovements(lineValues, "\\+");
            }
            else if (lineTitle.equals("Button B"))
            {
                assert(curInfo != null);
                if (curInfo != null)
                {
                    curInfo.b = parseMovements(lineValues, "\\+");
                }
            }
            else if (lineTitle.equals("Prize"))
            {
                assert(curInfo != null);
                if (curInfo != null)
                {
                    curInfo.prize = parseMovements(lineValues, "=");
                    m_PrizeInfos.add(curInfo);
                }
                curInfo = null;
            }
        }
    }

    public Prizes AddScale(long scale)
    {
        for (var prize : m_PrizeInfos)
        {
            prize.prize.setAxisValue(EAxis.X, prize.prize.getX() + scale);
            prize.prize.setAxisValue(EAxis.Y, prize.prize.getY() + scale);
        }
        return this;
    }

    public long getAllPrizes()
    {
        long sum = 0;
        /*
        *  AX + BY = C
        *  DX + EY = F
        *  X = (CE - BF) / (AE - BD)
        *  Y = (AF - CD) / (AE - BD)
        */        
        for (var prize : m_PrizeInfos)
        {
            final double base = (prize.a.getX() * prize.b.getY() - prize.a.getY() * prize.b.getX());
            final double x = (prize.prize.getX() * prize.b.getY() - prize.b.getX() * prize.prize.getY()) / base;
            final double y = (prize.a.getX() * prize.prize.getY() - prize.prize.getX() * prize.a.getY()) / base;

            if (Math.floor(x) == x && Math.floor(y) == y)
            {
                sum += x * 3 + y;
            }
        }
        return sum;
    }

    private IPoint parseMovements(String moveStr, String delimiter)
    {
        var moves = SplitHelper.CheckSplit(moveStr, ", ", 2);
        var moveCoords = new ArrayList<Long>();

        for (var move : moves)
        {
            var moveParts = SplitHelper.CheckSplit(move, delimiter, 2);
            moveCoords.add(Long.parseLong(moveParts[1]));
        }
        return new Point(moveCoords.get(0)  , moveCoords.get(1));
    }

    private String getPrizeString()
    {
        StringBuilder sb = new StringBuilder();
        for (var prize : m_PrizeInfos)
        {
            sb.append(String.format("[A:%s][B:%s][Prize:%s]\n", prize.a, prize.b, prize.prize));
        }
         
        return sb.toString();
    }
}
