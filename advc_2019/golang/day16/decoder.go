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

func decodeReapeatedOnePhase(bytearr []byte) []byte {
	totalLen := len(bytearr)

	backStuffValue := 0
	for i := OutputLegth; i < totalLen; i++ {
		backStuffValue += int(bytearr[i])
	}

	output := make([]byte, totalLen)

	for i := 0; i < OutputLegth; i++ {
		val := 0
		for j := i; j < OutputLegth; j++ {
			val += int(bytearr[j])
		}
		totalVal := val + backStuffValue
		output[i] = byte(utils.Abs(totalVal) % 10)
	}

	for i := OutputLegth; i < totalLen; i++ {
		output[i] = byte(utils.Abs(backStuffValue) % 10)
		backStuffValue -= int(bytearr[i])
	}

	return output
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

	bytearr := []byte(stubStr)

	for i := range bytearr {
		bytearr[i] = bytearr[i] - '0'
	}

	for i := 0; i < FullPhases; i++ {
		bytearr = decodeReapeatedOnePhase(bytearr)
	}

	for i := range bytearr {
		bytearr[i] = bytearr[i] + '0'
	}

	return string(bytearr[:OutputLegth])
}
