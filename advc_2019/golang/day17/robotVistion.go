package day17

import (
	"fmt"
	"strings"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/utils"
)

type Tile byte

const (
	TILE_EMPTY       Tile = '.'
	TILE_ROAD        Tile = '#'
	TILE_ROBOT_UP    Tile = '^'
	TILE_ROBOT_RIGHT Tile = '>'
	TILE_ROBOT_LEFT  Tile = '<'
	TILE_ROBOT_DOWN  Tile = 'v'
)

type Robot struct {
	dir utils.Direction
	loc utils.Vector
}

type MyGrid struct {
	com   *computer.Computer
	grid  *utils.Grid[byte]
	robot Robot
}

func NewMyGrid(str string) *MyGrid {
	com := computer.NewComputer(str)
	com.RunProgram()

	builder := strings.Builder{}
	for com.HasOutput() {
		builder.WriteRune(rune(com.PopOutput()))
	}
	mapStr := builder.String()
	mapStr = strings.Replace(mapStr, "\r", "", -1)
	mapLines := strings.Split(mapStr, "\n")

	grid := utils.NewGrid[byte](utils.NewVector2D(len(mapLines[0]), len(mapLines)))
	robot := Robot{}

	for _, line := range mapLines {
		for _, c := range line {
			loc := grid.Add(byte(c))
			switch Tile(c) {
			case TILE_ROBOT_DOWN:
				robot.loc = loc
				robot.dir = utils.DIR_UP // coordination reversed
			case TILE_ROBOT_UP:
				robot.loc = loc
				robot.dir = utils.DIR_DOWN
			case TILE_ROBOT_RIGHT:
				robot.loc = loc
				robot.dir = utils.DIR_RIGHT
			case TILE_ROBOT_LEFT:
				robot.loc = loc
				robot.dir = utils.DIR_LEFT
			}
		}
	}

	g := MyGrid{
		com:   com,
		grid:  grid,
		robot: robot,
	}
	return &g
}

func (m *MyGrid) cntIntersection() int {
	ret := 0
	isRoadInDir := func(v utils.Vector, dir utils.Direction) bool {
		moved := v.GetMovedOneBlock(dir)
		return m.grid.IsValidVector(moved) && m.grid.Get(moved) == byte(TILE_ROAD)
	}
	m.grid.Foreach(func(v utils.Vector, t byte) {
		if t == byte(TILE_ROAD) &&
			isRoadInDir(v, utils.DIR_DOWN) &&
			isRoadInDir(v, utils.DIR_UP) &&
			isRoadInDir(v, utils.DIR_LEFT) &&
			isRoadInDir(v, utils.DIR_RIGHT) {
			ret += (v.X * v.Y)
		}
	})
	return ret
}

func (m *MyGrid) String() string {
	builder := strings.Builder{}

	m.grid.Foreach(func(v utils.Vector, t byte) {
		if v.X == 0 {
			builder.WriteString("\n")
		}
		builder.WriteRune(rune(t))
	})

	builder.WriteString(fmt.Sprint("\n Robot dir ", m.robot.dir, " loc ", m.robot.loc))

	return builder.String()
}
