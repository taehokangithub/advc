package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestRingQueue(t *testing.T) {
	rq := utils.NewRingQueue[int](5)

	if !rq.IsEmpty() {
		t.Error("Queue is not empty")
	}
	if rq.IsFull() {
		t.Error("Queue is already full")
	}

	samples := []int{3, 5, 11, 6, 8, 1, 2}

	for i := 0; i < 5; i++ {
		rq.Push(samples[i])
	}

	if rq.IsEmpty() || !rq.IsFull() {
		t.Error("After full-pushing, empty", rq.IsEmpty(), "full", rq.IsFull())
	}

	expectPanic(t, "Queue overflow", func() {
		rq.Push(0)
	})

	for i := 0; i < 3; i++ {
		v := rq.Pop()
		if v != samples[i] {
			t.Error("At", i, "got", v, "expected", samples[i])
		}
	}

	for i := 5; i < 7; i++ {
		rq.Push(samples[i])
	}

	for i := 3; i < 7; i++ {
		v := rq.Pop()
		if v != samples[i] {
			t.Error("At", i, "got", v, "expected", samples[i])
		}
	}
}
