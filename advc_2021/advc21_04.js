const fs = require('fs');
const readline = require('readline');

const BoardLength = 5;

class Board 
{
	constructor()
	{
		this.data = [];
		this.marks = [];
		this.getAt = (x, y) => this.data[Board.getIndex(x, y)];
	}

	static getIndex(x, y) 
	{
		return (x + y * BoardLength);
	}

	static getCoord(index)
	{
		return ((index % BoardLength), (index / BoardLength));
	}

	static getBingoPatterns()
	{
		if (Board.bingoPatterns.length == 0)
		{
			for (let y = 0; y < BoardLength; y ++)
			{
				const pattern1 = [];
				const pattern2 = []; 
				for (let x = 0; x < BoardLength; x ++)
				{
					pattern1.push(Board.getIndex(x, y));
					pattern2.push(Board.getIndex(y, x));
				}
				Board.bingoPatterns.push(pattern1);
				Board.bingoPatterns.push(pattern2);
			}			
		}
		return Board.bingoPatterns;
	}

	setMark(val)
	{
		for (let i = 0; i < this.data.length; i ++)
		{
			if (this.data[i] == val)
			{
				this.marks[i] = true;
				return;
			}
		}
	}

	isBingo()
	{
		const patterns = Board.getBingoPatterns();

		for (let i = 0; i < patterns.length; i ++)
		{
			const pattern = patterns[i];
			
			let bingo = true;
			for (let p = 0; p < pattern.length; p ++)
			{
				const index = pattern[p];
				if (!this.marks[index])
				{
					bingo = false;
					break;
				}
			}

			if (bingo)
			{
				console.log("Found bingo! at pattern " + pattern);
				return true;
			}
		}

		return false;
	}

	getSumUnmakred()
	{
		let sum = 0;

		this.marks.forEach((v, i) =>
			{
				if (!v)
				{
					sum += parseInt(this.data[i]);
				}
			});

		return sum;		
	}

	addLine(line) 
	{
		line.forEach(v => 
			{
				if (v.length)
				{
					this.data.push(v);
					this.marks.push(false);
				}
			});
	}

	printData()
	{
		for (let y = 0; y < BoardLength; y ++)
		{
			let line = '';
			for (let x = 0; x < BoardLength; x ++)
			{
				line += this.getAt(x, y) + ' ';
			}
			console.log(line);
		}
	}

	playOneValue(val)
	{
		this.setMark(val);
		if (this.isBingo())
		{
			return val * this.getSumUnmakred();
		}
		return 0;
	}
}

Board.bingoPatterns = [];

class Solver 
{
	constructor()
	{
		this.boards = [];
		this.cardDeck = [];
		this.addBoard = (b) => this.boards.push(b);
		this.setCardDeck = (c) => this.cardDeck = c;
		this.hasCardDeck = () => (this.cardDeck.length != 0);
		this.bingoPattern = [];
	}

	playBingo()
	{
		for (let i = 0; i < this.cardDeck.length; i ++)
		{
			const val = this.cardDeck[i];

			console.log("Playing deck [%d] : %d", i, val);

			for (let k = 0; k <  this.boards.length; k ++)
			{
				const board = this.boards[k];
				const score = board.playOneValue(val);

				if (score > 0)
				{
					board.printData();
					console.log("Bingo at board %d, Found answer : %d", k, score);
					return;
				}
			}
		}
	}

	playBingoLastWin()
	{
		const completed = new Set();
		this.cardDeck.forEach(card =>
			{
				this.boards.forEach((board, id) =>
				{
					if (!completed.has(id))
					{
						const score = board.playOneValue(card);

						if (score > 0)
						{
							completed.add(id);

							if (completed.size == this.boards.length)
							{
								board.printData();
								console.log("Final bingo at board %d, Found answer : %d", id, score);
							}
							else 
							{
								console.log("Bingo at board %d, current completed boards %d", id, completed.size);
							}
						}
					}
				});
			});
	}

	printAllBoards()
	{
		console.log("Solver::printAllBoards. Number of boards : " + this.boards.length);

		this.boards.forEach((b, i) =>
			{
				console.log("-------------Board %s ----------------------",  i);
				b.printData();
			});
	}
}

function solveMain(inputFilePath)
{
	const fileStream = fs.createReadStream(inputFilePath);
	const lineStream = readline.createInterface({ input : fileStream });
	
	const solver = new Solver();
	let cardDeck = null;
	let board = null;

	lineStream.on('line', (line) =>
	{
		if (!solver.hasCardDeck())
		{
			solver.setCardDeck(line.split(','));
		}
		else if (line.length == 0)
		{
			board = null;
		}
		else
		{
			if (!board)
			{
				board = new Board();
				solver.addBoard(board);
			}
			board.addLine(line.split(/\s+/));
		}
	});

	lineStream.on('close', () =>
	{
		solver.playBingo();
		solver.playBingoLastWin();
	});
}

solveMain("../../data/advc20_04.txt");