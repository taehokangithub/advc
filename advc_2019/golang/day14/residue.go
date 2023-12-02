package day14

import (
	"fmt"
	"sort"
	"strings"
)

type Residue struct {
	data map[*Ingredient]int64
}

func NewResidue() *Residue {
	return &Residue{
		data: make(map[*Ingredient]int64),
	}
}

func (r *Residue) Copy() *Residue {
	copied := NewResidue()

	for k, v := range r.data {
		copied.data[k] = v
	}

	return copied
}

func (r *Residue) Add(ingr *Ingredient, cnt int64) {
	r.data[ingr] = r.data[ingr] + cnt
}

func (r *Residue) GetCount(ingr *Ingredient) int64 {
	return r.data[ingr]
}

func (r *Residue) Remove(ingr *Ingredient, cnt int64) {
	if r.GetCount(ingr) < cnt {
		panic(fmt.Sprintf("%s has only %d residue, demanding %d", ingr.name, r.GetCount(ingr), cnt))
	}
	r.data[ingr] -= cnt
}

func (r *Residue) String() string {
	var builder strings.Builder

	arr := make([]*Ingredient, len(r.data))

	index := 0
	for ingr := range r.data {
		arr[index] = ingr
		index++
	}

	sort.Slice(arr, func(a, b int) bool {
		return arr[a].name > arr[b].name
	})

	for _, ingr := range arr {
		cnt := r.data[ingr]
		builder.WriteString(fmt.Sprintf("[%s:%d]", ingr.name, cnt))
	}

	return builder.String()
}
