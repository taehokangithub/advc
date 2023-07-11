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

func (c *CircularBuff) GetIndexRaw(rel int) int {
	return (rel * c.dir) + c.pos
}

func (c *CircularBuff) IsInBoundary(rel int) bool {
	index := c.GetIndex(rel)
	return index >= 0 && index < len(c.arr)
}

func (c *CircularBuff) GetIndex(rel int) int {
	offset := c.GetIndexRaw(rel)
	if offset >= len(c.arr) {
		offset -= len(c.arr)
	} else if offset < 0 {
		offset += len(c.arr)
	}
	return offset
}

func (c *CircularBuff) GetAt(rel int) int64 {
	return c.arr[c.GetIndex(rel)]
}

func (c *CircularBuff) SetAt(rel int, val int64) {
	c.arr[c.GetIndex(rel)] = val
}
