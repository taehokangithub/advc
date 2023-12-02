package day24

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int64 {
	smg := NewSmallGrid(str)
	return smg.BiodiversityRating()
}

func solve02(str string) int {
	smg := NewSmallGridMultiLevel(str)
	return smg.CntMultiLevelAfter(200)
}

func Solve() {
	content, err := os.ReadFile("../data/input24.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY24", ans1, 30442557)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY24", ans2, 1987)
}
