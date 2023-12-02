package day23

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int64 {
	r := NewRouter(50, str)

	return r.FindYOfFirstPacketToN(255)
}

func solve02(str string) int64 {
	r := NewRouter(50, str)

	return r.FindYOfFirstSameY_FromNat()
}

func Solve() {
	content, err := os.ReadFile("../data/input23.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY23", ans1, 17949)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY23", ans2, 12326)
}
