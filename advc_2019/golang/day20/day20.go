package day20

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	mz := NewMaze(str)
	return mz.FindShortestPath(false)
}

func solve02(str string) int {
	mz := NewMaze(str)
	return mz.FindShortestPath(true)
}

func Solve() {
	content, err := os.ReadFile("../data/input20.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY20", ans1, 602)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY20", ans2, 6986)
}
