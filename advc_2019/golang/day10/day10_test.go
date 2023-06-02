package day10

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestVisible(t *testing.T) {
	str := `.#..#
			..... 
			#####
			....#
			...##`
	ast := createAsteroidGrid(str)

	type Test struct {
		x, y, ret int
	}

	cases := []Test{
		{3, 4, 8},
		{4, 4, 7},
		{0, 2, 6},
	}

	for _, c := range cases {
		got := ast.getVisiblesFrom(utils.NewVector2D(c.x, c.y))
		if len(got) != c.ret {
			t.Errorf("case %v, got %d expected %d, vectors %v", c, len(got), c.ret, got)
		}
	}
}

func TestTotalVisibleCount(t *testing.T) {
	str := `.#..#
			..... 
			#####
			....#
			...##`

	ast := createAsteroidGrid(str)
	ret, _ := ast.getAllVisibleCount()
	if ret != 8 {
		t.Errorf("TestTotalVisibleCount case 1 got %d expected 8", ret)
	}

	str = `......#.#.
			#..#.#....
			..#######.
			.#.#.###..
			.#..#.....
			..#....#.#
			#..#....#.
			.##.#..###
			##...#..#.
			.#....####`
	ast = createAsteroidGrid(str)
	ret, _ = ast.getAllVisibleCount()
	if ret != 33 {
		t.Errorf("TestTotalVisibleCount case 2 got %d expected 33", ret)
	}
}

func TestNthDestroyed(t *testing.T) {
	str := `.#..##.###...#######
	##.############..##.
	.#.######.########.#
	.###.#######.####.#.
	#####.##.#.##.###.##
	..#####..#.#########
	####################
	#.####....###.#.#.##
	##.#################
	#####.##.###..####..
	..######..##.#######
	####.##.####...##..#
	.#####..#.######.###
	##...#.##########...
	#.##########.#######
	.####.#.###.###.#.##
	....##.##.###..#####
	.#.#.###########.###
	#.#.#.#####.####.###
	###.##.####.##.#..##`

	ast := createAsteroidGrid(str)
	cnt, loc := ast.getAllVisibleCount()

	if cnt != 210 {
		t.Errorf("TestNthDestroyed, visible cnt at %v is %d, expected 210", loc, cnt)
	}
	if loc != utils.NewVector2D(11, 13) {
		t.Errorf("TestNthDestroyed, visible cnt at %v is %d, expected loc (11, 13)", loc, cnt)
	}

	v := ast.findNthDestroyed(loc, 200)

	if v != utils.NewVector2D(8, 2) {
		t.Errorf("TestNthDestroyed, 200th location %v, expected (8, 2)", v)
	}
}

/*
func TestRadian(t *testing.T) {
	vecs := []utils.Vector{
		utils.NewVector2D(1, 0),
		utils.NewVector2D(1, 1),
		utils.NewVector2D(0, 1),
		utils.NewVector2D(-1, 1),
		utils.NewVector2D(-1, 0),
		utils.NewVector2D(-1, -1),
		utils.NewVector2D(0, -1),
		utils.NewVector2D(1, -1),
	}

	for _, v := range vecs {
		fmt.Printf("%v => %f\n", v, math.Atan2(float64(v.X), float64(v.Y)))
	}
}
*/
