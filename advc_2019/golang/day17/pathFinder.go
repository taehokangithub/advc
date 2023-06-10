package day17

import (
	"fmt"
	"strings"
	"taeho/advc19_go/utils"
)

func (m *MyGrid) getFullPath() *PathSet {
	visited := map[utils.Vector]bool{}

	ret := NewPathSet()
	dirsToCheck := []utils.Direction{utils.DIR_LEFT, utils.DIR_RIGHT}

	for {
		curDirVec := utils.DIR_VECTORS[m.robot.dir]
		selectedDir := utils.DIR_INVALID

		for _, dirToCheck := range dirsToCheck {
			rotatedVector := curDirVec.GetRotated(dirToCheck)
			movedLoc := m.robot.loc.GetAdded(rotatedVector)
			if m.grid.IsValidVector(movedLoc) &&
				!visited[movedLoc] &&
				m.grid.Get(movedLoc) == byte(TILE_ROAD) {
				selectedDir = dirToCheck
				break
			}
		}

		if selectedDir == utils.DIR_INVALID {
			break
		}

		m.robot.dir = m.robot.dir.Rotate(selectedDir)
		cntSteps := 0

		for {
			moved := m.robot.loc.GetMovedOneBlock(m.robot.dir)
			if !m.grid.IsValidVector(moved) || m.grid.Get(moved) != byte(TILE_ROAD) {
				break
			}
			m.robot.loc = moved
			visited[moved] = true
			cntSteps++
		}
		ret.AddPath(selectedDir, cntSteps)
	}
	return ret
}

func (p *PathSet) dividePathSetInternal(depth int, maxDepth int) ([]*PathSet, bool) {
	if depth == maxDepth {
		return []*PathSet{}, false
	}

	candidate := NewPathSet()

	for _, path := range p.paths {
		if len(candidate.paths) > 0 && path.replacedBy != nil {
			break
		}
		if path.replacedBy != nil {
			continue
		}
		if candidate.CanAddPath(path) {
			candidate.AddPathUnit(path)
		} else {
			break
		}
	}

	for len(candidate.paths) > 1 {
		copied := p.Copy()
		copied.Eliminate(candidate)

		if copied.GetActualLength() == 0 {
			// This is the last path set eliminating all paths
			return []*PathSet{candidate}, true
		}

		// otherwise, keep searching further
		got, ok := copied.dividePathSetInternal(depth+1, maxDepth)
		if ok {
			got = append(got, candidate)
			return got, true
		}

		candidate.paths = candidate.paths[:len(candidate.paths)-1]
	}

	return []*PathSet{}, false
}

func (p *PathSet) dividePathSet() []*PathSet {
	pathSets, ok := p.dividePathSetInternal(0, 3)

	if !ok {
		panic("Failed to find answer")
	}
	return pathSets
}

func (p *PathSet) getInstructions() string {
	pathSets := p.dividePathSet()

	builder := strings.Builder{}

	cp := p.Copy()

	for i, pathSet := range pathSets {
		cp.Eliminate(pathSet)
		curName := rune('A' + i)
		for j, path := range cp.paths {
			if path.replacedBy != nil && path.replacedByName == 0 {
				path.replacedByName = curName
				cp.paths[j] = path
			}
		}
	}

	mainRoutineBuilder := strings.Builder{}
	for _, path := range cp.paths {
		mainRoutineBuilder.WriteString(fmt.Sprintf("%v,", string(path.replacedByName)))
	}
	mainRoutine := mainRoutineBuilder.String()
	mainRoutine = mainRoutine[:len(mainRoutine)-1]
	builder.WriteString(mainRoutine)
	builder.WriteByte(10)

	for _, pathSet := range pathSets {
		builder.WriteString(pathSet.String())
		builder.WriteByte(10)
	}
	builder.WriteString("n")
	builder.WriteByte(10)

	return builder.String()
}
