package day18

import (
	"fmt"
	"sort"
	"strings"
	"taeho/advc19_go/utils"
)

const (
	TILE_WALL  rune = '#'
	TILE_EMPTY rune = '.'
	TILE_ME    rune = '@'
)

type solveState struct {
	myLocs []utils.Vector
	keys   map[rune]bool
}

type keyGrid struct {
	grid  *utils.Grid[rune]
	steps int
	state solveState
}

func (s *solveState) String() string {
	keyArr := make([]string, 0, len(s.keys))
	for k, v := range s.keys {
		if v {
			keyArr = append(keyArr, string(rune(k)))
		}
	}
	sort.Strings(keyArr)
	return fmt.Sprint("L", s.myLocs, "F[", len(keyArr), "]K", keyArr)
}

func (k *keyGrid) String() string {
	b := strings.Builder{}
	b.WriteString(fmt.Sprintln("Steps", k.steps))
	b.WriteString(fmt.Sprintln("state", k.state.String()))
	k.grid.ForeachWithNewline(func(v utils.Vector, t rune, isNewline bool) {
		if k.isMyLoc(v) {
			b.WriteRune('!')
		} else {
			b.WriteRune(t)
		}

		if isNewline {
			b.WriteRune('\n')
		}
	})
	return b.String()
}

func makeKeyame(c rune) rune {
	if c >= 'a' && c <= 'z' {
		return 'A' + c - 'a'
	} else if c >= 'A' && c <= 'Z' {
		return c
	}
	panic(fmt.Sprint("Unknown key name", c))
}

func NewKeyGrid(str string) *keyGrid {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")
	k := keyGrid{
		grid: utils.NewGrid[rune](utils.NewVector2D(len(lines[0]), len(lines))),
		state: solveState{
			myLocs: make([]utils.Vector, 0, 10),
			keys:   map[rune]bool{},
		},
	}

	for _, line := range lines {
		for _, c := range line {
			if c == TILE_ME {
				v := k.grid.Add(TILE_EMPTY)
				k.state.myLocs = append(k.state.myLocs, v)
			} else if c >= 'a' && c <= 'z' {
				keyName := makeKeyame(c)
				k.state.keys[keyName] = false
				k.grid.Add(c)
			} else {
				k.grid.Add(c)
			}
		}
	}
	return &k
}

func (k *keyGrid) isMyLoc(v utils.Vector) bool {
	for _, loc := range k.state.myLocs {
		if loc == v {
			return true
		}
	}
	return false
}

func (k *keyGrid) Copy() *keyGrid {
	copied := keyGrid{
		grid: k.grid.Copy(),
		state: solveState{
			myLocs: make([]utils.Vector, len(k.state.myLocs)),
			keys:   map[rune]bool{},
		},
	}
	copy(copied.state.myLocs, k.state.myLocs)

	copied.steps = k.steps

	for k, v := range k.state.keys {
		copied.state.keys[k] = v
	}
	return &copied
}

func (k *keyGrid) HasFoundAllKeys() bool {
	for _, v := range k.state.keys {
		if !v {
			return false
		}
	}
	return true
}
