package day18

import (
	"taeho/advc19_go/utils"
)

func (k *keyGrid) HasFoundAllKeys() bool {
	for _, v := range k.state.keys {
		if !v {
			return false
		}
	}
	return true
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
	ck := k.Copy()
	moves := make([]move, 0)

	for i, myloc := range k.state.myLocs {
		reachableKeyMoves := findReachableKeys(ck, myloc, 0)
		for k, move := range reachableKeyMoves {
			move.myLocIndex = i
			reachableKeyMoves[k] = move
		}
		moves = append(moves, reachableKeyMoves...)
	}

	for _, move := range moves {
		ck = k.Copy()
		tile := ck.grid.Get(move.keyLoc)
		keyName := makeKeyame(tile)
		ck.state.keys[keyName] = true
		ck.grid.Set(move.keyLoc, TILE_EMPTY)
		ck.steps += move.steps
		ck.state.myLocs[move.myLocIndex] = move.keyLoc
		ret = append(ret, ck)
	}
	return ret
}

func findReachableKeys(k *keyGrid, loc utils.Vector, steps int) []move {
	ret := make([]move, 0)
	steps++
	k.grid.Set(loc, TILE_VISITED)

	for _, dvec := range utils.DIR_VECTORS {

		movedLoc := loc.GetAdded(dvec)
		if k.grid.IsValidVector(movedLoc) {
			tile := k.grid.Get(movedLoc)
			if tile == TILE_VISITED || tile == TILE_WALL {
				continue
			}
			if tile >= 'A' && tile <= 'Z' { // Door found
				if !k.state.keys[tile] {
					continue
				}
			} else if tile >= 'a' && tile <= 'z' { // Key found
				ret = append(ret, move{
					keyLoc: movedLoc,
					steps:  steps,
				})
				continue
			}

			// go further
			furtherMoves := findReachableKeys(k, movedLoc, steps)
			if len(furtherMoves) > 0 {
				ret = append(ret, furtherMoves...)
			}
		}
	}

	return ret
}
