package day20

import (
	"fmt"
	"strings"
	"taeho/advc19_go/utils"
)

const (
	TILE_ENTRANCE byte = 10
	TILE_EXIT     byte = 11
	TILE_PORTAL   byte = 12
	TILE_WALL     byte = 13
	TILE_ROAD     byte = 14
)

const (
	TILECHAR_WALL  byte = '#'
	TILECHAR_ROAD  byte = '.'
	TILECHAR_EMPTY byte = ' '
)

type Portal struct {
	locOuter utils.Vector
	locInner utils.Vector
	name     string
}

type Maze struct {
	grid     *utils.Grid[byte]
	portals  map[utils.Vector]*Portal
	entrance utils.Vector
	exit     utils.Vector
}

func (p *Portal) String() string {
	return fmt.Sprint("[", p.name, "/", p.locOuter, "-", p.locInner, "]")
}

func (p *Portal) GetPortalExit(loc *utils.Vector) *utils.Vector {
	if *loc == p.locOuter {
		return &p.locInner
	} else if *loc == p.locInner {
		return &p.locOuter
	}
	panic(fmt.Sprint(p.String(), "does not have entrane", *loc))
}

func NewMaze(str string) (m *Maze) {
	str = strings.Replace(str, string(TILECHAR_EMPTY), string(TILECHAR_WALL), -1)
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")
	m = &Maze{
		grid:    utils.NewGrid[byte](utils.NewVector2D(len(lines[0]), len(lines))),
		portals: make(map[utils.Vector]*Portal),
	}
	p := NewPortalNameContainer()

	for _, line := range lines {
		var tile byte = 0
		for _, c := range line {
			switch byte(c) {
			case TILECHAR_EMPTY:
				panic("Empty char not allowed")
			case TILECHAR_WALL:
				tile = TILE_WALL
			case TILECHAR_ROAD:
				tile = TILE_ROAD
			default:
				if !(c >= 'A' && c <= 'Z') {
					panic(fmt.Sprint("Unknown char", c))
				}
				tile = TILE_WALL
				p.AddPartialName(byte(c), m.grid.GetAddLoc())
			}
			m.grid.Add(tile)
		}
	}

	p.SetPortals(m)
	return
}
