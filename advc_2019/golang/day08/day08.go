package day08

import (
	"math"
	"os"
	"taeho/advc19_go/etc"
	"taeho/advc19_go/utils"
)

type Layer = utils.Grid[byte]

func splitLayers(str string, x, y int) []*Layer {
	ret := make([]*Layer, 0)
	stream := utils.NewStrStream(str)

	var grid *Layer

	for {
		if c, ok := stream.GetNext(); ok {
			if grid == nil {
				grid = utils.NewGrid[byte](utils.NewVector2D(x, y))
			}
			grid.Add(c - '0')

			if grid.IsFilled() {
				ret = append(ret, grid)
				grid = nil
			}
		} else {
			break
		}
	}

	return ret
}

func solve01(str string) int {
	layers := splitLayers(str, 25, 6)

	chosenIndex := -1
	leastZeroCnt := math.MaxInt
	for i, layer := range layers {
		zeroCnt := 0
		layer.Foreach(func(v utils.Vector, t byte) {
			if t == 0 {
				zeroCnt++
			}
		})
		if zeroCnt < leastZeroCnt {
			chosenIndex = i
			leastZeroCnt = zeroCnt
		}
	}

	layer := layers[chosenIndex]
	cnt1 := 0
	cnt2 := 0
	layer.Foreach(func(v utils.Vector, t byte) {
		if t == 1 {
			cnt1++
		} else if t == 2 {
			cnt2++
		}
	})

	return cnt1 * cnt2
}

func solve02(str string) {
	layers := splitLayers(str, 25, 6)
	layer := utils.NewGrid[byte](utils.NewVector2D(25, 6))

	layer.Foreach(func(v utils.Vector, _ byte) {
		for _, l := range layers {
			layer_val := l.Get(v)
			if layer_val != 2 {
				layer.Set(v, layer_val)
				return
			}
		}
	})

	layer.DumpToStringOnly(1)
}

func Solve() {
	content, err := os.ReadFile("../data/input08.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY08", ans1, 1088)

	solve02(str)
	etc.AnswerChecker("DAY08", 0, 0)
}
