package advc_utils;

import java.util.function.Function;
import org.junit.jupiter.api.Test;

import advc_utils.Points.*;
import advc_utils.Points.IPoint.EAxis;
import advc_utils.Points.IPoint.EDir;

import static org.junit.jupiter.api.Assertions.*;

public class PointTest {
    @Test 
    void testAddSub() 
    {
        Point p1 = new Point(1, 2, -3, -7);
        Point p2 = new Point(-3, -4, 7, 5);

        Point pAdd = new Point(-2, -2, 4, -2);
        Point pSub = new Point(4, 6, -10, -12);

        assertTrue(p1.getNumAxis() == 4);
        assertTrue(p1.getAdded(p2).equals(pAdd));
        assertTrue(p1.getSubbed(p2).equals(pSub));
    }

    @Test
    void testCopy()
    {
        Point p1 = new Point(3, 7, -1, -3);
        assertTrue(p1.getAxisValue(IPoint.EAxis.X) == 3);
        assertTrue(p1.getAxisValue(IPoint.EAxis.Y) == 7);
        assertTrue(p1.getAxisValue(IPoint.EAxis.Z) == -1);
        assertTrue(p1.getAxisValue(IPoint.EAxis.W) == -3);

        Point p2 = new Point(p1);
        Point p3 = new Point(p2);

        assertTrue(p1.getNumAxis() == 4);
        assertTrue(p1.equals(p2));
        assertTrue(p1.equals(p3));
        assertTrue(p1.toString().equals(p2.toString()));

        p3.setAxisValue(IPoint.EAxis.W, 5);
        assertFalse(p1.equals(p3));
        assertTrue(p1.equals(p2));

        Point p4 = new Point(3, 4, 5);

        assertTrue(p1.getNumAxis() != p4.getNumAxis(), String.format("%d should be different from %d", p1.getNumAxis(), p4.getNumAxis()));
        assertTrue(p1.getNumAxis() == p3.getNumAxis(), String.format("%d should be same as %d", p1.getNumAxis(), p3.getNumAxis()));
    }

    @Test
    void testMove()
    {
        Point p1 = new Point(1, 3, 5);
        p1.move(EDir.UP);
        p1.move(EDir.RIGHT);
        p1.move(EDir.LEFT, 3);
        p1.move(EDir.DOWN, 7);
        p1.move(EDir.RIGHT, 2);
        p1.move(EDir.DOWN);
        p1.move(EDir.UP, 4);
        p1.move(EDir.LEFT);

        Point p2 = new Point(0, 6, p1.getAxisValue(EAxis.Z));

        assertTrue(p1.equals(p2), p1.toString() + " != " + p2.toString());
    }


    @Test
    void testRotate()
    {
        Point p1 = new Point(3, -1);
        assertTrue(p1.getNumAxis() == 2);

        var p2 = p1.getRotated(EDir.UP);
        assertTrue(p1.equals(p2));

        p2 = p2.getRotated(EDir.DOWN);
        assertTrue(p2.equals(new Point(-3, 1)));

        p2 = p1.getRotated(EDir.RIGHT);
        assertTrue(p2.equals(new Point(1, 3)));

        p2 = p1.getRotated(EDir.LEFT);
        assertTrue(p2.equals(new Point(-1, -3)));

        p2 = new Point(p1);
        var p3 = new Point(p1);

        for (int i = 0; i < 3; i ++)
        {
            p2.rotate(EDir.RIGHT);
            p3.rotate(EDir.LEFT);
        }

        assertFalse(p1.equals(p2));
        assertFalse(p1.equals(p3));

        p2.rotate(EDir.RIGHT);
        p3.rotate(EDir.LEFT);

        assertTrue(p1.equals(p2));
        assertTrue(p1.equals(p3));
    }

    @Test
    void testManhattan()
    {
        IPoint p = new Point(1, 3, -7, -9);
        assertTrue(p.getManhattanDistance() == 20);

        p = new Point(-7, 2);
        assertTrue(p.getManhattanDistance() == 9);

        final long distance = p.getManhattanDistance(new Point(3, 5));
        assertTrue(distance == 13, "distance " + distance);
    }

    @Test
    void testMinMax()
    {
        IPoint p1 = new Point(-3, -7, 2, 8);
        IPoint p2 = new Point(8, 2, -12, -5);
        IPoint p3 = new Point(9, 3, -3, 4);
        IPoint p4 = new Point(-5, -3, 10, -9);

        IPoint pmin = new Point(p1);
        pmin.setMin(p2);
        pmin.setMin(p3);
        pmin.setMin(p4);

        assertTrue(pmin.equals(new Point(-5, -7, -12, -9)));

        IPoint pmax = new Point(p1);
        pmax.setMax(p2);
        pmax.setMax(p3);
        pmax.setMax(p4);

        assertTrue(pmax.equals(new Point(9, 3, 10, 8)));
    }

    @Test
    void testMinMax2D()
    {
        IPoint p1 = new Point(-13, -3);
        IPoint p2 = new Point(-8, 11);
        IPoint p3 = new Point(5, 4);
        IPoint p4 = new Point(2, -17);

        IPoint pmin = new Point(p1);
        pmin.setMin(p2);
        pmin.setMin(p3);
        pmin.setMin(p4);

        assertTrue(pmin.equals(new Point(-13, -17)));

        IPoint pmax = new Point(p1);
        pmax.setMax(p2);
        pmax.setMax(p3);
        pmax.setMax(p4);

        assertTrue(pmax.equals(new Point(5, 11)));
    }

    @Test
    void testConstructor()
    {
        IPoint p = new Point(Long.MAX_VALUE);
        Point p2 = new Point(Long.MAX_VALUE, Long.MAX_VALUE, Long.MAX_VALUE, Long.MAX_VALUE);

        assertTrue(p.equals(p2));
    }

    @Test
    void testToString()
    {
        Function<IPoint, Boolean> stringTest = p -> {
            String str = p.toString();
            IPoint p2 = new Point(str);
            return p2.toString().equals(str);
        };

        assertTrue(stringTest.apply(new Point(1, 3, 6, -8)));
        assertTrue(stringTest.apply(new Point(1, 3, -6, -8)));
        assertTrue(stringTest.apply(new Point(-11, -39, 116, 832837364)));
    }
}
