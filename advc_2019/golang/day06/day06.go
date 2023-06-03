package day06

import (
	"os"
	"strings"
	"taeho/advc19_go/etc"
	"taeho/advc19_go/utils"
)

func createTree(str string) *utils.Tree {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")

	tree := utils.NewTree()

	for _, line := range lines {
		nodeNames := strings.Split(line, ")")
		node1 := strings.TrimSpace(nodeNames[0])
		node2 := strings.TrimSpace(nodeNames[1])
		tree.AddEdge(node1, node2)
	}

	return tree
}

func solve01(str string) int {
	g := createTree(str)
	g.SetRoot("COM")
	g.SetAllDistances()

	ans1 := 0
	g.ForEachNodes(func(n *utils.GraphNode) {
		ans1 += n.DistanceFromRoot
	})
	return ans1
}

func solve02(str string) int {
	g := createTree(str)
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
	etc.AnswerChecker("DAY06", ans1, 273985)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY06", ans2, 460)
}
