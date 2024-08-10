package advc_utils.Graphs;

public class Edge 
{
    public enum Dir
    {
        NotDirected, 

        // Indicating which one is source and destination in case of directed edge
        EdgeTargetIsSource,
        EdgeTargetIsDestination,
    }

    public INode target;
    public long distance;
    public Dir dir = Dir.NotDirected;
}
