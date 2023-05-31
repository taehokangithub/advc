package tests

import (
	"strings"
	"taeho/advc19_go/utils"
	"testing"
)

func TestGraphBasics(t *testing.T) {
	g := utils.NewTree()

	node1 := "Taeho"
	node2 := "BongBong"
	node3 := "Bongu"
	node4 := "Bread"
	node5 := "Vegetable"

	g.AddNode(node1)
	g.AddEdge(node1, node2)
	g.AddEdge(node2, node3)
	g.AddEdge(node1, node4)
	g.AddEdge(node3, node5)
	g.SetAllDistances()

	node := g.GetNode(node3)

	if node == nil {
		t.Errorf("%s not found", node3)
	} else if node.Name != node3 {
		t.Errorf("node 3 name %s is wrong, expected %s", node.Name, node3)
	} else if node.DistanceFromRoot != 2 {
		t.Errorf("%s distance %d is wrong, expected 2", node3, node.DistanceFromRoot)
	}
}

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

func TestTraversAndDistance(t *testing.T) {
	str := `COM)B
	B)C
	C)D
	D)E
	E)F
	B)G
	G)H
	D)I
	E)J
	J)K
	K)L`

	tree := createTree(str)

	tree.SetRoot("COM")
	tree.SetAllDistances()

	sumDist := 0
	tree.ForEachNodes(func(n *utils.GraphNode) {
		sumDist += n.DistanceFromRoot
	})

	expected := 42
	if sumDist != expected {
		t.Errorf("sum of distance %d is wrong, expected %d", sumDist, expected)
	}
}

func TestDistanceBetweenNodes(t *testing.T) {
	str := `COM)B
	B)C
	C)D
	D)E
	E)F
	B)G
	G)H
	D)I
	E)J
	J)K
	K)L
	K)YOU
	I)SAN`

	tree := createTree(str)

	tree.SetRoot("YOU")
	tree.SetAllDistances()
	node := tree.GetNode("SAN")
	expected := 6

	if node.DistanceFromRoot != expected {
		t.Errorf("%v distance %d is wrong! expected %d", node, node.DistanceFromRoot, expected)
	}
}
