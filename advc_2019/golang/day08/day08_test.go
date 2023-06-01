package day08

import "testing"

func TestLayers(t *testing.T) {
	str := "123456789012"

	layers := splitLayers(str, 3, 2)

	if len(layers) != 2 {
		t.Errorf("Layers len is not 2, got %d", len(layers))
	}
}
