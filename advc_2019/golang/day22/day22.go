package day22

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int64 {
	s := NewShuffler(10007, str)
	return s.GetFinalPositionOf(2019)
}

func solve02(str string) int64 {
	s := NewShuffler(119315717514047, str)
	return s.GetCardAtAfterIterations(2020, 101741582076661)
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
	etc.AnswerChecker("DAY22", ans2, 1041334417227)
}
