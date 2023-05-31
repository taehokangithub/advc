package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestDijkstra(t *testing.T) {
	g := utils.NewGraph()

	g.AddEdgeWithDistance("Root", "A", 1)
	g.AddEdgeWithDistance("A", "B", 3)
	g.AddEdgeWithDistance("A", "C", 2)
	g.AddEdgeWithDistance("B", "D", 6)
	g.AddEdgeWithDistance("C", "D", 3)

	g.AddEdgeWithDistance("D", "E", 2)
	g.AddEdgeWithDistance("E", "H", 1)
	g.AddEdgeWithDistance("D", "F", 4)
	g.AddEdgeWithDistance("F", "G", 3)
	g.AddEdgeWithDistance("G", "H", 2)

	g.SetAllDistances()

	expectedD := 6
	expectedG := 11

	nodeD := g.GetNode("D")
	nodeG := g.GetNode("G")

	if nodeD.DistanceFromRoot != expectedD {
		t.Errorf("Node D distance %d expected %d", nodeD.DistanceFromRoot, expectedD)
	}

	if nodeG.DistanceFromRoot != expectedG {
		t.Errorf("Node G distance %d expectd %d", nodeG.DistanceFromRoot, expectedG)
	}
}
