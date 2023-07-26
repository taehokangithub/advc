package main

import (
	"fmt"
	"taeho/advc19_go/day01"
	"taeho/advc19_go/day02"
	"taeho/advc19_go/day03"
	"taeho/advc19_go/day04"
	"taeho/advc19_go/day05"
	"taeho/advc19_go/day06"
	"taeho/advc19_go/day07"
	"taeho/advc19_go/day08"
	"taeho/advc19_go/day09"
	"taeho/advc19_go/day10"
	"taeho/advc19_go/day11"
	"taeho/advc19_go/day12"
	"taeho/advc19_go/day13"
	"taeho/advc19_go/day14"
	"taeho/advc19_go/day15"
	"taeho/advc19_go/day16"
	"taeho/advc19_go/day17"
	"taeho/advc19_go/day18"
	"taeho/advc19_go/day19"
	"taeho/advc19_go/day20"
	"taeho/advc19_go/day21"
	"taeho/advc19_go/day22"
	"taeho/advc19_go/day23"
	"taeho/advc19_go/day24"
	"taeho/advc19_go/etc"
	"time"
)

var colorReset string = "\033[0m"
var colorTotalElapsed string = "\033[93m"
var colorElapsed string = "\033[90m"

func measureTime(f func()) {
	t := time.Now()
	f()
	elapsed := time.Since(t)

	fmt.Println(colorElapsed, "Elapsed ", elapsed.Milliseconds(), "ms", colorReset)
}

func runAll() {
	measureTime(day01.Solve)
	measureTime(day02.Solve)
	measureTime(day03.Solve)
	measureTime(day04.Solve)
	measureTime(day05.Solve)
	measureTime(day06.Solve)
	measureTime(day07.Solve)
	measureTime(day08.Solve)
	measureTime(day09.Solve)
	measureTime(day10.Solve)
	measureTime(day11.Solve)
	measureTime(day12.Solve)
	measureTime(day13.Solve)
	measureTime(day14.Solve)
	measureTime(day15.Solve)
	measureTime(day16.Solve)
	measureTime(day17.Solve)
	measureTime(day18.Solve)
	measureTime(day19.Solve)
	measureTime(day20.Solve)
	measureTime(day21.Solve)
	measureTime(day22.Solve)
	measureTime(day23.Solve)
	measureTime(day24.Solve)
	etc.FinalChecker()
}

func main() {
	t := time.Now()
	runAll()
	elapsed := time.Since(t)
	fmt.Println(colorTotalElapsed, "Total Elapsed :", elapsed.Milliseconds(), "ms", colorReset)
}
