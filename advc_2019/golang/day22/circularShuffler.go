package day22

import (
	"fmt"
)

type CircularShuffler struct {
	*Shuffler
	*CircularBuff
}

func NewCircularShuffler(size int64, str string) *CircularShuffler {
	s := NewShuffler(size, str)
	min, max := s.Analyse()
	if min > 0 {
		panic(fmt.Sprintln("min should be minus, not", min))
	}
	totalLen := max - min + 1

	fmt.Println("Analyse - min", min, "max", max, "total", totalLen)

	// Test figures
	if totalLen < 11 {
		max = 6
		min = -3
	} else if int64(totalLen) > size {
		min += totalLen - int(size)
	}

	b := NewCircularBuff(min, max)

	c := &CircularShuffler{
		Shuffler:     s,
		CircularBuff: b,
	}

	for i := 0; i <= max; i++ {
		c.SetAt(i, int64(i))
	}
	for i := -1; i >= min; i-- {
		c.SetAt(i, size+int64(i))
	}

	fmt.Println("New circular - min", min, "max", max, "totallen", totalLen, "arrlen", len(c.arr), "curpos", b.pos)
	return c
}

func (c *CircularShuffler) RunCut(param int) {
	c.pos += param
	if c.pos > len(c.arr) {
		c.pos -= len(c.arr)
	} else if c.pos < 0 {
		c.pos += len(c.arr)
	}
}

func (c *CircularShuffler) RunReverse() {
	c.pos -= c.dir
	c.dir = c.dir * (-1)
}

func (c *CircularShuffler) RunIncre(param int) {
	remainder := c.size % int64(param)
	quotient := c.size / int64(param)

	curStartPos := 0
	curStartIndex := 0
	copied := CopyCircularBuff(c.CircularBuff)
	finished := false
	execution := 0
	for !finished {
		execution++
		if execution > len(c.arr) {
			panic(fmt.Sprintln("execution", execution, "exceeds", len(c.arr)))
		}
		// first forward should exist at any cost
		if !c.IsInBoundary(curStartPos) {
			if c.IsInBoundary(curStartPos + len(c.arr)) {
				curStartPos += len(c.arr)
			} else if c.IsInBoundary(curStartPos - len(c.arr)) {
				curStartPos -= len(c.arr)
			} else {
				panic(fmt.Sprintln("failed to set the first forward at", curStartPos))
			}
		}
		fmt.Println("[", execution, "]", "curpos", curStartPos, "curindex", curStartIndex, "(param", param, "remainder", remainder, "quo", quotient, ")")
		nextStartPos := curStartPos - int(remainder)
		nextStartIndex := curStartIndex + 1

		// forward
		for nextPos, nextIndex := curStartPos, curStartIndex; c.IsInBoundary(nextPos); nextPos += param {
			fmt.Println("   ==> pos ", nextPos, "index", nextIndex, "value setting", copied.GetAt(nextIndex))
			c.SetAt(nextPos, copied.GetAt(nextIndex))
			nextIndex++

			// fall-back for not finding out backward
			nextStartPos = nextPos - int(remainder)
			nextStartIndex = nextIndex
		}

		// backward
		isFirst := true

		for backPos, backIndex := (curStartPos - int(remainder)), (curStartIndex + int(quotient)); c.IsInBoundary(backPos); backPos -= param {
			if backIndex > len(c.arr) {
				fmt.Println("Backindex", backIndex, "is greater than length", len(c.arr), "is it okay?")
				finished = true
			} else if backIndex == len(c.arr) {
				finished = true
			} else {
				fmt.Println("  ==> backpos ", backPos, "backIndex", backIndex, "setting", copied.GetAt(backIndex))
				c.SetAt(backPos, copied.GetAt(backIndex))

				if isFirst {
					isFirst = false
					nextStartPos = backPos + param
					nextStartIndex = backIndex + 1
				}
			}
			backIndex--
		}

		if curStartPos == nextStartPos {
			fmt.Println("Breaking at", nextStartPos, "execution", execution)
			break
		}
		curStartIndex = nextStartIndex
		curStartPos = nextStartPos

	}
}

func (c *CircularShuffler) RunInstructions() {
	for _, inst := range c.instructions {
		switch inst.instType {
		case INST_CUT:
			c.RunCut(inst.param)
		case INST_REVERSE:
			c.RunReverse()
		case INST_INCRE:
			c.RunIncre(inst.param)
		default:
			panic(fmt.Sprintln("Unknown inst", inst.instType))
		}
		if !c.SanityCheck(c.arr) {
			panic(fmt.Sprintln("Sanity check failed after", inst))
		}
	}
}

func (c *CircularShuffler) SanityCheck(arr []int64) bool {
	m := map[int64]bool{}
	ret := true

	for i := range arr {
		if _, ok := m[arr[i]]; ok {
			fmt.Println("Already have", arr[i], "at", i)
			ret = false
		}
		m[arr[i]] = true
	}
	fmt.Println("Sanity check", ret)
	return ret
}

func (c *CircularShuffler) Shuffle() []int64 {
	if c.size > 100*100*100 {
		panic(fmt.Sprintln("Size", c.size, "can't be used for enumerating all elements"))
	}

	c.RunInstructions()

	ret := make([]int64, c.size)

	for i := range ret {
		ret[i] = c.GetAt(i)
	}

	return ret
}
