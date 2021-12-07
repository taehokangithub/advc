declare var require: any
export {};

const fs = require("fs");

namespace advc21_27
{
	type LocationMap = 
	{
		[key : number] : number	// location, count
	};

	class LocGroup 
	{
		m_locations : LocationMap = {};
		m_totalCnt : number = 0;
		m_min : number = Number.MAX_SAFE_INTEGER;
		m_max : number = Number.MIN_SAFE_INTEGER;

		addLocation(location : number, cnt : number = 1)
		{
			if (this.m_locations[location] == undefined)
			{
				this.m_locations[location] = 0;
			}
			this.m_locations[location] += cnt;
			this.m_totalCnt += cnt;
			this.m_min = Math.min(this.m_min, location);
			this.m_max = Math.max(this.m_max, location);
		}

		removeLocation(location : number) : number
		{
			const ret : number = this.m_locations[location];
			if (ret == undefined || ret <= 0)
			{
				return 0;
			}
			
			delete this.m_locations[location];
			this.m_totalCnt -= ret;
			return ret;
		}

		divideAndRemoveBigger(location : number) : LocGroup
		{
			const otherLocGroup = new LocGroup();
			this.m_max = Number.MIN_SAFE_INTEGER;

			Object.keys(this.m_locations).forEach(key =>
				{
					const loc : number = parseInt(key);
					if (loc > location)
					{
						const cnt = this.removeLocation(loc);
						otherLocGroup.addLocation(loc, cnt);
					}
					else
					{
						this.m_max = Math.max(this.m_max, loc);
					}
				});

			return otherLocGroup;
		}

		printSummary()
		{
			console.log(`[Location Group] Data min ${this.m_min}, max ${this.m_max}, cnt ${this.m_totalCnt}`)
		}

		calculateWeight(point : number) : number
		{
			let weight : number = 0;

			Object.keys(this.m_locations).forEach(key =>
				{
					const loc : number = parseInt(key);
					const cnt : number = this.m_locations[loc];
					weight += Math.abs(loc - point) * cnt;
				});

			return weight;
		}
	};

	class Solver 
	{
		m_locGroupLeft : LocGroup = new LocGroup();
		m_locGroupRight : LocGroup = new LocGroup();

		constructor(data : number[])
		{
			data.forEach(i => this.m_locGroupLeft.addLocation(i));
			this.m_locGroupLeft.printSummary();
		}

		getMoveDir() : number
		{
			return this.m_locGroupLeft.m_totalCnt > this.m_locGroupRight.m_totalCnt ? -1 : 1;
		}

		solve()
		{
			let midVal = Math.floor((this.m_locGroupLeft.m_max + this.m_locGroupLeft.m_min) / 2);
			this.m_locGroupRight = this.m_locGroupLeft.divideAndRemoveBigger(midVal);
			//console.log(`[Solve] Divided at ${midVal}`);

			const moveDir = this.getMoveDir();
			let toGroup : LocGroup = (moveDir < 0) ? this.m_locGroupLeft : this.m_locGroupRight;
			let fromGroup : LocGroup = (moveDir < 0) ? this.m_locGroupRight : this.m_locGroupLeft;

			while (this.getMoveDir() == moveDir)
			{
				midVal += moveDir;

				const removed : number = toGroup.removeLocation(midVal);

				if (removed > 0)
				{
					fromGroup.addLocation(midVal, removed);
				}
			}

			//midVal -= moveDir; // backout the last move
			const ans : number = this.m_locGroupLeft.calculateWeight(midVal) + this.m_locGroupRight.calculateWeight(midVal);

			console.log("Answer : " + ans + ", loc at " + midVal);	
		}

	}
	
	// ---------------------------------------------------------
	function main(path : string)
	{
		const text : string = fs.readFileSync(path,"utf8");
		const lines : number[] = text.split(',').map(s=>parseInt(s));

		const solver : Solver = new Solver(lines);
		solver.solve();
	}	

	main("../../data/advc21_07_sample.txt");
	main("../../data/advc21_07.txt");
}

