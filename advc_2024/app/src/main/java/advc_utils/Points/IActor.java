package advc_utils.Points;

public interface IActor    
{
    IPoint GetLocation();
    IPoint GetVelocity();

    void SetLocation(IPoint loc);
    void SetVelocity(IPoint vel);
    
    void MoveOneTurn();
}
