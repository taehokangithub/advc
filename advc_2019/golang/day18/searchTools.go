package day18

import "taeho/advc19_go/utils"

type move struct {
	myLocIndex int
	loc        utils.Vector
	steps      int
}

type searchSet struct {
	k        *keyGrid
	visited  *utils.Grid[bool]
	retMoves *map[rune]move
	thisMove move
}

func newSearchSet(k *keyGrid) *searchSet {
	return &searchSet{
		k:        k,
		visited:  utils.NewGrid[bool](k.grid.Size),
		retMoves: &map[rune]move{},
	}
}

func (search *searchSet) isVisited(v utils.Vector) bool {
	return search.visited.Get(v)
}

func (search *searchSet) setVisited(v utils.Vector) {
	search.visited.Set(v, true)
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
