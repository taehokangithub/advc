package day18

import (
	"taeho/advc19_go/utils"
)

var MovePool *utils.MemoryPool[move]

func (k *keyGrid) FindMinSteps() int {
	MovePool = utils.NewMemoryPool[move](k.grid.Size.X * k.grid.Size.Y * 20)

	c := NewKeyGridHeap()
	c.PushKeyGrid(k)
	for c.Len() > 0 {
		kptr := c.PopKeyGrid()
		if kptr.HasFoundAllKeys() {
			return kptr.steps
		}
		possibleStates := kptr.findPossibleMoves()
		for _, ps := range possibleStates {
			c.PushKeyGrid(ps)
		}
	}
	return 0
}

func (k *keyGrid) findPossibleMoves() []*keyGrid {
	ret := make([]*keyGrid, 0, 64)
	search := newSearchSet(k)
	MovePool.Reset()
	for i, myloc := range k.state.myLocs {
		search.thisMove = &move{
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
	sq := newSearchRingQueue(search.k.grid.Size.X * search.k.grid.Size.Y)

	sq.PushMove(search.thisMove)
	possibleMoves := make([]*move, 0, 4)

	for !sq.IsEmpty() {
		thisMove := sq.PopMove()
		search.setVisited(&thisMove.loc)
		thisMove.steps++
		possibleMoves = possibleMoves[:0]

		for _, dvec := range utils.DIR_VECTORS {
			nextLoc := &utils.Vector{
				X: thisMove.loc.X + dvec.X,
				Y: thisMove.loc.Y + dvec.Y,
			}
			tile := search.k.grid.GetFast(nextLoc)
			if tile == TILE_WALL {
				continue
			}
			if search.isVisited(nextLoc) {
				continue
			}
			nextMove := MovePool.New()
			nextMove.myLocIndex = thisMove.myLocIndex
			nextMove.steps = thisMove.steps
			nextMove.loc = *nextLoc

			if tile == TILE_EMPTY || search.checkIfPossibleMove(nextMove, tile) {
				possibleMoves = append(possibleMoves, nextMove)
			}
		}

		if search.hasSearchFinished() {
			return
		}

		for i := 0; i < len(possibleMoves); i++ {
			sq.PushMove(possibleMoves[i])
		}
	}
}

func (search *searchSet) checkIfPossibleMove(thisMove *move, tile rune) bool {
	k := search.k

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
