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

func decodeFullPhaseGetResultAt(str string) string {
	for i := 0; i < FullPhases; i++ {
		str = decodeOnePhase(str)
	}

	return str[:OutputLegth]
}

func decodeReapeatedOnePhase(str string) string {
	totalLen := len(str)
	intarr := make([]int, totalLen)

	for i, c := range str {
		intarr[i] = int(c - '0')
	}

	backStuffValue := 0
	for i := OutputLegth; i < totalLen; i++ {
		backStuffValue += intarr[i]
	}

	var builder strings.Builder

	for i := 0; i < OutputLegth; i++ {
		val := 0
		for j := i; j < OutputLegth; j++ {
			val += intarr[j]
		}
		totalVal := val + backStuffValue
		valToWrite := utils.Abs(totalVal) % 10
		builder.WriteString(fmt.Sprint(valToWrite))
	}

	for i := OutputLegth; i < len(str); i++ {
		valToWrite := utils.Abs(backStuffValue) % 10
		builder.WriteString(fmt.Sprint(valToWrite))
		backStuffValue -= intarr[i]
	}

	return builder.String()
}

func decodeRepeatedFullPhase(str string, repeat int) string {
	loc, err := strconv.Atoi(str[:7])
	if err != nil {
		panic(err)
	}

	strlen := len(str)
	totalLen := strlen * repeat
	gap := totalLen - loc
	requiredSets := 1 + gap/strlen
	startAt := loc - (totalLen - strlen*requiredSets)
	stubStr := strings.Repeat(str, requiredSets)[startAt:]

	for i := 0; i < FullPhases; i++ {
		stubStr = decodeReapeatedOnePhase(stubStr)
	}

	return stubStr[:OutputLegth]
}
