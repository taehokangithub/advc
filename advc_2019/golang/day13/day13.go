package day13

import (
	"os"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	arc := NewArcade(str)
	arc.ProcessScreen()
	return arc.CountTile(TILE_BLOCK)
}

func solve02(str string) int {
	arc := NewArcade(str)
	arc.PlayGame()
	return arc.score
}

func Solve() {
	content, err := os.ReadFile("../data/input13.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY13", ans1, 296)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY13", ans2, 13824)
}
