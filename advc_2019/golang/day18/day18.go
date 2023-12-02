package day18

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01() int {
	content, err := os.ReadFile("../data/input18.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)
	return NewKeyGrid(str).FindMinSteps()
}

func solve02() int {
	content, err := os.ReadFile("../data/input18B.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)
	return NewKeyGrid(str).FindMinSteps()
}

func Solve() {
	{
		ans1 := solve01()
		etc.AnswerChecker("DAY18", ans1, 4510)
	}
	{
		ans2 := solve02()
		etc.AnswerChecker("DAY18", ans2, 1816)
	}
}
