package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestStrStream(t *testing.T) {
	const str = "abcde12345"
	stream := utils.NewStrStream(str)

	result := make([]byte, 0)

	for {
		if c, ok := stream.GetNext(); ok {
			result = append(result, c)
		} else {
			break
		}
	}

	for i, r := range result {
		if r != str[i] {
			t.Errorf("at %d, got %c expected %c", i, r, str[i])
		}
	}
}
