package advc_utils.Points;

public class Actor implements IActor
{
    private IPoint m_location;
    private IPoint m_velocity;

    public Actor()
    {
        m_location = new Point();
        m_velocity = new Point();
    }

    public Actor(IPoint loc, IPoint vel)
    {
        this.SetLocation(loc);
        this.SetVelocity(vel);
    }

    public IPoint GetLocation() 
    {
        return m_location;
    }

    public IPoint GetVelocity()
    {
        return m_velocity;
    }

    public void SetLocation(IPoint loc)
    {
        m_location = new Point(loc);
    }
    
    public void SetVelocity(IPoint vel)
    {
        m_velocity = new Point(vel);
    }

    public void MoveOneTurn()
    {
        m_location.add(m_velocity);
    }
    
    @Override
    public String toString()
    {
        return String.format("[%s:%s]", m_location.toString(), m_velocity.toString());
    }
}
