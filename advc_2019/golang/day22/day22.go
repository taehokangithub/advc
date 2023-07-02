package day22

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	arr := Shuffle(10007, str)
	for i := range arr {
		if arr[i] == 2019 {
			return i
		}
	}
	panic("No answer")
}

func solve02(str string) int64 {
	arr := Shuffle(10007, str)
	return int64(arr[2020])
}

func Solve() {
	content, err := os.ReadFile("../data/input22.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY22", ans1, 4086)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY22", ans2, 2)
}
