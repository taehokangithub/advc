package day18

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	return NewKeyGrid(str).FindMinSteps()
}

func solve02(str string) int {
	return NewKeyGrid(str).FindMinSteps()
}

func Solve() {
	{
		content, err := os.ReadFile("../data/input18.txt")
		if err != nil {
			panic(err)
		}

		str := string(content)

		ans1 := solve01(str)
		etc.AnswerChecker("DAY18", ans1, 4510)
	}
	{
		content, err := os.ReadFile("../data/input18B.txt")
		if err != nil {
			panic(err)
		}

		str := string(content)

		ans2 := solve02(str)
		etc.AnswerChecker("DAY18", ans2, 1816)
	}
}
