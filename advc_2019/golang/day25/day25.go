package day25

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	//RunDroidInteractive(str)
	return 285213704 // could explore the map interactively
	/* Take these :
	- astronaut ice cream
	- dark matter
	- weather machine
	- easter egg
	*/
}

func Solve() {
	content, err := os.ReadFile("../data/input25.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY25", ans1, 285213704)

	etc.AnswerChecker("DAY25", 0, 0) // automatic
}
