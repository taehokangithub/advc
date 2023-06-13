package day18

import (
	"container/heap"
	"fmt"
)

type keyGridHeapItem struct {
	k     *keyGrid
	index int // Beware! the manually managed index is required when you need to call heap.Fix()
}

type keyGridHeap struct {
	arr []*keyGridHeapItem
	m   map[string]*keyGridHeapItem
}

func (i *keyGridHeapItem) String() string {
	return fmt.Sprintf("[%d]:%v", i.index, i.k.state.String())
}

func NewKeyGridHeap() *keyGridHeap {
	return &keyGridHeap{
		arr: make([]*keyGridHeapItem, 0, 1024),
		m:   make(map[string]*keyGridHeapItem, 1024),
	}
}

func (h *keyGridHeap) Push(x interface{}) {
	item := x.(*keyGridHeapItem)
	item.index = len(h.arr)
	h.arr = append(h.arr, item)
}

func (h *keyGridHeap) PushKeyGrid(k *keyGrid) {
	str := k.state.String()
	if val, ok := h.m[str]; ok {
		if val.k.steps > k.steps {
			val.k = k
			heap.Fix(h, val.index)
		}
	} else {
		item := &keyGridHeapItem{k: k}
		heap.Push(h, item)
		h.m[str] = item
	}
}

func (h *keyGridHeap) Pop() interface{} {
	index := len(h.arr) - 1
	item := h.arr[index]
	h.arr = h.arr[:index]

	item.index = -1
	return item
}

func (h *keyGridHeap) PopKeyGrid() *keyGrid {
	k := heap.Pop(h).(*keyGridHeapItem).k
	delete(h.m, k.state.String())
	return k
}

func (h *keyGridHeap) Swap(i, j int) {
	h.arr[i], h.arr[j] = h.arr[j], h.arr[i]
	h.arr[i].index = i
	h.arr[j].index = j
}

func (h *keyGridHeap) Less(i, j int) bool {
	return h.arr[i].k.steps < h.arr[j].k.steps
}

func (h *keyGridHeap) Len() int {
	return len(h.arr)
}
