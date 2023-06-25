package day20

import (
	"os"
	"taeho/advc19_go/etc"
)

func Solve() {
	content, err := os.ReadFile("../data/input20.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)
	mz := NewMageGraph(str)
	ans1 := mz.FindShortestPathToExit()
	etc.AnswerChecker("DAY20", ans1, 602)

	ans2 := NewMageGraph(str).FindLayeredShortestPathToExit()
	etc.AnswerChecker("DAY20", ans2, 6986)
}
