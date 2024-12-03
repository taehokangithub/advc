package advc_utils;

import advc_utils.Points.*;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class ActorTest 
{
    @Test
    void BasicTest()
    {
        IPoint loc = new Point(3, 5, 8, -1);
        IPoint vel = new Point(-5, -1, 7, 3);

        IActor act = new Actor(loc, vel);

        assertEquals(act.GetLocation(), loc);
        assertEquals(act.GetVelocity(), vel);

        act.MoveOneTurn();
        assertEquals(act.GetVelocity(), vel);
        assertEquals(act.GetLocation(), loc.getAdded(vel));

        act.MoveOneTurn();
        assertEquals(act.GetVelocity(), vel);
        assertEquals(act.GetLocation(), loc.getAdded(vel).getAdded(vel));
    }
}
