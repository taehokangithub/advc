package tests

import (
	"testing"
)

func expectPanic(t *testing.T, testName string, cb func()) {
	defer func() {
		if r := recover(); r == nil {
			t.Errorf("Failed to receive panic while %s", testName)
		}
	}()

	cb()
}
