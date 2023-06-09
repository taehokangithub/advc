package day16

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) string {
	return decodeFullPhaseGetResultAt(str, 0)
}

func solve02(str string) int {
	return 0
}

func Solve() {
	content, err := os.ReadFile("../data/input16.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY16", ans1, "67481260")

	ans2 := solve02(str)
	etc.AnswerChecker("DAY16", ans2, 2)
}
