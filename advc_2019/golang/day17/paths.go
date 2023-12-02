package day17

import (
	"fmt"
	"strconv"
	"strings"
	"taeho/advc19_go/utils"
)

const MaxPathSetStringLength = 20

type PathUnit struct {
	dir            utils.Direction
	steps          int
	replacedBy     *PathSet
	replacedByName rune
}

type PathSet struct {
	paths []PathUnit
}

func (p *PathUnit) String() string {
	if p.replacedBy != nil {
		fmt.Print("<Replaced>")
	}
	c := 'L'
	if p.dir == utils.DIR_LEFT {
		c = 'R' // Left/right reversed in grid direction
	}
	return fmt.Sprintf("%v,%d", string(c), p.steps)
}

func (p *PathSet) String() string {
	builder := strings.Builder{}

	for _, path := range p.paths {
		builder.WriteString(fmt.Sprint(path.String(), ","))
	}

	str := builder.String()

	if len(str) > 0 {
		return str[:len(str)-1]
	}
	return ""
}

func NewPathSet() *PathSet {
	return &PathSet{
		paths: make([]PathUnit, 0),
	}
}

func ParsePathSet(str string) *PathSet {
	p := NewPathSet()

	split := strings.Split(str, ",")
	for i := 0; i < len(split); i += 2 {
		steps, _ := strconv.Atoi(split[i+1])
		if split[i] == "R" {
			p.AddPath(utils.DIR_LEFT, steps)
		} else {
			p.AddPath(utils.DIR_RIGHT, steps)
		}
	}

	return p
}

func (p *PathSet) Copy() *PathSet {
	copied := PathSet{
		paths: make([]PathUnit, len(p.paths)),
	}

	copy(copied.paths, p.paths)
	return &copied
}

func (p *PathSet) AddPathUnit(path PathUnit) {
	p.paths = append(p.paths, path)
}

func (p *PathSet) AddPath(dir utils.Direction, cntSteps int) {
	p.paths = append(p.paths, PathUnit{
		dir:   dir,
		steps: cntSteps,
	})
}

func (p *PathSet) CanAddPath(path PathUnit) bool {
	copied := p.Copy()
	copied.AddPathUnit(path)
	return len(copied.String()) <= MaxPathSetStringLength
}

func (p *PathSet) GetActualLength() int {
	cnt := 0
	for _, path := range p.paths {
		if path.replacedBy == nil {
			cnt++
		}
	}
	return cnt
}

func (p *PathSet) Eliminate(other *PathSet) {
	newPaths := []PathUnit{}

	for i := 0; i < len(p.paths); i++ {
		path := p.paths[i]
		found := true

		for j := 0; j < len(other.paths); j++ {
			if i+j >= len(p.paths) || p.paths[i+j] != other.paths[j] {
				found = false
				break
			}
		}

		if found {
			i += len(other.paths) - 1
			newPaths = append(newPaths, PathUnit{
				dir:        utils.DIR_INVALID,
				replacedBy: other,
			})
		} else {
			newPaths = append(newPaths, path)
		}
	}

	p.paths = newPaths
}
