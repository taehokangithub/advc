#include <assert.h>
#include <iostream>

#include "../../utils/input_utils.h"
#include "modules.h"

using namespace std;

namespace advc_2023::day20
{
    static const string BUTTON = "button";
    static const string BROADCASTER = "broadcaster";
    static const char FLIP_FLOP_MARK = '%';
    static const char CONJUNCTION_MARK = '&';

    void Modules_manager::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            const auto split = utils::split(line, " -> ");
            assert(split.size() == 2);

            const std::string module_part = split.front();
            const std::string output_part = split.back();

            shared_ptr<Module> module{};
            std::string name;

            if (module_part == BROADCASTER)
            {
                module = make_shared<Broadcaster>();
                name = module_part;
            }
            else if (module_part.front() == FLIP_FLOP_MARK)
            {
                module = make_shared<Flipflop>();
                name = module_part.substr(1);
            }
            else if (module_part.front() == CONJUNCTION_MARK)
            {
                module = make_shared<Conjunction>();
                name = module_part.substr(1);
            }

            assert(module);

            for (const auto& output : utils::split(output_part, ", "))
            {
                module->add_output(output);
            }

            module->set_name(name);
            m_modules[name] = module;
        }

        // add input information
        for (const auto& [name, module] : m_modules)
        {
            for (const auto& output : module->get_outputs())
            {
                if (const auto ite = m_modules.find(output); ite != m_modules.end())
                {
                    ite->second->add_input(name);
                }
            }
        }
    }

    // ----------------------------------------------------

    signal Flipflop::process_input(const signal_sent& input_signal)
    {
        assert(input_signal.target == get_name());

        if (input_signal.signal == signal::high)
        {
            return signal::none;
        }
        m_state = !m_state;

        return (m_state) ? signal::high : signal::low;
    }

    // ----------------------------------------------------

    bool Conjunction::is_original_state() const
    {
        for (const auto& [input, state] : m_inputs)
        {
            if (get_input_state(input) != signal::low)
            {
                return false;
            }
        }
        return true;
    }

    // ----------------------------------------------------

    signal Conjunction::process_input(const signal_sent& input_signal)
    {
        assert(input_signal.target == get_name());

        m_inputs[input_signal.source] = input_signal.signal;

        for (const auto& [input, state] : m_inputs)
        {
            if (get_input_state(input) == signal::low)
            {
                return signal::high;
            }
        }
        return signal::low;
    }

    // ----------------------------------------------------

    bool Modules_manager::is_original_state() const
    {
        for (const auto& [_, module] : m_modules)
        {
            if (!module->is_original_state())
            {
                return false;
            }
        }
        return true;
    }

    // ----------------------------------------------------

    void Modules_manager::handle_sent_signal(std::queue<signal_sent>& q, const signal_sent ss) const
    {
        if (const auto ite = m_modules.find(ss.target); ite != m_modules.end())
        {
            const auto& module = ite->second;

            if (const auto output_signal = module->process_input(ss); output_signal != signal::none)
            {
                for (const auto& output_name : module->get_outputs())
                {
                    signal_sent new_item{ module->get_name(), output_name, output_signal };
                    q.push(new_item);
                }
            }
        }
    }

    // ----------------------------------------------------

    int Modules_manager::get_total_signal_score(const int total_loops) const
    {
        std::unordered_map<signal, int> counter;
        int count_button_pressed = 0;

        do
        {
            count_button_pressed++;

            queue<signal_sent> q;
            q.push({ BUTTON, BROADCASTER, signal::low });

            while (!q.empty())
            {
                const auto item = q.front(); q.pop();

                counter[item.signal]++;

                handle_sent_signal(q, item);
            }

        } while (count_button_pressed < total_loops && !is_original_state());

        
        const int required_sets = total_loops / count_button_pressed;
        const int loop_score = counter[signal::high] * counter[signal::low] * required_sets * required_sets;

        return loop_score;
    }

    uint64_t Modules_manager::get_cnt_button_pressed(std::string target) const
    {
        int count_button_pressed = 0;

        std::unordered_map<string, int> first_high;

        do
        {
            count_button_pressed++;

            queue<signal_sent> q;
            q.push({ BUTTON, BROADCASTER, signal::low });

            while (!q.empty())
            {
                const auto item = q.front(); q.pop();

                if (item.target == target)
                {
                    auto module = m_modules.find(item.source)->second;
                    Conjunction* con = static_cast<Conjunction*>(module.get());

                    for (const auto& [input_name, s] : con->get_inputs())
                    {
                        if (first_high[input_name] == 0)
                        {
                            if (s == signal::high)
                            {
                                first_high[input_name] = count_button_pressed;
                            }
                            else
                            {
                                first_high[input_name] = 0;
                            }
                        }
                    }

                    uint64_t sum = 1;
                    for (const auto& [input_name, cnt] : first_high)
                    {
                        sum *= cnt;
                    }
                    if (sum > 0)
                    {
                        return sum;
                    }
                }

                handle_sent_signal(q, item);
            }

        } while (true);
    }

}