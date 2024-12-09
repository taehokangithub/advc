package advc_2024.day09;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;

import advc_utils.Etc.Logger;
import advc_utils.Etc.Logger.LoggerType;

class FileInfo
{
    public int fileId;
    public int fileSize;
    public int filePos;
}

public class DiskSpace 
{
    public enum Version { ver1, ver2 };

    private static final int FREE_SPACE = -1;
    private ArrayList<Integer> m_space = new ArrayList<>();
    private HashMap<Integer, FileInfo> m_fileInfos = new HashMap<>(); // file id / size
    private HashMap<Integer, Integer> m_freeSpaces = new HashMap<>(); // start position / size
    private Logger m_Logger = new Logger(LoggerType.DummyLogger);

    public DiskSpace(List<String> line)
    {
        parseDiskSpace(line.get(0));
    }

    public void debugPrintStatus()
    {
        for (var filesize : m_fileInfos.entrySet())
        {
            m_Logger.log("file id %d size %d pos %d", filesize.getKey(), filesize.getValue().fileSize, filesize.getValue().filePos);
        }
        for (var freespace : m_freeSpaces.entrySet())
        {
            m_Logger.log("free space pos %d, size %d", freespace.getKey(), freespace.getValue());
        }
    }

    public void debugPrintSpace(ArrayList<Integer> space)
    {
        StringBuilder sb = new StringBuilder();
        for (var val : space)
        {
            if (val == FREE_SPACE)
            {
                sb.append(".");
            }
            else
            {
                sb.append(String.format("[%d]", val));
            }
        }
        m_Logger.log(sb.toString());
    }

    public long getDefragmentedChecksum(Version ver)
    {
        long sum = 0;

        var newSpace = ver == Version.ver1 ? defragmentation() : deepDefragmentation();
        debugPrintSpace(newSpace);

        for (int i = 0; i < newSpace.size(); i ++)
        {
            final int fileId = newSpace.get(i);

            if (fileId != FREE_SPACE)
            {
                sum += (i * newSpace.get(i));
            }
        }

        return sum;
    }

    private ArrayList<Integer> defragmentation()
    {
        ArrayList<Integer> newSpace = new ArrayList<>(m_space);

        for (int freeSpacePoint = 0; freeSpacePoint < newSpace.size(); freeSpacePoint ++)
        {
            if (newSpace.get(freeSpacePoint) != FREE_SPACE) // find the next free space point
            {
                continue;
            }

            final int fileToMove = newSpace.get(newSpace.size() - 1);
            newSpace.removeLast();

            if (fileToMove == FREE_SPACE)             // find something to move
            {
                freeSpacePoint --;
                continue;
            }

            newSpace.set(freeSpacePoint, fileToMove);
        }

        return newSpace;
    }

    private ArrayList<Integer> deepDefragmentation()
    {
        ArrayList<Integer> newSpace = new ArrayList<>(m_space);

        var sortedFileIds = new ArrayList<Integer>(m_fileInfos.keySet());
        sortedFileIds.sort(Collections.reverseOrder());

        var sortedFreeSpacesPos = new ArrayList<Integer>(m_freeSpaces.keySet());
        sortedFreeSpacesPos.sort(Comparator.naturalOrder());

        final int REMOVED_FREE_SPACE = -2;

        for (var fileId : sortedFileIds)
        {
            final var fileinfo = m_fileInfos.get(fileId);
            m_Logger.log("Dealing with file %d size %d pos %d", fileId, fileinfo.fileSize, fileinfo.filePos);


            for (int p = 0; p < sortedFreeSpacesPos.size(); p ++)
            {
                final int freeSpacePos = sortedFreeSpacesPos.get(p);
                if (freeSpacePos == REMOVED_FREE_SPACE)   // removed free space slot.
                {
                    continue;
                }
                if (freeSpacePos > fileinfo.filePos)    // Don't push back a file. Search is done.
                {
                    break;
                }

                final int freeSpaceSize = m_freeSpaces.get(freeSpacePos);

                if (freeSpaceSize >= fileinfo.fileSize)  // found a free space to fit the file!
                {
                    m_Logger.log("* File id %d, loc %d, size %d", fileinfo.fileId, freeSpacePos, fileinfo.fileSize);

                    for (int i = 0; i < fileinfo.fileSize; i ++)
                    {
                        newSpace.set(i + fileinfo.filePos, FREE_SPACE);
                        newSpace.set(i + freeSpacePos, fileinfo.fileId);
                        
                    }
                    m_freeSpaces.remove(freeSpacePos);
                    sortedFreeSpacesPos.set(p, REMOVED_FREE_SPACE);

                    final int remainingSpace = freeSpaceSize - fileinfo.fileSize;
                    if (remainingSpace > 0)
                    {
                        final int newFreeSpacePos = freeSpacePos + fileinfo.fileSize;
                        m_freeSpaces.put(newFreeSpacePos, remainingSpace);
                        sortedFreeSpacesPos.set(p, newFreeSpacePos);

                        m_Logger.log("     !! remaining free space at %d, size %d", newFreeSpacePos, remainingSpace);
                    }
                    break;
                }
            }
        }
        return newSpace;
    }

    private void parseDiskSpace(String line)
    {
        boolean filesPhase = true;
        int fileId = 0;
        for (final char c : line.toCharArray())
        {
            final int val = c - '0';
            final int thingToWrite = filesPhase ? fileId : FREE_SPACE;

            if (filesPhase)
            {
                FileInfo info = new FileInfo();
                info.fileId = fileId;
                info.filePos = m_space.size();
                info.fileSize = val;

                m_fileInfos.put(fileId, info);
                fileId ++;
            }
            else if (val > 0)
            {
                m_freeSpaces.put(m_space.size(), val);
            }

            for (int i = 0; i < val; i ++)
            {
                m_space.add(thingToWrite);
            }            

            filesPhase = !filesPhase;
        }
    }
}
