const fs = require("fs");

namespace advc20_24
{
	const DIRS : string[] = ['e', 'w', 'sw', 'nw', 'se', 'ne'];

	// ---------------------------------------------------------
	class Coord
	{
		m_x : number = 0;
		m_y : number = 0;

		constructor(x?: number, y?: number)
		{
			if (typeof x !== 'undefined' && typeof y !== 'undefined')
			{
				this.m_x = x;
				this.m_y = y;
			}
		}

		add(coord : Coord)
		{
			this.m_x += coord.m_x;
			this.m_y += coord.m_y;
		}

		toString() : string
		{
			return "[" + this.m_x + ',' + this.m_y + "]";
		}

		setMin(coord : Coord)
		{
			this.m_x = Math.min(this.m_x, coord.m_x);
			this.m_y = Math.min(this.m_y, coord.m_y);
		}
		
		setMax(coord : Coord)
		{
			this.m_x = Math.max(this.m_x, coord.m_x);
			this.m_y = Math.max(this.m_y, coord.m_y);
		}

		getAdjacents() : Coord[] 
		{
			const coords : Coord[] = [];

			DIRS.forEach(unit =>
				{
					const coord = new Coord();
					coord.add(this);
					coord.add(Locator.processUnit(unit, coord));
					coords.push(coord);
				});

			return coords;
		}
	}

	// ---------------------------------------------------------
	class Locator
	{
		m_coord : Coord;
		m_line : string;

		constructor(line : string)
		{
			this.m_coord = new Coord();
			this.m_line = line;
			this.processLocation(line);
		}

		static processUnit(unit : string, curLoc : Coord) :  Coord
		{
			let coord = new Coord();
			const curY : number = Math.abs(curLoc.m_y);

			switch(unit)
			{
				case 'e' :	coord.m_x ++; break;
				case 'w' :	coord.m_x --; break;
				case 'sw' :	 coord.m_y ++; if (curY % 2 == 0) coord.m_x --; break;
				case 'nw' :	 coord.m_y --; if (curY % 2 == 0) coord.m_x --; break;
				case 'se' :	 coord.m_y ++; if (curY % 2 == 1) coord.m_x ++; break;
				case 'ne' :	 coord.m_y --; if (curY % 2 == 1) coord.m_x ++; break;
				default:
					throw new Error("Unknown Unit" + unit);
			}
			return coord;
		}

		processLocation(line : string)
		{
			for (let i = 0; i < line.length; i ++)
			{
				let unit : string = line[i];

				if (unit == '\r')
				{
					continue;
				}

				if (['n','s'].includes(unit))
				{
					unit += line[++i];
				}

				const delta = Locator.processUnit(unit, this.m_coord);
				this.m_coord.add(delta);
			}
		}

		toString() : string 
		{
			return this.m_coord.toString();
		}
	}

	type VisitMap = 
	{
		[id : string] : boolean;
	}
	// ---------------------------------------------------------
	class Solver
	{
		m_locators : Locator[];
		m_visits : VisitMap;
		m_minCoord : Coord = new Coord(Number.MAX_SAFE_INTEGER, Number.MAX_SAFE_INTEGER);
		m_maxCoord : Coord = new Coord(Number.MIN_SAFE_INTEGER, Number.MIN_SAFE_INTEGER);

		constructor()
		{
			this.m_locators = [];
			this.m_visits = {};
		}

		addLocator(line:string)
		{
			const loc = new Locator(line);
			const id = loc.toString();

			this.m_visits[id] = !this.m_visits[id];
			this.m_locators.push(loc);

			this.updateMinMax(loc.m_coord);
		}

		updateMinMax(coord : Coord)
		{
			this.m_minCoord.setMin(coord);
			this.m_maxCoord.setMax(coord);
		}

		copyVisitMap() : VisitMap 
		{
			const newMap : VisitMap = {};
			for (let id in this.m_visits)
			{
				newMap[id] = this.m_visits[id];
			}
			return newMap;
		}

		flip()
		{
			const newMap : VisitMap = this.copyVisitMap();

			for (let y = this.m_minCoord.m_y - 1; y <= this.m_maxCoord.m_y + 1; y ++)
			{
				for (let x = this.m_minCoord.m_x - 1; x <= this.m_maxCoord.m_x + 1; x ++)
				{
					const coord = new Coord(x, y);
					const adjacents = coord.getAdjacents();
					const myId = coord.toString();

					let cntBlack = 0;
					adjacents.forEach(c =>
						{
							if (this.m_visits[c.toString()])
							{
								cntBlack ++;
							}
						});
					
					const isBlack : boolean = this.m_visits[myId];
					const shouldFlip : boolean = ((isBlack && (cntBlack == 0 || cntBlack > 2)) || !isBlack && cntBlack == 2);
					const value : boolean = newMap[myId] = shouldFlip ? !this.m_visits[myId] : this.m_visits[myId];

					if (value)
					{
						this.updateMinMax(coord);
					}
				}
			}

			this.m_visits = newMap;
		}

		countBlack() : number
		{
			let count : number = 0;

			for (let id in this.m_visits)
			{
				if (this.m_visits[id])
				{
					count ++;
				}
			}
			return count;
		}

		getAnswerPart1() : number
		{
			console.log("Solving part 1, total " + this.m_locators.length + " locations" + ", " + Object.keys(this.m_visits).length + " visits");

			return this.countBlack();
		}

		getAnswerPart2() : number
		{
			const numLoop = 100;

			for (let i = 0; i < numLoop; i ++)
			{
				this.flip();
				//console.log("[" + (i + 1) + "] : " + this.countBlack());
			}
			
			return this.countBlack();
		}
	}
	
	// ---------------------------------------------------------
	function main(path : string)
	{
		const text : string = fs.readFileSync(path,"utf8");
		const lines : string[] = text.split('\n');
	
		const solver = new Solver();

		for (let line of lines)
		{
			if (line.length)
			{
				solver.addLocator(line);
			}
		}

		const ans1 = solver.getAnswerPart1();
		const ans2 = solver.getAnswerPart2();

		console.log("ans1 = " + ans1);
		console.log("ans2 = " + ans2);
	}	

	main("../../data/advc20_24.txt");
}

