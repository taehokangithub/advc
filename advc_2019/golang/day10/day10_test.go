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

func testNthInternal(t *testing.T, str string, visibleLoc utils.Vector, visibleCnt int, target utils.Vector, nth int) {

	ast := createAsteroidGrid(str)
	cnt, loc := ast.getAllVisibleCount()

	if cnt != visibleCnt {
		t.Errorf("TestNthDestroyed, visible cnt at %v is %d, expected %d", loc, cnt, visibleCnt)
	}
	if loc != visibleLoc {
		t.Errorf("TestNthDestroyed, visible cnt at %v is %d, expected loc %v", loc, cnt, visibleLoc)
	}

	v := ast.findNthDestroyed(loc, nth)

	if v != target {
		t.Errorf("TestNthDestroyed, %dth location %v, expected %v", nth, v, target)
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

	testNthInternal(t, str, utils.NewVector2D(11, 13), 210, utils.NewVector2D(8, 2), 200)

	str2 := `.#....#####...#..
	##...##.#####..##
	##...#...#.#####.
	..#.....#...###..
	..#.#.....#....##`

	testNthInternal(t, str2, utils.NewVector2D(8, 3), 30, utils.NewVector2D(7, 0), 30)

	str3 := `..#..###....#####....###........#
	.##.##...#.#.......#......##....#
	#..#..##.#..###...##....#......##
	..####...#..##...####.#.......#.#
	...#.#.....##...#.####.#.###.#..#
	#..#..##.#.#.####.#.###.#.##.....
	#.##...##.....##.#......#.....##.
	.#..##.##.#..#....#...#...#...##.
	.#..#.....###.#..##.###.##.......
	.##...#..#####.#.#......####.....
	..##.#.#.#.###..#...#.#..##.#....
	.....#....#....##.####....#......
	.#..##.#.........#..#......###..#
	#.##....#.#..#.#....#.###...#....
	.##...##..#.#.#...###..#.#.#..###
	.#..##..##...##...#.#.#...#..#.#.
	.#..#..##.##...###.##.#......#...
	...#.....###.....#....#..#....#..
	.#...###..#......#.##.#...#.####.
	....#.##...##.#...#........#.#...
	..#.##....#..#.......##.##.....#.
	.#.#....###.#.#.#.#.#............
	#....####.##....#..###.##.#.#..#.
	......##....#.#.#...#...#..#.....
	...#.#..####.##.#.........###..##
	.......#....#.##.......#.#.###...
	...#..#.#.........#...###......#.
	.#.##.#.#.#.#........#.#.##..#...
	.......#.##.#...........#..#.#...
	.####....##..#..##.#.##.##..##...
	.#.#..###.#..#...#....#.###.#..#.
	............#...#...#.......#.#..
	.........###.#.....#..##..#.##...`

	testNthInternal(t, str3, utils.NewVector2D(27, 19), 314, utils.NewVector2D(15, 13), 200)

}
