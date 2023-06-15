package day18

import "taeho/advc19_go/utils"

var SearchMH *searchMoveHeap

type move struct {
	myLocIndex int
	loc        utils.Vector
	steps      int
}

func (k *keyGrid) FindMinSteps() int {
	SearchMH = newSearchMoveHeap()

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

	for i, myloc := range k.state.myLocs {
		SearchMH.Clear()
		visited := map[*vertex]bool{}
		SearchMH.PushMove(&move{
			loc:   myloc,
			steps: 0,
		})
		for SearchMH.Len() > 0 {
			mv := SearchMH.PopMove()
			vt := k.graph.vertexMap[mv.loc.String()]
			visited[vt] = true
			for targetVertex, dist := range vt.edges {
				if visited[targetVertex] {
					continue
				} else if targetVertex.tile == TILE_ME || k.HasKeyUnlocked(targetVertex.tile) {
					SearchMH.PushMove(&move{
						loc:   targetVertex.loc,
						steps: mv.steps + dist,
					})
				} else if k.IsKey(targetVertex.tile) {
					visited[targetVertex] = true
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
