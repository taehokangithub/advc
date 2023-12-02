package day21

import (
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
)

func runScript(program string, script string) int {
	com := computer.NewAsciiCom(program)

	com.AddStrInput(script)

	com.RunProgram()

	outputs := com.GetOutput()
	return int(outputs[len(outputs)-1])
}

func solve01(str string) int {
	script := `NOT A J
NOT C T
AND D T
OR T J
WALK
`
	return runScript(str, script)
}

func solve02(str string) int {
	script := `NOT C J
AND D J
OR E T
OR H T
AND T J
NOT B T
AND D T
OR T J
NOT A T
OR T J
RUN
`
	return runScript(str, script)
}

func Solve() {
	content, err := os.ReadFile("../data/input21.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY21", ans1, 19352720)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY21", ans2, 1143652885)
}
