package day13

import (
	"fmt"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/utils"
)

type TileType byte

const (
	TILE_EMPTY TileType = 0
	TILE_WALL  TileType = 1
	TILE_BLOCK TileType = 2
	TILE_PAD   TileType = 3
	TILE_BALL  TileType = 4
)

type Arcade struct {
	game  *utils.FreeMap[byte]
	com   *computer.Computer
	ball  utils.Vector
	pad   utils.Vector
	score int
}

func NewArcade(memory string) *Arcade {
	a := Arcade{}
	a.game = utils.NewFreeMap[byte]()
	a.com = computer.NewComputer(memory)
	return &a
}

func (arc *Arcade) ProcessScreen() {
	arc.com.RunProgram()

	for arc.com.HasOutput() {
		o1 := arc.com.PopOutput()
		o2 := arc.com.PopOutput()
		o3 := arc.com.PopOutput()

		loc := utils.NewVector2D(int(o1), int(o2))
		if loc.X < 0 {
			arc.score = int(o3)
		} else {
			tile := byte(o3)
			if tile == byte(TILE_BALL) {
				arc.ball = loc
			} else if tile == byte(TILE_PAD) {
				arc.pad = loc
			}
			arc.game.Set(loc, tile)
		}
	}
}

func (arc *Arcade) PlayGame() {
	arc.com.Set(0, 2)
	for arc.com.Status != computer.COMSTATUS_FINISHED {
		arc.ProcessScreen()
		input := int64(0)
		if arc.ball.X > arc.pad.X {
			input = 1
		} else if arc.ball.X < arc.pad.X {
			input = -1
		}
		arc.com.AddInput(input)
	}
}

func (arc *Arcade) DrawScreen() {
	for y := arc.game.Min.Y; y <= arc.game.Max.Y; y++ {
		for x := arc.game.Min.X; x <= arc.game.Max.X; x++ {
			tileByte, _ := arc.game.Get(utils.NewVector2D(x, y))
			tile := TileType(tileByte)
			c := " "
			switch tile {
			case TILE_BALL:
				c = "O"
			case TILE_BLOCK:
				c = "X"
			case TILE_PAD:
				c = "="
			case TILE_WALL:
				c = "*"
			}
			fmt.Print(c)
		}
		fmt.Println("")
	}
}

func (arc *Arcade) CountTile(tile TileType) int {
	cnt := 0
	arc.game.Foreach(func(v utils.Vector, b byte) {
		if TileType(b) == tile {
			cnt++
		}
	})
	return cnt
}
