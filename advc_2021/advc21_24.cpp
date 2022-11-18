#include <algorithm>
#include <cmath>
#include <chrono>
#include <cstdio>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <list>
#include <map>
#include <memory>
#include <queue>
#include <set>
#include <sstream>
#include <stack>
#include <unordered_map>
#include <unordered_set>
#include <vector>

#include <assert.h>
#include <memory.h>
#include <stdlib.h>

using namespace std;

/*
* The code is only for simulating ALU logic, not to find min/max model number. 
* Only I could confirm the hand-figured model numbers right or wrong, using the simulator
* 
* Here's how I could predict min/max model number manually: 

01) *26, w + 6
02) *26, w + 14
03) *26, w + 13
04) /26, w == (z % 26) - 14 ==> (w4 == w3 + 13 - 14) == (w4 == w3 - 1)
05) *26, w + 6
06) /26, w == (z % 26)		==> (w6 == w5 + 6)
07) /26, w == (z % 26) - 6	==> (w7 == w2 + 14 - 6) == (w7 == w2 + 8)
08) *26, w + 3
09) /26, w == (z % 26) - 3	==> (w9 == w8 + 3 - 3) ==> (w9 == w8)
10) *26, w + 14
11) *26, w + 4
12) /26, w == (z % 26) - 2	==> (w12 == w11 + 4 - 2) ==> (w12 == w11 + 2)
13) /26, w == (z % 26) - 9  ==> (w13 == w10 + 14 - 9) ==> (w13 == w10 + 5)
14) /26, w == (z % 26) - 2	==> (w14 == w1 + 6 - 2) ==> (w14 == w1 + 4)

z02 (w1 + 6)
z03 ((w1 + 6) * 26 + w2 + 14)
z04 (((w1 + 6) * 26 + w2 + 14) * 26 + w3 + 13)
z05 ((w1 + 6) * 26 + w2 + 14)
z06 (((w1 + 6) * 26 + w2 + 14) * 26 + w5 + 6
z07 ((w1 + 6) * 26 + w2 + 14)
z08 (w1 + 6)
z09 ((w1 + 6) * 26 + w8 + 3)
z10 (w1 + 6)
z11 ((w1 + 6) * 26 + w10 + 14)
z12 (((w1 + 6) * 26 + w10 + 14) * 26 + w11 + 4)
z13 ((w1 + 6) * 26 + w10 + 14)
z14 (w1 + 6)

: Relations between inputs

(w4 == w3 - 1)
(w6 == w5 + 6)
(w7 == w2 + 8)
(w9 == w8)
(w12 == w11 + 2)
(w13 == w10 + 5)
(w14 == w1 + 4)

: Possible ranges per input

w1 : 1 - 5
w2 : 1
w3 : 2 - 9
w4 : 1 - 8
w5 : 1 - 3
w6 : 7 - 9
w7 : 9
w8 : 1 - 9
w9 : 1 - 9 (w8)
w10 : 1 - 4
w11 : 1 - 7
w12 : 3 - 9
w13 : 6 - 9
w14 : 5 - 9

: Predicted min/max numbers

51983999947999
11211791111365
*/

namespace ADVC_21_24
{
	const char REGW = 'w';
	const char REGX = 'x';
	const char REGY = 'y';
	const char REGZ = 'z';

	const vector<char> REGISTERS{ REGW, REGX, REGY, REGZ };

	const string INST_INP = "inp";
	const string INST_ADD = "add";
	const string INST_MUL = "mul";
	const string INST_DIV = "div";
	const string INST_MOD = "mod";
	const string INST_EQL = "eql";

	const vector<string> INSTRUCTIONS{ INST_INP, INST_ADD, INST_MUL, INST_DIV, INST_MOD, INST_EQL };

	enum class REGISTER_TYPE : uint8_t
	{
		X, Y, W, Z, CNT
	};

	enum class INSTRUCTION_TYPE : uint8_t
	{
		INP, ADD, MUL, DIV, MOD, EQL, CNT
	};

	// ----------------------------------------------------------
	class Operand
	{
	public:
		enum class OPERAND_TYPE
		{
			NIL, REGISTER, CONSTANT
		};

		Operand(string str)
		{
			if (!str.empty())
			{
				const char regChar = str[0];

				m_reg = Operand::ParseRegisterType(regChar);

				if (m_reg == REGISTER_TYPE::CNT)
				{
					m_val = stoi(str);
					m_opType = OPERAND_TYPE::CONSTANT;
				}
				else
				{
					m_opType = OPERAND_TYPE::REGISTER;
				}
			}
		}

		string ToString() const
		{
			stringstream stream;

			if (m_opType == OPERAND_TYPE::REGISTER)
			{
				stream << "[" << REGISTERS[(int)m_reg] << "]";
			}
			else if (m_opType == OPERAND_TYPE::CONSTANT)
			{
				stream << m_val;
			}
			return stream.str();
		}

		OPERAND_TYPE GetOperandType() const { return m_opType; }
		REGISTER_TYPE GetRegister() const { return m_reg;  }
		int GetValue() const { return m_val; }

		static REGISTER_TYPE ParseRegisterType(const char regChar)
		{
			for (int i = 0; i < (int)REGISTER_TYPE::CNT; i++)
			{
				if (regChar == REGISTERS[i])
				{
					return (REGISTER_TYPE)i;
				}
			}

			return REGISTER_TYPE::CNT;
		}

	private:
		OPERAND_TYPE m_opType{ OPERAND_TYPE::NIL };
		REGISTER_TYPE m_reg{ REGISTER_TYPE::CNT };
		int m_val{ 0 };
	};

	// ----------------------------------------------------------

	class Instruction
	{
	public:
		Instruction() = default;

		Instruction(string inst, string a, string b)
			: m_op(b)
		{
			for (int i = 0; i < (int)INSTRUCTION_TYPE::CNT; i++)
			{
				if (inst == INSTRUCTIONS[i])
				{
					m_inst = (INSTRUCTION_TYPE)i;
					break;
				}
			}

			assert(a.length() == 1);
			m_reg = Operand::ParseRegisterType(a[0]);
		}

		string ToString() const
		{
			stringstream stream;
			stream << "<" << INSTRUCTIONS[(int)m_inst] << "> " << "[" << REGISTERS[(int)m_reg];
			stream << "] " << m_op.ToString();

			return stream.str();
		}

		INSTRUCTION_TYPE GetInstruction() const { return  m_inst; }
		REGISTER_TYPE GetRegister() const { return m_reg; }
		const Operand& GetOperand() const { return m_op; }

	private:
	
		INSTRUCTION_TYPE m_inst{ INSTRUCTION_TYPE::CNT };
		REGISTER_TYPE m_reg{ REGISTER_TYPE::CNT };
		Operand m_op;
	};

	// ----------------------------------------------------------

	class Program
	{
	public:
		void AddInstruction(string inst, string a, string b)
		{
			m_instructions.push_back(make_unique<Instruction>(inst, a, b));
		}

		void PrintAllInstructions() const
		{
			for (const auto& inst : m_instructions)
			{
				cout << inst->ToString() << endl;
			}
		}

		const vector<unique_ptr<Instruction>>& GetInstructions() const { return m_instructions; }

	private:
		vector<unique_ptr<Instruction>> m_instructions;
	};


	// ----------------------------------------------------------

	class ALU
	{
	public:
		ALU()
		{
			Reset();
		}

		void Reset()
		{
			m_registers.clear();

			while (!m_inputs.empty())
			{
				m_inputs.pop();
			}

			for (int i = 0; i < (int)REGISTER_TYPE::CNT; i++)
			{
				m_registers.push_back(0);
			}
		}

		void SetRegisterValue(REGISTER_TYPE r, int val)
		{
			m_registers[(int)r] = val;
		}

		int GetRegisterValue(REGISTER_TYPE r)
		{
			return m_registers[(int)r];
		}

		int GetOperandValue(const Operand& op)
		{
			if (op.GetOperandType() == Operand::OPERAND_TYPE::REGISTER)
			{
				return GetRegisterValue(op.GetRegister());
			}
			else if (op.GetOperandType() == Operand::OPERAND_TYPE::CONSTANT)
			{
				return op.GetValue();
			}

			assert(false);
			return 0;
		}

		void Run(const Instruction* instruction)
		{
			const REGISTER_TYPE reg = instruction->GetRegister();

			if (instruction->GetInstruction() == INSTRUCTION_TYPE::INP)
			{
				assert(m_inputs.size() > 0);

				SetRegisterValue(reg, m_inputs.front());
				m_inputs.pop();
			}
			else
			{
				const int regValue = GetRegisterValue(reg);
				const int opValue = GetOperandValue(instruction->GetOperand());

				switch (instruction->GetInstruction())
				{
				case INSTRUCTION_TYPE::ADD:
					SetRegisterValue(reg, regValue + opValue);
					break;

				case INSTRUCTION_TYPE::DIV:
					SetRegisterValue(reg, regValue / opValue);
					break;

				case INSTRUCTION_TYPE::MUL:
					SetRegisterValue(reg, regValue * opValue);
					break;

				case INSTRUCTION_TYPE::MOD:
					SetRegisterValue(reg, regValue % opValue);
					break;

				case INSTRUCTION_TYPE::EQL:
					SetRegisterValue(reg, regValue == opValue ? 1 : 0);
					break;
				}
			}
		}

		void AddInput(int input)
		{
			m_inputs.push(input);
		}

		void PrintRegisters() const
		{
			cout << "---------- ALU Registers ---------- " << endl;
			for (int i = 0; i < (int)m_registers.size(); i++)
			{
				cout << "[" << REGISTERS[i] << "] : " << m_registers[i] << endl;
			}
		}

	private:
		vector<int> m_registers;
		queue<int> m_inputs;
	};

	// ----------------------------------------------------------

	class Computer
	{
	public:

		void ParseInstruction(string line)
		{
			string inst, a, b;

			stringstream stream(line);

			stream >> inst;
			stream >> a;

			if (!stream.eof())
			{
				stream >> b;
			}

			if (m_programs.empty() || inst == INST_INP)
			{
				m_programs.push_back(make_unique<Program>());
			}

			Program* program = m_programs[m_programs.size() - 1].get();

			program->AddInstruction(inst, a, b);
		}

		void PrintAllInstructions() const
		{
			for (int i = 0; i < (int)m_programs.size(); i++)
			{
				cout << "----------- program " << (i + 1) << " ------------" << endl;
				m_programs[i]->PrintAllInstructions();
			}
		}

		void Solve()
		{
			string maxModel = "51983999947999";
			string minModel = "11211791111365";

			TestModelNumber(maxModel);
			TestModelNumber(minModel);
		}

		bool TestModelNumber(string modelNumStr)
		{
			Reset();

			for (char c : modelNumStr)
			{
				AddInput(c - '0');
			}

			RunAll();

			PrintAllRegisters();

			bool ret = m_alu.GetRegisterValue(REGISTER_TYPE::Z) == 0;

			if (ret)
			{
				cout << "Model number " << modelNumStr << " is vallid" << endl;
			}
			else
			{
				cout << "Incorrect model number " << modelNumStr << endl;
			}

			return ret;
		}

	private:

		void Run(int programIndex)
		{
			for (const auto& inst : m_programs[programIndex]->GetInstructions())
			{
				m_alu.Run(inst.get());
			}
		}

		void RunAll()
		{
			for (int i = 0; i < (int)m_programs.size(); i++)
			{
				Run(i);
			}
		}

		void Reset()
		{
			m_alu.Reset();
		}

		void AddInput(int val)
		{
			m_alu.AddInput(val);
		}

		void PrintAllRegisters()
		{
			m_alu.PrintRegisters();
		}

		ALU m_alu;
		vector<unique_ptr<Program>> m_programs;
	};

	// ----------------------------------------------------------

	int RunMain(string path)
	{
		cout << "Starting program : reading " << path << endl;

		ifstream fin(path);

		if (fin.fail())
		{
			cout << "Could not open " << path << endl;
			return -1;
		}

		Computer computer;

		while (!fin.eof())
		{
			string line;
			std::getline(fin, line);

			if (line.length())
			{
				computer.ParseInstruction(line);
			}
		}

		computer.Solve();

		return 0;
	}
}


int main()
{
	ADVC_21_24::RunMain("../../../data/advc21_24.txt");

	return 0;
}