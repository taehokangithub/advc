package day22

import (
	"fmt"
)

type CircularShuffler struct {
	*Shuffler
	min int
	max int
	pos int
	dir int // 1 for positive, -1 for negative
	arr []int64
}

func NewCircularShuffler(size int64, str string) *CircularShuffler {
	s := NewShuffler(size, str)
	min, max := s.Analyse()
	totalLen := min + max

	c := &CircularShuffler{
		Shuffler: s,
		min:      min,
		max:      max,
		dir:      1,
		arr:      make([]int64, totalLen),
		pos:      totalLen - max,
	}

	return c
}

func (c *CircularShuffler) GetIndexRaw(pos int) int {
	return (pos * c.dir) + c.pos
}

func (c *CircularShuffler) IsInBoundary(pos int) bool {
	index := c.GetIndexRaw(pos)
	return index <= c.max && index >= c.min
}

func (c *CircularShuffler) GetIndex(pos int) int {
	offset := c.GetIndexRaw(pos)
	if offset > c.max {
		offset = (offset - c.max - 1) + c.min
	} else if offset < c.min {
		offset = c.max - (c.min - offset - 1)
	}
	if offset > c.max || offset < c.min {
		panic(fmt.Sprintln("offset", offset, "is invalid. pos", pos, ", from curpos", c.pos, ", min", c.min, "max", c.max))
	}
	return offset
}

func (c *CircularShuffler) GetAt(pos int) int64 {
	return c.arr[c.GetIndex(pos)]
}

func (c *CircularShuffler) SetAt(pos int, val int64) {
	c.arr[c.GetIndex(pos)] = val
}

func (c *CircularShuffler) RunCut(param int) {
	c.pos += param
	if c.pos > c.max || c.pos < c.min {
		panic(fmt.Sprintln("min", c.min, "max", c.max, "param", param, "pos", c.pos))
	}
}

func (c *CircularShuffler) RunReverse() {
	c.dir = c.dir * (-1)
}

/*
ex: 3 for 10

0 1 2 3 4 5 6 7 8 9
0     1     2     3
    4     5     6
  7     8     9

r : 1
q : 3
-4 -3 -2 -1 00 +1 +2 +3 +4 +5
 2        3  0        1
       6
    9


ex : 3 for 8
0 1 2 3 4 5 6 7
0     1     2
  3     4     5
    6     7

ex : 4 for 15
0 1 2 3 4 5 6 7 8 9 A B C D E
0       1       2       3
  4       5       6       7
    8       9       A       B
      C       D       E
*/

func (c *CircularShuffler) RunIncre(param int) {
	//remainder := c.size % int64(param)
	//quotient := c.size / int64(param)

	for i := 0; i < param; i++ {

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
	}
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
