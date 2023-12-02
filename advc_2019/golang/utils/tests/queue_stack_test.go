package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestQueue(t *testing.T) {
	q := utils.NewQueue[int64]()
	data := []int64{3, 5, 7, 8, 9}

	for _, d := range data {
		q.Push(d)
	}

	for i := range data {
		val := q.Pop()
		if val != data[i] {
			t.Errorf("[Queue] at %d, got %d expected %d", i, val, data[i])
		}
	}
}

func TestStack(t *testing.T) {
	q := utils.NewStack[int64]()
	data := []int64{3, 5, 7, 8, 9}

	for _, d := range data {
		q.Push(d)
	}

	for i := len(data) - 1; i >= 0; i-- {
		val := q.Pop()
		if val != data[i] {
			t.Errorf("[Stack] at %d, got %d expected %d", i, val, data[i])
		}
	}
}
