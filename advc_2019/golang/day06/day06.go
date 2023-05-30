package day06

import (
	"fmt"
	"os"
	"strings"
	"taeho/advc19_go/utils"
)

func createGraphFromString(str string) *utils.Graph {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")

	graph := utils.NewGraph()

	for _, line := range lines {
		nodeNames := strings.Split(line, ")")
		node1 := strings.TrimSpace(nodeNames[0])
		node2 := strings.TrimSpace(nodeNames[1])
		graph.AddEdge(node1, node2)
	}

	return graph
}

func solve01(str string) int {
	g := createGraphFromString(str)
	g.SetRoot("COM")
	g.SetAllDistances()

	ans1 := 0
	g.ForEachNodes(func(n *utils.GraphNode) {
		ans1 += n.DistanceFromRoot
	})
	return ans1
}

func solve02(str string) int {
	g := createGraphFromString(str)
	g.SetRoot("YOU")
	g.SetAllDistances()
	node := g.GetNode("SAN")
	return node.DistanceFromRoot - 2
}

func Solve() {
	content, err := os.ReadFile("../data/input06.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	fmt.Println("DAY06 ans1", ans1, "expected", 273985)

	ans2 := solve02(str)
	fmt.Println("DAY06 ans2", ans2, "expected", 460)
}
