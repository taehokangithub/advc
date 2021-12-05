const fs = require("fs");

namespace advc20_24
{
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
	}

	// ---------------------------------------------------------
	class Locator
	{
		m_coord : Coord;

		constructor(line : string)
		{
			this.m_coord = new Coord();
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
					console.log("Error! unknown unit " + unit);
			}
			return coord;
		}

		processLocation(line : string)
		{
			//console.log("[" + line + "] processing...");

			for (let i = 0; i < line.length; i ++)
			{
				let unit : string = line[i];
				if (['n','s'].includes(unit))
				{
					unit += line[++i];
				}

				const delta = Locator.processUnit(unit, this.m_coord);
				this.m_coord.add(delta);

				//console.log("<" + unit + "> " + delta + " = " + this.m_coord);
			}

			//console.log("[" + line + "] " + this.m_coord);
		}

		toString() : string 
		{
			return this.m_coord.toString();
		}
	}

	// ---------------------------------------------------------
	class Solver
	{
		m_locators : Locator[];
		m_visits : { [id : string] : number; };

		constructor()
		{
			this.m_locators = [];
			this.m_visits = {};
		}

		addLocator(line:string)
		{
			const loc = new Locator(line);
			const id = loc.toString();

			if (this.m_visits.hasOwnProperty(id))
			{
				this.m_visits[id] ++;
			}
			else
			{
				this.m_visits[id] = 1;
			}

			this.m_locators.push(loc);
		}

		getAnswerPart1() : number
		{
			console.log("Solving part 1, total " + this.m_locators.length + " locations" + ", " + Object.keys(this.m_visits).length + " visits");

			let count : number = 0;

			const idList = Object.keys(this.m_visits).sort();

			for (let id of idList)
			{
				if (this.m_visits[id] % 2 == 1)
				{
					count ++;
					console.log(id.toString() + " : " + this.m_visits[id] + " times, ====> Total " + count);
				}
				else
				{
					console.log(id.toString() + " : " + this.m_visits[id] + " times");
				}
			}
			return count;
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
			solver.addLocator(line);	
		}

		const ans1 = solver.getAnswerPart1();

		console.log("ans1 = " + ans1);
	}	

	main("../data/advc20_24.txt");	// 480 too high
	//main("../data/advc20_24.sample.txt");
}

