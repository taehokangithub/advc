package day12

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	b := NewBodies(str)
	b.Simulate(1000)
	return b.TotalEnergy()
}

func solve02(str string) int64 {
	b := NewBodies(str)
	return b.GetReturnToBase()
}

func Solve() {
	content, err := os.ReadFile("../data/input12.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY12", ans1, 10028)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY12", ans2, 314610635824376)
}
