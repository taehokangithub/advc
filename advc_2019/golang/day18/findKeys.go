package day18

import (
	"taeho/advc19_go/utils"
)

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

func (k *keyGrid) FindMinSteps() int {
	c := NewCandidate()
	c.AddCandidate(k)
	for c.Count() > 0 {
		kptr := c.PopCandidate()
		if kptr.HasFoundAllKeys() {
			return kptr.steps
		}
		possibleStates := findPossibleMoves(kptr)
		for _, ps := range possibleStates {
			c.AddCandidate(ps)
		}
	}
	return 0
}

func findPossibleMoves(k *keyGrid) []*keyGrid {
	ret := make([]*keyGrid, 0)
	search := newSearchSet(k)
	for i, myloc := range k.state.myLocs {
		search.thisMove = move{
			myLocIndex: i,
			loc:        myloc,
			steps:      0,
		}

		search.findNextMovesV2()
	}

	for _, move := range *search.retMoves {
		ck := k.Copy()
		tile := ck.grid.Get(move.loc)
		keyName := makeKeyame(tile)
		ck.state.keys[keyName] = true
		ck.grid.Set(move.loc, TILE_EMPTY)
		ck.steps += move.steps
		ck.state.myLocs[move.myLocIndex] = move.loc
		ret = append(ret, ck)
	}
	return ret
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

func (search *searchSet) findNextMovesV2() {
	sq := newSearchQueue()
	sq.push(search.thisMove)

	for !sq.isEmpty() {
		thisMove := sq.pop()
		//fmt.Println("* popped loc", thisMove.loc, "got steps", thisMove.steps+search.k.steps, "qlen", len(sq.q), "isempty", sq.isEmpty())
		search.setVisited(thisMove.loc)
		possibleMoves := make([]move, 0, 4)
		for _, dvec := range utils.DIR_VECTORS {
			nextMove := move{
				myLocIndex: thisMove.myLocIndex,
				steps:      thisMove.steps + 1,
				loc:        thisMove.loc.GetAdded(dvec),
			}
			if search.checkIfPossibleMove(nextMove) {
				possibleMoves = append(possibleMoves, nextMove)
			}
		}

		numPossibleMoves := len(possibleMoves)
		if numPossibleMoves > 0 {
			for i := 0; i < numPossibleMoves; i++ {
				sq.push(possibleMoves[i])
			}
		}
	}
}

func (search *searchSet) checkIfPossibleMove(thisMove move) bool {
	k := search.k
	if !k.grid.IsValidVector(thisMove.loc) {
		return false
	}

	tile := k.grid.Get(thisMove.loc)
	if tile == TILE_WALL {
		return false
	}

	if search.isVisited(thisMove.loc) {
		return false
	}

	if tile >= 'A' && tile <= 'Z' { // Door found
		if !k.state.keys[tile] {
			return false
		}
	} else if tile >= 'a' && tile <= 'z' { // Key found
		if existingMove, exists := (*search.retMoves)[tile]; exists {
			if existingMove.steps <= thisMove.steps {
				return false // discard this move
			}
		}
		(*search.retMoves)[tile] = thisMove // Save this move as a key-founder
		return false                        // but return false, should not go further
	}

	return true
}
