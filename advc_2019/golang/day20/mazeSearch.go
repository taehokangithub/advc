package day20

import (
	"fmt"
	"taeho/advc19_go/utils"
)

/* This model is not used! MoveGraphSearch.go is the advanced model */

type Move struct {
	loc     utils.Vector
	dist    int
	layer   int
	history []utils.Vector
}

const useHistoryForDbg = false

var moveQueue *utils.RingQueue[*Move] = utils.NewRingQueue[*Move](10000)

func (m *Maze) FindShortestPath(useLayer bool) int {
	moveQueue.Clear()
	mv := &Move{
		loc:     m.entrance,
		history: make([]utils.Vector, 0),
	}
	moveQueue.Push(mv)
	visited := NewVisitMap()

	AddIfNotVisited := func(loc *utils.Vector, mv *Move, isUsingPortal bool) *Move {
		newMv := &Move{}
		newMv.dist = mv.dist
		newMv.loc = *loc

		if useLayer {
			newMv.layer = mv.layer
		}
		if isUsingPortal {
			if useLayer {
				isOuter := m.IsOuter(loc)
				if mv.layer == 0 && !isOuter {
					return nil
				}
				if isOuter {
					newMv.layer++
				} else {
					newMv.layer--
				}
			}
		}

		if visited.GetVisited(newMv.layer, &newMv.loc) {
			return nil
		}

		if useHistoryForDbg {
			newMv.history = make([]utils.Vector, len(mv.history), len(mv.history)+1)
			copy(newMv.history, mv.history)
			newMv.history = append(newMv.history, *loc)
		}

		moveQueue.Push(newMv)
		return newMv
	}

	for !moveQueue.IsEmpty() {
		mv = moveQueue.Pop()
		mv.dist++

		// Beware!! The queue can contain multiple of the same move content
		if visited.GetVisited(mv.layer, &mv.loc) {
			continue
		}
		visited.SetVisited(mv.layer, &mv.loc)

		if portal, ok := m.portals[mv.loc]; ok {
			newLoc := portal.GetPortalExit(&mv.loc)
			if newMv := AddIfNotVisited(newLoc, mv, true); newMv != nil && useHistoryForDbg {
				fmt.Println(mv.dist, "Used portal", portal.name, "from", mv.loc, "to", newLoc, "layer", newMv.layer, "qsize", moveQueue.Len())
			}
		}
		for _, dVec := range utils.DIR_VECTORS {
			movedLoc := dVec.GetAdded(mv.loc)
			if m.exit == movedLoc {
				if !useLayer || mv.layer == 0 {
					return mv.dist
				} else {
					continue
				}
			}
			newTile := m.grid.GetFast(&movedLoc)
			if TILE_ROAD == newTile || TILE_PORTAL == newTile {
				AddIfNotVisited(&movedLoc, mv, false)
			}
		}

	}
	panic("could not find the answer")
}
