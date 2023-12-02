package day10

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	ast := createAsteroidGrid(str)
	ans1, _ := ast.getAllVisibleCount()
	return ans1
}

func solve02(str string) int {
	ast := createAsteroidGrid(str)
	_, loc := ast.getAllVisibleCount()
	v := ast.findNthDestroyed(loc, 200)
	return v.X*100 + v.Y
}

func Solve() {
	content, err := os.ReadFile("../data/input10.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY10", ans1, 314)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY10", ans2, 1513)
}
