package day18

import (
	"taeho/advc19_go/utils"
)

var searchMH *searchMoveHeap
var movePool *utils.MemoryPool[move]

type move struct {
	loc   utils.Vector
	steps int
	index int
}

func (k *keyGrid) FindMinSteps() int {
	searchMH = newSearchMoveHeap()
	movePool = utils.NewMemoryPool[move](8192)

	k.graph = NewKeyGraph(k)
	k.graph.BuildEdges()

	c := NewKeyGridHeap()
	c.PushKeyGrid(k)
	for c.Len() > 0 {
		kptr := c.PopKeyGrid()
		if kptr.HasFoundAllKeys() {
			return kptr.steps
		}
		possibleStates := kptr.findPossibleMovesV3()
		for _, ps := range possibleStates {
			c.PushKeyGrid(ps)
		}
	}
	return 0
}

func (k *keyGrid) findPossibleMovesV3() []*keyGrid {
	ret := make([]*keyGrid, 0, 64)
	movePool.Reset()
	for i, myloc := range k.state.myLocs {
		searchMH.Clear()
		visitGrid := utils.NewGrid[bool](k.grid.Size)
		intialMove := movePool.New()
		intialMove.loc = myloc
		intialMove.steps = 0
		searchMH.PushMove(intialMove)
		for searchMH.Len() > 0 {
			mv := searchMH.PopMove()
			vt := k.graph.vertexMap[mv.loc.String()]
			visitGrid.SetFast(&mv.loc, true)
			for targetVertex, dist := range vt.edges {
				if visitGrid.GetFast(&targetVertex.loc) {
					continue
				} else if targetVertex.tile == TILE_ME || k.HasKeyUnlocked(targetVertex.tile) {
					newMove := movePool.New()
					newMove.loc = targetVertex.loc
					newMove.steps = mv.steps + dist
					searchMH.PushMove(newMove)
				} else if k.IsKey(targetVertex.tile) {
					visitGrid.SetFast(&targetVertex.loc, true)
					ck := k.Copy()
					ck.SetKeyUnlocked(targetVertex.tile, true)
					ck.steps += mv.steps + dist
					ck.state.myLocs[i] = targetVertex.loc
					ret = append(ret, ck)
				}
			}
		}
	}
	return ret
}
