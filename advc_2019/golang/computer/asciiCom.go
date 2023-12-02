package computer

import "strings"

type ASciiCom struct {
	*Computer
}

func NewAsciiCom(str string) *ASciiCom {
	com := NewComputer(str)
	asc := ASciiCom{
		Computer: com,
	}
	return &asc
}

func (com *ASciiCom) AddStrInput(str string) {
	for _, c := range str {
		input := int64(c)
		com.AddInput(input)
	}
}

func (com *ASciiCom) GetStrOutput() string {
	builder := strings.Builder{}
	for com.HasOutput() {
		i := com.PopOutput()
		builder.WriteRune(rune(i))
	}
	return builder.String()
}
