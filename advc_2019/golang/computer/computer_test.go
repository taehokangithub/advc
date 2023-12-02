package computer

import (
	"fmt"
	"strconv"
	"strings"
	"testing"
)

func Test_init_memory(t *testing.T) {
	com := NewComputer("")

	com.InitMemory("1,3,5,7,8")
	com.Set(1, 22)
	ret := com.DumpMemory()
	expected := "1,22,5,7,8"
	if ret != expected {
		t.Error("got", ret, "expected", expected)
	}
}

func Test_run_add_mul(t *testing.T) {
	com := NewComputer("1,9,10,3,2,3,11,0,99,30,40,50")
	com.RunProgram()
	expected := "3500,9,10,70,2,3,11,0,99,30,40,50"
	dumped := com.DumpMemory()
	if expected != dumped {
		t.Error("Got", dumped, "\nexpected", expected)
	}
}

func Test_single_input(t *testing.T) {
	type TestCases struct {
		program string
		input   int64
		output  int64
	}

	oneLongProgram := "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"
	tests := []TestCases{
		{"3,9,8,9,10,9,4,9,99,-1,8", 8, 1},
		{"3,9,8,9,10,9,4,9,99,-1,8", 9, 0},
		{"3,9,7,9,10,9,4,9,99,-1,8", 8, 0},
		{"3,9,7,9,10,9,4,9,99,-1,8", 7, 1},
		{"3,3,1108,-1,8,3,4,3,99", 7, 0},
		{"3,3,1108,-1,8,3,4,3,99", 8, 1},
		{"3,3,1107,-1,8,3,4,3,99", 8, 0},
		{"3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 5, 1},
		{"3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, 0},
		{"3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 5, 1},
		{"3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 0, 0},
		{oneLongProgram, 7, 999},
		{oneLongProgram, 8, 1000},
		{oneLongProgram, 9, 1001},
	}

	for i, c := range tests {
		com := NewComputer(c.program)
		com.AddInput(c.input)
		com.RunProgram()
		got := com.GetOutput()[0]

		if got != c.output {
			t.Errorf("Case [%d], got %d, expected %d", i, got, c.output)
		}
	}
}

func TestRelativeMode(t *testing.T) {
	mem := "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99"
	memarr := strings.Split(mem, ",")
	com := NewComputer(mem)
	com.RunProgram()
	out := com.GetOutput()
	for i := range out {
		memval, _ := strconv.Atoi(memarr[i])
		if int(out[i]) != memval {
			t.Errorf("at %d, %d != %d", i, out[i], memval)
		}
	}
}

func TestLargeValue(t *testing.T) {
	mem := "104,1125899906842624,99"
	expected := 1125899906842624
	com := NewComputer(mem)
	com.RunProgram()
	out := com.PopOutput()

	if out != int64(expected) {
		t.Errorf("got %d expected %d", out, expected)
	}
}

func TestLargeValue2(t *testing.T) {
	mem := "1102,34915192,34915192,7,4,7,99,0"
	com := NewComputer(mem)
	com.RunProgram()
	got := com.PopOutput()
	gotstr := fmt.Sprintf("%v", got)
	if len(gotstr) != 16 {
		t.Errorf("got %d, length %d, not 16", got, len(gotstr))
	}
}

func TestCopy(t *testing.T) {
	mem := "1,3,5,6,8,8,22,33,66,88,99"
	com := NewComputer(mem)
	com.input = []int64{3, 7, 6, 4, 12, 33}
	com.output = []int64{2, 3, 4, 5, 6, 7}
	com.Status = COMSTATUS_PAUSED
	com.relBase = 11

	com2 := com.Copy()
	compareComputers(t, com, com2)
}

func compareComputers(t *testing.T, a *Computer, b *Computer) {
	if a.Status != b.Status {
		t.Errorf("Status different - %d vs %d", a.Status, b.Status)
	}
	if a.relBase != b.relBase {
		t.Errorf("relBase different - %d vs %d", a.relBase, b.relBase)
	}
	for k, v := range a.memory {
		if b.memory[k] != v {
			t.Errorf("memory different - at %d, %d vs %d", k, a.memory[k], b.memory[k])
			break
		}
	}
	for i, v := range a.input {
		if b.input[i] != v {
			t.Errorf("input different - at %d, %d vs %d", i, a.input[i], b.input[i])
			break
		}
	}
	for i, v := range a.output {
		if b.output[i] != v {
			t.Errorf("input different - at %d, %d vs %d", i, a.output[i], b.output[i])
			break
		}
	}
}
