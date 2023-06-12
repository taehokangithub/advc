package day18

import (
	"sort"
)

type candidate struct {
	arr []string
	m   map[string]*keyGrid
}

func NewCandidate() *candidate {
	return &candidate{
		arr: make([]string, 0),
		m:   map[string]*keyGrid{},
	}
}

func (c *candidate) AddCandidate(k *keyGrid) {
	kstr := k.state.String()
	if v, exists := c.m[kstr]; exists {
		if v.steps > k.steps {
			c.m[kstr] = k
			sort.Slice(c.arr, func(i, j int) bool {
				return c.m[c.arr[i]].steps < c.m[c.arr[j]].steps
			})
		}
		return // do nothing ; did not qualify
	}
	c.m[kstr] = k
	insertAt := len(c.arr)
	for i := 0; i < insertAt; i++ {
		if k.steps < c.m[c.arr[i]].steps {
			insertAt = i
			break
		}
	}
	if insertAt >= len(c.arr) {
		c.arr = append(c.arr, kstr)
	} else {
		c.arr = append(c.arr[:insertAt+1], c.arr[insertAt:]...)
		c.arr[insertAt] = kstr
	}
}

func (c *candidate) PopCandidate() *keyGrid {
	poppedStr := c.arr[0]
	popped := c.m[poppedStr]

	c.arr = c.arr[1:]
	delete(c.m, poppedStr)
	return popped
}

func (c *candidate) Count() int {
	return len(c.arr)
}
