package day14

import (
	"fmt"
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	bl := ParseBlueprint(str)
	fmt.Println(bl)
	return 0
}

func solve02(str string) int {
	return 0
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
