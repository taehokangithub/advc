package advc_2018.day09;

import java.util.HashMap;

public class MarblesSolver 
{
    public long getHighScore(int numPlayers, int maxMarble)
    {
        HashMap<Integer, Long> playerScores = new HashMap<>();

        Marble curMable = null;

        for (int i = 0; i <= maxMarble; i ++)
        {
            if (i % 23 == 0 && curMable != null)
            {   
                Marble target = curMable.getLink(Marble.Direction.Left, 7);
                curMable = target.getLinkDirect(Marble.Direction.Right);
                deleteMarble(target);

                final int playerId = i % numPlayers;
                final long score = i + target.getValue() + playerScores.getOrDefault(playerId, 0L);
                playerScores.put(playerId, score);
            }
            else 
            {
                var newMarble = new Marble(i);

                if (curMable != null)
                {
                    Marble targetLeft = curMable.getLinkDirect(Marble.Direction.Right);
                    insertMarbleToRight(targetLeft, newMarble);
                }
                curMable = newMarble;
            }

        }

        return playerScores.values().stream().max(Long::compare).orElse(0L);
    }

    private void insertMarbleToRight(Marble targetMarble, Marble newMarble)
    {
        Marble targetRight = targetMarble.getLinkDirect(Marble.Direction.Right);

        // insert newMarble to the left of targetRight
        newMarble.setLink(Marble.Direction.Right, targetRight);
        targetRight.setLink(Marble.Direction.Left, newMarble);

        // insert newMarble to the right of targetLeft
        targetMarble.setLink(Marble.Direction.Right, newMarble);
        newMarble.setLink(Marble.Direction.Left, targetMarble);
    }
    
    private void deleteMarble(Marble marble)
    {
        Marble targetLeft = marble.getLinkDirect(Marble.Direction.Left);
        Marble targetRight = marble.getLinkDirect(Marble.Direction.Right);

        targetLeft.setLink(Marble.Direction.Right, targetRight);
        targetRight.setLink(Marble.Direction.Left, targetLeft);
    }
}
