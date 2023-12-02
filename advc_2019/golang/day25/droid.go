package day25

import (
	"bufio"
	"fmt"
	"os"
	"strings"
	"taeho/advc19_go/computer"
)

func RunDroidInteractive(str string) {
	com := computer.NewAsciiCom(str)
	reader := bufio.NewReader(os.Stdin)

	for i := 0; i < 10000; i++ {
		com.RunProgram()
		out := com.GetStrOutput()
		if len(out) == 0 {
			break
		}
		fmt.Println("[COM/", i, "]", out)
		if strings.Contains(out, "Command?") {

			input, _ := reader.ReadString('\n')
			input = strings.ReplaceAll(input, "\n", "")
			input = strings.ReplaceAll(input, "\r", "")

			cmd := input + string(rune(10))
			fmt.Println("==>", cmd)
			com.AddStrInput(cmd)
		}
	}
}
