package day24

import (
	"fmt"
	"math"
	"strings"
	"taeho/advc19_go/utils"
)

type SmallGrid struct {
	grid               map[int]*utils.Grid[bool]
	baseGrid           *utils.Grid[bool]
	isMultiLevel       bool
	minLevel, maxLevel int
	centerLoc          utils.Vector
}

func NewSmallGrid(str string) *SmallGrid {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")
	g := utils.NewGrid[bool](utils.NewVector2D(len(lines[0]), len(lines)))

	for _, line := range lines {
		for _, c := range line {
			g.Add(c == '#')
		}
	}
	if !g.IsFilled() {
		panic("internal grid is not filled")
	}

	smg := &SmallGrid{
		grid:         map[int]*utils.Grid[bool]{},
		isMultiLevel: false,
		centerLoc:    utils.NewVector2D(g.Size.X/2, g.Size.Y/2),
	}

	smg.grid[0] = g
	smg.baseGrid = g

	return smg
}

func NewSmallGridMultiLevel(str string) *SmallGrid {
	smg := NewSmallGrid(str)
	smg.isMultiLevel = true
	return smg
}

func (smg *SmallGrid) Copy() *SmallGrid {
	other := &SmallGrid{
		grid:         map[int]*utils.Grid[bool]{},
		isMultiLevel: smg.isMultiLevel,
		minLevel:     smg.minLevel,
		maxLevel:     smg.maxLevel,
		baseGrid:     smg.baseGrid,
		centerLoc:    smg.centerLoc,
	}

	for level, grid := range smg.grid {
		other.grid[level] = grid.Copy()
	}
	return other
}

func (smg *SmallGrid) GetAdjacentTiles(level int, v utils.Vector) int {
	cnt := 0
	g := smg.grid[level]
	checked := 0

	cntForGrid := func(grid *utils.Grid[bool], loc utils.Vector) {
		if grid.Get(loc) {
			cnt++
		}
		checked++
	}

	cntYForX := func(grid *utils.Grid[bool], x int) {
		for y := 0; y < smg.baseGrid.Size.Y; y++ {
			cntForGrid(grid, utils.NewVector2D(x, y))
		}
	}

	cntXForY := func(grid *utils.Grid[bool], y int) {
		for x := 0; x < smg.baseGrid.Size.X; x++ {
			cntForGrid(grid, utils.NewVector2D(x, y))
		}
	}

	for _, dvec := range utils.DIR_VECTORS {
		newLoc := dvec.GetAdded(v)
		if g.IsValidVector(newLoc) {
			if smg.isMultiLevel && newLoc == smg.centerLoc {
				if innerGrid, ok := smg.grid[level+1]; ok {
					if dvec == utils.DIR_VECTORS[utils.DIR_LEFT] {
						cntYForX(innerGrid, smg.baseGrid.Size.X-1)
					} else if dvec == utils.DIR_VECTORS[utils.DIR_RIGHT] {
						cntYForX(innerGrid, 0)
					} else if dvec == utils.DIR_VECTORS[utils.DIR_UP] {
						cntXForY(innerGrid, 0)
					} else if dvec == utils.DIR_VECTORS[utils.DIR_DOWN] {
						cntXForY(innerGrid, smg.baseGrid.Size.Y-1)
					}
				} else {
					checked++
				}
			} else {
				cntForGrid(g, newLoc)
			}
		} else if smg.isMultiLevel {
			if outerGrid, ok := smg.grid[level-1]; ok {
				outerLoc := dvec.GetAdded(smg.centerLoc)
				cntForGrid(outerGrid, outerLoc)
			} else {
				checked++
			}
		}
	}
	if smg.isMultiLevel && checked != 8 && checked != 4 {
		panic(fmt.Sprintln("Level", level, "loc", v, "checked", checked, "is not 4 or 8"))
	}
	if cnt > 8 {
		panic(fmt.Sprintln("Level", level, "loc", v, "cnt", cnt, "is greater than 8"))
	}

	return cnt
}

func IsGridEmpty(grid *utils.Grid[bool]) bool {
	for y := 0; y < grid.Size.Y; y++ {
		for x := 0; x < grid.Size.X; x++ {
			if grid.Get(utils.NewVector2D(x, y)) {
				return false
			}
		}
	}
	return true
}

func (smg *SmallGrid) ProcessOneLevel(level int, target *utils.Grid[bool]) {
	if _, ok := smg.grid[level]; !ok {
		smg.grid[level] = utils.NewGrid[bool](smg.baseGrid.Size)
	}

	src := smg.grid[level]

	src.Foreach(func(v utils.Vector, val bool) {
		if smg.isMultiLevel && smg.centerLoc == v {
			return
		}
		cnt := smg.GetAdjacentTiles(level, v)
		if val {
			target.Set(v, cnt == 1)
		} else {
			target.Set(v, (cnt == 1 || cnt == 2))
		}
	})
}

func (smg *SmallGrid) ProcessOneMinute() {
	org := smg.Copy()

	for level := range org.grid {
		targetGrid := smg.grid[level]
		org.ProcessOneLevel(level, targetGrid)
	}

	if smg.isMultiLevel {
		newLevels := []int{org.maxLevel + 1, org.minLevel - 1}

		for _, level := range newLevels {
			newGrid := utils.NewGrid[bool](smg.baseGrid.Size)
			org.ProcessOneLevel(level, newGrid)

			if !IsGridEmpty(newGrid) {
				smg.grid[level] = newGrid
				if level == smg.minLevel-1 {
					smg.minLevel = level
				} else if level == smg.maxLevel+1 {
					smg.maxLevel = level
				} else {
					panic(fmt.Sprintln("Invalid new level", level, ", while previous minLevel", smg.minLevel, "max level", smg.maxLevel))
				}
			}
		}
	}
}

func (smg *SmallGrid) FindSamePattern() {
	record := map[string]bool{}

	if smg.isMultiLevel {
		panic("FindSamePatter does not support multi level")
	}

	record[smg.baseGrid.String()] = true

	for {
		smg.ProcessOneMinute()
		str := smg.baseGrid.String()

		if _, ok := record[str]; ok {
			return
		}
		record[str] = true
	}
}

func (smg *SmallGrid) BiodiversityRating() int64 {
	smg.FindSamePattern()
	g := smg.baseGrid
	rating := int64(0)
	g.Foreach(func(v utils.Vector, t bool) {
		if t {
			index := v.X + v.Y*g.Size.X
			value := math.Pow(2.0, float64(index))
			rating += int64(value)
		}
	})
	return rating
}

func (smg *SmallGrid) CntMultiLevelAfter(minutes int) int {
	for i := 0; i < minutes; i++ {
		smg.ProcessOneMinute()
	}
	return smg.CntAllTiles()
}

func (smg *SmallGrid) CntAllTiles() int {
	cnt := 0
	for _, grid := range smg.grid {
		grid.Foreach(func(v utils.Vector, t bool) {
			if t {
				if smg.isMultiLevel && v == smg.centerLoc {
					panic("Multi level center tile shouldn't be true")
				}
				cnt++
			}
		})
	}

	return cnt
}

func (smg *SmallGrid) PrintAll() {

	for level := smg.minLevel; level <= smg.maxLevel; level++ {
		g := smg.grid[level]
		fmt.Println("-------", level, "---------")
		for Y := 0; Y < g.Size.Y; Y++ {
			for X := 0; X < g.Size.X; X++ {
				v := g.Get(utils.NewVector2D(X, Y))
				if v {
					fmt.Printf("#")
				} else {
					fmt.Printf(".")
				}
			}
			fmt.Println("")
		}
	}
}
