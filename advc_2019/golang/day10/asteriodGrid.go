package day10

import (
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
				if _, ok := ast.asteriods[target]; ok {
					if !visited[target] {
						visited[target] = true
						if !hasFound {
							ret = append(ret, target)
							hasFound = true
						}
					}
				}
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
				return pa.Atan2Reverse() > pb.Atan2Reverse()
			})

			return visibles[n-cnt]
		}

		cnt += curCnt
	}

	panic("Shouldn't be here")
}
