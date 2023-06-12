package day18

import "taeho/advc19_go/utils"

type move struct {
	myLocIndex int
	loc        utils.Vector
	steps      int
}

type searchSet struct {
	k            *keyGrid
	visited      *utils.Grid[bool]
	retMoves     *map[rune]move
	thisMove     move
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
		retMoves:     &map[rune]move{},
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

type searchQueue struct {
	q []move
}

func newSearchQueue() *searchQueue {
	return &searchQueue{
		q: make([]move, 0, 255),
	}
}

func (sq *searchQueue) isEmpty() bool {
	return len(sq.q) == 0
}

func (sq *searchQueue) pop() move {
	ret := sq.q[0]
	sq.q = sq.q[1:]
	return ret
}

func (sq *searchQueue) push(m move) {
	insertAt := len(sq.q)
	for i := 0; i < insertAt; i++ {
		if m.steps < sq.q[i].steps {
			insertAt = i
			break
		}
	}

	if insertAt >= len(sq.q) {
		sq.q = append(sq.q, m)
	} else {
		sq.q = append(sq.q[:insertAt+1], sq.q[insertAt:]...)
		sq.q[insertAt] = m
	}
}
