package day18

import (
	"taeho/advc19_go/utils"
)

func (k *keyGrid) FindMinSteps() int {
	c := NewCandidate()
	c.AddCandidate(k)
	for c.Count() > 0 {
		kptr := c.PopCandidate()
		if kptr.HasFoundAllKeys() {
			return kptr.steps
		}
		possibleStates := kptr.findPossibleMoves()
		for _, ps := range possibleStates {
			c.AddCandidate(ps)
		}
	}
	return 0
}

func (k *keyGrid) findPossibleMoves() []*keyGrid {
	ret := make([]*keyGrid, 0, 64)
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
		ck.SetKeyUnlocked(tile, true)
		ck.steps += move.steps
		ck.state.myLocs[move.myLocIndex] = move.loc
		ret = append(ret, ck)
	}
	return ret
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

		for i := 0; i < len(possibleMoves); i++ {
			sq.push(possibleMoves[i])
		}
	}
}

func (search *searchSet) checkIfPossibleMove(thisMove move) bool {
	k := search.k

	tile := k.grid.GetFast(thisMove.loc)
	if tile == TILE_WALL {
		return false
	}

	if search.isVisited(thisMove.loc) {
		return false
	}

	if k.IsDoor(tile) { // Door found
		return k.HasKeyUnlocked(tile)
	}

	if k.IsKey(tile) && !k.HasKeyUnlocked(tile) { // Key found
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
