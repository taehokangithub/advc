package etc

import "fmt"

type AnswerType interface {
	int | int32 | int64 | float32 | float64 | string
}

var ansCnt map[string]int = make(map[string]int)
var wrongCnt int = 0
var rightCnt int = 0

var colorGreen string = "\033[32m"
var colorReset string = "\033[0m"
var colorRed string = "\033[31m"
var colorCyan string = "\033[36m"
var colorYellow string = "\033[33m"

func AnswerChecker[T AnswerType](title string, ans T, expected T) {

	ansCnt[title] = ansCnt[title] + 1
	cnt := ansCnt[title]

	fmt.Printf("%s<%s>%s answer %d = %v, expectd %v ", colorCyan, title, colorReset, cnt, ans, expected)

	if ans != expected {
		fmt.Printf("\n%s   ==> Wrong !!!%s\n", colorRed, colorReset)
		wrongCnt++
	} else {
		fmt.Printf("%s[OK]%s\n", colorGreen, colorReset)
		rightCnt++
	}
}

func FinalChecker() {

	if wrongCnt > 0 {
		fmt.Printf("%s[FAILED]%s %d correct, %d incorrect\n", colorRed, colorReset, rightCnt, wrongCnt)
	} else {
		fmt.Printf("%s[SUCCESS]%s %d correct answers\n", colorYellow, colorReset, rightCnt)
	}
}
