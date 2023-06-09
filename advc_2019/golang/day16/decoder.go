package day16

import (
	"fmt"
	"strconv"
	"strings"
	"taeho/advc19_go/utils"
)

const FullPhases = 100
const OutputLegth = 8

var BasePattern []int = []int{0, 1, 0, -1}

func decodeOnePhaseOneLine(input string, patternLength int) int {
	plength := patternLength - 1
	pIndex := 0
	val := 0
	for _, c := range input {
		n, err := strconv.Atoi(string(c))
		if err != nil {
			panic(fmt.Sprintf("%v(%d), curLen %d, curIndex %d, err %v", input, patternLength, plength, pIndex, err))
		}
		if plength == 0 {
			plength = patternLength
			pIndex = (pIndex + 1) % len(BasePattern)
		}
		pattern := BasePattern[pIndex]
		plength--
		val += pattern * n
	}
	return utils.Abs(val) % 10
}

func decodeOnePhase(input string) string {
	var builder strings.Builder

	for plength := 1; plength <= len(input); plength++ {
		oneVal := decodeOnePhaseOneLine(input, plength)
		builder.WriteString(fmt.Sprint(oneVal))
	}
	return builder.String()
}

func decodeFullPhaseGetResultAt(str string, location int) string {
	for i := 0; i < FullPhases; i++ {
		str = decodeOnePhase(str)
		//fmt.Println(i+1, str)
	}

	return str[location : location+OutputLegth]
}

func decodeRepeatedFullPhaseGetResultAt(str string, repeat int, location int) string {
	input := strings.Repeat(str, repeat)
	return decodeFullPhaseGetResultAt(input, 0)
}
