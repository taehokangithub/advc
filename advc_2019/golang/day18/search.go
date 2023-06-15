package day18

var SearchMH *searchMoveHeap

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

/* Proved to be slower - but leaving here for reference
var MovePool *utils.MemoryPool[move]
var SearchRQ *utils.RingQueue[*move]
// Also needs these initialisations somewhere
	//MovePool = utils.NewMemoryPool[move](k.grid.Size.X * k.grid.Size.Y * 20)
	//SearchRQ = utils.NewRingQueue[*move](k.grid.Size.X * k.grid.Size.Y)

func (k *keyGrid) findPossibleMovesV2() []*keyGrid {
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

	for _, mv := range *search.retMoves {
		ck := k.Copy()
		tile := ck.grid.Get(mv.loc)
		ck.SetKeyUnlocked(tile, true)
		ck.steps += mv.steps
		ck.state.myLocs[mv.myLocIndex] = mv.loc
		ret = append(ret, ck)
	}
	return ret
}

func (search *searchSet) findNextMovesV2() {
	sq := SearchRQ
	sq.Clear()

	sq.Push(search.thisMove)
	possibleMoves := make([]*move, 0, 4)

	for !sq.IsEmpty() {
		thisMove := sq.Pop()
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
			sq.Push(possibleMoves[i])
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
*/
