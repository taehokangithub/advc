package main

import (
	"bufio"
	"fmt"
	"os"
)

const file_name = "../data/input01.txt"

func read_file_sample_1() {
	fmt.Println("Sample reading version 1", file_name)

	file, _ := os.Open(file_name)
	defer file.Close()

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		fmt.Println(scanner.Text())
	}
}

func read_file_sample_2() {
	fmt.Println("Sample reading version 2", file_name)
	content, _ := os.ReadFile(file_name)
	fmt.Println(string(content))
}
func File_reading_sample() {
	read_file_sample_1()
	read_file_sample_2()

}
