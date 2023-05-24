package computer

import "testing"

func Test_init_memory(t *testing.T) {
	var com Computer

	com.InitMemory("1,3,5,7,8")
	com.Set(1, 22)
	ret := com.DumpMemory()
	expected := "1,22,5,7,8"
	if ret != expected {
		t.Error("got", ret, "expected", expected)
	}
}

func Test_run_add_mul(t *testing.T) {
	com := NewComputer()
	com.InitMemory("1,9,10,3,2,3,11,0,99,30,40,50")
	com.RunProgram()
	expected := "3500,9,10,70,2,3,11,0,99,30,40,50"
	dumped := com.DumpMemory()
	if expected != dumped {
		t.Error("Got", dumped, "\nexpected", expected)
	}
}
