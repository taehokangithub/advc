package day19

import (
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	cnt := 0
	comorg := computer.NewComputer(str)
	for x := 0; x < 50; x++ {
		for y := 0; y < 50; y++ {
			com := comorg.Copy()
			com.AddInput(int64(y))
			com.AddInput(int64(x))
			com.RunProgram()
			ret := com.PopOutput()
			if ret == 1 {
				cnt++
			}
		}
	}
	return cnt
}

func solve02(str string) int {
	return FindFit(str)
}

func Solve() {
	content, err := os.ReadFile("../data/input19.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY19", ans1, 217)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY19", ans2, 6840937)
}
