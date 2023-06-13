package day18

import "taeho/advc19_go/utils"

type move struct {
	myLocIndex int
	loc        utils.Vector
	steps      int
	heapIndex  int
}

type searchSet struct {
	k            *keyGrid
	visited      *utils.Grid[bool]
	retMoves     *map[rune]*move
	thisMove     *move
	unlockedKeys int
}

func newSearchSet(k *keyGrid) *searchSet {
	unlockedKeys := 0
	for _, v := range k.state.keys {
		if !v {
			unlockedKeys++
		}
	}
	return &searchSet{
		k:            k,
		visited:      utils.NewGrid[bool](k.grid.Size),
		retMoves:     &map[rune]*move{},
		unlockedKeys: unlockedKeys,
	}
}

func (search *searchSet) hasSearchFinished() bool {
	return search.unlockedKeys == len(*search.retMoves)
}

func (search *searchSet) isVisited(v utils.Vector) bool {
	return search.visited.GetFast(v)
}

func (search *searchSet) setVisited(v utils.Vector) {
	search.visited.SetFast(v, true)
}
