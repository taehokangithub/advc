using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    public static class TreeNodeExtension
    {
        public static bool IsDir(this TreeNode node)
        {
            return node.Name.Last() == '/';
        }
    }

    public class DirTree : Advc.Utils.NamedTree
    {
        public TreeNode GoRoot()
        {
            var root = GetOrCreateNodeByName("/");
            SetRoot(root);
            return root;
        }

        public TreeNode ChangeDir(TreeNode parent, string name)
        {
            Debug.Assert(parent.IsDir());

            var childName = $"{parent.Name}{name}/";
            AddNode(parent.Name, childName);

            return GetNodeByName(childName);
        }

        public void AddFile(TreeNode parent, string fileName, long size)
        {
            Debug.Assert(parent.IsDir());

            var childName = $"{parent.Name}{fileName}";
            AddNode(parent.Name, childName);

            var child = GetNodeByName(childName);
            child.Value = size;
        }

        public long CalculateDirSize(TreeNode dir)
        {
            long size = dir.Value;

            foreach (var child in dir.Children)
            {
                size += CalculateDirSize(child);
            }

            dir.Value = size;

            if (dir.Parent != null && dir.Parent.Name == "/")
                LogDetail($"size => {dir.Name}, {dir.Value}");
            return size;
        }

        public long CalculateDirSize()
        {
            return CalculateDirSize(Root);
        }

        public List<TreeNode> GetDirsUnderSize(long sizeUpTo)
        {
            List<TreeNode> result = new();

            DFS((node) => 
            {
                if (node.IsDir() && node.Value <= sizeUpTo)
                {
                    LogDetail($"small {node.Name} {node.Value}");
                    result.Add(node);
                }
            }, null);

            return result;
        }

        public long FindSmallestBiggerThan(long sizeUpTo)
        {
            long result = int.MaxValue;

            DFS((node) => 
            {
                if (node.IsDir() && node.Value >= sizeUpTo)
                {
                    if (node.Value < result)
                    {
                        result = node.Value;
                        LogDetail($"Found candidate {node.Value} >= {sizeUpTo}");
                    }
                    else 
                    {
                        LogDetail($"Discarding candidate {node.Value} > {result}");
                    }
                    
                }
            }, null);

            return result;            
        }
    }
    
    class Problem07 : Advc.Utils.Loggable
    {
        public DirTree BuildDirTree(List<string> lines)
        {
            var linesQueue = new Queue<string>(lines);
            DirTree dirTree = new();
            dirTree.AllowLogDetail = AllowLogDetail;
            TreeNode curDir = dirTree.GoRoot();
            TreeNode rootDir = curDir;

            while (linesQueue.Count > 0)
            {
                string line = linesQueue.Dequeue();
                LogDetail(line);
                var commandLine = line.Split(" ");
                Debug.Assert(commandLine[0] == "$");

                var command = commandLine[1];
                if (command == "cd")
                {
                    var dirName = commandLine[2];
                    if (dirName == "/")
                    {
                        curDir = dirTree.GoRoot();
                    }
                    else if (dirName == "..")
                    {
                        curDir = curDir.Parent!;
                    }
                    else
                    {
                        curDir = dirTree.ChangeDir(curDir, dirName);
                    }
                }
                else if (command == "ls")
                {
                    while (linesQueue.Count > 0 && linesQueue.First().First() != '$')
                    {
                        var fileLine = linesQueue.Dequeue().Split(" ");
                        var typeOrSize = fileLine[0];
                        var childName = fileLine[1];

                        if (typeOrSize == "dir")
                        {
                            dirTree.ChangeDir(curDir, childName);
                        }
                        else 
                        {
                            long size = long.Parse(typeOrSize);
                            dirTree.AddFile(curDir, childName, size);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException($"Unknown command {command}");
                }
            }
            dirTree.CalculateDirSize();
            return dirTree;
        }

        public long Solve1(List<string> lines)
        {
            AllowLogDetail = false;
            var dirTree = BuildDirTree(lines);
            var smallDirs = dirTree.GetDirsUnderSize(100000);

            return smallDirs.Sum(d => d.Value);
        }

        public long Solve2(List<string> lines)
        {
            AllowLogDetail = false;
            var dirTree = BuildDirTree(lines);

            const long requiredSize = 30000000;
            const long totalSize = 70000000;
            long unusedSpace = totalSize - dirTree.Root.Value;
            long sizeToFind = requiredSize - unusedSpace;
            long ans = dirTree.FindSmallestBiggerThan(sizeToFind);

            LogDetail($"{totalSize} - {requiredSize} - {dirTree.Root.Value} = {sizeToFind} => {ans}");
            return ans;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input07.txt");
            var textArr = textData.Split(Environment.NewLine).ToList();

            Problem07 prob1 = new();

            var ans1 = prob1.Solve1(textArr);
            var ans2 = prob1.Solve2(textArr);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


