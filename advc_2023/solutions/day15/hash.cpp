#include <assert.h>
#include <iostream>
#include <map>

#include "../../utils/input_utils.h"
#include "hash.h"

using namespace std;

namespace advc_2023::day15
{
    void Hash::parse(const string& line)
    {
        m_sequence = utils::split(line, ",");
    }

    // -----------------------------------------

    int Hash::get_hash(const std::string& str)
    {
        int hash = 0;
        for (const char c : str)
        {
            hash += c;
            hash *= 17;
            while (hash >= 256)
            {
                hash -= 256;
            }
        }
        return hash;
    }

    // -----------------------------------------

    int Hash::get_sum_hash() const
    {
        int hash_sum = 0;

        for (const string& seq : m_sequence)
        {

            hash_sum += get_hash(seq);
        }

        return hash_sum;
    }

    // -----------------------------------------

    int Hash::get_focusing_power() const
    {
        enum operation { add, remove };

        map<int, vector<pair<string, int>>> boxes;

        for (const string& seq : m_sequence)
        {
            string label;
            operation op = operation::add;
            int value = 0;

            if (seq.back() == '-')
            {
                label = seq.substr(0, seq.size() - 1);
                op = operation::remove;
            }
            else
            {
                const auto seq_parts = utils::split(seq, "=");

                label = seq_parts.front();
                value = stoi(seq_parts.back());
            }
            
            auto& box = boxes[get_hash(label)];

            if (op == operation::add)
            {
                bool found = false;
                for (auto& ite : box)
                {
                    if (label == ite.first)
                    {
                        ite.second = value;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    box.push_back(make_pair(label, value));
                }
            }
            else // op == operation::remove
            {
                for (int i = 0; i < (int)box.size(); i++)
                {
                    if (box[i].first == label)
                    {
                        for (int k = i; k < (int)box.size() - 1; k++)
                        {
                            box[k] = box[k + 1];
                        }
                        box.pop_back();
                        break;
                    }
                }
            }
        }

        int sum = 0;

        for (int i = 0; i < (int)boxes.size(); i ++)
        {
            const auto& box = boxes[i];
            for (int k = 0; k < (int)box.size(); k++)
            {
                sum += (i + 1) * (k + 1) * box[k].second;
            }
        }

        return sum;
    }
}