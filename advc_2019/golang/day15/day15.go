package day15

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	m := NewMaze(str)
	return m.ExploreGoal(true)
}

func solve02(str string) int {
	m := NewMaze(str)
	m.ExploreGoal(false)
	return m.Expand()
}

func Solve() {
	content, err := os.ReadFile("../data/input15.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY15", ans1, 262)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY15", ans2, 314)
}
