declare var require: any
export {};

const fs = require("fs");

namespace advc20_08
{
	const CONST = 
	{
		inst :
		{
			ACC : "acc",
			JMP : "jmp",
			NOP : "nop"
		}
	};

	class CPU
	{
		m_instructionPointer : number = 0;
		m_accumulator : number = 0;

		getInstructionPointer() : number 
		{
			return this.m_instructionPointer;
		}

		jump(delta : number)
		{
			this.m_instructionPointer += delta;
		}

		increaseInstructionPointer()
		{
			this.m_instructionPointer ++;
		}

		accumulate(value : number)
		{
			this.m_accumulator += value;
		}

		getAccumulator() : number
		{
			return this.m_accumulator;
		}

		runProgram(code : Instruction[])
		{
			const visited = new Set();

			while (true)
			{
				const ip = this.getInstructionPointer();

				if (visited.has(ip))
				{
					break;
				}
				else if (ip == code.length)
				{
					break;
				}
				else if (ip > code.length)
				{
					console.log(`Error! program ip ${ip}, program length ${code.length}`);
				}

				visited.add(ip);

				//console.log(`[${ip}/${code[ip].m_command}]`);

				code[ip].run(this);
			}

		}
	};

	class Instruction
	{
		m_command : string;
		m_value : number;

		constructor(command : string, sign : boolean, value : number)
		{
			this.m_command = command;
			this.m_value = value * (sign ? 1 : -1);
		}

		run(cpu : CPU)
		{
			switch(this.m_command)
			{
				case CONST.inst.ACC: 
					cpu.accumulate(this.m_value); 
					// Fall through
				case CONST.inst.NOP: 
					cpu.increaseInstructionPointer(); 
					break;

				case CONST.inst.JMP: 
					cpu.jump(this.m_value); 
					break;

				default : console.log("Error! Unknown command " + this.m_command);
			}
		}
	};

	class Program
	{
		m_code : Instruction[] = [];

		addInstruction(line : string)
		{
			const parsed = line.split(' ');
			const command = parsed[0];
			const sign = parsed[1][0] == '+' ? true : false;
			const value = parseInt(parsed[1].substr(1));

			this.m_code.push(new Instruction(command, sign, value));
		}

		SolvePart1()
		{
			const cpu = new CPU();

			cpu.runProgram(this.m_code);

			console.log("[Solve1] Accumulator : " + cpu.getAccumulator());
		}

		SolvePart2()
		{
			const swapCommand = (inst : Instruction) : boolean =>
			{
				if (inst.m_command == CONST.inst.ACC)
				{
					return false;
				}
				else if (inst.m_command == CONST.inst.JMP)
				{
					inst.m_command = CONST.inst.NOP;
				}
				else if (inst.m_command == CONST.inst.NOP)
				{
					inst.m_command = CONST.inst.JMP;
				}
				else
				{
					throw new Error("Unknown instruction " + inst.m_command);
				}
				return true;
			}

			for (let i in this.m_code)
			{
				const inst = this.m_code[i];

				if (swapCommand(inst))
				{
					const cpu = new CPU();

					cpu.runProgram(this.m_code);
	
					if (cpu.getInstructionPointer() >= this.m_code.length)
					{
						console.log("[Solve2] Accumulator : " + cpu.getAccumulator());
						break;
					}

					swapCommand(inst);
				}
			}
		}
	};
	// ---------------------------------------------------------
	function main(path : string)
	{
		const text : string = fs.readFileSync(path,"utf8");
		const lines : string[] = text.split('\n');
		const program = new Program();

		lines.forEach(l =>
			{
				if (l.length)
				{
					program.addInstruction(l);
				}
			});
	
		program.SolvePart1();
		program.SolvePart2();
	}	

	main("../../data/advc20_08_sample.txt");
	main("../../data/advc20_08.txt");
}

