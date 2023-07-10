package day22

import "fmt"

type CircularBuff struct {
	min int
	max int
	pos int
	dir int // 1 for positive, -1 for negative
	arr []int64
}

func NewCircularBuff(min, max int) *CircularBuff {
	if min > 0 {
		panic(fmt.Sprintln("min should be minus, not", min))
	}
	totalLen := max - min + 1
	return &CircularBuff{
		min: min,
		max: max,
		dir: 1,
		arr: make([]int64, totalLen),
		pos: totalLen - max - 1,
	}
}

func CopyCircularBuff(other *CircularBuff) *CircularBuff {
	b := &CircularBuff{
		min: other.min,
		max: other.max,
		dir: other.dir,
		arr: make([]int64, len(other.arr)),
		pos: other.pos,
	}
	copy(b.arr, other.arr)
	return b
}

func (c *CircularBuff) GetIndexRaw(pos int) int {
	return (pos * c.dir) + c.pos
}

func (c *CircularBuff) IsInBoundary(pos int) bool {
	index := c.GetIndexRaw(pos)
	return index >= 0 && index < len(c.arr)
}

func (c *CircularBuff) GetIndex(pos int) int {
	offset := c.GetIndexRaw(pos)
	if offset >= len(c.arr) {
		offset -= len(c.arr)
	} else if offset < 0 {
		offset += len(c.arr)
	}
	return offset
}

func (c *CircularBuff) GetAt(pos int) int64 {
	return c.arr[c.GetIndex(pos)]
}

func (c *CircularBuff) SetAt(pos int, val int64) {
	c.arr[c.GetIndex(pos)] = val
}
