package day10

import (
	"fmt"
	"math"
	"sort"
	"strings"
	"taeho/advc19_go/utils"
)

type AsteroidGrid struct {
	grid      utils.Grid[bool]
	asteriods map[utils.Vector]bool
}

func createAsteroidGrid(str string) *AsteroidGrid {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")
	maxX, maxY := len(lines[0]), len(lines)

	myGrid := AsteroidGrid{}
	myGrid.grid = *(utils.NewGrid[bool](utils.NewVector2D(maxX, maxY)))

	for _, line := range lines {
		line = strings.TrimSpace(line)
		for _, c := range line {
			myGrid.grid.Add(c == '#')
		}
	}

	if !myGrid.grid.IsFilled() {
		panic("grid is not filled")
	}

	myGrid.asteriods = map[utils.Vector]bool{}

	myGrid.grid.Foreach(func(v utils.Vector, val bool) {
		if val {
			myGrid.asteriods[v] = true
		}
	})

	return &myGrid
}

func getDirVector(v utils.Vector) utils.Vector {
	if v.X == 0 || v.Y == 0 {
		return v.GetBaseVector()
	}
	gcd := utils.Gcd(v.X, v.Y)
	return utils.NewVector2D(v.X/gcd, v.Y/gcd)
}

func (ast *AsteroidGrid) getVisiblesFrom(loc utils.Vector) []utils.Vector {
	ret := []utils.Vector{}

	visited := map[utils.Vector]bool{}

	for k := range ast.asteriods {
		visited[k] = false
	}

	for a := range ast.asteriods {
		if a != loc && !visited[a] {
			dir := a.GetSubtracted(loc)
			dirBase := getDirVector(dir)
			hasFound := false
			for target := loc.GetAdded(dirBase); ast.grid.IsValidVector(target); target.Add(dirBase) {
				if !hasFound && !visited[target] {
					ret = append(ret, target)
					hasFound = true
				}
				//fmt.Println("checking", loc, "to", a, "dir", dir, "step", dirBase, "target", target, "visited", visited[target])
				visited[target] = true
			}
		}
	}
	return ret
}

func (ast *AsteroidGrid) getAllVisibleCount() (int, utils.Vector) {
	maxVisible := 0
	loc := utils.NewVector2D(0, 0)

	for v := range ast.asteriods {
		visibles := ast.getVisiblesFrom(v)
		cnt := len(visibles)
		if cnt > maxVisible {
			maxVisible = cnt
			loc = v
		}
	}

	return maxVisible, loc
}

func (ast *AsteroidGrid) findNthDestroyed(loc utils.Vector, n int) utils.Vector {

	cnt := 1

	for cnt <= n {
		visibles := ast.getVisiblesFrom(loc)
		curCnt := len(visibles)

		if cnt+curCnt >= n {
			sort.Slice(visibles, func(a, b int) bool {
				pa := visibles[a].GetSubtracted(loc)
				pb := visibles[b].GetSubtracted(loc)
				return math.Atan2(float64(pa.X), float64(pa.Y)) > math.Atan2(float64(pb.X), float64(pb.Y))
			})

			for i, v := range visibles {
				dir := v.GetSubtracted(loc)
				fmt.Printf("[%d]th => %v, (dir %v) rad %f\n", cnt+i, v, dir, math.Atan2(float64(dir.X), float64(dir.Y)))
			}
			return visibles[n-cnt]
		}

		cnt += curCnt
	}

	panic("Shouldn't be here")
}

/* atan values
(dir [1:0]) rad 0.000000
 (dir [1:-1]) rad -0.785398
  (dir [0:-1]) rad -1.570796

  (dir [-1:1]) rad 2.356194
*/
