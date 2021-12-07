declare var require: any
export {};

const fs = require("fs");

namespace advc21_27
{
	class PrecalcuationDistToCost
	{
		m_cost : number[];

		constructor(maxDist : number)
		{
			this.m_cost = [];
			this.m_cost.push(0);

			for (let i = 1; i <= maxDist; i ++)
			{
				this.m_cost.push(this.m_cost[i - 1] + i);
			}
		}

		getCost(dist : number) : number
		{
			return this.m_cost[dist];
		}
	}

	const s_distToCost = new PrecalcuationDistToCost(2000);

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

		calculateCostPart2(point : number) : number
		{
			let cost : number = 0;

			for (let loc in this.m_locations)
			{
				const localCost = s_distToCost.getCost(Math.abs(parseInt(loc) - point));
				cost += localCost * this.m_locations[loc];
			}

			return cost;
		}
	};

	class Solver
	{
		m_locGroupLeft : LocGroup;
		m_locGroupRight : LocGroup;
		m_intialMidVal : number;

		constructor(data : number[])
		{
			this.m_locGroupLeft = new LocGroup();
			data.forEach(i => this.m_locGroupLeft.addLocation(i));

			this.m_intialMidVal = Math.floor((this.m_locGroupLeft.m_max + this.m_locGroupLeft.m_min) / 2);
			this.m_locGroupRight = this.m_locGroupLeft.divideAndRemoveBigger(this.m_intialMidVal);			
		}

		solvePart2()
		{
			const totalCosts : {[key : number] : number} = {};

			const getTotalCost = (point : number) =>
			{
				if (totalCosts[point] ==  undefined)
				{
					totalCosts[point] = this.m_locGroupLeft.calculateCostPart2(point) + this.m_locGroupRight.calculateCostPart2(point);
				}
				return totalCosts[point];
			};

			const getMoveDir = (p : number) => 
			{
				if (getTotalCost(p) > getTotalCost(p - 1))
				{
					return -1;
				}
				if (getTotalCost(p) > getTotalCost(p + 1))
				{
					return 1;
				}
				return 0;
			};

			let point = this.m_intialMidVal;
			let moveDir = getMoveDir(point);

			while (moveDir != 0)
			{
				point += moveDir;
				moveDir = getMoveDir(point);
			}

			const ans : number = getTotalCost(point);
			
			console.log("[Part 2] Answer : " + ans + ", loc at " + point);
		}

		solvePart1()
		{
			const getMoveDir = () => (this.m_locGroupLeft.m_totalCnt > this.m_locGroupRight.m_totalCnt ? -1 : 1);

			const moveDir = getMoveDir();

			let toGroup : LocGroup = (moveDir < 0) ? this.m_locGroupLeft : this.m_locGroupRight;
			let fromGroup : LocGroup = (moveDir < 0) ? this.m_locGroupRight : this.m_locGroupLeft;
			let point = this.m_intialMidVal;

			while (getMoveDir() == moveDir)
			{
				point += moveDir;

				const removed : number = toGroup.removeLocation(point);

				if (removed > 0)
				{
					fromGroup.addLocation(point, removed);
				}
			}

			const ans : number = this.m_locGroupLeft.calculateWeight(point) + this.m_locGroupRight.calculateWeight(point);

			console.log("[Part 1] Answer : " + ans + ", loc at " + point);
		}

	}
	
	// ---------------------------------------------------------
	function main(path : string)
	{
		const text : string = fs.readFileSync(path,"utf8");
		const lines : number[] = text.split(',').map(s=>parseInt(s));

		const solver = new Solver(lines);
		solver.solvePart1();
		solver.solvePart2();
		
	}	

	main("../../../data/advc21_07_sample.txt");
	main("../../../data/advc21_07.txt");
}

