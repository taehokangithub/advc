package day15

import (
	"fmt"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/utils"
)

type RET_TYPE byte

const (
	RET_WALL   RET_TYPE = 0
	RET_OK     RET_TYPE = 1
	RET_OXYGEN RET_TYPE = 2
)

type maze struct {
	com       *computer.Computer
	grid      *utils.FreeMap[byte]
	oxygenLoc utils.Vector
}

type explore struct {
	com   *computer.Computer
	loc   utils.Vector
	dir   utils.Direction
	steps int
}

func NewExplore(com *computer.Computer, dir utils.Direction) *explore {
	return &explore{
		com:   com.Copy(),
		loc:   utils.NewVector2D(0, 0),
		dir:   dir,
		steps: 1,
	}
}

func (ex *explore) CloneNewExplore(dir utils.Direction) *explore {
	return &explore{
		com:   ex.com.Copy(),
		loc:   ex.loc,
		dir:   dir,
		steps: ex.steps + 1,
	}
}

func NewMaze(str string) *maze {
	m := maze{
		com:  computer.NewComputer(str),
		grid: utils.NewFreeMap[byte](),
	}
	return &m
}

func (m *maze) ExploreGoal(stopOnFound bool) int {
	stack := []*explore{}

	for _, dir := range utils.DIR_DIRECTIONS {
		ex := NewExplore(m.com, dir)
		stack = append(stack, ex)
	}

	for len(stack) > 0 {
		ex := stack[len(stack)-1]
		stack = stack[:len(stack)-1]
		ex.com.AddInput(int64(ex.dir + 1))

		ex.com.RunProgram()
		result := RET_TYPE(ex.com.PopOutput())
		ex.loc.Move(ex.dir, 1)

		if result == RET_OXYGEN {
			m.oxygenLoc = ex.loc
			if stopOnFound {
				return ex.steps
			}
		}

		if result == RET_WALL {
			m.grid.Set(ex.loc, byte(RET_WALL))
		} else if result == RET_OK || result == RET_OXYGEN {
			m.grid.Set(ex.loc, byte(result))
			for _, dir := range utils.DIR_DIRECTIONS {
				newLoc := ex.loc.GetMoved(dir, 1)
				if _, exist := m.grid.Get(newLoc); !exist {
					newEx := ex.CloneNewExplore(dir)
					stack = append(stack, newEx)
				}
			}
		} else {
			panic(fmt.Sprintf("unknown result %d", result))
		}
	}
	if stopOnFound {
		panic("Answer not found")
	}
	return 0
}

func (m *maze) Expand() int {
	newOxygens := []utils.Vector{m.oxygenLoc}

	minutes := 0

	for len(newOxygens) > 0 {
		nextNewOxygens := []utils.Vector{}
		for _, loc := range newOxygens {
			for _, dir := range utils.DIR_DIRECTIONS {
				newLoc := loc.GetMoved(dir, 1)

				if v, exist := m.grid.Get(newLoc); exist {
					if v == byte(RET_OK) {
						m.grid.Set(newLoc, byte(RET_OXYGEN))
						nextNewOxygens = append(nextNewOxygens, newLoc)
					}
				}
			}
		}
		if len(nextNewOxygens) > 0 {
			minutes++
		}
		newOxygens = nextNewOxygens
	}

	return minutes
}

func (m *maze) Draw() {
	for y := m.grid.Min.Y; y <= m.grid.Max.Y; y++ {
		for x := m.grid.Min.X; x <= m.grid.Max.X; x++ {
			if v, exists := m.grid.Get(utils.NewVector2D(x, y)); exists {
				switch RET_TYPE(v) {
				case RET_WALL:
					fmt.Print("#")
				case RET_OXYGEN:
					fmt.Print("O")
				case RET_OK:
					fmt.Print(" ")
				}
			} else {
				fmt.Print("?")
			}
		}
		fmt.Println("")
	}
}
