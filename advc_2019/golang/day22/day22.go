package day22

import (
	"fmt"
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	c := NewCircularShuffler(10007, str)
	arr := c.Shuffle()

	//arr := Shuffle(10007, str)
	ans := 0
	for i := range arr {
		if arr[i] == 2019 {
			ans = i
		}
		fmt.Println("[", i, "] =>", arr[i])
	}
	return ans
	//panic("No answer")
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
