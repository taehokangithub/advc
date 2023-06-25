package day20

import (
	"container/heap"
	"fmt"
)

func (mv *GraphMove) MapKey() string {
	return fmt.Sprintf("[%v:%d]", mv.node.loc, mv.layer)
}

type MoveHeapNode struct {
	move  *GraphMove
	index int
}
type MoveHeap struct {
	heap    []*MoveHeapNode
	moveMap map[string]*MoveHeapNode
}

func NewMoveHeap() *MoveHeap {
	return &MoveHeap{
		heap:    make([]*MoveHeapNode, 0),
		moveMap: make(map[string]*MoveHeapNode),
	}
}

func (m *MoveHeap) Swap(i, j int) {
	m.heap[i], m.heap[j] = m.heap[j], m.heap[i]
	m.heap[i].index = i
	m.heap[j].index = j
}

func (m *MoveHeap) Len() int {
	return len(m.heap)
}

func (m *MoveHeap) Less(i, j int) bool {
	return m.heap[i].move.dist < m.heap[j].move.dist
}

func (m *MoveHeap) Push(x interface{}) {
	node := x.(*MoveHeapNode)
	m.heap = append(m.heap, node)
}

func (m *MoveHeap) Pop() interface{} {
	node := m.heap[len(m.heap)-1]
	node.index = -1
	m.heap = m.heap[0 : len(m.heap)-1]
	return node
}

func (m *MoveHeap) PushMove(mv *GraphMove) {
	mvKey := mv.MapKey()
	if oldMv, ok := m.moveMap[mvKey]; ok {
		if oldMv.move.dist > mv.dist {
			oldMv.move = mv
			heap.Fix(m, oldMv.index)
		}
	} else {
		node := &MoveHeapNode{
			move:  mv,
			index: len(m.heap),
		}
		m.moveMap[mvKey] = node
		heap.Push(m, node)
	}
}

func (m *MoveHeap) PushNewMove(node *Node, dist int, layer int) {
	m.PushMove(&GraphMove{
		node:  node,
		dist:  dist,
		layer: layer,
	})
}

func (m *MoveHeap) PopMove() *GraphMove {
	item := heap.Pop(m)
	mvNode := item.(*MoveHeapNode)
	delete(m.moveMap, mvNode.move.MapKey())
	return mvNode.move
}
