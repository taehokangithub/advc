package day20

import (
	"fmt"
	"taeho/advc19_go/utils"
)

type Move struct {
	loc     utils.Vector
	dist    int
	layer   int
	history []utils.Vector
}

const UseHistoryForDbg = false

const MaxPoolSize = 10000

var MoveQueue *utils.RingQueue[*Move] = utils.NewRingQueue[*Move](10000)
var MovePool *utils.MemoryPool[Move] = utils.NewMemoryPool[Move](10) //(10000000)

func (m *Maze) FindShortestPath(useLayer bool) int {
	MoveQueue.Clear()
	MovePool.Reset()
	mv := &Move{
		loc:     m.entrance,
		history: make([]utils.Vector, 0),
	}
	MoveQueue.Push(mv)
	visited := map[int]map[string]bool{}

	SetVisited := func(mv *Move) {
		if visited[mv.layer] == nil {
			visited[mv.layer] = map[string]bool{}
		}
		visited[mv.layer][mv.loc.String()] = true
	}

	GetVisited := func(mv *Move) bool {
		return visited[mv.layer][mv.loc.String()]
	}

	AddIfNotVisited := func(loc *utils.Vector, mv *Move, isUsingPortal bool) *Move {
		newMv := &Move{} //MovePool.New()
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

		if GetVisited(newMv) {
			return nil
		}

		if UseHistoryForDbg {
			newMv.history = make([]utils.Vector, len(mv.history), len(mv.history)+1)
			copy(newMv.history, mv.history)
			newMv.history = append(newMv.history, *loc)
		}

		MoveQueue.Push(newMv)
		return newMv
	}

	for !MoveQueue.IsEmpty() {
		mv = MoveQueue.Pop()
		mv.dist++
		SetVisited(mv)

		if portal, ok := m.portals[mv.loc]; ok {
			newLoc := portal.GetPortalExit(&mv.loc)
			if newMv := AddIfNotVisited(newLoc, mv, true); newMv != nil && UseHistoryForDbg {
				fmt.Println(mv.dist, "Used portal", portal.name, "from", mv.loc, "to", newLoc, "layer", newMv.layer, "qsize", MoveQueue.Len())
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
