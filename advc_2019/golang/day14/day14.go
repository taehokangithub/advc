package day14

import (
	"os"
	"taeho/advc19_go/etc"
)

const MAX_ORE_CNT = 1000000000000

func solve01(str string) int64 {
	f := NewFactory(str)
	f.ProduceOneFuel()
	return f.cost
}

func solve02(str string) int64 {
	f := NewFactory(str)
	return f.ConsumeOre(MAX_ORE_CNT)
}

func Solve() {
	content, err := os.ReadFile("../data/input14.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY14", ans1, 273638)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY14", ans2, 4200533)
}
