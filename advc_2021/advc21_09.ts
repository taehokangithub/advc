declare var require: any
export {};

const fs = require("fs");

namespace advc21_09
{
    enum Dir
    {
        Up, Down, Left, right
    };

    class Point 
    {
        m_x : number = 0;
        m_y : number = 0;

        constructor(x? : number, y? : number)
        {
            if (x != undefined && y != undefined)
            {
                this.m_x = x;
                this.m_y = y;
            }
        }

        copyFrom(p : Point)
        {
            this.m_x = p.m_x;
            this.m_y = p.m_y;
        }

        moveTodir(dir : Dir)
        {
            switch(dir)
            {
                case Dir.Up : this.m_y --; break;
                case Dir.Down : this.m_y ++; break;
                case Dir.Left : this.m_x --; break;
                case Dir.right : this.m_x ++; break;
            } 
        }

        getCanonical() : string
        {
            return `[${this.m_x}:${this.m_y}]`;
        }
    }

    class Grid
    {
        m_grid : number[][];
        static s_maxVal : number = 9;
        
        constructor(grid : number[][])
        {
            this.m_grid = grid;
        }

        getVal(p : Point) : number
        {
            if (p.m_x < 0 || p.m_y < 0 || p.m_y >= this.m_grid.length ||p.m_x >= this.m_grid[p.m_y].length)
            {
                return Grid.s_maxVal;
            }
            else
            {
                return this.m_grid[p.m_y][p.m_x];
            }
        }

        getDirVal(p : Point, dir : Dir) : number
        {
            const point : Point = new Point();
            point.copyFrom(p);
            point.moveTodir(dir);

            return this.getVal(point);
        }
    };
    
    function solve2(grid : Grid)
    {
        const visited = new Set();

        const search = (p : Point) : number =>
        {
            let sum : number = 1;
            const val : number = grid.getVal(p);
            visited.add(p.getCanonical());

            const searchDir = (dir : Dir) =>
            {
                const point : Point = new Point;
                point.copyFrom(p);
                point.moveTodir(dir);

                if (grid.getVal(point) != Grid.s_maxVal && !visited.has(point.getCanonical()))
                {
                    sum += search(point);
                }
            }

            searchDir(Dir.Up);
            searchDir(Dir.Down);
            searchDir(Dir.Left);
            searchDir(Dir.right);

            return sum;
        };

        const clusterSizes : number[] = [];

        grid.m_grid.forEach((arr, y) =>
        {
            arr.forEach((val, x) =>
            {
                const p : Point = new Point(x, y);

                if (val == Grid.s_maxVal || visited.has(p.getCanonical()))
                {
                    return;
                }

                const clusterSize = search(p);

                //console.log(`Found a cluster with size ${clusterSize}, from ${p.getCanonical()}`);
                clusterSizes.push(clusterSize);
            });
        });

        clusterSizes.sort((a,b) => b - a);

        const ans : number = clusterSizes[0] * clusterSizes[1] * clusterSizes[2];

        console.log(`solve2 : ${ans}`);
    }


    function solve1(grid : Grid)
    {
        let sum : number = 0;

        grid.m_grid.forEach((arr, y) =>
        {
            arr.forEach((val, x) =>
            {
                const p : Point = new Point(x, y);
                if (val < grid.getDirVal(p, Dir.Up) 
                    && val < grid.getDirVal(p, Dir.Down)
                    && val < grid.getDirVal(p,  Dir.Left)
                    && val < grid.getDirVal(p,  Dir.right))
                {
                    sum += val + 1;
                }
            });
        });

        console.log("solve1 " + sum);
    }
    // ---------------------------------------------------------
    function main(path : string)
    {
        const text : string = fs.readFileSync(path,"utf8");
        const lines : string[] = text.split('\n');
        const grid : number[][] = [];

        lines.forEach((line : string) =>
            {
                if (line.length)
                {
                    grid.push(line.split('').map(x => parseInt(x)));
                }
            });

        const gridObject = new Grid(grid);
        solve1(gridObject);
        solve2(gridObject);

    }

    main("../../data/advc21_09 sample.txt");
    main("../../data/advc21_09.txt");
}

