#include <string>
#include <vector>

namespace advc_2023::day15
{
    class Hash
    {
    public:
        void parse(const std::string& line);

        int get_sum_hash() const;
        int get_focusing_power() const;

    private:
        static int get_hash(const std::string& str);

        std::vector<std::string> m_sequence;
    };
}